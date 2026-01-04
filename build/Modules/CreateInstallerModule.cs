using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Enums;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Shouldly;
using Sourcy.DotNet;
using File = ModularPipelines.FileSystem.File;

namespace Build.Modules;

/// <summary>
///     Create the .msi installer.
/// </summary>
[DependsOn<ResolveVersioningModule>]
[DependsOn<CompileProjectModule>]
public sealed class CreateInstallerModule : Module
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var wixTarget = new File(Projects.LookupEngine.FullName);
        var wixInstaller = new File(Projects.Installer.FullName);
        var wixToolFolder = await InstallWixAsync(context, cancellationToken);

        await context.DotNet().Build(new DotNetBuildOptions
        {
            ProjectSolution = wixInstaller.Path,
            Configuration = Configuration.Release
        }, cancellationToken);

        var builderFile = wixInstaller.Folder!
            .GetFolder("bin")
            .FindFile(file => file.NameWithoutExtension == wixInstaller.NameWithoutExtension && file.Extension == ".exe");

        builderFile.ShouldNotBeNull($"No installer builder was found for the project: {wixInstaller.NameWithoutExtension}");

        var targetDirectories = wixTarget.Folder!
            .GetFolder("bin")
            .GetFolders(folder => folder.Name == "publish")
            .Select(folder => folder.Path)
            .ToArray();

        targetDirectories.ShouldNotBeEmpty("No content were found to create an installer");

        await targetDirectories.ForEachAsync(async targetDirectory =>
            {
                await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions(builderFile.Path)
                {
                    Arguments = [targetDirectory],
                    WorkingDirectory = context.Git().RootDirectory,
                    CommandLogging = CommandLogging.Default & ~CommandLogging.Input,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        {"PATH", $"{Environment.GetEnvironmentVariable("PATH")};{wixToolFolder}"}
                    }
                }, cancellationToken);
            }, cancellationToken)
            .ProcessInParallel();

        return await NothingAsync();
    }

    /// <summary>
    ///     Installs the WiX toolset required for building installers.
    /// </summary>
    private static async Task<Folder> InstallWixAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var wixToolFolder = context.FileSystem.CreateTemporaryFolder();
#if (_needToUpdatePipelineVersion)
        await context.DotNet().Tool.Install(new DotNetToolInstallOptions("wix")
        {
            ToolPath = wixToolFolder.Path
        }, cancellationToken);
#endif
        await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments =
            [
                "tool",
                "install",
                "--tool-path", wixToolFolder.Path,
                "wix"
            ]
        }, cancellationToken);

        return wixToolFolder;
    }
}
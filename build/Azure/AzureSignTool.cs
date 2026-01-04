using Build.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace Build.Azure;

public sealed class AzureSignTool(IFileSystemContext fileSystemContext, IPipelineContext context)
{
    private readonly Folder _temporaryFolder = fileSystemContext.CreateTemporaryFolder();
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    public async Task<CommandResult> Sign(AzureSignToolOptions options, CancellationToken cancellationToken = default)
    {
        await SemaphoreSlim.WaitAsync(cancellationToken);

        try
        {
#if (_needToUpdatePipelineVersion)
            await context.DotNet().Tool.Install(new DotNetToolInstallOptions("AzureSignTool")
            {
                ToolPath = _temporaryFolder.Path
            }, cancellationToken);
#endif
            await context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
            {
                Arguments =
                [
                    "tool",
                    "install",
                    "--tool-path", _temporaryFolder.Path,
                    "AzureSignTool"
                ]
            }, cancellationToken);

            return await context.Command.ExecuteCommandLineTool(options with
            {
                Tool = Path.Combine(_temporaryFolder.Path, options.Tool)
            }, cancellationToken);
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }
}
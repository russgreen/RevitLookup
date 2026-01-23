using Build.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.DotNet.Services;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;

namespace Build.Azure;

public sealed class AzureSignTool(IDotNet dotNet, ICommand command)
{
    private readonly Folder _temporaryFolder = Folder.CreateTemporaryFolder();
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);

    public async Task<CommandResult> Sign(AzureSignToolOptions options, CancellationToken cancellationToken = default)
    {
        await SemaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            await dotNet.Tool.Execute(new DotNetToolOptions
            {
                Arguments = ["install", "AzureSignTool", "--tool-path", _temporaryFolder.Path]
            }, cancellationToken: cancellationToken);

            return await command.ExecuteCommandLineTool(options with
            {
                Tool = _temporaryFolder.GetFile("AzureSignTool.exe")
            }, cancellationToken: cancellationToken);
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }
}
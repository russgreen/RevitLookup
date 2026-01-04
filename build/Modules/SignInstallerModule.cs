using Build.Azure;
using Build.Azure.Options;
using Build.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Shouldly;

namespace Build.Modules;

[DependsOn<CreateInstallerModule>]
public sealed class SignInstallerModule(IOptions<SigningOptions> signingOptions, IOptions<BuildOptions> buildOptions) : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var targetFiles = context.Git().RootDirectory.GetFolder(buildOptions.Value.OutputDirectory)
            .GetFiles(file => file.Extension is ".msi" or ".exe")
            .Select(file => file.Path)
            .ToArray();

        targetFiles.ShouldNotBeEmpty("No files were found to sign");

        var inputFile = context.FileSystem.GetNewTemporaryFilePath();
        File.WriteAllLines(inputFile, targetFiles);

        context.Logger.LogInformation("Signing {Count} files", targetFiles.Length);

        return await context.Azure().Sign(new AzureSignToolOptions
        {
            KeyVaultUrl = signingOptions.Value.VaultUri,
            KeyVaultTenantId = signingOptions.Value.TenantId,
            KeyVaultClientId = signingOptions.Value.ClientId,
            KeyVaultClientSecret = signingOptions.Value.ClientSecret,
            KeyVaultCertificateName = signingOptions.Value.CertificateName,
            TimestampRfc3161Url = "http://timestamp.digicert.com",
            SkipSigned = true,
            ContinueOnError = true
        }, cancellationToken);
    }
}
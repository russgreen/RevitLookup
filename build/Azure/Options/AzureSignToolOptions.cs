using JetBrains.Annotations;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace Build.Azure.Options;

[PublicAPI]
[Serializable]
[CliSubCommand("sign")]
public sealed record AzureSignToolOptions : CommandLineToolOptions
{
    [CliOption("--azure-key-vault-url")]
    public string? KeyVaultUrl { get; set; }

    [CliOption("--azure-key-vault-client-id")]
    public string? KeyVaultClientId { get; set; }

    [CliOption("--azure-key-vault-client-secret")]
    public string? KeyVaultClientSecret { get; set; }

    [CliOption("--azure-key-vault-tenant-id")]
    public string? KeyVaultTenantId { get; set; }

    [CliOption("--azure-key-vault-certificate")]
    public string? KeyVaultCertificateName { get; set; }

    [CliOption("--azure-key-vault-accesstoken")]
    public string? KeyVaultAccessToken { get; set; }

    [CliFlag("--azure-key-vault-managed-identity")]
    public bool? KeyVaultManagedIdentity { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--description-url")]
    public string? DescriptionUrl { get; set; }

    [CliOption("--timestamp-rfc3161")]
    public string? TimestampRfc3161Url { get; set; }

    [CliOption("--timestamp-authenticode")]
    public string? TimestampAuthenticodeUrl { get; set; }

    [CliOption("--timestamp-digest")]
    public string? TimestampDigest { get; set; }

    [CliOption("--file-digest")]
    public string? FileDigest { get; set; }

    [CliOption("--additional-certificates")]
    public IEnumerable<string>? AdditionalCertificates { get; set; }

    [CliFlag("--verbose")]
    public bool? Verbose { get; set; }

    [CliFlag("--quiet")]
    public bool? Quiet { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliOption("--input-file-list")]
    public string? InputFileList { get; set; }

    [CliFlag("--skip-signed")]
    public bool? SkipSigned { get; set; }

    [CliFlag("--append-signature")]
    public bool? AppendSignature { get; set; }

    [CliFlag("--page-hashing")]
    public bool? PageHashing { get; set; }

    [CliFlag("--no-page-hashing")]
    public bool? NoPageHashing { get; set; }

    [CliOption("--max-degree-of-parallelism")]
    public int? MaxDegreeOfParallelism { get; set; }
}
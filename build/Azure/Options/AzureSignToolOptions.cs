using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace Build.Azure.Options;

[Serializable]
public sealed record AzureSignToolOptions : CommandLineToolOptions
{
    public AzureSignToolOptions() : base("AzureSignTool")
    {
        CommandParts = ["sign"];
    }

    [CommandSwitch("--azure-key-vault-url")]
    public string? KeyVaultUrl { get; set; }

    [CommandSwitch("--azure-key-vault-client-id")]
    public string? KeyVaultClientId { get; set; }

    [CommandSwitch("--azure-key-vault-client-secret")]
    public string? KeyVaultClientSecret { get; set; }

    [CommandSwitch("--azure-key-vault-tenant-id")]
    public string? KeyVaultTenantId { get; set; }

    [CommandSwitch("--azure-key-vault-certificate")]
    public string? KeyVaultCertificateName { get; set; }

    [CommandSwitch("--azure-key-vault-accesstoken")]
    public string? KeyVaultAccessToken { get; set; }

    [BooleanCommandSwitch("--azure-key-vault-managed-identity")]
    public bool? KeyVaultManagedIdentity { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--description-url")]
    public string? DescriptionUrl { get; set; }

    [CommandSwitch("--timestamp-rfc3161")]
    public string? TimestampRfc3161Url { get; set; }

    [CommandSwitch("--timestamp-authenticode")]
    public string? TimestampAuthenticodeUrl { get; set; }

    [CommandSwitch("--timestamp-digest")]
    public string? TimestampDigest { get; set; }

    [CommandSwitch("--file-digest")]
    public string? FileDigest { get; set; }

    [CommandSwitch("--additional-certificates")]
    public IEnumerable<string>? AdditionalCertificates { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CommandSwitch("--input-file-list")]
    public string? InputFileList { get; set; }

    [BooleanCommandSwitch("--skip-signed")]
    public bool? SkipSigned { get; set; }

    [BooleanCommandSwitch("--append-signature")]
    public bool? AppendSignature { get; set; }

    [BooleanCommandSwitch("--page-hashing")]
    public bool? PageHashing { get; set; }

    [BooleanCommandSwitch("--no-page-hashing")]
    public bool? NoPageHashing { get; set; }

    [CommandSwitch("--max-degree-of-parallelism")]
    public int? MaxDegreeOfParallelism { get; set; }
}
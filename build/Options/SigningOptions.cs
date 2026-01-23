namespace Build.Options;

[Serializable]
public sealed class SigningOptions
{
    /// <summary>
    ///     Azure Key Vault URI
    /// </summary>
    public string? VaultUri { get; init; }

    /// <summary>
    ///     Azure Key Vault tenant ID
    /// </summary>
    public string? TenantId { get; init; }

    /// <summary>
    ///     Azure Key Vault client ID
    /// </summary>
    public string? ClientId { get; init; }

    /// <summary>
    ///     Azure Key Vault client secret
    /// </summary>
    public string? ClientSecret { get; init; }

    /// <summary>
    ///     Azure Key Vault certificate name
    /// </summary>
    public string? CertificateName { get; init; }
}
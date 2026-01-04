using System.ComponentModel.DataAnnotations;

namespace Build.Options;

public sealed class SigningOptions
{
    /// <summary>
    ///     Azure Key Vault URI
    /// </summary>
    [Required] public string VaultUri { get; init; } = null!;

    /// <summary>
    ///     Azure Key Vault tenant ID
    /// </summary>
    [Required] public string TenantId { get; init; } = null!;

    /// <summary>
    ///     Azure Key Vault client ID
    /// </summary>
    [Required] public string ClientId { get; init; } = null!;

    /// <summary>
    ///     Azure Key Vault client secret
    /// </summary>
    [Required] public string ClientSecret { get; init; } = null!;

    /// <summary>
    ///     Azure Key Vault certificate name
    /// </summary>
    [Required] public string CertificateName { get; init; } = null!;
}
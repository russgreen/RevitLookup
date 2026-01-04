using System.ComponentModel.DataAnnotations;

namespace Build.Options;

[Serializable]
public sealed record BuildOptions
{
    /// <summary>
    ///     Application versions mapping to compile configuration
    /// </summary>
    /// <example>
    ///     1.0.0-alpha.1.250101 <br/>
    ///     1.0.0-beta.2.250101 <br/>
    ///     1.0.0
    /// </example>
    [Required] public Dictionary<string, string> Versions { get; init; } = null!;

    /// <summary>
    ///     Path to build output
    /// </summary>
    [Required] public string OutputDirectory { get; init; } = null!;
}
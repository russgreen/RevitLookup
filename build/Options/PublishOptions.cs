using System.ComponentModel.DataAnnotations;

namespace Build.Options;

[Serializable]
public sealed record PublishOptions
{
    /// <summary>
    ///     Release version
    /// </summary>
    [Required] public string Version { get; init; } = null!;

    /// <summary>
    ///     Path to the changelog file
    /// </summary>
    public string? ChangelogFile { get; init; }
}
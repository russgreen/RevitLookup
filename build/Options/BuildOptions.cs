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
    public Dictionary<string, Version> Versions { get; init; } = [];

    /// <summary>
    ///     Path to build output
    /// </summary>
    public string OutputDirectory { get; init; } = "output";
}
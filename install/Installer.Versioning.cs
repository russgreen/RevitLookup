// Copyright (c) Lookup Foundation and Contributors
// 
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
// 
// THIS PROGRAM IS PROVIDED "AS IS" AND WITH ALL FAULTS.
// NO IMPLIED WARRANTY OF MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE IS PROVIDED.
// THERE IS NO GUARANTEE THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.

using System.Text.RegularExpressions;

namespace Installer;

public static partial class Versioning
{
    /// <summary>
    ///     Resolve versions using the specified version string.
    /// </summary>
    public static ResolveVersioningResult CreateFromVersionStringAsync(string version)
    {
        var versionParts = version.Split('-');
        var semanticVersion = Version.Parse(versionParts[0]);

        return new ResolveVersioningResult
        {
            Version = version,
            VersionPrefix = semanticVersion,
            VersionSuffix = versionParts.Length > 1 ? versionParts[1] : null,
            MsiVersion = semanticVersion.Major > 255 ? new Version(semanticVersion.Major % 100, semanticVersion.Minor, semanticVersion.Build) : semanticVersion
        };
    }

    /// <summary>
    ///     A regular expression to match the last sequence of numeric characters in a string.
    /// </summary>
    [GeneratedRegex(@"(\d+)(?!.*\d)")]
    private static partial Regex VersionRegex();
}

public sealed record ResolveVersioningResult
{
    /// <summary>
    ///     Release version, includes version number and release stage.
    /// </summary>
    /// <remarks>Version format: <c>version-environment.n.date</c>.</remarks>
    /// <example>
    ///     1.0.0-alpha.1.250101 <br/>
    ///     1.0.0-beta.2.250101 <br/>
    ///     1.0.0
    /// </example>
    public required string Version { get; init; }

    /// <summary>
    ///     The normal part of the release version number.
    /// </summary>
    /// <example>
    ///     1.0.0 <br/>
    ///     12.3.6 <br/>
    ///     2026.4.0
    /// </example>
    public required Version VersionPrefix { get; init; }

    /// <summary>
    ///     The pre-release label of the release version number.
    /// </summary>
    /// <example>
    ///     alpha <br/>
    ///     beta <br/>
    ///     rc.1.250101
    /// </example>
    public required string? VersionSuffix { get; init; }

    /// <summary>
    ///     Msi compatible version number.
    /// </summary>
    /// <remarks>The max major version is 255.</remarks>
    /// <example>
    ///     2026.4.0 -> 26.4.0
    /// </example>
    public required Version MsiVersion { get; init; }
}
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

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Installer;

/// <summary>
///     Installer versions metadata.
/// </summary>
public sealed class Versions
{
    public required Version InstallerVersion { get; init; }
    public required Version AssemblyVersion { get; init; }
    public int RevitVersion { get; init; }
}

public static partial class Tools
{
    /// <summary>
    ///     Compute installer versions based on the RevitLookup.dll file.
    /// </summary>
    public static Versions ComputeVersions(string[] args)
    {
        if (!TryParseVersion(args[0], out var fileVersion))
        {
            throw new Exception($"Could not parse version from directory name: {args[0]}");
        }

        var version = new Version(fileVersion);
        return new Versions
        {
            AssemblyVersion = version,
            RevitVersion = version.Major,
            InstallerVersion = version.Major > 255 ? new Version(version.Major % 100, version.Minor, version.Build) : version
        };
    }

    /// <summary>
    ///     Parse a version string from the given input.
    /// </summary>
    public static bool TryParseVersion(string input, [NotNullWhen(true)] out string? version)
    {
        version = null;
        var match = VersionRegex().Match(input);
        if (!match.Success) return false;

        switch (match.Value.Length)
        {
            case 4:
                version = match.Value;
                return true;
            case 2:
                version = $"20{match.Value}";
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    ///     A regular expression to match the last sequence of numeric characters in a string.
    /// </summary>
    [GeneratedRegex(@"(\d+)(?!.*\d)")]
    private static partial Regex VersionRegex();
}
using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Modules;

namespace Build.Modules;

/// <summary>
///     Generate and format the changelog for publishing on the GitHub.
/// </summary>
[DependsOn<GenerateChangelogModule>]
[DependsOn<ResolveProductVersionModule>]
public sealed class GenerateGitHubChangelogModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var versioningResult = await GetModule<ResolveProductVersionModule>();
        var changelogResult = await GetModule<GenerateChangelogModule>();
        var versioning = versioningResult.Value!;
        var changelog = changelogResult.Value!;

        return AppendExtraUrls(context, changelog, versioning);
    }

    /// <summary>
    ///     Append extra links for GitHub release.
    /// </summary>
    private static string AppendExtraUrls(IPipelineContext context, string changelog, ResolveVersioningResult versioning)
    {
        var repositoryInfo = context.GitHub().RepositoryInfo;
        var changelogBuilder = new StringBuilder(changelog)
            .AppendLine()
            .AppendLine()
            .Append("**Full changelog**: ")
            .AppendLine($"https://github.com/{repositoryInfo.Identifier}/compare/{versioning.PreviousVersion}...{versioning.Version}");

        if (!versioning.IsPrerelease)
        {
            changelogBuilder.AppendLine()
                .Append("**RevitLookup versions**: ")
                .Append("https://github.com/lookup-foundation/RevitLookup/wiki/Versions");
        }

        return changelogBuilder.ToString();
    }
}
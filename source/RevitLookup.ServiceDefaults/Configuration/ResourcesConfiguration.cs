using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Abstractions.Options;
using RevitLookup.Common.Extensions;

namespace RevitLookup.ServiceDefaults.Configuration;

[PublicAPI]
public static class ResourcesConfiguration
{
    public static TBuilder ConfigureResourceLocations<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddOptions<ResourceLocationsOptions>().Configure<IHostEnvironment>((options, environment) =>
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version ??= new Version(1, 0);
            var majorVersion = version.Major.ToString();

            options.ApplicationDataDirectory = Environment
                .GetFolderPath(Environment.SpecialFolder.ApplicationData)
                .AppendPath(environment.ApplicationName)
                .AppendPath(majorVersion);

            options.LocalApplicationDataDirectory = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                .AppendPath(environment.ApplicationName)
                .AppendPath(majorVersion);

            //Local directories
            options.DownloadsFolder = options.LocalApplicationDataDirectory.AppendPath("DownloadCache");

            //Roaming directories
            options.SettingsDirectory = options.ApplicationDataDirectory.AppendPath("Settings");

            //Roaming files
            options.ApplicationSettingsPath = options.SettingsDirectory.AppendPath("Application.json");
            options.DecompositionSettingsPath = options.SettingsDirectory.AppendPath("LookupEngine.json");
            options.VisualizationSettingsPath = options.SettingsDirectory.AppendPath("Visualization.json");
        }).ValidateDataAnnotations();

        return builder;
    }
}
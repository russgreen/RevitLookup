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
                .JoinPath(environment.ApplicationName)
                .JoinPath(majorVersion);

            options.LocalApplicationDataDirectory = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                .JoinPath(environment.ApplicationName)
                .JoinPath(majorVersion);

            //Local directories
            options.DownloadsFolder = options.LocalApplicationDataDirectory.JoinPath("DownloadCache");

            //Roaming directories
            options.SettingsDirectory = options.ApplicationDataDirectory.JoinPath("Settings");

            //Roaming files
            options.ApplicationSettingsPath = options.SettingsDirectory.JoinPath("Application.json");
            options.DecompositionSettingsPath = options.SettingsDirectory.JoinPath("LookupEngine.json");
            options.VisualizationSettingsPath = options.SettingsDirectory.JoinPath("Visualization.json");
        }).ValidateDataAnnotations();

        return builder;
    }
}
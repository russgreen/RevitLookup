using System.Diagnostics;
using System.Reflection;
using System.Runtime.Versioning;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RevitLookup.Abstractions.Options;
using RevitLookup.Common.Utils;

namespace RevitLookup.ServiceDefaults.Configuration;

[PublicAPI]
public static class AssemblyConfiguration
{
    public static TBuilder ConfigureAssembly<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyLocation = assembly.Location;
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        var fileVersion = new Version(FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion!);
        var targetFrameworkAttribute = assembly.GetCustomAttribute<TargetFrameworkAttribute>()!;

        builder.Services.Configure<AssemblyOptions>(options =>
        {
            options.Framework = targetFrameworkAttribute.FrameworkDisplayName ?? targetFrameworkAttribute.FrameworkName;
            options.Version = new Version(fileVersion.Major, fileVersion.Minor, fileVersion.Build);
            options.HasAdminAccess = assemblyLocation.StartsWith(appDataPath) || !AccessUtils.CheckWriteAccess(assemblyLocation);
        });

        return builder;
    }
}
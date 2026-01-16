using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RevitLookup.ServiceDefaults.Configuration;

[PublicAPI]
public static class HostingConfiguration
{
    public static TBuilder ConfigureHosting<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);

        return builder;
    }
}
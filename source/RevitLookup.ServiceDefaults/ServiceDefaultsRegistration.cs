using Microsoft.Extensions.Hosting;
using RevitLookup.ServiceDefaults.Configuration;

namespace RevitLookup.ServiceDefaults;

public static class ServiceDefaultsRegistration
{
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureHosting();
        builder.ConfigureAssembly();
        builder.ConfigureJsonSerializer();
        builder.ConfigureResourceLocations();

        return builder;
    }
}
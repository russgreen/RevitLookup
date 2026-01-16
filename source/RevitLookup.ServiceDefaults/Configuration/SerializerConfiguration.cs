using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace RevitLookup.ServiceDefaults.Configuration;

[PublicAPI]
public static class SerializerConfiguration
{
    public static TBuilder ConfigureJsonSerializer<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.Configure<JsonSerializerOptions>(options =>
            {
                options.WriteIndented = true;
                options.PropertyNameCaseInsensitive = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                options.Converters.Add(new JsonStringEnumConverter());
            });
        }
        else
        {
            builder.Services.Configure<JsonSerializerOptions>(options =>
            {
                options.WriteIndented = true;
                options.PropertyNameCaseInsensitive = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.Converters.Add(new JsonStringEnumConverter());
            });
        }

        return builder;
    }
}
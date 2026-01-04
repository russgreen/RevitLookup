using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace Build.Azure;

public static class AzureSignToolExtensions
{
    [ModuleInitializer]
    public static void RegisterAzureSignToolContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => collection.RegisterAzureSignToolContext());
    }

    public static IServiceCollection RegisterAzureSignToolContext(this IServiceCollection services)
    {
        services.TryAddScoped<AzureSignTool>();
        return services;
    }

    public static AzureSignTool Azure(this IPipelineHookContext context) => context.ServiceProvider.GetRequiredService<AzureSignTool>();
}
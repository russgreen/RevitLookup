using Build.Modules;
using Build.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Host;

await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json")
            .AddCommandLine(args)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, collection) =>
    {
        collection.AddOptions<BuildOptions>().Bind(context.Configuration.GetSection("Build")).ValidateDataAnnotations();

        collection.AddModule<ResolveConfigurationsModule>();
        collection.AddModule<CleanProjectModule>();
        collection.AddModule<CompileProjectModule>();

        if (args.Contains("pack"))
        {
            collection.AddOptions<ProductOptions>().Bind(context.Configuration.GetSection("Product")).ValidateDataAnnotations();

            collection.AddModule<ResolveProductVersionModule>();
            collection.AddModule<CreateBundleModule>();
            collection.AddModule<CreateInstallerModule>();
        }

        if (args.Contains("publish"))
        {
            collection.AddOptions<SigningOptions>().Bind(context.Configuration.GetSection("Signing")).ValidateDataAnnotations();

            collection.AddModule<SignAssembliesModule>();
            collection.AddModule<SignInstallerModule>();
            collection.AddModule<GenerateChangelogModule>();
            collection.AddModule<GenerateGitHubChangelogModule>();
            collection.AddModule<PublishGithubModule>();
        }
    })
    .ExecutePipelineAsync();
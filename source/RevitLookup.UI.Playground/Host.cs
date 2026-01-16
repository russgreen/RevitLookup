using System.Windows.Threading;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RevitLookup.Abstractions.Services.Appearance;
using RevitLookup.Abstractions.Services.Application;
using RevitLookup.Abstractions.Services.Decomposition;
using RevitLookup.Abstractions.Services.Presentation;
using RevitLookup.Abstractions.Services.Settings;
using RevitLookup.ServiceDefaults;
using RevitLookup.UI.Framework.Services;
using RevitLookup.UI.Framework.Services.Presentation;
using RevitLookup.UI.Playground.Configuration;
using RevitLookup.UI.Playground.Mocks.Services.Appearance;
using RevitLookup.UI.Playground.Mocks.Services.Application;
using RevitLookup.UI.Playground.Mocks.Services.Decomposition;
using RevitLookup.UI.Playground.Mocks.Services.Settings;
using RevitLookup.UI.Playground.Services.Host;
using RevitLookup.UI.Playground.Services.Mvvm;
using Wpf.Ui;
using Wpf.Ui.Abstractions;

namespace RevitLookup.UI.Playground;

/// <summary>
///     Provides a host for the application's services and manages their lifetimes.
/// </summary>
public static class Host
{
    private static IHost? _host;

    /// <summary>
    ///     Starts the host and configures the application's services
    /// </summary>
    public static void Start()
    {
        var builder = new HostApplicationBuilder();

        //Logging
        builder.Logging.ClearProviders();
        builder.AddSerilogLoggingProvider();

        //Configuration
        builder.AddServiceDefaults();

        //Host services
        builder.Services.AddHostedService<RevitApplicationService>();

        //Application Services
        builder.Services.AddSingleton<ISoftwareUpdateService, MockSoftwareUpdateService>();
        builder.Services.AddSingleton<ISettingsService, MockSettingsService>();
        builder.Services.AddSingleton<IThemeWatcherService, MockThemeWatcherService>();

        //MVVM services
        builder.Services.RegisterViews();
        builder.Services.RegisterViewModels();

        //Frontend services
        builder.Services.AddScoped<INavigationViewPageProvider, DependencyInjectionNavigationViewPageProvider>();
        builder.Services.AddScoped<INavigationService, NavigationService>();
        builder.Services.AddScoped<IContentDialogService, ContentDialogService>();
        builder.Services.AddScoped<ISnackbarService, SnackbarService>();
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<IWindowIntercomService, WindowIntercomService>();
        builder.Services.AddScoped<IMessenger, WeakReferenceMessenger>();

        //Composer services
        builder.Services.AddScoped<IDecompositionService, MockDecompositionService>();
        builder.Services.AddScoped<IVisualDecompositionService, MockVisualDecompositionService>();
        builder.Services.AddScoped<IDecompositionSearchService, MockDecompositionSearchService>();
        builder.Services.AddTransient<IUiOrchestratorService, MockUiOrchestratorService>();

        _host = builder.Build();

        var frame = new DispatcherFrame();
        _host.StartAsync().ContinueWith(_ => frame.Continue = false);

        Dispatcher.PushFrame(frame);
    }

    /// <summary>
    ///     Stops the host and handle <see cref="IHostedService"/> services
    /// </summary>
    public static void Stop()
    {
        if (_host is null) throw new InvalidOperationException("Host is not running");

        var frame = new DispatcherFrame();
        _host.StopAsync().ContinueWith(_ => frame.Continue = false);

        Dispatcher.PushFrame(frame);
    }

    /// <summary>
    ///     Get service of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of service object to get</typeparam>
    /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="T"/></exception>
    public static T GetService<T>() where T : class
    {
        return _host!.Services.GetRequiredService<T>();
    }
}
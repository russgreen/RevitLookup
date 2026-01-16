using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Controls;

namespace RevitLookup.UI.Playground.Services.Mvvm;

public static class ViewsRegistration
{
    public static void RegisterViews(this IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<Framework.App>()
            .AddClasses(filter => filter.AssignableTo<FluentWindow>()).AsSelf().WithScopedLifetime()
            .AddClasses(filter => filter.AssignableTo<ContentDialog>()).AsSelf().WithTransientLifetime()

            //Scoped navigation pages
            .AddClasses(filter =>
            {
                filter.AssignableTo<Page>();
                filter.Where(type => type.IsAssignableTo(typeof(INavigableView<object>)));
            }).AsSelf().WithScopedLifetime()

            //Transient pages
            .AddClasses(filter =>
            {
                filter.AssignableTo<Page>();
                filter.Where(type => !type.IsAssignableTo(typeof(INavigableView<object>)));
            }).AsSelf().WithTransientLifetime());

        services.Scan(selector => selector.FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<FluentWindow>()).AsSelf().WithScopedLifetime()
            .AddClasses(filter => filter.AssignableTo<Page>()).AsSelf().WithScopedLifetime());
    }
}
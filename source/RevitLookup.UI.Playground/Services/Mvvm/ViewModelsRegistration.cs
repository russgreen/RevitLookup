using Microsoft.Extensions.DependencyInjection;

namespace RevitLookup.UI.Playground.Services.Mvvm;

public static class ViewModelsRegistration
{
    public static void RegisterViewModels(this IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<App>()

            // Client ViewModels
            .AddClasses(filter => filter.InNamespaces("RevitLookup.UI.Playground.ViewModels"))
            .AsSelf()
            .WithScopedLifetime()

            // Mock ViewModels
            .AddClasses(filter => filter.NotInNamespaces("RevitLookup.UI.Playground.ViewModels").Where(type => type.Name.EndsWith("ViewModel")))
            .As(FilterViewModelInterface).WithScopedLifetime());
    }

    private static IEnumerable<Type> FilterViewModelInterface(Type serviceType)
    {
        var className = serviceType.Name;
        if (serviceType.IsInterface) return [];

        var viewModelInterfaces = new List<Type>(1);
        foreach (var serviceInterface in serviceType.GetInterfaces())
        {
            var interfaceName = serviceInterface.Name;
            if (interfaceName.Length < 2 || interfaceName[0] != 'I') continue;

            var coreName = interfaceName[1..];
            var tickIndex = coreName.IndexOf('`');
            if (tickIndex >= 0)
            {
                coreName = coreName[..tickIndex];
            }

            if (!className.EndsWith(coreName, StringComparison.Ordinal)) continue;

            viewModelInterfaces.Add(serviceInterface);
            return viewModelInterfaces;
        }

        return viewModelInterfaces;
    }
}
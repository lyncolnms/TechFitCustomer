
using TechFitCustomer.Services;

namespace TechFitCustomer.DependencyInjections.Services;

public static class ServicesDependencyInjections
{
    public static void RegisterServicesDependencyInjections(this IServiceCollection services)
    {
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IPreferenceService, PreferenceService>();
    }
}
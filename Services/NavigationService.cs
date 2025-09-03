using System.Reflection;
using Microsoft.Extensions.Logging;

namespace TechFitCustomer.Services;

public class NavigationService : INavigationService
{
    private readonly ILogger<NavigationService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(ILogger<NavigationService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public bool CanGoBack => Shell.Current.Navigation.NavigationStack.Count > 1;

    public PlatformType CurrentPlatform
    {
        get
        {
#if WINDOWS
            return PlatformType.Windows;
#elif MACCATALYST
            return PlatformType.MacCatalyst;
#elif IOS
            return PlatformType.iOS;
#elif ANDROID
            return PlatformType.Android;
#else
            return PlatformType.Unknown;
#endif
        }
    }

    public async Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        try
        {
            if (parameters is not null)
            {
                await Shell.Current.GoToAsync(route, parameters);
            }
            else
            {
                await Shell.Current.GoToAsync(route);
            }

            _logger.LogInformation("Navigated to route: {Route}", route);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error navigating to route: {Route}", route);
            throw;
        }
    }

    public async Task NavigateToAsync<T>(IDictionary<string, object>? parameters = null) where T : Page
    {
        string route = typeof(T).Name;
        await NavigateToAsync(route, parameters);
    }

    public async Task GoBackAsync()
    {
        try
        {
            if (CanGoBack)
            {
                await Shell.Current.GoToAsync("..");
                _logger.LogInformation("Navigated back");
            }
            else
            {
                _logger.LogWarning("Cannot go back - no pages in navigation stack");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error going back");
            throw;
        }
    }

    public async Task GoBackToRootAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("//");
            _logger.LogInformation("Navigated to root");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error going back to root");
            throw;
        }
    }

    public async Task OpenNewWindowAsync(string route, IDictionary<string, object>? parameters = null)
    {
        try
        {
            switch (CurrentPlatform)
            {
                case PlatformType.Windows:
                    await OpenDesktopWindowAsync(route, parameters);
                    break;

                case PlatformType.iOS or PlatformType.MacCatalyst or PlatformType.Android:
                    await PresentModalAsync(route, parameters);
                    break;

                default:
                    await NavigateToAsync(route, parameters);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening new window for route: {Route}", route);
            throw;
        }
    }

    public async Task OpenNewWindowAsync<T>(IDictionary<string, object>? parameters = null) where T : Page
    {
        string route = typeof(T).Name;
        await OpenNewWindowAsync(route, parameters);
    }

    public async Task CloseCurrentWindowAsync()
    {
        try
        {
            switch (CurrentPlatform)
            {
                case PlatformType.Windows:
                    await CloseDesktopWindowAsync();
                    break;

                case PlatformType.iOS or PlatformType.MacCatalyst or PlatformType.Android:
                    if (Shell.Current.Navigation.ModalStack.Count > 0)
                    {
                        await DismissModalAsync();
                    }
                    else
                    {
                        await GoBackAsync();
                    }
                    break;

                default:
                    await GoBackAsync();
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing current window");
            throw;
        }
    }

    public async Task PresentModalAsync(string route, IDictionary<string, object>? parameters = null)
    {
        try
        {
            Page? page = CreatePageFromRoute(route);

            if (page is not null)
            {
                ApplyParametersToPage(page, parameters);
                await Shell.Current.Navigation.PushModalAsync(page);
                _logger.LogInformation("Presented modal: {Route}", route);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error presenting modal: {Route}", route);
            throw;
        }
    }

    public async Task PresentModalAsync<T>(IDictionary<string, object>? parameters = null) where T : Page
    {
        try
        {
            T page = _serviceProvider.GetService<T>() ?? Activator.CreateInstance<T>();
            ApplyParametersToPage(page, parameters);

            await Shell.Current.Navigation.PushModalAsync(page);
            _logger.LogInformation("Presented modal: {PageType}", typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error presenting modal: {PageType}", typeof(T).Name);
            throw;
        }
    }

    public async Task DismissModalAsync()
    {
        try
        {
            if (Shell.Current.Navigation.ModalStack.Count > 0)
            {
                await Shell.Current.Navigation.PopModalAsync();
                _logger.LogInformation("Dismissed modal");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dismissing modal");
            throw;
        }
    }

    private async Task OpenDesktopWindowAsync(string route, IDictionary<string, object>? parameters = null)
    {
#if WINDOWS
        try
        {
            Window newWindow = new();
            Page? page = CreatePageFromRoute(route);

            if (page is not null)
            {
                ApplyParametersToPage(page, parameters);

                newWindow.Page = page;
                Application.Current?.OpenWindow(newWindow);

                _logger.LogInformation("Opened new desktop window: {Route}", route);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error opening desktop window: {Route}, falling back to modal", route);
            await PresentModalAsync(route, parameters);
        }
#else
        await PresentModalAsync(route, parameters);
#endif
    }

    private async Task CloseDesktopWindowAsync()
    {
#if WINDOWS
        try
        {
            List<Window>? windows = Application.Current?.Windows.ToList();
            Window? currentWindow = windows?.LastOrDefault();

            if (currentWindow is not null && windows?.Count > 1)
            {
                Application.Current?.CloseWindow(currentWindow);
                _logger.LogInformation("Closed desktop window");
            }
            else
            {
                await GoBackAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing desktop window");
            await GoBackAsync();
        }
#else
        await GoBackAsync();
#endif
    }

    private Page? CreatePageFromRoute(string route)
    {
        try
        {
            Type? pageType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(route, StringComparison.OrdinalIgnoreCase) &&
                                   t.IsSubclassOf(typeof(Page)));

            if (pageType is not null)
            {
                return _serviceProvider.GetService(pageType) as Page ?? Activator.CreateInstance(pageType) as Page;
            }

            _logger.LogWarning("Could not find page type for route: {Route}", route);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating page from route: {Route}", route);
            return null;
        }
    }

    private void ApplyParametersToPage(Page page, IDictionary<string, object>? parameters)
    {
        if (parameters == null || !parameters.Any()) return;

        try
        {
            object? bindingContext = page.BindingContext;
            if (bindingContext == null) return;

            Type bindingContextType = bindingContext.GetType();

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                PropertyInfo? property = bindingContextType.GetProperty(parameter.Key);
                if (property != null && property.CanWrite)
                {
                    object convertedValue = Convert.ChangeType(parameter.Value, property.PropertyType);
                    property.SetValue(bindingContext, convertedValue);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error applying parameters to page");
        }
    }
}

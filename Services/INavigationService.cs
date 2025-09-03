namespace TechFitCustomer.Services;

public interface INavigationService
{
    Task NavigateToAsync(string route, IDictionary<string, object>? parameters = null);
    Task NavigateToAsync<T>(IDictionary<string, object>? parameters = null) where T : Page;
    Task GoBackAsync();
    Task GoBackToRootAsync();
    Task OpenNewWindowAsync(string route, IDictionary<string, object>? parameters = null);
    Task OpenNewWindowAsync<T>(IDictionary<string, object>? parameters = null) where T : Page;
    Task CloseCurrentWindowAsync();
    Task PresentModalAsync(string route, IDictionary<string, object>? parameters = null);
    Task PresentModalAsync<T>(IDictionary<string, object>? parameters = null) where T : Page;
    Task DismissModalAsync();
    bool CanGoBack { get; }
    PlatformType CurrentPlatform { get; }
}

public enum PlatformType
{
    Windows,
    MacCatalyst,
    iOS,
    Android,
    Unknown
}
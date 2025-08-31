using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TechFitCustomer.DependencyInjections.Pages;

namespace TechFitCustomer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .Services.AddPagesDependencyInjections();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

namespace TechFitCustomer;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Window window = new Window(new AppShell())
        {
            Title = "Customer Manager"
        };

#if WINDOWS
        window.Created += (_, _) =>
        {
            if (window.Handler?.PlatformView is Microsoft.UI.Xaml.Window nativeWindow)
            {
                IntPtr handle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(handle);
                AppWindow? appWindow = AppWindow.GetFromWindowId(windowId);
                
                if (appWindow != null)
                {
                    DisplayArea? displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
                    if (displayArea != null)
                    {
                        RectInt32 workArea = displayArea.WorkArea;
                        appWindow.MoveAndResize(new RectInt32(
                            workArea.X, 
                            workArea.Y, 
                            workArea.Width, 
                            workArea.Height));
                    }
                }
            }
        };
#endif

        return window;
    }
}
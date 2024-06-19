using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace COMP609Task4
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if WINDOWS
            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        Microsoft.UI.WindowId win32WindowsId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                        Microsoft.UI.Windowing.AppWindow winuiAppWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(win32WindowsId);
                        if (winuiAppWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter p)
                        {
                            p.Maximize();
                            //p.IsResizable = false;
                            //p.IsMaximizable = false;
                            //p.IsMinimizable = false;
                        }
                    });
                });
            });
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // register the following services to make them available in the app
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<EditPage>();
            builder.Services.AddTransient<FinancePage>();
            builder.Services.AddTransient<LivestockPage>();
            builder.Services.AddTransient<ForecastPage>();
            return builder.Build();
        }
    }
}

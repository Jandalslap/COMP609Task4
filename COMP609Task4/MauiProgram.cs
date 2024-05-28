using Microsoft.Extensions.Logging;

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

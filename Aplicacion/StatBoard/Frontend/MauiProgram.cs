using Microsoft.Extensions.Logging;

using Frontend.Resources.DataAccess;
using Frontend.Resources.ViewModels;
using Frontend.Pages;

namespace Frontend;

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

        builder.Services.AddDbContext<StatBoard_DbContext>();

        builder.Services.AddTransient<PlayerPage>();
        builder.Services.AddTransient<ActionsPage>();

        builder.Services.AddTransient<Players_ViewModel>();
        builder.Services.AddTransient<Actions_ViewModel>();
        builder.Services.AddTransient<MainViewModel>();

        Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));
        Routing.RegisterRoute(nameof(ActionsPage), typeof(ActionsPage));

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

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

        CreateAllTables();

        builder.Services.AddDbContext<Club_DbContext>();
        builder.Services.AddDbContext<Player_DbContext>();
        builder.Services.AddDbContext<Match_DbContext>();
        builder.Services.AddDbContext<PlayerMatch_DbContext>();
        builder.Services.AddDbContext<PlayerAction_DbContext>();
        builder.Services.AddDbContext<Tournament_DbContext>();

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

    private static void CreateAllTables()
    {
        using (var dbContext = new Club_DbContext())
        {
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        using (var dbContext = new Match_DbContext())
        {
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        using (var dbContext = new PlayerAction_DbContext())
        {
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        using (var dbContext = new PlayerMatch_DbContext())
        {
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        using (var dbContext = new Player_DbContext())
        {
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }

        using (var dbContext = new Tournament_DbContext())
        {
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();
        }
    }
}

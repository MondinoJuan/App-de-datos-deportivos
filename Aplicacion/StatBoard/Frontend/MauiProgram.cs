using Microsoft.Extensions.Logging;

using Frontend.Resources.DataAccess;

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

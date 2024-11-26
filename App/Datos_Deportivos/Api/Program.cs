using Entities.Service;
using Entities.Entities;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpLogging(o => { });
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();
            //Falta configurar de manera correcta        
            app.UseHttpLogging();

            app.UseHttpsRedirection();

            // CRUD Club

            app.MapGet("/clubs/{id}", (int id) =>
            {
                return Club_Services.GetClub(id);
            })
            .WithName("LeerClub");

            app.MapGet("/clubs", () =>
            {
                return Club_Services.GetAllClub();
            })
            .WithName("GetAllClubs");

            app.MapPost("/clubs", (Club club) =>
            {
                try
                {
                    Club_Services.CreateClub(club);
                    return Results.Ok(new { message = "Club creado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CrearClub");

            app.MapPut("/clubs", (Club club) =>
            {
                try
                {
                    Club_Services.UpdateClub(club);
                    return Results.Ok(new { message = "Club actualizado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("ActualizarClub");

            app.MapDelete("/clubs/{id}", (int id) =>
            {
                Club_Services.DeleteClub(id);
            })
            .WithName("EliminarClub");

            // CRUD Match

            app.MapGet("/matchs/{id}", (int id) =>
            {
                return Match_Services.GetMatch(id);
            })
            .WithName("LeerMatch");

            app.MapGet("/matchs", () =>
            {
                return Match_Services.GetAllMatch();
            })
            .WithName("GetAllMatchs");

            app.MapPost("/matchs", (Match match) =>
            {
                try
                {
                    Match_Services.CreateMatch(match);
                    return Results.Ok(new { message = "Match creado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CrearMatch");

            app.MapPut("/matchs", (Match match) =>
            {
                try
                {
                    Match_Services.UpdateMatch(match);
                    return Results.Ok(new { message = "Match actualizado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("ActualizarMatch");

            app.MapDelete("/matchs/{id}", (int id) =>
            {
                Match_Services.DeleteMatch(id);
            })
            .WithName("EliminarMatch");

            // CRUD Player

            app.MapGet("/players/{id}", (int id) =>
            {
                return Player_Services.GetPlayer(id);
            })
            .WithName("LeerPlayer");

            app.MapGet("/players", () =>
            {
                return Player_Services.GetAllPlayer();
            })
            .WithName("GetAllPlayers");

            app.MapPost("/players", (Player player) =>
            {
                try
                {
                    Player_Services.CreatePlayer(player);
                    return Results.Ok(new { message = "Player creado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CrearPlayer");

            app.MapPut("/players", (Player player) =>
            {
                try
                {
                    Player_Services.UpdatePlayer(player);
                    return Results.Ok(new { message = "Player actualizado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("ActualizarPlayer");

            app.MapDelete("/players/{id}", (int id) =>
            {
                Player_Services.DeletePlayer(id);
            })
            .WithName("EliminarPlayer");

            // CRUD PlayerMatch

            app.MapGet("/playersMatchs/{id}", (int id) =>
            {
                return PlayerMatch_Services.GetPlayerMatch(id);
            })
            .WithName("LeerPlayerMatch");

            app.MapGet("/playersMatchs", () =>
            {
                return PlayerMatch_Services.GetAllPlayerMatch();
            })
            .WithName("GetAllPlayersMatchs");

            app.MapPost("/playersMatchs", (PlayerMatch playerMatch) =>
            {
                try
                {
                    PlayerMatch_Services.CreatePlayerMatch(playerMatch);
                    return Results.Ok(new { message = "PlayerMatch creado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CrearPlayerMatch");

            app.MapPut("/playersMatchs", (PlayerMatch playerMatch) =>
            {
                try
                {
                    PlayerMatch_Services.UpdatePlayerMatch(playerMatch);
                    return Results.Ok(new { message = "PlayerMatch actualizado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("ActualizarPlayerMatch");

            app.MapDelete("/playersMatchs/{id}", (int id) =>
            {
                PlayerMatch_Services.DeletePlayerMatch(id);
            })
            .WithName("EliminarPlayerMatch");

            // CRUD PlayerAction

            app.MapGet("/playersActions/{id}", (int id) =>
            {
                return PlayerAction_Services.GetPlayerAction(id);
            })
            .WithName("LeerPlayerAction");

            app.MapGet("/playersActions", () =>
            {
                return PlayerAction_Services.GetAllPlayerActions();
            })
            .WithName("GetAllPlayersActions");

            app.MapPost("/playersActions", (PlayerAction playerAction) =>
            {
                try
                {
                    PlayerAction_Services.CreatePlayerAction(playerAction);
                    return Results.Ok(new { message = "PlayerAction creado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CrearPlayerAction");

            app.MapPut("/playersActions", (PlayerAction playerAction) =>
            {
                try
                {
                    PlayerAction_Services.UpdatePlayerAction(playerAction);
                    return Results.Ok(new { message = "PlayerAction actualizado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("ActualizarPlayerAction");

            app.MapDelete("/playersActions/{id}", (int id) =>
            {
                PlayerAction_Services.DeletePlayerAction(id);
            })
            .WithName("EliminarPlayerAction");

            // CRUD Tournament

            app.MapGet("/tournaments/{id}", (int id) =>
            {
                return Tournament_Services.GetTournament(id);
            })
            .WithName("LeerTournament");

            app.MapGet("/tournaments", () =>
            {
                return Tournament_Services.GetAllTournament();
            })
            .WithName("GetAllTournaments");

            app.MapPost("/tournaments", (Tournament tournament) =>
            {
                try
                {
                    Tournament_Services.CreateTournament(tournament);
                    return Results.Ok(new { message = "Tournament creado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("CrearTournament");

            app.MapPut("/tournaments", (Tournament tournament) =>
            {
                try
                {
                    Tournament_Services.UpdateTournament(tournament);
                    return Results.Ok(new { message = "PlayerAction actualizado exitosamente" });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("ActualizarTournament");

            app.MapDelete("/tournaments/{id}", (int id) =>
            {
                Tournament_Services.DeleteTournament(id);
            })
            .WithName("EliminarTournament");

            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

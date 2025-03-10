using BdD_Android.Servicios;
using BdD_Android.Modelos;

namespace API_Local;

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


        //CRUD CLUB

        app.MapGet("/clubs/{id}", (int id) =>
        {

            return Club_Services.GetOneClubId(id);
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
                Club_Services.AgregarClub(club);
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
                Club_Services.ActualizarClub(club);
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

            Club_Services.EliminarClub(id);
        })
        .WithName("EliminarClub");

        app.MapDelete("/clubs", () =>
        {
            Club_Services.EliminarTodosLosClubes();
            return Results.Ok(new { message = "Todos los clubes han sido eliminados exitosamente" });
        })
        .WithName("EliminarTodosLosClubes");

        // CRUD MATCH
        app.MapGet("/matches/{id}", (int id) =>
        {
            return Match_Services.GetOneMatchId(id);
        })
        .WithName("LeerMatch");

        app.MapGet("/matches", () =>
        {
            return Match_Services.GetAllMatch();
        })
        .WithName("GetAllMatches");

        app.MapPost("/matches", (Match match) =>
        {
            try
            {
                Match_Services.AgregarMatch(match);
                return Results.Ok(new { message = "Match creado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CrearMatch");

        app.MapPut("/matches", (Match match) =>
        {
            try
            {
                Match_Services.ActualizarMatch(match);
                return Results.Ok(new { message = "Match actualizado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ActualizarMatch");

        app.MapDelete("/matches/{id}", (int id) =>
        {
            Match_Services.EliminarMatch(id);
        })
        .WithName("EliminarMatch");

        // CRUD PLAYER
        app.MapGet("/players/{id}", (int id) =>
        {
            return Player_Services.GetOnePlayerId(id);
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
                Player_Services.AgregarPlayer(player);
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
                Player_Services.ActualizarPlayer(player);
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
            Player_Services.EliminarPlayer(id);
        })
        .WithName("EliminarPlayer");

        // CRUD PLAYER ACTION
        app.MapGet("/playeractions/{id}", (int id) =>
        {
            return PlayerAction_Services.GetOnePlayerActionId(id);
        })
        .WithName("LeerPlayerAction");

        app.MapGet("/playeractions", () =>
        {
            return PlayerAction_Services.GetAllPlayerAction();
        })
        .WithName("GetAllPlayerActions");

        app.MapPost("/playeractions", (PlayerAction playerAction) =>
        {
            try
            {
                PlayerAction_Services.AgregarPlayerAction(playerAction);
                return Results.Ok(new { message = "PlayerAction creada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CrearPlayerAction");

        app.MapPut("/playeractions", (PlayerAction playerAction) =>
        {
            try
            {
                PlayerAction_Services.ActualizarPlayerAction(playerAction);
                return Results.Ok(new { message = "PlayerAction actualizada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ActualizarPlayerAction");

        app.MapDelete("/playeractions/{id}", (int id) =>
        {
            PlayerAction_Services.EliminarPlayerAction(id);
        })
        .WithName("EliminarPlayerAction");

        // CRUD PLAYER MATCH
        app.MapGet("/playermatches/{id}", (int id) =>
        {
            return PlayerMatch_Services.GetOnePlayerMatchId(id);
        })
        .WithName("LeerPlayerMatch");

        app.MapGet("/playermatches", () =>
        {
            return PlayerMatch_Services.GetAllPlayerMatch();
        })
        .WithName("GetAllPlayerMatches");

        app.MapPost("/playermatches", (PlayerMatch playerMatch) =>
        {
            try
            {
                PlayerMatch_Services.AgregarPlayerMatch(playerMatch);
                return Results.Ok(new { message = "PlayerMatch creada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CrearPlayerMatch");

        app.MapPut("/playermatches", (PlayerMatch playerMatch) =>
        {
            try
            {
                PlayerMatch_Services.ActualizarPlayerMatch(playerMatch);
                return Results.Ok(new { message = "PlayerMatch actualizada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ActualizarPlayerMatch");

        app.MapDelete("/playermatches/{id}", (int id) =>
        {
            PlayerMatch_Services.EliminarPlayerMatch(id);
        })
        .WithName("EliminarPlayerMatch");

        // CRUD TOURNAMENT
        app.MapGet("/tournaments/{id}", (int id) =>
        {
            return Tournament_Services.GetOneTournamentId(id);
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
                Tournament_Services.AgregarTournament(tournament);
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
                Tournament_Services.ActualizarTournament(tournament);
                return Results.Ok(new { message = "Tournament actualizado exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ActualizarTournament");

        app.MapDelete("/tournaments/{id}", (int id) =>
        {
            Tournament_Services.EliminarTournament(id);
        })
        .WithName("EliminarTournament");


        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using Frontend.Resources.Components;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Frontend;

public partial class MatchView : ContentPage
{
    private Match_Dto Match { get; set; }
    private Club_Dto LocalTeam { get; set; }
    private Club_Dto AwayTeam { get; set; }
    public static ObservableCollection<Player_Dto> TeamLocalPlayers { get; set; }
    public static ObservableCollection<Player_Dto> TeamAwayPlayers { get; set; }

    public MatchView(Match_Dto match)
    {
        InitializeComponent();
        Match = match;

        lblMatchViewTitle.Text = $"Partido de la fecha {Match.MatchWeek} el día {Match.Date.ToString("d")}";
        lblTournament.Text = Match.Tournament ?? "N/A";

        LoadTeams();

        BindingContext = this;
    }

    private void LoadTeams()
    {
        var buscoTeamLocal = Simulo_BdD.GetOneClub(Match.IdTeamLocal);
        Console.WriteLine(buscoTeamLocal.Message);
        if (buscoTeamLocal.Success)
        {
            LocalTeam = buscoTeamLocal.Data;
            lblTeamLocal.Text = LocalTeam.Name;
            TeamLocalPlayers = new ObservableCollection<Player_Dto>(GetAllPlayersOfATeam(LocalTeam.IdPlayers));
        }

        var buscoTeamAway = Simulo_BdD.GetOneClub(Match.IdTeamAway);
        Console.WriteLine(buscoTeamAway.Message);
        if (buscoTeamAway.Success)
        {
            AwayTeam = buscoTeamAway.Data;
            lblTeamAway.Text = AwayTeam.Name;
            TeamAwayPlayers = new ObservableCollection<Player_Dto>(GetAllPlayersOfATeam(AwayTeam.IdPlayers));
        }
    }

    private List<Player_Dto> GetAllPlayersOfATeam(List<Guid> playerIds)
    {
        return Simulo_BdD.Database.Players.Where(p => playerIds.Contains(p.Id)).ToList();
    }

    private async void OnAddPlayer(object sender, EventArgs e)
    {
        var modalPage = new CreateModify_PlayerModal();
        await Navigation.PushModalAsync(modalPage);
        var result = await modalPage.GetResultAsync();
        var newPlayer = await modalPage.GetPlayerAsync();

        if (result == 1)
        {
            LocalTeam.IdPlayers.Add(newPlayer.Id);
            TeamLocalPlayers.Add(newPlayer);
            CreatePlayerMatch(newPlayer.Id, Match.Id);
            Simulo_BdD.ReplaceClub(LocalTeam);
        }
        else if (result == 2)
        {
            AwayTeam.IdPlayers.Add(newPlayer.Id);
            TeamAwayPlayers.Add(newPlayer);
            CreatePlayerMatch(newPlayer.Id, Match.Id);

            Simulo_BdD.ReplaceClub(AwayTeam);
        }
        else if (result == 0)
        {
            Console.WriteLine("Se canceló la carga de un jugador");
        }
    }

    private void CreatePlayerMatch(Guid idPlayer, Guid idMatch)
    {
        var newPlayerMatch = new PlayerMatch_Dto
        {
            Id = Guid.NewGuid(),
            IdPlayer = idPlayer,
            IdMatch = idMatch
        };

        var result = Simulo_BdD.AddPlayerMatch(newPlayerMatch);
        Console.WriteLine(result.Message);
    }

    public static void RemovePlayer(Player_Dto player)
    {
        if (TeamLocalPlayers.Remove(player))
        {
            // Remove player from LocalTeam
            var localTeam = Simulo_BdD.GetOneClub(player.Id).Data;
            localTeam.IdPlayers.Remove(player.Id);
            Simulo_BdD.ReplaceClub(localTeam);
        }
        else if (TeamAwayPlayers.Remove(player))
        {
            // Remove player from AwayTeam
            var awayTeam = Simulo_BdD.GetOneClub(player.Id).Data;
            awayTeam.IdPlayers.Remove(player.Id);
            Simulo_BdD.ReplaceClub(awayTeam);
        }
    }

    private void OnFinish(object sender, EventArgs e)
    {
        // Finalizar el partido, debería armar el PDF en esta función.
    }
}
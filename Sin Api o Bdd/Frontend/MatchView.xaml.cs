using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using Frontend.Resources.Components;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Frontend.Resources;
using System;

namespace Frontend;

public partial class MatchView : ContentPage
{
    private Match_Dto Match { get; set; }
    private Club_Dto LocalTeam { get; set; }
    private Club_Dto AwayTeam { get; set; }
    public ObservableCollection<Player_Dto> TeamLocalPlayers { get; set; } = new ObservableCollection<Player_Dto>();
    public ObservableCollection<Player_Dto> TeamAwayPlayers { get; set; } = new ObservableCollection<Player_Dto>();
    public int ActionCountLocal { get; set; }
    public int ActionCountAway { get; set; }

    public MatchView(Match_Dto match)
    {
        InitializeComponent();
        Match = match;

        lblMatchViewTitle.Text = $"Partido de la fecha {Match.MatchWeek} el día {Match.Date.ToString()}";
        lblTournament.Text = Match.Tournament ?? "N/A";

        LoadTeams();

        BindingContext = this;
    }

    public MatchView()
    {
        InitializeComponent();
        var result = Simulo_BdD.GetAllMatches();
        Console.WriteLine(result.Message);

        Match = result.Data.First();

        lblMatchViewTitle.Text = $"Partido de la fecha {Match.MatchWeek} el día {Match.Date.ToString()}";
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
            if (LocalTeam.IdPlayers != null)
            {
                TeamLocalPlayers.Clear();
                foreach (var player in GetAllPlayersOfATeam(LocalTeam.IdPlayers))
                {
                    TeamLocalPlayers.Add(player);
                }
            }
        }

        var buscoTeamAway = Simulo_BdD.GetOneClub(Match.IdTeamAway);
        Console.WriteLine(buscoTeamAway.Message);
        if (buscoTeamAway.Success)
        {
            AwayTeam = buscoTeamAway.Data;
            lblTeamAway.Text = AwayTeam.Name;
            if (AwayTeam.IdPlayers != null)
            {
                TeamAwayPlayers.Clear();
                foreach (var player in GetAllPlayersOfATeam(AwayTeam.IdPlayers))
                {
                    TeamAwayPlayers.Add(player);
                }
            }
        }
    }

    private static List<Player_Dto> GetAllPlayersOfATeam(List<Guid> playerIds)
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
            if (LocalTeam.IdPlayers == null)
            {
                LocalTeam.IdPlayers = new List<Guid>();
            }
            LocalTeam.IdPlayers.Add(newPlayer.Id);
            TeamLocalPlayers.Add(newPlayer);
            CreatePlayerMatch(newPlayer.Id, Match.Id);
            Simulo_BdD.ReplaceClub(LocalTeam);
        }
        else if (result == 2)
        {
            if (AwayTeam.IdPlayers == null)
            {
                AwayTeam.IdPlayers = new List<Guid>();
            }
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

    public void RemovePlayer(Player_Dto player)
    {
        if (TeamLocalPlayers.Remove(player))
        {
            var localTeamResult = Simulo_BdD.GetOneClub(player.Id);
            if (localTeamResult.Success && localTeamResult.Data != null)
            {
                var localTeam = localTeamResult.Data;
                localTeam.IdPlayers.Remove(player.Id);
                Simulo_BdD.ReplaceClub(localTeam);
            }
        }
        else if (TeamAwayPlayers.Remove(player))
        {
            var awayTeamResult = Simulo_BdD.GetOneClub(player.Id);
            if (awayTeamResult.Success && awayTeamResult.Data != null)
            {
                var awayTeam = awayTeamResult.Data;
                awayTeam.IdPlayers.Remove(player.Id);
                Simulo_BdD.ReplaceClub(awayTeam);
            }
        }
    }

    private void OnLocalPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedAction = picker.SelectedItem.ToString();
            if (Enum.TryParse(selectedAction, out Ending actionValue))
            {
                CalculateActionsQuantity(actionValue, true);
            }
        }
    }

    private void OnAwayPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedAction = picker.SelectedItem.ToString();
            if (Enum.TryParse(selectedAction, out Ending actionValue))
            {
                CalculateActionsQuantity(actionValue, false);
            }
        }
    }

    private void CalculateActionsQuantity(Ending actionType, bool local)
    {
        var result = Simulo_BdD.GetAllPlayerMatches();
        Console.WriteLine(result.Message);

        if (result.Success)
        {
            var players = local ? TeamLocalPlayers : TeamAwayPlayers;

            foreach (var player in players)
            {
                var playerMatch = result.Data.FirstOrDefault(a => a.IdPlayer == player.Id);
                if (playerMatch == null) continue;

                int count = playerMatch.IdActions
                    .Select(idAction => Simulo_BdD.GetOneAction(idAction))
                    .Where(result1 => result1.Success)
                    .Count(result1 => result1.Data.Ending == actionType);

                if (local)
                {
                    ActionCountLocal = count;
                }
                else
                {
                    ActionCountAway = count;
                }
            }
        }
    }

    private void OnFinish(object sender, EventArgs e)
    {
        // Finalizar el partido, debería armar el PDF en esta función.
    }
}

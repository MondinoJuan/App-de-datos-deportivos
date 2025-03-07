using Frontend.Resources.Entities;
using System.Collections.ObjectModel;
using Frontend.Resources;
using Frontend.Resources.PDF_Pages;
using System.Globalization;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Pages;

public partial class MatchView : ContentPage, INotifyPropertyChanged
{
    private Match_Dto Match { get; set; }
    private Club_Dto LocalTeam { get; set; } = new();
    private Club_Dto AwayTeam { get; set; } = new();
    public ObservableCollection<Player_Dto> TeamLocalPlayers { get; set; } = new ObservableCollection<Player_Dto>();
    public ObservableCollection<Player_Dto> TeamAwayPlayers { get; set; } = new ObservableCollection<Player_Dto>();

    private string _localActionSelected = string.Empty;
    public string LocalActionSelected
    {
        get => _localActionSelected;
        set
        {
            _localActionSelected = value;
            OnPropertyChanged(nameof(LocalActionSelected));
        }
    }
    private string _awayActionSelected = string.Empty;
    public string AwayActionSelected
    {
        get => _awayActionSelected;
        set
        {
            _awayActionSelected = value;
            OnPropertyChanged(nameof(AwayActionSelected));
        }
    }

    public MatchView(Match_Dto match)
    {
        InitializeComponent();
        Match = match;

        lblMatchViewTitle.Text = $"Partido de la fecha {Match.MatchWeek} el d�a {Match.Date.ToString("d")}";
        lblTournament.Text = Match.Tournament ?? "N/A";
        lblScoreL.Text = Match.GoalsTeamA.ToString();
        lblScoreA.Text = Match.GoalsTeamB.ToString();

        LoadTeams();

        BindingContext = this;
    }

    public MatchView()
    {
        InitializeComponent();
        var result = Simulo_BdD.GetAllMatches();
        Console.WriteLine(result.Message);

        Match = result.Data.First();

        lblMatchViewTitle.Text = $"Partido de la fecha {Match.MatchWeek} el d�a {Match.Date.ToString()}";
        lblTournament.Text = Match.Tournament ?? "N/A";
        lblScoreL.Text = Match.GoalsTeamA.ToString();
        lblScoreA.Text = Match.GoalsTeamB.ToString();

        LoadTeams();

        BindingContext = this;
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdateScore()
    {
        // Reiniciar los contadores de goles
        Match.GoalsTeamA = 0;
        Match.GoalsTeamB = 0;

        var result = Simulo_BdD.GetAllPlayerMatches();
        if (!result.Success) return;

        var playerMatches = result.Data.Where(pm => pm.IdMatch == Match.Id).ToList();

        foreach (var playerMatch in playerMatches)
        {
            // Buscar todas las acciones del jugador que sean Ending.Goal
            if (playerMatch.IdActions == null)
                return;

            foreach (var idAction in playerMatch.IdActions)
            {
                var action = Simulo_BdD.GetOneAction(idAction);
                if (action.Success && action.Data.Ending == Ending.Goal)
                {
                    if (LocalTeam.IdPlayers.Contains(playerMatch.IdPlayer))
                    {
                        Match.GoalsTeamA++;
                    }
                    else if (AwayTeam.IdPlayers.Contains(playerMatch.IdPlayer))
                    {
                        Match.GoalsTeamB++;
                    }
                }
            }
        }

        // Actualizar la interfaz de usuario
        lblScoreL.Text = Match.GoalsTeamA.ToString();
        lblScoreA.Text = Match.GoalsTeamB.ToString();
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
        UpdateScore();
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
            Console.WriteLine("Se cancel� la carga de un jugador");
        }

        UpdateScore();
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
            var localTeamResult = Simulo_BdD.GetOneClub(LocalTeam.Id);
            if (localTeamResult.Success && localTeamResult.Data != null)
            {
                var localTeam = localTeamResult.Data;
                localTeam.IdPlayers.Remove(player.Id);
                Simulo_BdD.ReplaceClub(localTeam);
            }
        }
        else if (TeamAwayPlayers.Remove(player))
        {
            var awayTeamResult = Simulo_BdD.GetOneClub(AwayTeam.Id);
            if (awayTeamResult.Success && awayTeamResult.Data != null)
            {
                var awayTeam = awayTeamResult.Data;
                awayTeam.IdPlayers.Remove(player.Id);
                Simulo_BdD.ReplaceClub(awayTeam);
            }
        }
        UpdateScore();
    }

    private void OnLocalPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedAction = "";
            switch (picker.SelectedItem.ToString())
            {
                case "Gol":
                    selectedAction = "Goal";
                    break;
                case "Foul":
                    selectedAction = "Foul";
                    break;
                case "Atajada":
                    selectedAction = "Save";
                    break;
                case "Errada":
                    selectedAction = "Miss";
                    break;
                case "Perdida":
                    selectedAction = "Steal_L";
                    break;
                case "Robo":
                    selectedAction = "Steal_W";
                    break;
                case "Bloqueo":
                    selectedAction = "Blocked";
                    break;
                default:
                    break;
            }
            if (Enum.TryParse(selectedAction, out Ending actionValue))
            {
                LocalActionSelected = selectedAction;
                RefreshView();
            }
        }
    }

    private void OnAwayPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedAction = "";
            switch (picker.SelectedItem.ToString())
            {
                case "Gol": selectedAction = "Goal"; break;
                case "Foul": selectedAction = "Foul"; break;
                case "Atajada": selectedAction = "Save"; break;
                case "Errada": selectedAction = "Miss"; break;
                case "Perdida": selectedAction = "Steal_L"; break;
                case "Robo": selectedAction = "Steal_W"; break;
                case "Bloqueo": selectedAction = "Blocked"; break;
            }

            if (Enum.TryParse(selectedAction, out Ending actionValue))
            {
                AwayActionSelected = selectedAction;

                // Forzar actualizaci�n de los Bindings para los jugadores visitantes
                foreach (var player in TeamAwayPlayers)
                {
                    // Notificar que la propiedad "Id" ha cambiado (aunque no sea verdad)
                    // Esto disparar� la reevaluaci�n del Converter
                    player.OnPropertyChanged(nameof(player.Id));
                }
            }
        }
    }

    private void OnCancel(object sender, EventArgs e)
    {
        Simulo_BdD.CleanClubList();
        Simulo_BdD.CleanPlayerList();
        Simulo_BdD.CleanPlayerMatchList();
        Simulo_BdD.CleanMatchList();
        Simulo_BdD.CleanPlayerActionList();
        Simulo_BdD.CleanTournamentList();

        Application.Current.Quit();
    }

    private async void OnFinish(object sender, EventArgs e)
    {
        var result = Simulo_BdD.ReplaceMatch(Match);
        if (!result.Success)
        {
            Console.WriteLine(result.Message);
            return;
        }
        var result1 = Simulo_BdD.ReplaceClub(LocalTeam);
        if (!result1.Success)
        {
            Console.WriteLine(result1.Message);
            return;
        }
        var result2 = Simulo_BdD.ReplaceClub(AwayTeam);
        if (!result2.Success)
        {
            Console.WriteLine(result2.Message);
            return;
        }

        // Finalizar el partido, deber�a armar el PDF en esta funci�n.
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            var pdf = new CrearPDF_Android();
            _ = await pdf.CrearPDF_A(Match.Id);
        }
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            var pdf = new CrearPDF();
            pdf.CrearPDF_PC(Match.Id);
        }

        Simulo_BdD.CleanClubList();
        Simulo_BdD.CleanPlayerList();
        Simulo_BdD.CleanPlayerMatchList();
        Simulo_BdD.CleanMatchList();
        Simulo_BdD.CleanPlayerActionList();
        Simulo_BdD.CleanTournamentList();

        Application.Current.Quit();
    }

    private void GoSummary(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ShowMiddleGame(Match.Id));
    }

    private void RefreshView()
    {
        // Actualizar el BindingContext
        BindingContext = this;
    }
}

using Frontend.Resources.DTOs;
using System.Collections.ObjectModel;
using Frontend.Resources;
using Frontend.Resources.Modelos;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Resources.Components;

public partial class MatchSummary : ContentView, INotifyPropertyChanged
{
    public static readonly BindableProperty MatchProperty = BindableProperty.Create(
        nameof(Match),
        typeof(Match),
        typeof(MatchSummary),
        default(Match),
        propertyChanged: OnMatchChanged
    );
    public Match Match
    {
        get => (Match)GetValue(MatchProperty);
        set => SetValue(MatchProperty, value);
    }
    private Club LocalTeam { get; set; } = new();
    private Club AwayTeam { get; set; } = new();
    public ObservableCollection<Player> TeamLocalPlayers { get; set; } = new ObservableCollection<Player>();
    public ObservableCollection<Player> TeamAwayPlayers { get; set; } = new ObservableCollection<Player>();

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

    public MatchSummary()
    {
        InitializeComponent();
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

        var result = Services.GetPlayerMatches();
        if (result == null) return;

        var playerMatches = result.Where(pm => pm.IdMatch == Match.Id).ToList();

        foreach (var playerMatch in playerMatches)
        {
            // Buscar todas las acciones del jugador que sean Ending.Goal
            if (playerMatch.IdActions == null)
                return;

            foreach (var idAction in playerMatch.IdActions)
            {
                var action = Services.GetPlayerAction(idAction);
                if (action != null && action.Ending == Ending.Goal)
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
        var buscoTeamLocal = Services.GetClub(Match.IdTeamLocal);
        if (buscoTeamLocal != null)
        {
            LocalTeam = buscoTeamLocal;
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

        var buscoTeamAway = Services.GetClub(Match.IdTeamAway);
        if (buscoTeamAway != null)
        {
            AwayTeam = buscoTeamAway;
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

    private static List<Player> GetAllPlayersOfATeam(List<int> playerIds)
    {
        return playerIds.Select(id => Services.GetPlayer(id)).Where(player => player != null).ToList();
    }

    private void CreatePlayerMatch(int idPlayer, int idMatch)
    {
        var newPlayerMatch = new PlayerMatch_Dto
        {
            IdPlayer = idPlayer,
            IdMatch = idMatch,
            IdActions = new List<int>()
        };

        Services.AddPlayerMatch(newPlayerMatch);
    }

    public void RemovePlayer(Player player)
    {
        if (TeamLocalPlayers.Remove(player))
        {
            var localTeamResult = Services.GetClub(LocalTeam.Id);
            if (localTeamResult != null)
            {
                var localTeam = localTeamResult;
                localTeam.IdPlayers.Remove(player.Id);
                Services.UpdateClub(localTeam);
            }
        }
        else if (TeamAwayPlayers.Remove(player))
        {
            var awayTeamResult = Services.GetClub(AwayTeam.Id);
            if (awayTeamResult != null)
            {
                var awayTeam = awayTeamResult;
                awayTeam.IdPlayers.Remove(player.Id);
                Services.UpdateClub(awayTeam);
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

                // Forzar actualización de los Bindings para los jugadores visitantes
                foreach (var player in TeamAwayPlayers)
                {
                    // Notificar que la propiedad "Id" ha cambiado (aunque no sea verdad)
                    // Esto disparará la reevaluación del Converter
                    player.OnPropertyChanged(nameof(player.Id));
                }
            }
        }
    }

    private static void OnMatchChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MatchSummary matchSummary && newValue is Match newMatch)
        {
            matchSummary.lblMatchSummaryTitle.Text = $"Partido de la fecha {newMatch.MatchWeek} el día {newMatch.Date:dd/MM/yyyy}";
            matchSummary.lblTournament.Text = newMatch.Tournament ?? "N/A";
            matchSummary.lblScoreL.Text = newMatch.GoalsTeamA.ToString();
            matchSummary.lblScoreA.Text = newMatch.GoalsTeamB.ToString();

            matchSummary.LoadTeams();
        }
    }
}
using Frontend.Resources.Components;
using Frontend.Resources.DTOs;
using Frontend.Resources;
using Frontend.Resources.Modelos;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Pages;

public partial class ShowMiddleGame : ContentPage, INotifyPropertyChanged
{
    private Match match = new();
    private Club teamLocal = new();
    private Club teamAway = new();
    private List<int> teamsIds = new();
    private List<Club> teams = new();

    public List<Club> Teams
    {
        get => teams;
        set
        {
            if (teams != value)
            {
                teams = value;
                OnPropertyChanged();
            }
        }
    }

    public List<int> TeamsIds
    {
        get => teamsIds;
        set
        {
            if (teamsIds != value)
            {
                teamsIds = value;
                OnPropertyChanged();
            }
        }
    }

    public Match Match
    {
        get => match;
        set
        {
            if (match != value)
            {
                match = value;
                OnPropertyChanged();
            }
        }
    }

    public Club TeamLocal
    {
        get => teamLocal;
        set
        {
            if (teamLocal != value)
            {
                teamLocal = value;
                OnPropertyChanged();
            }
        }
    }

    public Club TeamAway
    {
        get => teamAway;
        set
        {
            if (teamAway != value)
            {
                teamAway = value;
                OnPropertyChanged();
            }
        }
    }

    public ShowMiddleGame(int idMatch)
    {
        InitializeComponent();
        BindingContext = this;
        LoadData(idMatch);
    }

    private void LoadData(int idMatch)
    {
        var result = Services.GetMatch(idMatch);
        if (result != null)
        {
            Match = result;

            var result1 = Services.GetClub(Match.IdTeamLocal);
            if (result1 != null)
            {
                TeamLocal = result1;
                OnPropertyChanged(nameof(TeamLocal));
            }

            var result2 = Services.GetClub(Match.IdTeamAway);
            if (result2 != null)
            {
                TeamAway = result2;
                OnPropertyChanged(nameof(TeamAway));
            }

            Teams = new List<Club> { TeamLocal, TeamAway };
            TeamsIds = new List<int> { TeamLocal.Id, TeamAway.Id };

            OnPropertyChanged(nameof(Teams));
            OnPropertyChanged(nameof(TeamsIds));
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void OnGoBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

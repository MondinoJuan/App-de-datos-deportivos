using Frontend.Resources.Components;
using Frontend.Resources.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Pages;

public partial class ShowMiddleGame : ContentPage, INotifyPropertyChanged
{
    private Match_Dto match = new();
    private Club_Dto teamLocal = new();
    private Club_Dto teamAway = new();
    private List<Guid> teamsIds = new();
    private List<Club_Dto> teams = new();

    public List<Club_Dto> Teams
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

    public List<Guid> TeamsIds
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

    public Match_Dto Match
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

    public Club_Dto TeamLocal
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

    public Club_Dto TeamAway
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

    public ShowMiddleGame(Guid idMatch)
    {
        InitializeComponent();
        BindingContext = this;
        LoadData(idMatch);
    }

    private void LoadData(Guid idMatch)
    {
        var result = Simulo_BdD.GetOneMatch(idMatch);
        if (result.Success && result.Data != null)
        {
            Match = result.Data;

            var result1 = Simulo_BdD.GetOneClub(Match.IdTeamLocal);
            if (result1.Success && result1.Data != null)
            {
                TeamLocal = result1.Data;
                OnPropertyChanged(nameof(TeamLocal));
            }

            var result2 = Simulo_BdD.GetOneClub(Match.IdTeamAway);
            if (result2.Success && result2.Data != null)
            {
                TeamAway = result2.Data;
                OnPropertyChanged(nameof(TeamAway));
            }

            Teams = new List<Club_Dto> { TeamLocal, TeamAway };
            TeamsIds = new List<Guid> { TeamLocal.Id, TeamAway.Id };

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

using Frontend.Resources.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Pages;

public partial class ShowMiddleGame : ContentPage
{
    public ShowMiddleGame(Guid idMatch)
    {
        InitializeComponent();
        BindingContext = new ShowMiddleGame_ViewModel(idMatch);
    }

    private async void OnGoBack(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

public class ShowMiddleGame_ViewModel : INotifyPropertyChanged
{
    private Match_Dto match;
    private Club_Dto teamLocal;
    private Club_Dto teamAway;

    public Match_Dto Match
    {
        get => match;
        set { match = value; OnPropertyChanged(); }
    }

    public Club_Dto TeamLocal
    {
        get => teamLocal;
        set { teamLocal = value; OnPropertyChanged(); }
    }

    public Club_Dto TeamAway
    {
        get => teamAway;
        set { teamAway = value; OnPropertyChanged(); }
    }

    public ShowMiddleGame_ViewModel(Guid idMatch)
    {
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
                TeamLocal = result1.Data;

            var result2 = Simulo_BdD.GetOneClub(Match.IdTeamAway);
            if (result2.Success && result2.Data != null)
                TeamAway = result2.Data;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using Microsoft.Maui.Controls;
using Frontend.Pages;

namespace Frontend;
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnNewMatch(object sender, EventArgs e)
    {
        API_Calls.CleanClubList();
        API_Calls.CleanPlayerList();
        API_Calls.CleanPlayerMatchList();
        API_Calls.CleanMatchList();
        API_Calls.CleanPlayerActionList();
        API_Calls.CleanTournamentList();

        Navigation.PushAsync(new CreateMatchPage());
    }
}

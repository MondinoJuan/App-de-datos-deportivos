using Frontend.Resources.Entities;
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
        Simulo_BdD.CleanClubList();
        Simulo_BdD.CleanPlayerList();
        Simulo_BdD.CleanPlayerMatchList();
        Simulo_BdD.CleanMatchList();
        Simulo_BdD.CleanPlayerActionList();
        Simulo_BdD.CleanTournamentList();

        Navigation.PushAsync(new CreateMatchPage());
    }
}

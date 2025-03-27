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
        Navigation.PushAsync(new CreateMatchPage());
    }
}

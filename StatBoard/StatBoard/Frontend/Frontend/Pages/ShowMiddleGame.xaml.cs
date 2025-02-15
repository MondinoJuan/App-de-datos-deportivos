using Frontend.Resources.Entities;

namespace Frontend.Pages;

public partial class ShowMiddleGame : ContentPage
{
	public Club_Dto TeamLocal { get; set; }
	public Club_Dto TeamAway { get; set; }

    public ShowMiddleGame()
	{
		InitializeComponent();
	}
}
using Frontend.Resources.Entities;

namespace Frontend.Resources.Components;

public partial class PageOfActions : ContentView
{
	public PageOfActions(Player_Dto player)
	{
		InitializeComponent();
	}

    public PageOfActions(Club_Dto team)
    {
        InitializeComponent();
    }
}
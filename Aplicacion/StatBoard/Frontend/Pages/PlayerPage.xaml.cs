using Frontend.Resources.ViewModels;

namespace Frontend.Pages;

public partial class PlayerPage : ContentPage
{
	public PlayerPage(Players_ViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
using Frontend.Resources.ViewModels;

namespace Frontend.Pages;

public partial class ActionsPage : ContentPage
{
	public ActionsPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
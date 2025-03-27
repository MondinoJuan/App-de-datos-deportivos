using Frontend.Resources.PDF_Pages;
using Frontend.Pages;
using Frontend.Resources;

namespace Frontend.Resources.Components;

public partial class MenuHamburguesa : ContentView
{
    public static readonly BindableProperty Actions_HUBProperty =
        BindableProperty.Create(nameof(Actions_HUB), typeof(ActionsHUB), typeof(MenuHamburguesa), false);

    public ActionsHUB Actions_HUB
    {
        get => (ActionsHUB)GetValue(Actions_HUBProperty);
        set => SetValue(Actions_HUBProperty, value);
    }

    public MenuHamburguesa()
	{
		InitializeComponent();

        // Agrega el gesto de tap para abrir/cerrar el menú
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnHamburgerClicked;
        HamburgerIcon.GestureRecognizers.Add(tapGesture);
    }

    // Muestra o esconde el menú
    private void OnHamburgerClicked(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = !MenuOptions.IsVisible;
    }

    private void OnGoModifyPlayer(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = false;
        //await Application.Current.MainPage.Navigation.PushAsync(new CreateModify_Player(IdPlayer, IsLocal));
        Actions_HUB.GoModifyPlayer();
    }

    private async void OnDeletePlayer(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = false;
        //await Application.Current.MainPage.Navigation.PushAsync(new DeletePlayer());
        await Actions_HUB.DeletePlayer();
    }

    // Lógica al presionar "Finalizar partido"
    private void OnFinalizarPartido(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = false;

        Actions_HUB.CreoPDF();
    }

    // Lógica al presionar "Salir sin guardar"
    private void OnSalirSinGuardar(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = false;
        Actions_HUB.SalirSinGuardar();
    }

}
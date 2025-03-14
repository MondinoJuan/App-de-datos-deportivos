using Frontend.Resources.PDF_Pages;

namespace Frontend.Resources.Components;

public partial class MenuHamburguesa : ContentView
{
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

    // Lógica al presionar "Finalizar partido"
    private void OnFinalizarPartido(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = false;

        var creoPDF = CrearPDF_Android();
        if (creoPDF)
        {
            Application.Current.MainPage.DisplayAlert("Partido finalizado", "Has finalizado el partido", "OK");
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear el PDF", "OK");
        }

    }

    // Lógica al presionar "Salir sin guardar"
    private void OnSalirSinGuardar(object sender, EventArgs e)
    {
        MenuOptions.IsVisible = false;
        Application.Current.MainPage.DisplayAlert("Salir sin guardar", "Has salido sin guardar los datos", "OK");
    }
}
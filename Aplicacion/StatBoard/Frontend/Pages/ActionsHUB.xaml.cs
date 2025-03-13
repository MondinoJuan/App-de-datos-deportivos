using Frontend.Resources;
using Frontend.Resources.Modelos;

namespace Frontend.Pages;

public partial class ActionsHUB : ContentPage
{
    private bool IsPlayerLocal { get; set; }
    private Player PlayerSeleccionado { get; set; }

    public ActionsHUB()
	{
		InitializeComponent();
    }



    // Verifico el estado del toggle, lo tengo que utilizar a la hora de guardar.
    //	if (MyCustomToggle.IsToggled)
    //    {
    //        DisplayAlert("Estado", "El toggle está activado", "OK");
    //	  }
    //    else
    //    {
    //        DisplayAlert("Estado", "El toggle está desactivado", "OK");
    //    }

    private void OnPlayerAwayTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Player player)
        {
            IsPlayerLocal = false;
            PlayerSeleccionado = player;

            //DisplayAlert("Jugador seleccionado", $"Seleccionaste a {player.Name}", "OK");
        }
    }

    private void OnPlayerLocalTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Player player)
        {
            IsPlayerLocal = true;
            PlayerSeleccionado = player;

            //DisplayAlert("Jugador seleccionado", $"Seleccionaste a {player.Name}", "OK");
        }
    }


    private void AgregarAccion()
    {
        var color = new Color();
        // Restaurar el color del último Label si ya hay alguno
        if (AccionesContainer.Children.Count > 0)
        {
            var ultimoLabel = AccionesContainer.Children.Last() as Label;
            if (ultimoLabel != null)
            {
                ultimoLabel.TextColor = Colors.Black;
            }
        }

        if (IsPlayerLocal)
        {
            color = Colors.Orange;
        }
        else
        {
            color = Colors.Blue;
        }

            // Crear el nuevo Label con el color resaltado
            var nuevaAccion = new Label
            {
                Text = $"{EndingSeleccionado} - {PlayerSeleccionado.Number} {PlayerSeleccionado.Name}",
                FontSize = 16,
                TextColor = color,
                HorizontalOptions = LayoutOptions.Start
            };

        // Agregar el nuevo Label a la lista
        AccionesContainer.Children.Add(nuevaAccion);
    }
}
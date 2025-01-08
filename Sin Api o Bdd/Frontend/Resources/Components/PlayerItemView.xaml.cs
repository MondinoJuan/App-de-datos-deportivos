using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;

namespace Frontend.Resources.Components;

public partial class PlayerItemView : ContentView
{
    public Player_Dto? Player => BindingContext as Player_Dto;

    public PlayerItemView()
    {
        InitializeComponent();
        // Escuchar cambios en el BindingContext
        this.BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        // Cuando cambie el BindingContext, actualiza cualquier lógica necesaria
        if (Player != null)
        {
            // Ejemplo: Mostrar información del jugador en consola (puede ser removido en producción)
            Console.WriteLine($"Player: {Player.Name}, Number: {Player.Number}");
        }
    }

    private async void OnTapped(object sender, EventArgs e)
    {
        if (Player != null)
        {
            await Navigation.PushAsync(new ActionCreate(Player.Id));
        }
    }

    private async void OnEdit(object sender, EventArgs e)
    {
        if (Player != null)
        {
            await Navigation.PushAsync(new CreateModify_PlayerModal(Player));
        }
        // Detener la propagación del evento Tapped
        //((Button)sender).Command = new Command(() => { });
    }

    private void OnDelete(object sender, EventArgs e)
    {
        if (Player != null)
        {
            Simulo_BdD.RemovePlayer(Player.Id);
            MatchView.RemovePlayer(Player);
        }
    }
}



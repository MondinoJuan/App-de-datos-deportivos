using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using System;

namespace Frontend.Resources.Components;

public partial class PlayerItemView : ContentView
{
    public static readonly BindableProperty MatchViewProperty =
        BindableProperty.Create(nameof(MatchView), typeof(MatchView), typeof(PlayerItemView));

    public MatchView MatchView
    {
        get => (MatchView)GetValue(MatchViewProperty);
        set => SetValue(MatchViewProperty, value);
    }

    public static readonly BindableProperty PlayerProperty =
        BindableProperty.Create(nameof(Player), typeof(Player_Dto), typeof(PlayerItemView));

    public Player_Dto Player
    {
        get => (Player_Dto)GetValue(PlayerProperty);
        set => SetValue(PlayerProperty, value);
    }

    public PlayerItemView()
    {
        InitializeComponent();
        this.BindingContext = this;
        // Escuchar cambios en el BindingContext
        this.BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object? sender, EventArgs? e)
    {
        // Cuando cambie el BindingContext, actualiza cualquier lógica necesaria
        if (Player != null)
        {
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
    }

    private void OnDelete(object sender, EventArgs e)
    {
        if (Player != null)
        {
            try
            {
                var result = Simulo_BdD.GetAllPlayerMatches();
                Console.WriteLine(result.Message);

                if (result.Success)
                {
                    var playerMatches = result.Data.Where(pm => pm.IdPlayer == Player.Id).ToList();

                    foreach (var pm in playerMatches)
                    {
                        RemovePlayerActions(pm);
                        Simulo_BdD.RemovePlayerMatch(pm.Id);
                    }
                }

                Simulo_BdD.RemovePlayer(Player.Id);
                MatchView.RemovePlayer(Player);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el jugador: {ex.Message}");
            }
        }
    }

    private static void RemovePlayerActions(PlayerMatch_Dto playerMatch)
    {
        if (playerMatch.IdActions != null && playerMatch.IdActions.Count != 0)
        {
            foreach (var actionId in playerMatch.IdActions)
            {
                Simulo_BdD.RemoveAction(actionId);
            }
            playerMatch.IdActions.Clear();
        }
    }
}

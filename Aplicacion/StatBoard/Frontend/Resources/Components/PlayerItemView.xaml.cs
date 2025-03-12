using Microsoft.Maui.Controls;
using Frontend.Resources.DTOs;
using Frontend.Resources.Modelos;
using System;
using Frontend.Pages;

namespace Frontend.Resources.Components;

public partial class PlayerItemView : ContentView
{
    public static readonly BindableProperty MatchViewProperty =
        BindableProperty.Create(nameof(MatchSummary), typeof(MatchSummary), typeof(PlayerItemView));

    public MatchSummary MatchSummary
    {
        get => (MatchSummary)GetValue(MatchViewProperty);
        set => SetValue(MatchViewProperty, value);
    }

    public static readonly BindableProperty PlayerProperty =
        BindableProperty.Create(nameof(Player), typeof(Player), typeof(PlayerItemView));

    public Player Player
    {
        get => (Player)GetValue(PlayerProperty);
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
            await Navigation.PushAsync(new CreateModify_Player(Player));
        }
    }

    private void OnDelete(object sender, EventArgs e)
    {
        if (Player != null)
        {
            try
            {
                var result = Services.GetPlayerMatches();

                if (result != null)
                {
                    var playerMatches = result.Where(pm => pm.IdPlayer == Player.Id).ToList();

                    foreach (var pm in playerMatches)
                    {
                        RemovePlayerActions(pm);
                        Services.DeletePlayerMatch(pm.Id);
                    }
                }

                Services.DeletePlayer(Player.Id);
                MatchSummary.RemovePlayer(Player);
                MatchSummary.UpdateScore();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el jugador: {ex.Message}");
            }
        }
    }

    private static void RemovePlayerActions(PlayerMatch playerMatch)
    {
        if (playerMatch.IdActions != null && playerMatch.IdActions.Count != 0)
        {
            foreach (var actionId in playerMatch.IdActions)
            {
                Services.DeletePlayerAction(actionId);
            }
            playerMatch.IdActions.Clear();
        }
    }
}

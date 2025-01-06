using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using Frontend.Resources.Components;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Frontend;

public partial class MatchView : ContentPage
{
    public MatchView()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }

    private void OnAddPlayer(object sender, EventArgs e)
    {
        // Abrir pagina modal para agregar un jugador
    }

    private void OnAddAction(object sender, EventArgs e)
    {
        // Abrir pagina modal para agregar un jugador
        // Esta creada la content page de ActionCreate, podria cambiarla a modal.
    }

    public class MainViewModel
    {
        public ObservableCollection<Player_Dto> Players { get; set; }

        public MainViewModel()
        {
            Players = new ObservableCollection<Player_Dto>
            {
            new Player_Dto { Number = "3", Name = "Pedro" },
            new Player_Dto { Number = "17", Name = "Juan" },
            // Agrega más jugadores hasta tener 16
            };
        }
    }
}
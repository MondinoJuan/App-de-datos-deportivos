using Frontend.Resources.Entities;
using Frontend.Resources;
using Microsoft.Maui.Controls;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Layouts;

namespace Frontend.Resources.Components
{
    public partial class PageOfActions : ContentView, INotifyPropertyChanged
    {
        // BindableProperty para IdPlayer
        public static readonly BindableProperty IdPlayerProperty =
            BindableProperty.Create(
                nameof(IdPlayer),
                typeof(Guid),
                typeof(PageOfActions),
                defaultValue: Guid.Empty,
                propertyChanged: OnIdPlayerChanged);

        public Guid IdPlayer
        {
            get => (Guid)GetValue(IdPlayerProperty);
            set => SetValue(IdPlayerProperty, value);
        }

        // Método que se ejecuta cuando cambia el valor de IdPlayer
        private static void OnIdPlayerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PageOfActions)bindable;
            control.LoadPlayerData((Guid)newValue);
        }

        // BindableProperty para IdTeam
        public static readonly BindableProperty IdTeamProperty =
            BindableProperty.Create(
                nameof(IdTeam),
                typeof(Guid),
                typeof(PageOfActions),
                defaultValue: Guid.Empty,
                propertyChanged: OnIdTeamChanged);

        public Guid IdTeam
        {
            get => (Guid)GetValue(IdTeamProperty);
            set => SetValue(IdTeamProperty, value);
        }

        // Método que se ejecuta cuando cambia el valor de IdTeam
        private static void OnIdTeamChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PageOfActions)bindable;
            control.LoadTeamData((Guid)newValue);
        }

        // ViewModel: el contexto de datos de la vista
        public PageOfActions_ViewModel ViewModel { get; }

        public PageOfActions()
        {
            InitializeComponent();
            ViewModel = new PageOfActions_ViewModel();
            BindingContext = ViewModel;
        }

        // Método para notificar el cambio de propiedad
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Métodos para cargar los datos desde el ViewModel
        private void LoadPlayerData(Guid idPlayer)
        {
            if (ViewModel != null && idPlayer != Guid.Empty)
            {
                ViewModel.LoadPlayerData(idPlayer);
            }
        }

        private void LoadTeamData(Guid idTeam)
        {
            if (ViewModel != null && idTeam != null)
            {
                var result = Simulo_BdD.GetOneClub(idTeam);
                if (!result.Success) return;
                ViewModel.LoadTeamData(result.Data);
            }
        }
    }

    public class PageOfActions_ViewModel : INotifyPropertyChanged
    {
        private string _title;
        private string _blockeds;
        private string _goals;
        private string _stealsW;
        private string _saves;
        private string _stealsL;
        private string _misses;
        private string _twoMinutes;
        private string _redCards;
        private string _blueCards;

        // Propiedades públicas para el enlace de datos
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();  // Notifica el cambio de propiedad
                }
            }
        }

        public string Blockeds
        {
            get => _blockeds;
            set
            {
                if (_blockeds != value)
                {
                    _blockeds = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Goals
        {
            get => _goals;
            set
            {
                if (_goals != value)
                {
                    _goals = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StealsW
        {
            get => _stealsW;
            set
            {
                if (_stealsW != value)
                {
                    _stealsW = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Saves
        {
            get => _saves;
            set
            {
                if (_saves != value)
                {
                    _saves = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StealsL
        {
            get => _stealsL;
            set
            {
                if (_stealsL != value)
                {
                    _stealsL = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Misses
        {
            get => _misses;
            set
            {
                if (_misses != value)
                {
                    _misses = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TwoMinutes
        {
            get => _twoMinutes;
            set
            {
                if (_twoMinutes != value)
                {
                    _twoMinutes = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RedCards
        {
            get => _redCards;
            set
            {
                if (_redCards != value)
                {
                    _redCards = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BlueCards
        {
            get => _blueCards;
            set
            {
                if (_blueCards != value)
                {
                    _blueCards = value;
                    OnPropertyChanged();
                }
            }
        }

        // Método para notificar el cambio de propiedad
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Métodos para cargar los datos de jugador y equipo
        public void LoadPlayerData(Guid playerId)
        {
            var result = Simulo_BdD.GetOnePlayer(playerId);
            if (!result.Success) return;

            Title = $"{result.Data.Number} {result.Data.Name}";

            Blockeds = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Blocked).ToString();
            Goals = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Goal).ToString();
            StealsW = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Steal_W).ToString();
            Saves = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Save).ToString();
            StealsL = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Steal_L).ToString();
            Misses = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Miss).ToString();

            var fouls = GetQuantityAndPlaceOfActionForPlayer(playerId, Ending.Foul);
            TwoMinutes = ((fouls % 1000) / 100).ToString();
            RedCards = ((fouls % 100) / 10).ToString();
            BlueCards = (fouls % 10).ToString();
        }

        public void LoadTeamData(Club_Dto team)
        {
            Title = team.Name;

            int cantidadBlockeds = 0;
            int cantidadGoals = 0;
            int cantidadStealsW = 0;
            int cantidadSaves = 0;
            int cantidadStealsL = 0;
            int cantidadMisses = 0;
            int cantidadFoules = 0;
            int cantidad2Minutos = 0;
            int cantidadRojas = 0;
            int cantidadAzules = 0;

            foreach (var idPlayer in team.IdPlayers)
            {
                cantidadBlockeds += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Blocked);
                cantidadGoals += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Goal);
                cantidadStealsW += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Steal_W);
                cantidadSaves += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Save);
                cantidadStealsL += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Steal_L);
                cantidadMisses += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Miss);
                var fouls = GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Foul);
                cantidadFoules += fouls / 1000;
                cantidad2Minutos += (fouls % 1000) / 100;
                cantidadRojas += (fouls % 100) / 10;
                cantidadAzules += fouls % 10;
            }

            Blockeds = cantidadBlockeds.ToString();
            Goals = cantidadGoals.ToString();
            StealsW = cantidadStealsW.ToString();
            Saves = cantidadSaves.ToString();
            StealsL = cantidadStealsL.ToString();
            Misses = cantidadMisses.ToString();
            TwoMinutes = cantidad2Minutos.ToString();
            RedCards = cantidadRojas.ToString();
            BlueCards = cantidadAzules.ToString();
        }

        //// Lógica para obtener las acciones de un jugador
        //private int GetQuantityAndPlaceOfActionForPlayer(Guid idPlayer, Ending ending)
        //{
        //    var result = Functions.GetActionCountForPlayer(idPlayer, ending);
        //    if (!result.Success) return 0;

        //    // Lógica para transformar la cantidad y ubicar la acción en el campo
        //    return result.QuantityEnding;
        //}

        private int GetQuantityAndPlaceOfActionForPlayer(Guid idPlayer, Ending ending)
        {
            var result = Functions.GetActionCountForPlayer(idPlayer, ending);
            if (!result.Success) return 0;
            var container = new AbsoluteLayout();
            switch (ending)
            {
                case Ending.Blocked:
                    container = markContainerBlockeds;
                    break;
                case Ending.Goal:
                    container = markContainerGoalsField;
                    break;
                case Ending.Steal_W:
                    container = markContainerSteals_W;
                    break;
                case Ending.Save:
                    container = markContainerSavesField;
                    break;
                case Ending.Steal_L:
                    container = markContainerSteals_L;
                    break;
                case Ending.Miss:
                    container = markContainerMissesField;
                    break;
                case Ending.Foul:
                    container = markContainerFouls;
                    break;
            }
            AddMarkToImage(container, result.CooField);
            if (ending == Ending.Goal || ending == Ending.Miss || ending == Ending.Save)
            {
                AddMarkToImage(markContainerGoalsGoal, result.CooGoal);
            }
            if (ending == Ending.Foul)
            {
                var cantidadFoules = result.QuantityEnding;
                var cantidadRojas = result.Red ?? 0;
                var cantidadAzules = result.Blue ?? 0;
                var cantidad2Minutos = result.Quantity2min ?? 0;
                var valorTransformado = cantidadFoules * 1000 + cantidad2Minutos * 100 + cantidadRojas * 10 + cantidadAzules;
                return valorTransformado;
            }
            return result.QuantityEnding;
        }

        private void AddMarkToImage(AbsoluteLayout container, List<Coordenates> coordenadas)
        {
            foreach (var coo in coordenadas)
            {
                // Crea una nueva marca (círculo)
                var circle = new BoxView
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    BackgroundColor = Colors.Red,
                    CornerRadius = 10,
                    Opacity = 0.6
                };
                // Calcula la posición en la pantalla
                AbsoluteLayout.SetLayoutBounds(circle, new Rect(coo.X, coo.Y, 20, 20));
                AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);
                // Añade el círculo al contenedor de marcas
                container.Children.Add(circle);
            }
        }
}

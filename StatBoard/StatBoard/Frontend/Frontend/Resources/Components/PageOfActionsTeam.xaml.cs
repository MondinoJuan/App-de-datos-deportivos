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
    public partial class PageOfActionsTeam : ContentView, INotifyPropertyChanged
    {
        public static readonly BindableProperty IdTeamProperty =
            BindableProperty.Create(
                nameof(IdTeam),
                typeof(Guid),
                typeof(PageOfActionsTeam),
                propertyChanged: OnIdTeamChanged);

        public Guid IdTeam
        {
            get => (Guid)GetValue(IdTeamProperty);
            set => SetValue(IdTeamProperty, value);
        }

        public static readonly BindableProperty TeamProperty =
            BindableProperty.Create(
                nameof(Team),
                typeof(Club_Dto),
                typeof(PageOfActionsTeam),
                propertyChanged: OnTeamChanged);

        public Club_Dto Team
        {
            get => (Club_Dto)GetValue(TeamProperty);
            set => SetValue(TeamProperty, value);
        }

        private string _title = string.Empty;
        private string _blockeds = string.Empty;
        private string _goals = string.Empty;
        private string _stealsW = string.Empty;
        private string _saves = string.Empty;
        private string _stealsL = string.Empty;
        private string _misses = string.Empty;
        private string _foules = string.Empty;
        private string _twoMinutes = string.Empty;
        private string _redCards = string.Empty;
        private string _blueCards = string.Empty;

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
                    lblTitle.Text = value;
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
                    lblBlockeds.Text = value;
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
                    lblGoals.Text = value;
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
                    lblSteals_W.Text = value;
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
                    lblSaves.Text = value;
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
                    lblSteals_L.Text = value;
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
                    lblMisses.Text = value;
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
                    lblTwoMinutes.Text = value;
                }
            }
        }

        public string Foules
        {
            get => _foules;
            set
            {
                if (_foules != value)
                {
                    _foules = value;
                    OnPropertyChanged();
                    lblFouls.Text = value;
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
                    lblRedCards.Text = value;
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
                    lblBlueCards.Text = value;
                }
            }
        }

        public PageOfActionsTeam()
        {
            InitializeComponent();
        }

        private static void OnIdTeamChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is Guid id && id != Guid.Empty)
            {
                var control = (PageOfActionsTeam)bindable;
                control.LoadTeamData(id);
            }
        }

        private static void OnTeamChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PageOfActionsTeam)bindable;
            control.LoadTeamData((Club_Dto)newValue);
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadTeamData(Guid idTeam)
        {
            var result = Simulo_BdD.GetOneClub(idTeam);
            if (!result.Success || result.Data == null) return;
            var team = result.Data;
            LoadTeamData(team);
        }

        public void LoadTeamData(Club_Dto team)
        {
            if (team == null) return;

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

        private int GetQuantityAndPlaceOfActionForPlayer(Guid idPlayer, Ending ending)
        {
            var result = Functions.GetActionCountForPlayer(idPlayer, ending);
            if (!result.Success) return 0;
            var container = new AbsoluteLayout();
            var container2 = new AbsoluteLayout();
            switch (ending)
            {
                case Ending.Blocked:
                    container = markContainerBlockeds;
                    break;
                case Ending.Goal:
                    container = markContainerGoalsField;
                    container2 = markContainerGoalsGoal;
                    break;
                case Ending.Steal_W:
                    container = markContainerSteals_W;
                    break;
                case Ending.Save:
                    container = markContainerSavesField;
                    container2 = markContainerSavesGoal;
                    break;
                case Ending.Steal_L:
                    container = markContainerSteals_L;
                    break;
                case Ending.Miss:
                    container = markContainerMissesField;
                    container2 = markContainerMissesGoal;
                    break;
                case Ending.Foul:
                    container = markContainerFouls;
                    break;
            }
            AddMarkToImage(container, result.CooField);
            if ((ending == Ending.Goal || ending == Ending.Miss || ending == Ending.Save) && result.CooGoal != null)
            {
                AddMarkToImage(container2, result.CooGoal);
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

        private static void AddMarkToImage(AbsoluteLayout container, List<Coordenates> coordenadas)
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
                AbsoluteLayout.SetLayoutBounds(circle, new Rect(coo.X + 10, coo.Y, 20, 20));
                AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);
                // Añade el círculo al contenedor de marcas
                container.Children.Add(circle);
            }
        }
    }        
}

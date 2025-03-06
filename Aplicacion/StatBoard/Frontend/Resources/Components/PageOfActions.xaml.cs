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

        public static readonly BindableProperty TeamProperty =
            BindableProperty.Create(
                nameof(Team),
                typeof(Club_Dto),
                typeof(PageOfActions),
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

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
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

        public PageOfActions()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private static void OnIdPlayerChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PageOfActions)bindable;
            control.LoadPlayerData((Guid)newValue);
        }

        private static void OnTeamChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PageOfActions)bindable;
            control.LoadTeamData((Club_Dto)newValue);
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            Foules = (fouls / 1000).ToString();
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
            Foules = cantidadFoules.ToString();
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

            // Ajusto las coordenadas
            var coorAdjustField = Functions.TranslateCoordenates(result.CooField, 20, -10);
            AddMarkToImage(container, coorAdjustField);
            if ((ending == Ending.Goal || ending == Ending.Miss || ending == Ending.Save) && result.CooGoal != null)
            {
                var coorAdjustGoal = Functions.TranslateCoordenates(result.CooGoal, -20, 0);
                AddMarkToImage(container2, coorAdjustGoal);
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
                var circle = new BoxView
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    BackgroundColor = Colors.Red,
                    CornerRadius = 10,
                    Opacity = 0.6
                };
                AbsoluteLayout.SetLayoutBounds(circle, new Rect(coo.X + 10, coo.Y, 20, 20));
                AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);
                container.Children.Add(circle);
            }
        }
    }        
}

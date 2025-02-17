using Frontend.Resources.Entities;
using Frontend.Resources;
using Microsoft.Maui.Controls;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Resources.Components
{
    public partial class PageOfActions : ContentView, INotifyPropertyChanged
    {
        // Propiedad bindable para IdPlayer
        private Guid _idPlayer;
        public Guid IdPlayer
        {
            get => _idPlayer;
            set
            {
                if (_idPlayer != value)
                {
                    _idPlayer = value;
                    OnPropertyChanged();
                    LoadPlayerData(_idPlayer); // Llamada directa a LoadPlayerData al cambiar el valor
                }
            }
        }

        // Propiedad bindable para IdTeam
        private Guid _idTeam;
        public Guid IdTeam
        {
            get => _idTeam;
            set
            {
                if (_idTeam != value)
                {
                    _idTeam = value;
                    OnPropertyChanged();
                    LoadTeamData(_idTeam); // Llamada directa a LoadTeamData al cambiar el valor
                }
            }
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

        // Lógica para obtener las acciones de un jugador
        private int GetQuantityAndPlaceOfActionForPlayer(Guid idPlayer, Ending ending)
        {
            var result = Functions.GetActionCountForPlayer(idPlayer, ending);
            if (!result.Success) return 0;

            // Lógica para transformar la cantidad y ubicar la acción en el campo
            return result.QuantityEnding;
        }
    }
}

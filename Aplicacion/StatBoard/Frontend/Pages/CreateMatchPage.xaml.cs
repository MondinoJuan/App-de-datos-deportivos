using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontend.Resources.DTOs;
using Frontend.Resources;
using Frontend.Resources.Modelos;

namespace Frontend.Pages
{
    public partial class CreateMatchPage : ContentPage, INotifyPropertyChanged
    {
        private bool enableCreateBtn;
        public bool EnableCreateBtn
        {
            get => enableCreateBtn;
            set
            {
                if (enableCreateBtn != value)
                {
                    enableCreateBtn = value;
                    OnPropertyChanged();
                }
            }
        }

        public CreateMatchPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            EnableCreateBtn = !string.IsNullOrEmpty(txtTournament.Text) &&
                              !string.IsNullOrEmpty(txtLocalTeam.Text) &&
                              !string.IsNullOrEmpty(txtAwayTeam.Text) &&
                              !string.IsNullOrEmpty(txtMatchWeek.Text) &&
                              !string.IsNullOrEmpty(txtPlace.Text);
        }

        private async void OnCreateMatch(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMatchWeek.Text, out int matchWeek))
            {
                await DisplayAlert("Error", "La jornada debe ser un número válido.", "OK");
                return;
            }

            Club_Dto teamLocal = new Club_Dto()
            {
                Name = txtLocalTeam.Text
            };
            Club_Dto teamAway = new Club_Dto()
            {
                Name = txtAwayTeam.Text
            };
                        
            try
            {
                var resultA = Services.AddClub(teamLocal);
                var idTeamLocal = SpecialServices.GetLastTeam();
                var resultB = Services.AddClub(teamAway);
                var idTeamAway = SpecialServices.GetLastTeam();

                Match_Dto match = new Match_Dto
                {
                    Date = DateTime.Now,
                    Place = txtPlace.Text,
                    State = "En juego",
                    MatchWeek = matchWeek,
                    Tournament = txtTournament.Text,
                    IdTeamLocal = idTeamLocal,
                    IdTeamAway = idTeamAway
                };

                var resultC = Services.AddMatch(match);

                await Navigation.PushAsync(new MatchView(match));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al crear el partido: {ex.Message}", "OK");
            }
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

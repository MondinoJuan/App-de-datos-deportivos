using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontend.Resources.Entities;

namespace Frontend
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
                Id = Guid.NewGuid(),
                Name = txtLocalTeam.Text
            };
            Club_Dto teamAway = new Club_Dto()
            {
                Id = Guid.NewGuid(),
                Name = txtAwayTeam.Text
            };

            Match_Dto match = new Match_Dto
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Place = txtPlace.Text,
                State = "En juego",
                MatchWeek = matchWeek,
                Tournament = txtTournament.Text,
                IdTeamLocal = teamLocal.Id,
                IdTeamAway = teamAway.Id
            };

            try
            {
                var resultA = Simulo_BdD.AddClub(teamLocal);            //hacer async cuando tenga BdD
                Console.WriteLine(resultA.Message);
                var resultB = Simulo_BdD.AddClub(teamAway);
                Console.WriteLine(resultB.Message);
                var resultC = Simulo_BdD.AddMatch(match);
                Console.WriteLine(resultC.Message);

                await Navigation.PushAsync(new MatchView(match));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error al crear el partido: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

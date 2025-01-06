using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;

namespace Frontend
{
    public partial class CreateMatchPage : ContentPage
    {
        public CreateMatchPage()
        {
            InitializeComponent();
        }

        public void OnCreateMatch()
        {
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
                MatchWeek = int.Parse(txtMatchWeek.Text),
                Tournament = txtTournament.Text,
                IdTeamLocal = teamLocal.Id,
                IdTeamAway = teamAway.Id
            };

            var resultA = Simulo_BdD.AddClub(teamLocal);
            Console.WriteLine(resultA.Message);
            var resultB = Simulo_BdD.AddClub(teamAway);
            Console.WriteLine(resultB.Message);
            var resultC = Simulo_BdD.AddMatch(match);
            Console.WriteLine(resultC.Message);

            Navigation.PushAsync(new MatchView(match));
        }
    }
}
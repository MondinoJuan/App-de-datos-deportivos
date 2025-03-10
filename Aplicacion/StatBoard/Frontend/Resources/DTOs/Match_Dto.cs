using CommunityToolkit.Mvvm.ComponentModel;

namespace Frontend.Resources.DTOs
{
    public partial class Match_Dto : ObservableObject
    {
        [ObservableProperty]
        public int idMatch;

        [ObservableProperty]
        public DateTime date = DateTime.Now;

        [ObservableProperty]
        public string place;

        [ObservableProperty]
        public string state;

        [ObservableProperty]
        public int matchWeek;

        [ObservableProperty]
        public string tournament;

        [ObservableProperty]
        public int idTeamLocal;

        [ObservableProperty]
        public int goalsTeamA = 0;

        [ObservableProperty]
        public int idTeamAway;

        [ObservableProperty]
        public int goalsTeamB = 0;
    }
}

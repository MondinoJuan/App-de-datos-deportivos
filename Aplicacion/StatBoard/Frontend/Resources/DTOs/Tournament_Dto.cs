using CommunityToolkit.Mvvm.ComponentModel;

namespace Frontend.Resources.DTOs
{
    public partial class Tournament_Dto : ObservableObject
    {
        [ObservableProperty]
        public int idTournament;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public int cupo;

        //[ObservableProperty]
        //public List<int> idClubs = new List<int>();
    }
}

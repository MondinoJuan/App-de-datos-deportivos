using CommunityToolkit.Mvvm.ComponentModel;

namespace Frontend.Resources.DTOs
{
    public partial class PlayerMatch_Dto : ObservableObject
    {
        [ObservableProperty]
        public int idPlayerMatch;

        [ObservableProperty]
        public int idPlayer;

        [ObservableProperty]
        public int idMatch;

        [ObservableProperty]
        public List<int>? idActions = new List<int>();
    }
}

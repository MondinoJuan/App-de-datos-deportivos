using CommunityToolkit.Mvvm.ComponentModel;

namespace Frontend.Resources.DTOs
{
    public partial class Player_Dto : ObservableObject 
    {
        [ObservableProperty]
        public int idPlayer;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public int number;
    }
}

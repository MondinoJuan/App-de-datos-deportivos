using CommunityToolkit.Mvvm.ComponentModel;

namespace Frontend.Resources.DTOs
{
    public partial class PlayerAction_Dto : ObservableObject
    {
        [ObservableProperty]
        public int idPlayerAction;

        [ObservableProperty]
        public bool whichHalf = false;

        [ObservableProperty]
        public Ending ending;

        [ObservableProperty]
        public float actionPositionX;
        [ObservableProperty]
        public float actionPositionY;

        [ObservableProperty]
        public float definitionPlaceX = 0;
        [ObservableProperty]
        public float definitionPlaceY = 0;

        [ObservableProperty]
        public Sanction sanction; 

        [ObservableProperty]
        public string? description;
    }
}

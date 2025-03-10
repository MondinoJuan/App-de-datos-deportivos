using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Frontend.Resources.Entities
{
    public partial class Club_Dto : ObservableObject
    {
        [ObservableProperty]
        public int idClub;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public List<int> idPlayers = new List<int>();
    }
}

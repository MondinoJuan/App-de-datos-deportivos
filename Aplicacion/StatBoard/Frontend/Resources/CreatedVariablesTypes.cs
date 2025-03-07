﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }

    public class EventsData
    {
        public List<Coordenates> CooField { get; set; }
        public List<Coordenates>? CooGoal { get; set; }
        public int QuantityEnding { get; set; } = 0;
        public int? Quantity2min { get; set; }
        public int? Red { get; set; }
        public int? Blue { get; set; }
        public bool Success { get; set; }
    }

    public struct Coordenates
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CreatedVariablesTypes
    {
        public static int QuantityOfPlayersPerClub { get; set; } = 16;
    }

    public enum Ending
    {
        Goal,
        Save,
        Miss,
        Blocked,
        Steal_W,
        Steal_L,
        Foul
    }

    public enum Sanction
    {
        Blue,
        Red,
        Two_Minutes
    }
}

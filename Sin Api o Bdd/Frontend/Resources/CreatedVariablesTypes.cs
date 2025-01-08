using System;
using System.Collections.Generic;
using System.Linq;
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

    public class CreatedVariablesTypes
    {
        public int QuantityOfPlayersPerClub { get; set; } = 16;
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

    public struct Coordenates
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum Sanction
    {
        Blue,
        Red,
        Two_Minutes
    }
}

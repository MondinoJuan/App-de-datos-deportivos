using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources
{
    public class CreatedVariablesTypes
    {
        
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
}

using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public struct MinutesSeconds
    {
        public int Minutes { get; set; }
        public int Seconds { get; set; }
    }

    public struct Coordenates
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum Ending
    {
        Goal,
        Save,
        Miss,
        Blocked,
        Steal_W,
        Steal_L
    }

    public class Action : EntityBase
    {
        public static MinutesSeconds MinutesSeconds { get; set; }

        public static Ending Ending {  get; set; }

        public static Coordenates ActionPosition { get; set; }

        public static Coordenates? DefinitionPlace { get; set; }

        public static string? Description { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public enum Sanction
    {
        Blue,
        Red,
        Two_Minutes
    }

    public class PlayerMatch : EntityBase
    {
        [Required]
        public static string IdPlayer { get; set; }

        [Required]
        public static string IdMatch { get; set; }

        public static string[]? IdActions { get; set; }

        [Required]
        public static Sanction[] State { get; set; }

    }
}

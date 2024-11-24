using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class PlayerMatch : EntityBase
    {
        [Required]
        public static string IdPlayer { get; set; }

        [Required]
        public static string IdMatch { get; set; }

        public static string[]? IdActions { get; set; }

        public static Boolean? Blue { get; set; }

        public static Boolean? Red { get; set; }

        public static MinutesSeconds[]? TwoMinutes { get; set; }

        [Required]
        public static string State { get; set; }

    }
}

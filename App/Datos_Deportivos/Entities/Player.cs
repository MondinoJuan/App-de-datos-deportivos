using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Player : EntityBase
    {
        [Required]
        public static string CompleteName { get; set; }

        [Required]
        public static string Birthdate { get; set; }

        [Required]
        public static decimal Hight { get; set; }

        [Required]
        public static decimal Weight { get; set; }

        [Required]
        public static string State { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Match : EntityBase
    {
        [Required]
        public static MinutesSeconds PlayTime { get; set; }

        [Required]
        public static DateTime Date { get; set; }

        [Required]
        public static string Place { get; set; }

        [Required]
        public static string State { get; set; }
    }
}

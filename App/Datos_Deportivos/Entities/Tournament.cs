using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Tournament : EntityBase
    {
        [Required]
        public static string Name { get; set; }

        [Required]
        public static int MaxAmountOfPlayersPerMatch { get; set; }
    }
}

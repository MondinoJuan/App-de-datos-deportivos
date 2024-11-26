using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class Tournament : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int MaxAmountOfPlayersPerMatch { get; set; }

        [Required]
        public string[] IdClubs { get; set; }
    }
}

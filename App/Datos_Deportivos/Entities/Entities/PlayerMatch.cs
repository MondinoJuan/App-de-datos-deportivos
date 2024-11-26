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
        public string IdPlayer { get; set; }

        [Required]
        public string IdMatch { get; set; }

        public string[]? IdActions { get; set; }

        [Required]
        public Sanction[]? State { get; set; }

    }
}

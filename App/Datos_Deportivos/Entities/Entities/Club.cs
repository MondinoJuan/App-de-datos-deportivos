using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class Club : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string[] IdPlayers { get; set; }
    }
}

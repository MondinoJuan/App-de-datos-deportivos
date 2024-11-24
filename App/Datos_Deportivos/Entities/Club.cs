using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Club : EntityBase
    {
        [Required]
        public static string Name { get; set; }
    }
}

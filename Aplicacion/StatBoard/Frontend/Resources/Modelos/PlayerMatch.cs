using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Modelos
{
    public class PlayerMatch : EntityBase
    {
        [ForeignKey("Player")]
        public int IdPlayer { get; set; }

        [ForeignKey("Match")]
        public int IdMatch { get; set; }

        public List<int>? IdActions { get; set; } = new();
    }
}

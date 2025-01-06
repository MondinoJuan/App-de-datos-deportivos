using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class PlayerMatch_Dto
    {
        public string IdPlayer { get; set; }
        public string IdMatch { get; set; }
        public string[]? IdActions { get; set; }
        public Sanction[]? State { get; set; }
    }
}

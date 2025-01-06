using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class PlayerMatch_Dto
    {                                           // Hace falta sacar los IDs? Porque sino podria almacenar los matchs en colecciones, no?
        public System.Guid Id { get; set; }
        public System.Guid IdPlayer { get; set; }
        public System.Guid IdMatch { get; set; }
        public List<System.Guid>? IdActions { get; set; }
        public List<Sanction>? State { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class Club_Dto
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public List<System.Guid> IdPlayers { get; set; }
    }
}

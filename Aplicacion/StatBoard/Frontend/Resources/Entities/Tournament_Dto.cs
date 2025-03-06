using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class Tournament_Dto
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }

        public int Cupo { get; set; }

        //public List<System.Guid> IdClubs { get; set; } = new List<Guid>();
    }
}

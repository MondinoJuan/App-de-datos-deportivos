using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Modelos
{
    public class Tournament : EntityBase
    {
        public string Name { get; set; }

        public int Cupo { get; set; }

        //public List<int> IdClubs { get; set; } = new List<int>();
    }
}

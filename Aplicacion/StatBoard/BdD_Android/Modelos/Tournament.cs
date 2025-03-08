using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdD_Android.Modelos
{
    public class Tournament : EntityBase
    {
        public string Name { get; set; }

        public int Cupo { get; set; }

        //public List<System.Guid> IdClubs { get; set; } = new List<Guid>();
    }
}

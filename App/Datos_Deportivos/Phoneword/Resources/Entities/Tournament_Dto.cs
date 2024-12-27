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
        public string Name { get; set; }

        public int Cupo { get; set; }

        public string[] IdClubs { get; set; }
    }
}

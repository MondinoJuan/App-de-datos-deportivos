using BdD_Android.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdD_Android.Modelos
{
    public class Player : EntityBase 
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}

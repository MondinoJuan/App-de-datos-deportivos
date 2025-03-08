using BdD_Android.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdD_Android.Modelos
{
    public class Club : EntityBase
    {
        public string Name { get; set; }
        public List<int> IdPlayers { get; set; } = new();           // Hacer listado del objeto directamente?
    }
}

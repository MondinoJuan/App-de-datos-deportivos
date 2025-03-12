using Frontend.Resources.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Modelos
{
    public class Player : EntityBase 
    {
        public string Name { get; set; }
        public int Number { get; set; }

        internal void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }
    }
}

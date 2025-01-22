using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class PlayerAction_Dto
    {
        public System.Guid Id { get; set; }

        public bool WhichHalf { get; set; } = false;

        public Ending Ending { get; set; }

        public double ActionPositionX { get; set; }
        public double ActionPositionY { get; set; }

        public double? DefinitionPlaceX { get; set; }
        public double? DefinitionPlaceY { get; set; }

        public Sanction Sanction { get; set; }              // Agregar a createAction

        public string? Description { get; set; }
    }
}

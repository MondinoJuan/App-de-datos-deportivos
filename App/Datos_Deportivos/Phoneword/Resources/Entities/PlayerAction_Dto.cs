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
        public bool WhichHalf { get; set; }

        public Ending Ending { get; set; }

        public double ActionPositionX { get; set; }
        public double ActionPositionY { get; set; }

        public double? DefinitionPlaceX { get; set; }
        public double? DefinitionPlaceY { get; set; }

        public string? Description { get; set; }


        //public Coordenates ActionPosition
        //{
        //    get => new Coordenates { X = ActionPositionX, Y = ActionPositionY };
        //    set
        //    {
        //        ActionPositionX = value.X;
        //        ActionPositionY = value.Y;
        //    }
        //}

        //public Coordenates? DefinitionPlace
        //{
        //    get => DefinitionPlaceX.HasValue && DefinitionPlaceY.HasValue
        //        ? new Coordenates { X = DefinitionPlaceX.Value, Y = DefinitionPlaceY.Value }
        //        : (Coordenates?)null;
        //    set
        //    {
        //        DefinitionPlaceX = value?.X;
        //        DefinitionPlaceY = value?.Y;
        //    }
        //}
    }
}

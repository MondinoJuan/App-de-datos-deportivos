using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    public struct Coordenates
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum Ending
    {
        Goal,
        Save,
        Miss,
        Blocked,
        Steal_W,
        Steal_L,
        Foul
    }

    public class PlayerAction : EntityBase
    {
        public bool WhichHalf { get; set; }                   // False para primer tiempo, True para segundo tiempo. 

        public Ending Ending { get; set; }

        public int ActionPositionX { get; set; }              // Coordenada X de ActionPosition
        public int ActionPositionY { get; set; }              // Coordenada Y de ActionPosition

        public int? DefinitionPlaceX { get; set; }            // Coordenada X de DefinitionPlace (opcional)
        public int? DefinitionPlaceY { get; set; }            // Coordenada Y de DefinitionPlace (opcional)

        public string? Description { get; set; }             // Si es de contra o de alguna forma en especial.

        // Propiedades de conveniencia para trabajar con Coordenates en el código
        [NotMapped]
        public Coordenates ActionPosition
        {
            get => new Coordenates { X = ActionPositionX, Y = ActionPositionY };
            set
            {
                ActionPositionX = value.X;
                ActionPositionY = value.Y;
            }
        }

        [NotMapped]
        public Coordenates? DefinitionPlace
        {
            get => DefinitionPlaceX.HasValue && DefinitionPlaceY.HasValue
                ? new Coordenates { X = DefinitionPlaceX.Value, Y = DefinitionPlaceY.Value }
                : (Coordenates?)null;
            set
            {
                DefinitionPlaceX = value?.X;
                DefinitionPlaceY = value?.Y;
            }
        }
    }
}

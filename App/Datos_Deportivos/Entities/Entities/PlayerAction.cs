using System.ComponentModel.DataAnnotations;

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

        public Coordenates ActionPosition { get; set; }          // Foto de media cancha como fondo de la matriz.

        public Coordenates? DefinitionPlace { get; set; }        // Foto de un arco como fondo de la matriz.

        public string? Description { get; set; }                 // Si es de contra o de alguna forma en especial.
    }
}

using Frontend.Resources;

namespace Frontend.Resources.Modelos
{
    public class PlayerAction : EntityBase
    {
        public bool WhichHalf { get; set; } = false;

        public Ending EndingA { get; set; }

        public float ActionPositionX { get; set; }
        public float ActionPositionY { get; set; }

        public float? DefinitionPlaceX { get; set; } = 0;
        public float? DefinitionPlaceY { get; set; } = 0;

        public Sanction? SanctionA { get; set; }

        //public string? Description { get; set; }
    }
}

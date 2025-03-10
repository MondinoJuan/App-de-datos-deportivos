using Frontend.Resources;

namespace Frontend.Resources.Modelos
{
    public class PlayerAction : EntityBase
    {
        public bool WhichHalf { get; set; } = false;

        public Ending Ending { get; set; }

        public float ActionPositionX { get; set; }
        public float ActionPositionY { get; set; }

        public float DefinitionPlaceX { get; set; } = 0;
        public float DefinitionPlaceY { get; set; } = 0;

        public Sanction Sanction { get; set; }

        public string? Description { get; set; }
    }
}


namespace Frontend.Resources.Modelos
{
    public class Club : EntityBase
    {
        public string Name { get; set; }
        public List<int> IdPlayers { get; set; } = new();
    }
}

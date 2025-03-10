using BdD_Android.DataAccess;
using BdD_Android.Modelos;
using BdD_Android.Utilidades;

namespace BdD_Android.Servicios
{
    public class Player_Services
    {
        //Metodos Normales
        public static void AgregarPlayer(Player player)
        {

            using var context = new Player_DbContext();

            // Llamadas a los métodos de validación
            Validador.ValidarTextoNoVacio(player.Name, "Nombre");

            context.Players.Add(player);
            context.SaveChanges();

        }

        public static Player? GetOnePlayerId(int id)
        {
            using var context = new Player_DbContext();

            return context.Players.Find(id);

        }

        public static IEnumerable<Player> GetAllPlayer()
        {
            using var context = new Player_DbContext();

            return context.Players.ToList();
        }

        public static void ActualizarPlayer(Player player)
        {
            using var context = new Player_DbContext();

            var playerToUpdate = context.Players.Find(player.Id);

            if (playerToUpdate != null)
            {
                // Validaciones
                Validador.ValidarTextoNoVacio(player.Name, "Nombre");

                playerToUpdate.Name = player.Name;
                playerToUpdate.Number = player.Number;
                playerToUpdate.Id = player.Id;

                context.SaveChanges();
            }
        }

        public static void EliminarPlayer(int id)
        {
            using var context = new Player_DbContext();

            var player = context.Players.Find(id);
            if (player != null)
            {
                context.Players.Remove(player);
                context.SaveChanges();
            }

        }
    }
}

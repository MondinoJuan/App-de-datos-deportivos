using BdD_Android.DataAccess;
using BdD_Android.Modelos;
using BdD_Android.Utilidades;

namespace BdD_Android.Servicios
{
    public class Club_Services
    {
        //Metodos Normales
        public static void AgregarClub(Club club)
        {

            using var context = new Club_DbContext();

            // Llamadas a los métodos de validación
            Validador.ValidarTextoNoVacio(club.Name, "Nombre");

            context.Clubes.Add(club);
            context.SaveChanges();

        }

        public static Club? GetOneClubId(int id)
        {
            using var context = new Club_DbContext();

            return context.Clubes.Find(id);

        }

        public static IEnumerable<Club> GetAllClub()
        {
            using var context = new Club_DbContext();

            return context.Clubes.ToList();
        }

        public static void ActualizarClub(Club club)
        {
            using var context = new Club_DbContext();

            var clubToUpdate = context.Clubes.Find(club.Id);

            if (clubToUpdate != null)
            {
                // Validaciones
                Validador.ValidarTextoNoVacio(club.Name, "Nombre");

                clubToUpdate.Name = club.Name;
                clubToUpdate.IdPlayers = club.IdPlayers;
                clubToUpdate.Id = club.Id;

                context.SaveChanges();
            }
        }

        public static void EliminarClub(int id)
        {
            using var context = new Club_DbContext();

            var club = context.Clubes.Find(id);
            if (club != null)
            {
                context.Clubes.Remove(club);
                context.SaveChanges();
            }
        }

        public static void EliminarTodosLosClubes()
        {
            using var context = new Club_DbContext();
            var allClubs = context.Clubes.ToList();
            context.Clubes.RemoveRange(allClubs);
            context.SaveChanges();
        }
    }
}

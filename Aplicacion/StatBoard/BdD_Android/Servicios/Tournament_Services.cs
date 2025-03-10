using BdD_Android.DataAccess;
using BdD_Android.Modelos;
using BdD_Android.Utilidades;

namespace BdD_Android.Servicios
{
    public class Tournament_Services
    {
        //Metodos Normales
        public static void AgregarTournament(Tournament tournament)
        {

            using var context = new Tournament_DbContext();

            // Llamadas a los métodos de validación
            Validador.ValidarTextoNoVacio(tournament.Name, "Nombre");

            context.Tournaments.Add(tournament);
            context.SaveChanges();

        }

        public static Tournament? GetOneTournamentId(int id)
        {
            using var context = new Tournament_DbContext();

            return context.Tournaments.Find(id);

        }

        public static IEnumerable<Tournament> GetAllTournament()
        {
            using var context = new Tournament_DbContext();

            return context.Tournaments.ToList();
        }

        public static void ActualizarTournament(Tournament tournament)
        {
            using var context = new Tournament_DbContext();

            var tournamentToUpdate = context.Tournaments.Find(tournament.Id);

            if (tournamentToUpdate != null)
            {
                // Validaciones
                Validador.ValidarTextoNoVacio(tournament.Name, "Nombre");

                tournamentToUpdate.Name = tournament.Name;
                tournamentToUpdate.Cupo = tournament.Cupo;
                tournamentToUpdate.Id = tournament.Id;

                context.SaveChanges();
            }
        }

        public static void EliminarTournament(int id)
        {
            using var context = new Tournament_DbContext();

            var tournament = context.Tournaments.Find(id);
            if (tournament != null)
            {
                context.Tournaments.Remove(tournament);
                context.SaveChanges();
            }

        }
    }
}

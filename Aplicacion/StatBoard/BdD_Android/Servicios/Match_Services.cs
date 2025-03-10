using BdD_Android.DataAccess;
using BdD_Android.Modelos;
using BdD_Android.Utilidades;

namespace BdD_Android.Servicios
{
    public class Match_Services
    {
        //Metodos Normales
        public static void AgregarMatch(Match match)
        {

            using var context = new Match_DbContext();

            // Llamadas a los métodos de validación
            Validador.ValidarTextoNoVacio(match.Tournament, "Torneo");
            Validador.ValidarTextoNoVacio(match.Place, "Lugar");
            Validador.ValidarTextoNoVacio(match.State, "Estado");

            context.Matches.Add(match);
            context.SaveChanges();

        }

        public static Match? GetOneMatchId(int id)
        {
            using var context = new Match_DbContext();

            return context.Matches.Find(id);

        }

        public static IEnumerable<Match> GetAllMatch()
        {
            using var context = new Match_DbContext();

            return context.Matches.ToList();
        }

        public static void ActualizarMatch(Match match)
        {
            using var context = new Match_DbContext();

            var matchToUpdate = context.Matches.Find(match.Id);

            if (matchToUpdate != null)
            {
                // Validaciones
                Validador.ValidarTextoNoVacio(match.Tournament, "Torneo");
                Validador.ValidarTextoNoVacio(match.Place, "Lugar");
                Validador.ValidarTextoNoVacio(match.State, "Estado");

                matchToUpdate.Tournament = match.Tournament;
                matchToUpdate.IdTeamLocal = match.IdTeamLocal;
                matchToUpdate.IdTeamAway = match.IdTeamAway;
                matchToUpdate.Place = match.Place;
                matchToUpdate.State = match.State;
                matchToUpdate.GoalsTeamA = match.GoalsTeamA;
                matchToUpdate.GoalsTeamB = match.GoalsTeamB;
                matchToUpdate.Date = match.Date;
                matchToUpdate.MatchWeek = match.MatchWeek;
                matchToUpdate.Id = match.Id;

                context.SaveChanges();
            }
        }

        public static void EliminarMatch(int id)
        {
            using var context = new Match_DbContext();

            var match = context.Matches.Find(id);
            if (match != null)
            {
                context.Matches.Remove(match);
                context.SaveChanges();
            }

        }
    }
}

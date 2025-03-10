using BdD_Android.DataAccess;
using BdD_Android.Modelos;
using BdD_Android.Utilidades;

namespace BdD_Android.Servicios
{
    public class PlayerMatch_Services
    {
        //Metodos Normales
        public static void AgregarPlayerMatch(PlayerMatch playerMatch)
        {

            using var context = new PlayerMatch_DbContext();

            context.PlayerMatches.Add(playerMatch);
            context.SaveChanges();

        }

        public static PlayerMatch? GetOnePlayerMatchId(int id)
        {
            using var context = new PlayerMatch_DbContext();

            return context.PlayerMatches.Find(id);

        }

        public static IEnumerable<PlayerMatch> GetAllPlayerMatch()
        {
            using var context = new PlayerMatch_DbContext();

            return context.PlayerMatches.ToList();
        }

        public static void ActualizarPlayerMatch(PlayerMatch playerMatch)
        {
            using var context = new PlayerMatch_DbContext();

            var playerMatchToUpdate = context.PlayerMatches.Find(playerMatch.Id);

            if (playerMatchToUpdate != null)
            {
                playerMatchToUpdate.IdMatch = playerMatch.IdMatch;
                playerMatchToUpdate.IdPlayer = playerMatch.IdPlayer;
                playerMatchToUpdate.IdActions = playerMatch.IdActions;
                playerMatchToUpdate.Id = playerMatch.Id;

                context.SaveChanges();
            }
        }

        public static void EliminarPlayerMatch(int id)
        {
            using var context = new PlayerMatch_DbContext();

            var playerMatch = context.PlayerMatches.Find(id);
            if (playerMatch != null)
            {
                context.PlayerMatches.Remove(playerMatch);
                context.SaveChanges();
            }

        }
    }
}

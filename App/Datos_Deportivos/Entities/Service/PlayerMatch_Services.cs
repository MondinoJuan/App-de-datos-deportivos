using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Entities.Service
{
    public class PlayerMatch_Services
    {
        public static void CreatePlayerMatch(PlayerMatch playerMatch)
        {
            using var context = new Context();

            // Validaciones.
            Validations.ValidatePlayerExists(playerMatch.IdPlayer, context);
            Validations.ValidateMatchExists(playerMatch.IdMatch, context);

            context.PlayersMatchs.Add(playerMatch);
            context.SaveChanges();
        }

        public static PlayerMatch? GetPlayerMatch(int id)
        {
            using var context = new Context();
            return context.PlayersMatchs.Find(id);
        }

        public static IEnumerable<PlayerMatch> GetAllPlayerMatch()
        {
            using var context = new Context();
            return context.PlayersMatchs.ToList();
        }

        public static void UpdatePlayerMatch(PlayerMatch playerMatch)
        {
            using var context = new Context();

            var playerMatchToUpdate = context.PlayersMatchs.Find(playerMatch.Id);

            if (playerMatchToUpdate != null)
            {
                // Validaciones por separado
                Validations.ValidatePlayerExists(playerMatch.IdPlayer, context);
                Validations.ValidateMatchExists(playerMatch.IdMatch, context);

                playerMatchToUpdate.IdActions = playerMatch.IdActions;
                playerMatchToUpdate.IdPlayer = playerMatch.IdPlayer;
                playerMatchToUpdate.IdMatch = playerMatch.IdMatch;
                playerMatchToUpdate.State = playerMatch.State;
                playerMatchToUpdate.Id = playerMatch.Id;
                context.SaveChanges();
            }
        }

        public static void DeletePlayerMatch(int id)
        {
            using var context = new Context();
            var playerMatch = context.PlayersMatchs.Find(id); 
            if (playerMatch != null)
            {
                context.PlayersMatchs.Remove(playerMatch);
                context.SaveChanges();
            }
        }
    }
}

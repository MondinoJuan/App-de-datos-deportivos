using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Entities.Service
{
    public class PlayerAction_Services
    {
        public static void CreatePlayerAction(PlayerAction playerAction)
        {
            using var context = new Context();

            // Validaciones.

            context.PlayersActions.Add(playerAction);
            context.SaveChanges();
        }

        public static PlayerAction? GetPlayerAction(int id)
        {
            using var context = new Context();
            return context.PlayersActions.Find(id);
        }

        public static IEnumerable<PlayerAction> GetAllPlayerAction()
        {
            using var context = new Context();
            return context.PlayersActions.ToList();
        }

        public static void UpdatePlayerAction(PlayerAction playerAction)
        {
            using var context = new Context();

            var playerActionToUpdate = context.PlayersActions.Find(playerAction.Id);

            if (playerActionToUpdate != null)
            {
                // Validaciones por separado

                playerActionToUpdate.WhichHalf = playerAction.WhichHalf;
                playerActionToUpdate.Ending = playerAction.Ending;
                playerActionToUpdate.ActionPosition = playerAction.ActionPosition;
                playerActionToUpdate.DefinitionPlace = playerAction.DefinitionPlace;
                playerActionToUpdate.Description = playerAction.Description;
                playerActionToUpdate.Id = playerAction.Id;
                context.SaveChanges();
            }
        }

        public static void DeletePlayerAction(int id)
        {
            using var context = new Context();
            var playerAction = context.PlayersActions.Find(id); 
            if (playerAction != null)
            {
                context.PlayersActions.Remove(playerAction);
                context.SaveChanges();
            }
        }
    }
}

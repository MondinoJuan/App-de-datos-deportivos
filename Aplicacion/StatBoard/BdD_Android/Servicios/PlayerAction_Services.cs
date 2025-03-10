using BdD_Android.DataAccess;
using BdD_Android.Modelos;
using BdD_Android.Utilidades;

namespace BdD_Android.Servicios
{
    public class PlayerAction_Services
    {
        //Metodos Normales
        public static void AgregarPlayerAction(PlayerAction playerAction)
        {

            using var context = new PlayerAction_DbContext();

            context.PlayerActions.Add(playerAction);
            context.SaveChanges();

        }

        public static PlayerAction? GetOnePlayerActionId(int id)
        {
            using var context = new PlayerAction_DbContext();

            return context.PlayerActions.Find(id);

        }

        public static IEnumerable<PlayerAction> GetAllPlayerAction()
        {
            using var context = new PlayerAction_DbContext();

            return context.PlayerActions.ToList();
        }

        public static void ActualizarPlayerAction(PlayerAction playerAction)
        {
            using var context = new PlayerAction_DbContext();

            var playerActionToUpdate = context.PlayerActions.Find(playerAction.Id);

            if (playerActionToUpdate != null)
            {
                playerActionToUpdate.WhichHalf = playerAction.WhichHalf;
                playerActionToUpdate.Ending = playerAction.Ending;
                playerActionToUpdate.ActionPositionX = playerAction.ActionPositionX;
                playerActionToUpdate.ActionPositionY = playerAction.ActionPositionY;
                playerActionToUpdate.DefinitionPlaceX = playerAction.DefinitionPlaceX;
                playerActionToUpdate.DefinitionPlaceY = playerAction.DefinitionPlaceY;
                playerActionToUpdate.Sanction = playerAction.Sanction;
                playerActionToUpdate.Description = playerAction.Description;
                playerActionToUpdate.Id = playerAction.Id;

                context.SaveChanges();
            }
        }

        public static void EliminarPlayerAction(int id)
        {
            using var context = new PlayerAction_DbContext();

            var playerAction = context.PlayerActions.Find(id);
            if (playerAction != null)
            {
                context.PlayerActions.Remove(playerAction);
                context.SaveChanges();
            }

        }
    }
}

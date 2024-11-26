using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Entities;

namespace Entities.Service
{
    public class PlayerAction_Services
    {
        public static void CreatePlayerAction(PlayerAction playerAction)
        {
            using var context = new Context();

            // Validaciones previas
            if (playerAction == null)
                throw new ArgumentNullException(nameof(playerAction), "PlayerAction no puede ser nulo.");

            // Guardar en base de datos
            context.PlayersActions.Add(playerAction);
            context.SaveChanges();
        }

        public static PlayerAction? GetPlayerAction(int id)
        {
            using var context = new Context();

            // Buscar por ID
            var playerAction = context.PlayersActions.Find(id);
            if (playerAction == null)
                throw new KeyNotFoundException($"No se encontró un PlayerAction con el ID {id}.");

            return playerAction;
        }

        public static IEnumerable<PlayerAction> GetAllPlayerActions()
        {
            using var context = new Context();

            // Obtener todos los registros
            return context.PlayersActions.ToList();
        }

        public static void UpdatePlayerAction(PlayerAction playerAction)
        {
            using var context = new Context();

            if (playerAction == null)
                throw new ArgumentNullException(nameof(playerAction), "PlayerAction no puede ser nulo.");

            // Buscar el registro existente
            var playerActionToUpdate = context.PlayersActions.Find(playerAction.Id);
            if (playerActionToUpdate == null)
                throw new KeyNotFoundException($"No se encontró un PlayerAction con el ID {playerAction.Id} para actualizar.");

            // Actualizar propiedades
            playerActionToUpdate.WhichHalf = playerAction.WhichHalf;
            playerActionToUpdate.Ending = playerAction.Ending;
            playerActionToUpdate.ActionPositionX = playerAction.ActionPosition.X;
            playerActionToUpdate.ActionPositionY = playerAction.ActionPosition.Y;

            if (playerAction.DefinitionPlace.HasValue)
            {
                playerActionToUpdate.DefinitionPlaceX = playerAction.DefinitionPlace.Value.X;
                playerActionToUpdate.DefinitionPlaceY = playerAction.DefinitionPlace.Value.Y;
            }
            else
            {
                playerActionToUpdate.DefinitionPlaceX = null;
                playerActionToUpdate.DefinitionPlaceY = null;
            }

            playerActionToUpdate.Description = playerAction.Description;

            // Guardar cambios
            context.SaveChanges();
        }

        public static void DeletePlayerAction(int id)
        {
            using var context = new Context();

            // Buscar por ID
            var playerAction = context.PlayersActions.Find(id);
            if (playerAction == null)
                throw new KeyNotFoundException($"No se encontró un PlayerAction con el ID {id} para eliminar.");

            // Eliminar registro
            context.PlayersActions.Remove(playerAction);
            context.SaveChanges();
        }
    }
}

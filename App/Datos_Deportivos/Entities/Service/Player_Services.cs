using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Entities.Service
{
    public class Player_Services
    {
        public static void CreatePlayer(Player player)
        {
            using var context = new Context();

            // Validaciones.

            context.Players.Add(player);
            context.SaveChanges();
        }

        public static Player? GetPlayer(int id)
        {
            using var context = new Context();
            return context.Players.Find(id);
        }

        public static IEnumerable<Player> GetAllPlayer()
        {
            using var context = new Context();
            return context.Players.ToList();
        }

        public static void UpdatePlayer(Player player)
        {
            using var context = new Context();

            var playerToUpdate = context.Players.Find(player.Id);

            if (playerToUpdate != null)
            {
                // Validaciones por separado

                playerToUpdate.Number = player.Number;
                playerToUpdate.Id = player.Id;
                context.SaveChanges();
            }
        }

        public static void DeletePlayer(int id)
        {
            using var context = new Context();
            var player = context.Players.Find(id); 
            if (player != null)
            {
                context.Players.Remove(player);
                context.SaveChanges();
            }
        }
    }
}

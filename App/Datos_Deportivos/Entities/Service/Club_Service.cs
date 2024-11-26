using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Entities.Service
{
    public class Club_Service
    {
        public static void CreateClub(Club club)
        {
            using var context = new Context();

            // Validaciones.

            context.Clubs.Add(club);
            context.SaveChanges();
        }

        public static Club? GetClub(int id)
        {
            using var context = new Context();
            return context.Clubs.Find(id);
        }

        public static IEnumerable<Club> GetAllClub()
        {
            using var context = new Context();
            return context.Clubs.ToList();
        }

        public static void UpdateClub(Club club)
        {
            using var context = new Context();

            var clubToUpdate = context.Clubs.Find(club.Id);

            if (clubToUpdate != null)
            {
                // Validaciones por separado

                clubToUpdate.Name = club.Name;
                clubToUpdate.IdPlayers = club.IdPlayers;
                clubToUpdate.Id = club.Id;
                context.SaveChanges();
            }
        }

        public static void DeleteClub(int id)
        {
            using var context = new Context();
            var club = context.Clubs.Find(id); 
            if (club != null)
            {
                context.Clubs.Remove(club);
                context.SaveChanges();
            }
        }
    }
}

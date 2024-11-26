using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Entities.Service
{
    public class Match_Services
    {
        public static void CreateMatch(Match match)
        {
            using var context = new Context();

            // Validaciones.
            Validations.ValidateFutureDate(match.Date);

            context.Matchs.Add(match);
            context.SaveChanges();
        }

        public static Match? GetMatch(int id)
        {
            using var context = new Context();
            return context.Matchs.Find(id);
        }

        public static IEnumerable<Match> GetAllMatch()
        {
            using var context = new Context();
            return context.Matchs.ToList();
        }

        public static void UpdateMatch(Match match)
        {
            using var context = new Context();

            var matchToUpdate = context.Matchs.Find(match.Id);

            if (matchToUpdate != null)
            {
                // Validaciones
                Validations.ValidateFutureDate(match.Date);

                matchToUpdate.PlayTime = match.PlayTime;
                matchToUpdate.Date = match.Date;
                matchToUpdate.Place = match.Place;
                matchToUpdate.State = match.State;
                matchToUpdate.Id = match.Id;
                context.SaveChanges();
            }
        }

        public static void DeleteMatch(int id)
        {
            using var context = new Context();
            var match = context.Matchs.Find(id); 
            if (match != null)
            {
                context.Matchs.Remove(match);
                context.SaveChanges();
            }
        }
    }
}

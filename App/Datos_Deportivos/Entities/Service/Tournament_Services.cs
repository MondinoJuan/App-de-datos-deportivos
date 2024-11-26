using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Entities.Service
{
    public class Tournament_Services
    {
        public static void CreateTournament(Tournament tournament)
        {
            using var context = new Context();

            // Validaciones.
            Validations.ValidateEmptyText(tournament.Name, "Nombre del torneo");
            Validations.ValidateCupo(tournament.Cupo);
            for (int i = 0; i < tournament.IdClubs.Count(); i++)
            {
                Validations.ValidateClubExists(tournament.IdClubs[i], context);
            }

            context.Tournaments.Add(tournament);
            context.SaveChanges();
        }

        public static Tournament? GetTournament(int id)
        {
            using var context = new Context();
            return context.Tournaments.Find(id);
        }

        public static IEnumerable<Tournament> GetAllTournament()
        {
            using var context = new Context();
            return context.Tournaments.ToList();
        }

        public static void UpdateTournament(Tournament tournament)
        {
            using var context = new Context();

            var tournamentToUpdate = context.Tournaments.Find(tournament.Id);

            if (tournamentToUpdate != null)
            {
                // Validaciones por separado
                Validations.ValidateEmptyText(tournament.Name, "Nombre del torneo");
                Validations.ValidateCupo(tournament.Cupo);
                for (int i = 0; i < tournament.IdClubs.Count(); i++)
                {
                    Validations.ValidateClubExists(tournament.IdClubs[i], context);
                }

                tournamentToUpdate.Name = tournament.Name;
                tournamentToUpdate.Cupo = tournament.Cupo;
                tournamentToUpdate.IdClubs = tournament.IdClubs;
                tournamentToUpdate.Id = tournament.Id;
                context.SaveChanges();
            }
        }

        public static void DeleteTournament(int id)
        {
            using var context = new Context();
            var tournament = context.Tournaments.Find(id); 
            if (tournament != null)
            {
                context.Tournaments.Remove(tournament);
                context.SaveChanges();
            }
        }
    }
}

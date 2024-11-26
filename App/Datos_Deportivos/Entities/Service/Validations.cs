using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Service
{
    public class Validations
    {
        public static void ValidateEmptyText(string campo, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(campo))
            {
                throw new ArgumentException($"{nombreCampo} no puede estar vacío.");
            }
        }

        public static void ValidateInt(int campo, string nombreCampo)
        {
            if (campo <= 0)
            {
                throw new ArgumentException($"{nombreCampo} deben ser mayores que 0.");
            }
        }

        public static void ValidateMatchExists(string id, Context contexto)
        {
            if (!contexto.Matchs.Any(m => m.Id == id))
            {
                throw new ArgumentException($"El partido ({id}) no existe.");
            }
        }

        public static void ValidateClubExists(string id, Context contexto)
        {
            if (!contexto.Clubs.Any(c => c.Id == id))
            {
                throw new ArgumentException($"El club ({id}) no existe.");
            }
        }

        public static void ValidatePlayerExists(string id, Context contexto)
        {
            if (!contexto.Players.Any(p => p.Id == id))
            {
                throw new ArgumentException($"El jugador ({id}) no existe.");
            }
        }

        public static void ValidatePlayerActionExists(string id, Context contexto)
        {
            if (!contexto.PlayersActions.Any(pa => pa.Id == id))
            {
                throw new ArgumentException($"La acción del jugador ({id}) no existe.");
            }
        }

        public static void ValidatePlayerMatchExists(string id, Context contexto)
        {
            if (!contexto.PlayersMatchs.Any(pm => pm.Id == id))
            {
                throw new ArgumentException($"El partido del jugador ({id}) no existe.");
            }
        }

        public static void ValidateTournamentExists(string id, Context contexto)
        {
            if (!contexto.Tournaments.Any(t => t.Id == id))
            {
                throw new ArgumentException($"El torneo ({id}) no existe.");
            }
        }

        public static void ValidateCupo(int cupo)
        {
            if (cupo <= 0 || cupo > 100)
            {
                throw new ArgumentException("El cupo debe ser un número positivo entre 1 y 100.");
            }
        }

        public static void ValidateFutureDate(DateTime fechaFuturo)
        {
            DateTime today = new DateTime();
            if (fechaFuturo < today)
            {
                throw new ArgumentException($"La fecha {fechaFuturo} no es a futuro.");
            }
        }

        public static void ValidateOldDate(DateTime fechaPrevia)
        {
            DateTime today = new DateTime();
            if (fechaPrevia >= today)
            {
                throw new ArgumentException($"La fecha {fechaPrevia} no sucedió todavía.");
            }
        }
    }
}

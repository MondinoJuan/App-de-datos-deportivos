using Frontend.Resources.DataAccess;
using Frontend.Resources;
using Frontend.Resources.DTOs;
using Frontend.Resources.Modelos;

namespace Frontend.Resources;

public class SpecialServices
{
    public static bool CleanDatabase()
    {
        using (var db = new StatBoard_DbContext())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return true;
        }
    }

    public static List<Player> GetPlayersOfATeam(List<int> playerIds)
    {
        return playerIds.Select(id => Services.GetPlayer(id)).Where(player => player != null).ToList();
    }

    public static PlayerMatch GetPlayerMatchWithIdPlayer(int idPlayer)
    {
        using (var db = new StatBoard_DbContext())
        {
            var lista = db.PlayerMatches.ToList();
            return lista.Find(pm => pm.IdPlayer == idPlayer);
        }
    }

}

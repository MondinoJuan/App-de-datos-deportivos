using Frontend.Resources.DataAccess;
using Frontend.Resources;

namespace Frontend.Resources;

public class SpecialServices
{
    public static int GetLastAction()
    {
        using (var db = new StatBoard_DbContext())
        {
            var lista = db.PlayerActions.ToList();

            return lista.Last().Id;
        }
    }

    public static int GetLastTeam()
    {
        using (var db = new StatBoard_DbContext())
        {
            var lista = db.Clubes.ToList();

            return lista.Last().Id;
        }
    }

}

using Frontend.Resources.DataAccess;
using Frontend.Resources.Modelos;
using Frontend.Resources.DTOs;

namespace Frontend.Resources;

public class Services
{
    // Clubes
    public static int AddClub(Club_Dto clubDTO)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var club = new Club
                {
                    Name = clubDTO.Name,
                    IdPlayers = clubDTO.IdPlayers
                };

                db.Clubes.Add(club);
                db.SaveChanges();
                return club.Id;
            }
        }
        catch
        {
            return -1; // Indica un error
        }
    }

    public static List<Club> GetClubs()
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Clubes.ToList();
        }
    }

    public static Club GetClub(int id)
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Clubes.Find(id);
        }
    }

    public static bool UpdateClub(Club club)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                db.Clubes.Update(club);
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool DeleteClub(int id)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var club = db.Clubes.Find(id);
                if (club == null) return false; 
                db.Clubes.Remove(club);
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // Matches
    public static int AddMatch(Match_Dto matchDTO)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var match = new Match
                {
                    Date = matchDTO.Date,
                    Place = matchDTO.Place,
                    State = matchDTO.State,
                    MatchWeek = matchDTO.MatchWeek,
                    Tournament = matchDTO.Tournament,
                    IdTeamLocal = matchDTO.IdTeamLocal,
                    GoalsTeamA = matchDTO.GoalsTeamA,
                    IdTeamAway = matchDTO.IdTeamAway,
                    GoalsTeamB = matchDTO.GoalsTeamB
                };

                db.Matches.Add(match);
                db.SaveChanges();
                return match.Id;
            }
        }
        catch
        {
            return -1; // Indica un error
        }
    }

    public static List<Match> GetMatches()
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Matches.ToList();
        }
    }

    public static Match GetMatch(int id)
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Matches.Find(id);
        }
    }

    public static bool UpdateMatch(Match match)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                db.Matches.Update(match);
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool DeleteMatch(int id)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var match = db.Matches.Find(id);
                if (match == null) return false; 
                db.Matches.Remove(match);
                db.SaveChanges();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // Players
    public static int AddPlayer(Player_Dto playerDTO)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var player = new Player
                {
                    Name = playerDTO.Name,
                    Number = playerDTO.Number
                };

                db.Players.Add(player);
                db.SaveChanges();
                return player.Id;
            }
        }
        catch
        {
            return -1; // Indica un error
        }
    }

    public static List<Player> GetPlayers()
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Players.ToList();
        }
    }

    public static Player GetPlayer(int id)
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Players.Find(id);
        }
    }

    public static bool UpdatePlayer(Player player)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                db.Players.Update(player);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool DeletePlayer(int id)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var player = db.Players.Find(id);

                if (player == null)
                    return false;

                db.Players.Remove(player);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }


    // PlayerActions
    public static int AddPlayerAction(PlayerAction_Dto playerActionDTO)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var playerAction = new PlayerAction
                {
                    WhichHalf = playerActionDTO.WhichHalf,
                    EndingA = playerActionDTO.EndingA,
                    ActionPositionX = playerActionDTO.ActionPositionX,
                    ActionPositionY = playerActionDTO.ActionPositionY,
                    DefinitionPlaceX = playerActionDTO.DefinitionPlaceX,
                    DefinitionPlaceY = playerActionDTO.DefinitionPlaceY,
                    SanctionA = playerActionDTO.SanctionA
                };

                db.PlayerActions.Add(playerAction);
                db.SaveChanges();
                return playerAction.Id;
            }
        }
        catch
        {
            return -1; // Indica un error
        }
    }

    public static List<PlayerAction> GetPlayerActions()
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.PlayerActions.ToList();
        }
    }

    public static PlayerAction GetPlayerAction(int id)
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.PlayerActions.Find(id);
        }
    }

    public static bool UpdatePlayerAction(PlayerAction playerAction)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                db.PlayerActions.Update(playerAction);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool DeletePlayerAction(int id)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var playerAction = db.PlayerActions.Find(id);

                if (playerAction == null)
                    return false;

                db.PlayerActions.Remove(playerAction);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }


    // PlayerMatches
    public static int AddPlayerMatch(PlayerMatch_Dto playerMatchDTO)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var playerMatch = new PlayerMatch
                {
                    IdPlayer = playerMatchDTO.IdPlayer,
                    IdMatch = playerMatchDTO.IdMatch,
                    IdActions = playerMatchDTO.IdActions ?? new List<int>()
                };

                db.PlayerMatches.Add(playerMatch);
                db.SaveChanges();
                return playerMatch.Id;
            }
        }
        catch
        {
            return -1; // Indica un error
        }
    }

    public static List<PlayerMatch> GetPlayerMatches()
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.PlayerMatches.ToList();
        }
    }

    public static PlayerMatch GetPlayerMatch(int id)
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.PlayerMatches.Find(id);
        }
    }

    public static bool UpdatePlayerMatch(PlayerMatch playerMatch)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                db.PlayerMatches.Update(playerMatch);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool DeletePlayerMatch(int id)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var playerMatch = db.PlayerMatches.Find(id);

                if (playerMatch == null)
                    return false;

                db.PlayerMatches.Remove(playerMatch);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    // Tournaments
    public static int AddTournament(Tournament_Dto tournamentDTO)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var tournament = new Tournament
                {
                    Name = tournamentDTO.Name,
                    Cupo = tournamentDTO.Cupo
                };

                db.Tournaments.Add(tournament);
                db.SaveChanges();
                return tournament.Id;
            }
        }
        catch
        {
            return -1; // Indica un error
        }
    }

    public static List<Tournament> GetTournaments()
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Tournaments.ToList();
        }
    }

    public static Tournament GetTournament(int id)
    {
        using (var db = new StatBoard_DbContext())
        {
            return db.Tournaments.Find(id);
        }
    }

    public static bool UpdateTournament(Tournament tournament)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                db.Tournaments.Update(tournament);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public static bool DeleteTournament(int id)
    {
        try
        {
            using (var db = new StatBoard_DbContext())
            {
                var tournament = db.Tournaments.Find(id);

                if (tournament == null)
                    return false;

                db.Tournaments.Remove(tournament);
                db.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }
}

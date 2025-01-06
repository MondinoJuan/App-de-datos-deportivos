using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Entities
{
    public class Simulo_BdD
    {
        public List<Club_Dto> Clubs { get; set; } = new List<Club_Dto>();
        public List<Match_Dto> Matches { get; set; } = new List<Match_Dto>();
        public List<Player_Dto> Players { get; set; } = new List<Player_Dto>();
        public List<PlayerAction_Dto> Actions { get; set; } = new List<PlayerAction_Dto>();
        public List<PlayerMatch_Dto> PlayerMatches { get; set; } = new List<PlayerMatch_Dto>();
        public List<Tournament_Dto> Tournaments { get; set; } = new List<Tournament_Dto>();

        private static Simulo_BdD _database = new Simulo_BdD();

        public static Simulo_BdD Database => _database;


        //Clubes
        public static Result<bool> AddClub(Club_Dto club)
        {
            if (_database.Clubs.Any(c => c.Id == club.Id))
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"Ya existe un club con el Id: {club.Id}",
                    Data = false
                };
            }

            _database.Clubs.Add(club);
            return new Result<bool>
            {
                Success = true,
                Message = "Club agregado exitosamente",
                Data = true
            };
        }

        public static Result<bool> RemoveClub(Guid idClub)
        {
            var club = _database.Clubs.FirstOrDefault(c => c.Id == idClub);
            if (club == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un club con el Id: {idClub}",
                    Data = false
                };
            }

            _database.Clubs.Remove(club);
            return new Result<bool>
            {
                Success = true,
                Message = "Club eliminado exitosamente",
                Data = true
            };
        }

        public static Result<Club_Dto> GetOneClub(Guid Id)
        {
            var club = _database.Clubs.FirstOrDefault(c => c.Id == Id);
            if (club == null)
            {
                return new Result<Club_Dto>
                {
                    Success = false,
                    Message = $"No se encontró un club con el Id: {Id}",
                    Data = null
                };
            }
            return new Result<Club_Dto>
            {
                Success = true,
                Message = "Club encontrado",
                Data = club
            };
        }

        public static Result<List<Club_Dto>> GetAllClubs()
        {
            var clubes = _database.Clubs.ToList();
            if (clubes == null)
            {
                return new Result<List<Club_Dto>>
                {
                    Success = false,
                    Message = "No se encontraron clubes",
                    Data = null
                };
            }
            return new Result<List<Club_Dto>>
            {
                Success = true,
                Message = "Clubes encontrado",
                Data = clubes
            };
        }

        public static Result<bool> ReplaceClub(Club_Dto club)
        {
            var clubBuscado = _database.Clubs.FirstOrDefault(c => c.Id == club.Id);
            if (clubBuscado == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un club con el Id: {club.Id}",
                    Data = false
                };
            }

            int index = _database.Clubs.IndexOf(clubBuscado);
            if (index != -1)
            {
                _database.Clubs[index] = club;
            }

            return new Result<bool>
            {
                Success = true,
                Message = "Club modificado exitosamente",
                Data = true
            };
        }

        // Matches
        public static Result<bool> AddMatch(Match_Dto match)
        {
            if (_database.Matches.Any(m => m.Id == match.Id))
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"Ya existe un partido con el Id: {match.Id}",
                    Data = false
                };
            }

            _database.Matches.Add(match);
            return new Result<bool>
            {
                Success = true,
                Message = "Partido agregado exitosamente",
                Data = true
            };
        }

        public static Result<bool> RemoveMatch(Guid idMatch)
        {
            var match = _database.Matches.FirstOrDefault(m => m.Id == idMatch);
            if (match == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un partido con el Id: {idMatch}",
                    Data = false
                };
            }

            _database.Matches.Remove(match);
            return new Result<bool>
            {
                Success = true,
                Message = "Partido eliminado exitosamente",
                Data = true
            };
        }

        public static Result<Match_Dto> GetOneMatch(Guid id)
        {
            var match = _database.Matches.FirstOrDefault(m => m.Id == id);
            if (match == null)
            {
                return new Result<Match_Dto>
                {
                    Success = false,
                    Message = $"No se encontró un partido con el Id: {id}",
                    Data = null
                };
            }
            return new Result<Match_Dto>
            {
                Success = true,
                Message = "Partido encontrado",
                Data = match
            };
        }

        public static Result<List<Match_Dto>> GetAllMatches()
        {
            var matches = _database.Matches.ToList();
            if (matches == null || !matches.Any())
            {
                return new Result<List<Match_Dto>>
                {
                    Success = false,
                    Message = "No se encontraron partidos",
                    Data = null
                };
            }
            return new Result<List<Match_Dto>>
            {
                Success = true,
                Message = "Partidos encontrados",
                Data = matches
            };
        }

        public static Result<bool> ReplaceMatch(Match_Dto match)
        {
            var matchBuscado = _database.Matches.FirstOrDefault(m => m.Id == match.Id);
            if (matchBuscado == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un partido con el Id: {match.Id}",
                    Data = false
                };
            }

            int index = _database.Matches.IndexOf(matchBuscado);
            if (index != -1)
            {
                _database.Matches[index] = match;
            }

            return new Result<bool>
            {
                Success = true,
                Message = "Partido modificado exitosamente",
                Data = true
            };
        }


        // Players
        public static Result<bool> AddPlayer(Player_Dto player)
        {
            if (_database.Players.Any(p => p.Id == player.Id))
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"Ya existe un jugador con el Id: {player.Id}",
                    Data = false
                };
            }

            _database.Players.Add(player);
            return new Result<bool>
            {
                Success = true,
                Message = "Jugador agregado exitosamente",
                Data = true
            };
        }

        public static Result<bool> RemovePlayer(Guid idPlayer)
        {
            var player = _database.Players.FirstOrDefault(p => p.Id == idPlayer);
            if (player == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un jugador con el Id: {idPlayer}",
                    Data = false
                };
            }

            _database.Players.Remove(player);
            return new Result<bool>
            {
                Success = true,
                Message = "Jugador eliminado exitosamente",
                Data = true
            };
        }

        public static Result<Player_Dto> GetOnePlayer(Guid id)
        {
            var player = _database.Players.FirstOrDefault(p => p.Id == id);
            if (player == null)
            {
                return new Result<Player_Dto>
                {
                    Success = false,
                    Message = $"No se encontró un jugador con el Id: {id}",
                    Data = null
                };
            }
            return new Result<Player_Dto>
            {
                Success = true,
                Message = "Jugador encontrado",
                Data = player
            };
        }

        public static Result<List<Player_Dto>> GetAllPlayers()
        {
            var players = _database.Players.ToList();
            if (players == null || !players.Any())
            {
                return new Result<List<Player_Dto>>
                {
                    Success = false,
                    Message = "No se encontraron jugadores",
                    Data = null
                };
            }
            return new Result<List<Player_Dto>>
            {
                Success = true,
                Message = "Jugadores encontrados",
                Data = players
            };
        }

        public static Result<bool> ReplacePlayer(Player_Dto player)
        {
            var playerBuscado = _database.Players.FirstOrDefault(p => p.Id == player.Id);
            if (playerBuscado == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un jugador con el Id: {player.Id}",
                    Data = false
                };
            }

            int index = _database.Players.IndexOf(playerBuscado);
            if (index != -1)
            {
                _database.Players[index] = player;
            }

            return new Result<bool>
            {
                Success = true,
                Message = "Jugador modificado exitosamente",
                Data = true
            };
        }

        // PlayerActions
        public static Result<bool> AddAction(PlayerAction_Dto action)
        {
            if (_database.Actions.Any(a => a.Id == action.Id))
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"Ya existe una acción con el Id: {action.Id}",
                    Data = false
                };
            }

            _database.Actions.Add(action);
            return new Result<bool>
            {
                Success = true,
                Message = "Acción agregada exitosamente",
                Data = true
            };
        }

        public static Result<bool> RemoveAction(Guid idAction)
        {
            var action = _database.Actions.FirstOrDefault(a => a.Id == idAction);
            if (action == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró una acción con el Id: {idAction}",
                    Data = false
                };
            }

            _database.Actions.Remove(action);
            return new Result<bool>
            {
                Success = true,
                Message = "Acción eliminada exitosamente",
                Data = true
            };
        }

        public static Result<PlayerAction_Dto> GetOneAction(Guid id)
        {
            var action = _database.Actions.FirstOrDefault(a => a.Id == id);
            if (action == null)
            {
                return new Result<PlayerAction_Dto>
                {
                    Success = false,
                    Message = $"No se encontró una acción con el Id: {id}",
                    Data = null
                };
            }
            return new Result<PlayerAction_Dto>
            {
                Success = true,
                Message = "Acción encontrada",
                Data = action
            };
        }

        public static Result<List<PlayerAction_Dto>> GetAllActions()
        {
            var actions = _database.Actions.ToList();
            if (actions == null || !actions.Any())
            {
                return new Result<List<PlayerAction_Dto>>
                {
                    Success = false,
                    Message = "No se encontraron acciones",
                    Data = null
                };
            }
            return new Result<List<PlayerAction_Dto>>
            {
                Success = true,
                Message = "Acciones encontradas",
                Data = actions
            };
        }

        public static Result<bool> ReplaceAction(PlayerAction_Dto action)
        {
            var actionBuscada = _database.Actions.FirstOrDefault(a => a.Id == action.Id);
            if (actionBuscada == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró una acción con el Id: {action.Id}",
                    Data = false
                };
            }

            int index = _database.Actions.IndexOf(actionBuscada);
            if (index != -1)
            {
                _database.Actions[index] = action;
            }

            return new Result<bool>
            {
                Success = true,
                Message = "Acción modificada exitosamente",
                Data = true
            };
        }

        // PlayerMatch
        public static Result<bool> AddPlayerMatch(PlayerMatch_Dto playerMatch)
        {
            if (_database.PlayerMatches.Any(pm => pm.Id == playerMatch.Id))
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"Ya existe una relación jugador-partido con el Id: {playerMatch.Id}",
                    Data = false
                };
            }

            _database.PlayerMatches.Add(playerMatch);
            return new Result<bool>
            {
                Success = true,
                Message = "Relación jugador-partido agregada exitosamente",
                Data = true
            };
        }

        public static Result<bool> RemovePlayerMatch(Guid idPlayerMatch)
        {
            var playerMatch = _database.PlayerMatches.FirstOrDefault(pm => pm.Id == idPlayerMatch);
            if (playerMatch == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró una relación jugador-partido con el Id: {idPlayerMatch}",
                    Data = false
                };
            }

            _database.PlayerMatches.Remove(playerMatch);
            return new Result<bool>
            {
                Success = true,
                Message = "Relación jugador-partido eliminada exitosamente",
                Data = true
            };
        }

        public static Result<PlayerMatch_Dto> GetOnePlayerMatch(Guid id)
        {
            var playerMatch = _database.PlayerMatches.FirstOrDefault(pm => pm.Id == id);
            if (playerMatch == null)
            {
                return new Result<PlayerMatch_Dto>
                {
                    Success = false,
                    Message = $"No se encontró una relación jugador-partido con el Id: {id}",
                    Data = null
                };
            }

            return new Result<PlayerMatch_Dto>
            {
                Success = true,
                Message = "Relación jugador-partido encontrada",
                Data = playerMatch
            };
        }

        public static Result<List<PlayerMatch_Dto>> GetAllPlayerMatches()
        {
            var playerMatches = _database.PlayerMatches.ToList();
            if (playerMatches.Count == 0)
            {
                return new Result<List<PlayerMatch_Dto>>
                {
                    Success = false,
                    Message = "No se encontraron relaciones jugador-partido",
                    Data = null
                };
            }

            return new Result<List<PlayerMatch_Dto>>
            {
                Success = true,
                Message = "Relaciones jugador-partido encontradas",
                Data = playerMatches
            };
        }

        public static Result<bool> ReplacePlayerMatch(PlayerMatch_Dto playerMatch)
        {
            var playerMatchBuscado = _database.PlayerMatches.FirstOrDefault(pm => pm.Id == playerMatch.Id);
            if (playerMatchBuscado == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró una relación jugador-partido con el Id: {playerMatch.Id}",
                    Data = false
                };
            }

            int index = _database.PlayerMatches.IndexOf(playerMatchBuscado);
            if (index != -1)
            {
                _database.PlayerMatches[index] = playerMatch;
            }

            return new Result<bool>
            {
                Success = true,
                Message = "Relación jugador-partido modificada exitosamente",
                Data = true
            };
        }

        // Tournaments
        public static Result<bool> AddTournament(Tournament_Dto tournament)
        {
            if (_database.Tournaments.Any(t => t.Id == tournament.Id))
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"Ya existe un torneo con el Id: {tournament.Id}",
                    Data = false
                };
            }

            _database.Tournaments.Add(tournament);
            return new Result<bool>
            {
                Success = true,
                Message = "Torneo agregado exitosamente",
                Data = true
            };
        }

        public static Result<bool> RemoveTournament(Guid idTournament)
        {
            var tournament = _database.Tournaments.FirstOrDefault(t => t.Id == idTournament);
            if (tournament == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un torneo con el Id: {idTournament}",
                    Data = false
                };
            }

            _database.Tournaments.Remove(tournament);
            return new Result<bool>
            {
                Success = true,
                Message = "Torneo eliminado exitosamente",
                Data = true
            };
        }

        public static Result<Tournament_Dto> GetOneTournament(Guid id)
        {
            var tournament = _database.Tournaments.FirstOrDefault(t => t.Id == id);
            if (tournament == null)
            {
                return new Result<Tournament_Dto>
                {
                    Success = false,
                    Message = $"No se encontró un torneo con el Id: {id}",
                    Data = null
                };
            }
            return new Result<Tournament_Dto>
            {
                Success = true,
                Message = "Torneo encontrado",
                Data = tournament
            };
        }

        public static Result<List<Tournament_Dto>> GetAllTournaments()
        {
            var tournaments = _database.Tournaments.ToList();
            if (tournaments == null || !tournaments.Any())
            {
                return new Result<List<Tournament_Dto>>
                {
                    Success = false,
                    Message = "No se encontraron torneos",
                    Data = null
                };
            }
            return new Result<List<Tournament_Dto>>
            {
                Success = true,
                Message = "Torneos encontrados",
                Data = tournaments
            };
        }

        public static Result<bool> ReplaceTournament(Tournament_Dto tournament)
        {
            var tournamentBuscado = _database.Tournaments.FirstOrDefault(t => t.Id == tournament.Id);
            if (tournamentBuscado == null)
            {
                return new Result<bool>
                {
                    Success = false,
                    Message = $"No se encontró un torneo con el Id: {tournament.Id}",
                    Data = false
                };
            }

            int index = _database.Tournaments.IndexOf(tournamentBuscado);
            if (index != -1)
            {
                _database.Tournaments[index] = tournament;
            }

            return new Result<bool>
            {
                Success = true,
                Message = "Torneo modificado exitosamente",
                Data = true
            };
        }
    }
}

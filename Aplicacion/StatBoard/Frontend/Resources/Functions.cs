using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources
{
    class Functions
    {
        public static Ending? StringAEnding(string e)
        {
            string selectedAction = "";
            switch (e)
            {
                case "Gol":
                    selectedAction = "Goal";
                    break;
                case "Foul":
                    selectedAction = "Foul";
                    break;
                case "Atajada":
                    selectedAction = "Save";
                    break;
                case "Errada":
                    selectedAction = "Miss";
                    break;
                case "Perdida":
                    selectedAction = "Steal_L";
                    break;
                case "Robo":
                    selectedAction = "Steal_W";
                    break;
                case "Bloqueo":
                    selectedAction = "Blocked";
                    break;
                default:
                    break;
            }
            if (Enum.TryParse(selectedAction, out Ending actionValue))
            {
                return actionValue;
            }

            return null;
        }

        public static string EndingAString (Ending e)
        {
            string selectedAction = "";
            switch (e)
            {
                case Ending.Goal:
                    selectedAction = "Gol";
                    break;
                case Ending.Foul:
                    selectedAction = "Foul";
                    break;
                case Ending.Save:
                    selectedAction = "Atajada";
                    break;
                case Ending.Miss:
                    selectedAction = "Errada";
                    break;
                case Ending.Steal_L:
                    selectedAction = "Perdida";
                    break;
                case Ending.Steal_W:
                    selectedAction = "Robo";
                    break;
                case Ending.Blocked:
                    selectedAction = "Bloqueo";
                    break;
                default:
                    break;
            }
            return selectedAction;
        }

        public static EventsData GetActionCountForPlayer(int playerId, Ending actionType)
        {
            var eventData = new EventsData()
            {
                CooField = new List<Coordenates>(),
                QuantityEnding = 0,
                Red = 0,
                Blue = 0,
                CooGoal = new List<Coordenates>(),
                Quantity2min = 0,
                Success = false
            };
            var result = Services.GetPlayerMatches();
            if (result == null) return eventData;
            var playerMatch = result.FirstOrDefault(a => a.IdPlayer == playerId);
            if (playerMatch?.IdActions == null) return eventData;


            foreach (var idAction in playerMatch.IdActions)
            {
                var actionResult = Services.GetPlayerAction(idAction);
                if (actionResult == null) continue;

                var action = actionResult;

                // Contar las acciones del tipo específico
                if (action.EndingA == actionType)
                {
                    // Guardar las coordenadas
                    eventData.CooField.Add(new Coordenates
                    {
                        X = action.ActionPositionX,
                        Y = action.ActionPositionY
                    });

                    // Guardar la posición de definición (si es válida)
                    if (action.DefinitionPlaceX != 0 && action.DefinitionPlaceY != 0 && action.DefinitionPlaceX != null && action.DefinitionPlaceY != null)
                    {
                        eventData.CooGoal.Add(new Coordenates
                        {
                            X = action.DefinitionPlaceX,
                            Y = action.DefinitionPlaceY
                        });
                    }

                    // Contar las sanciones
                    if (action.SanctionA == Sanction.Red)
                    {
                        eventData.Red++;
                    }
                    else if (action.SanctionA == Sanction.Blue)
                    {
                        eventData.Blue++;
                    }
                    else if (action.SanctionA == Sanction.Two_Minutes)
                    {
                        eventData.Quantity2min++;
                    }

                    eventData.QuantityEnding++;
                }
            }
            eventData.Success = true;

            return eventData;
        }

        public static List<Coordenates> TranslateCoordenates(List<Coordenates> oldCoord, float xModifier, float yModifier)
        {
            var newCoord = new List<Coordenates>();

            foreach (var coord in oldCoord)
            {
                newCoord.Add(new Coordenates
                {
                    X = coord.X + xModifier,
                    Y = coord.Y + yModifier
                });
            }
            return newCoord;
        }
    }
}

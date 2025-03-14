using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources
{
    class Functions
    {
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
                    if (action.DefinitionPlaceX != 0 || action.DefinitionPlaceY != 0)
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

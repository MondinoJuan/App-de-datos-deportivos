using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Resources.Entities;

namespace Frontend.Resources
{
    class Functions
    {
        public static EventsData GetActionCountForPlayer(Guid playerId, Ending actionType)
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
            var result = Simulo_BdD.GetAllPlayerMatches();
            if (!result.Success) return eventData;
            var playerMatch = result.Data.FirstOrDefault(a => a.IdPlayer == playerId);
            if (playerMatch?.IdActions == null) return eventData;


            foreach (var idAction in playerMatch.IdActions)
            {
                var actionResult = Simulo_BdD.GetOneAction(idAction);
                if (!actionResult.Success) continue;

                var action = actionResult.Data;

                // Contar las acciones del tipo específico
                if (action.Ending == actionType)
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
                    if (action.Sanction == Sanction.Red)
                    {
                        eventData.Red++;
                    }
                    else if (action.Sanction == Sanction.Blue)
                    {
                        eventData.Blue++;
                    }
                    else if (action.Sanction == Sanction.Two_Minutes)
                    {
                        eventData.Quantity2min++;
                    }

                    eventData.QuantityEnding++;
                }
            }
            eventData.Success = true;

            return eventData;
        }
    }
}

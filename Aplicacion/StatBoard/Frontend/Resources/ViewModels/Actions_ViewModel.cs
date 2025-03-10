using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using Frontend.Resources.DTOs;
using Frontend.Resources.DataAccess;
using Frontend.Resources.Utilidades;
using Frontend.Resources.Modelos;
using System.Threading.Tasks;

namespace Frontend.Resources.ViewModels
{
    public partial class Actions_ViewModel : ObservableObject, IQueryAttributable
    {
        private readonly PlayerAction_DbContext _dbContext;

        [ObservableProperty]
        private PlayerAction_Dto action_Dto = new PlayerAction_Dto();

        private int IdAction;

        [ObservableProperty]
        private bool loading = false;

        [ObservableProperty]
        private string tituloPagina;

        public Actions_ViewModel(PlayerAction_DbContext context)
        {
            _dbContext = context;
            action_Dto.WhichHalf = false;
        }


        public async Task ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());

            IdAction = id;

            if(IdAction == 0)
            {
                TituloPagina = "Crear Acción";
            }
            else
            {
                TituloPagina = "Editar Acción";
                Loading = true;

                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.PlayerActions.FirstAsync(pa => pa.Id == IdAction);

                    Action_Dto.IdPlayerAction = encontrado.Id;
                    Action_Dto.WhichHalf = encontrado.WhichHalf;
                    Action_Dto.ActionPositionX = encontrado.ActionPositionX;
                    Action_Dto.ActionPositionY = encontrado.ActionPositionY;
                    Action_Dto.DefinitionPlaceX = encontrado.DefinitionPlaceX;
                    Action_Dto.DefinitionPlaceY = encontrado.DefinitionPlaceY;
                    Action_Dto.Description = encontrado.Description;
                    Action_Dto.Sanction = encontrado.Sanction;
                    Action_Dto.Ending = encontrado.Ending;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Loading = false;
                    });
                });
            }
        }

        [RelayCommand]
        private async Task SaveAction()
        {
            Loading = true;
            ActionMessage mensaje = new ActionMessage();

            await Task.Run(async () =>
            {
                PlayerAction playerAction = new PlayerAction
                {
                    Id = Action_Dto.IdPlayerAction,
                    WhichHalf = Action_Dto.WhichHalf,
                    ActionPositionX = Action_Dto.ActionPositionX,
                    ActionPositionY = Action_Dto.ActionPositionY,
                    DefinitionPlaceX = Action_Dto.DefinitionPlaceX,
                    DefinitionPlaceY = Action_Dto.DefinitionPlaceY,
                    Description = Action_Dto.Description,
                    Sanction = Action_Dto.Sanction,
                    Ending = Action_Dto.Ending
                };
                if (playerAction.Id == 0)
                {
                    _dbContext.PlayerActions.Add(playerAction);
                }
                else
                {
                    _dbContext.PlayerActions.Update(playerAction);
                }
                await _dbContext.SaveChangesAsync();
                mensaje.EsCrear = playerAction.Id == 0;
                mensaje.PlayerAction = Action_Dto;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Loading = false;

                    WeakReferenceMessenger.Default.Send(new ActionMensajeria(mensaje));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }
    }
}

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
    public partial class Players_ViewModel : ObservableObject, IQueryAttributable
    {
        private readonly Player_DbContext _dbContext;

        [ObservableProperty]
        private Player_Dto player_Dto = new Player_Dto();

        private int IdPlayer;

        [ObservableProperty]
        private bool loading = false;

        [ObservableProperty]
        private string tituloPagina;

        public Players_ViewModel(Player_DbContext context)
        {
            _dbContext = context;
        }


        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());

            IdPlayer = id;

            if(IdPlayer == 0)
            {
                TituloPagina = "Crear Acción";
            }
            else
            {
                TituloPagina = "Editar Acción";
                Loading = true;

                await Task.Run(async () =>
                {
                    var encontrado = await _dbContext.Players.FirstAsync(pa => pa.Id == IdPlayer);

                    Player_Dto.IdPlayer = encontrado.Id;
                    Player_Dto.Number = encontrado.Number;
                    Player_Dto.Name = encontrado.Name;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Loading = false;
                    });
                });
            }
        }

        [RelayCommand]
        private async Task SavePlayer()
        {
            Loading = true;
            PlayerMessage mensaje = new PlayerMessage();

            await Task.Run(async () =>
            {
                Player player = new Player
                {
                    Id = Player_Dto.IdPlayer,
                    Number = Player_Dto.Number,
                    Name = Player_Dto.Name
                };
                if (player.Id == 0)
                {
                    _dbContext.Players.Add(player);
                }
                else
                {
                    _dbContext.Players.Update(player);
                }
                await _dbContext.SaveChangesAsync();
                mensaje.EsCrear = player.Id == 0;
                mensaje.Player = Player_Dto;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Loading = false;

                    WeakReferenceMessenger.Default.Send(new PlayerMensajeria(mensaje));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }
    }
}

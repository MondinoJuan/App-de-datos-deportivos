using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

using Microsoft.EntityFrameworkCore;
using Frontend.Resources.DTOs;
using Frontend.Resources.DataAccess;
using Frontend.Resources.Utilidades;
using Frontend.Resources.Modelos;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Frontend.Pages;

// Luego cambiar el nombre porque no es el viewModel del Main, sino que de la pagina ActionsPage.

namespace Frontend.Resources.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Player_DbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Player_Dto> listaPlayers = new ObservableCollection<Player_Dto>();

        public MainViewModel(Player_DbContext context)
        {
            _dbContext = context;

            MainThread.BeginInvokeOnMainThread(new Action(async () =>
            {
                await GetAll();
            }));

            WeakReferenceMessenger.Default.Register<PlayerMensajeria>(this, (r, m) =>
            {
                PlayerMensajeRecibido(m.Value);
            });
        }

        public async Task GetAll()
        {
            var list = await _dbContext.Players.ToListAsync();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    ListaPlayers.Add(new Player_Dto
                    {
                        IdPlayer = item.Id,
                        Name = item.Name,
                        Number = item.Number
                    });
                }
            }
        }

        private void PlayerMensajeRecibido(PlayerMessage playerMessage)
        {
            var playerDTO = playerMessage.Player;

            if (playerMessage.EsCrear)
            {
                ListaPlayers.Add(playerDTO);
            }
            else
            {
                var encontrado = ListaPlayers.First(p => p.IdPlayer == playerDTO.IdPlayer);

                encontrado.Name = playerDTO.Name;
                encontrado.Number = playerDTO.Number;

            }
        }

        [RelayCommand]
        private async Task CrearPlayer()
        {
            var uri = $"{nameof(PlayerPage)}?id=0";

            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task EditarPlayer(Player_Dto playerDTO)
        {
            var uri = $"{nameof(PlayerPage)}?id={playerDTO.IdPlayer}";

            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task BorrarPlayer(Player_Dto playerDTO)
        {
            bool answer = await Shell.Current
                .DisplayAlert("Mensaje", "¿Estás seguro de que quieres borrar este jugador?", "Sí", "No");

            if (answer)
            {
                var encontrado = await _dbContext.Players.FirstAsync(p => p.Id == playerDTO.IdPlayer);

                _dbContext.Players.Remove(encontrado);
                await _dbContext.SaveChangesAsync();
                ListaPlayers.Remove(playerDTO);
            }
        }
    }
}

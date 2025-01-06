using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using Frontend.Resources.Components;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Frontend;

public partial class MatchView : ContentPage
{
    private Match_Dto Match {  get; set; }
    private Club_Dto LocalTeam {  get; set; }
    private Club_Dto AwayTeam {  get; set; }

    public MatchView(Match_Dto match)
    {
        InitializeComponent();
        BindingContext = this;
        Match = match;

        lblMatchViewTitle.Text = "Partido de la fecha " + Match.MatchWeek.ToString() + " el día " + Match.Date.ToString();
        lblTournament.Text = Match.Tournament;

        var buscoTeamLocal = Simulo_BdD.GetOneClub(Match.IdTeamLocal);
        Console.WriteLine(buscoTeamLocal.Message);
        if (buscoTeamLocal.Success)
        {
            LocalTeam = buscoTeamLocal.Data;
            lblTeamLocal.Text = LocalTeam.Name;
        }

        var buscoTeamAway = Simulo_BdD.GetOneClub(Match.IdTeamAway);
        Console.WriteLine(buscoTeamAway.Message);
        if (buscoTeamAway.Success)
        {
            AwayTeam = buscoTeamAway.Data;
            lblTeamAway.Text = AwayTeam.Name;
        }
    }

    private async void OnAddPlayer(object sender, EventArgs e)
    {
        var newPlayer = new Player_Dto();
        newPlayer.Id = Guid.NewGuid();
        var result = await OpenAddPlayerModal(newPlayer);   // result sirve para guardar el jugador unicamente si se completaron
                                                            // todos los datos y la persona que los cargaba no cancelo dicha carga.
        if (result == 1)
        {
            var resultA = Simulo_BdD.AddPlayer(newPlayer);
            Console.WriteLine(resultA.Message);

            LocalTeam.IdPlayers.Add(newPlayer.Id);
            CreatePlayerMatch(newPlayer.Id, Match.Id);
        }
        else if (result == 2)
        {
            var resultB = Simulo_BdD.AddPlayer(newPlayer);
            Console.WriteLine(resultB.Message);

            AwayTeam.IdPlayers.Add(newPlayer.Id);
            CreatePlayerMatch(newPlayer.Id, Match.Id);
        }
        else if(result == 0)
        {
            Console.WriteLine("Se cancelo la carga de un jugador");
        }
    }

    private async Task<int> OpenAddPlayerModal(Player_Dto player)
    {
        var modalPage = new CreatePlayerModal(player);
        await Navigation.PushModalAsync(modalPage);
        return await modalPage.GetResultAsync();
    }

    private async void OnAddAction(object sender, EventArgs e)
    {
        // Abrir pagina modal para agregar una acción.
        // Esta creada la content page de ActionCreate, podria cambiarla a modal.

        var newAction = new PlayerAction_Dto();
        newAction.Id = Guid.NewGuid();
        var result = await OpenAddActionModal(newAction);

        if (result)
        {
            var resultA = Simulo_BdD.AddAction(newAction);
            Console.WriteLine(resultA.Message);
            if (resultA.Success)
            {

            }
        }
        else
        {

        }
    }

    private async Task<bool> OpenAddActionModal(PlayerAction_Dto newAction)
    {
        var modalPage = new ActionCreate(newAction);
        await Navigation.PushModalAsync(modalPage);
        return await modalPage.GetResultAsync();
    }

    private void CreatePlayerMatch(Guid idPlayer, Guid idMatch)
    {
        var newPlayerMatch = new PlayerMatch_Dto();
        newPlayerMatch.Id = Guid.NewGuid();
        newPlayerMatch.IdPlayer = idPlayer;
        newPlayerMatch.IdMatch = idMatch;
        
        var result = Simulo_BdD.AddPlayerMatch(newPlayerMatch);
        Console.WriteLine(result.Message);
    }

    private void OnFinish()
    {
        // Finalizar el partido, deberia armar el PDF en esta función.
    }
}
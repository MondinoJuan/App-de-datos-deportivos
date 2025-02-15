using Frontend.Resources.Entities;
using Frontend.Resources;
using Microsoft.Maui.Layouts;

namespace Frontend.Resources.Components;

public partial class PageOfActions : ContentView
{
    public static readonly BindableProperty IdPlayerProperty =
        BindableProperty.Create(nameof(IdPlayer), typeof(Guid), typeof(PageOfActions));

    // Cuando se asigne un jugador, se carga su información
    public Guid IdPlayer
    {
        get => (Guid)GetValue(IdPlayerProperty);
        set
        {
            SetValue(IdPlayerProperty, value);
            if (value != null)
            {
                var result = Simulo_BdD.GetOnePlayer(value);
                if (!result.Success) return;
                lblTitle.Text = result.Data.Number + " " + result.Data.Name;
                LoadDataPlayer(result.Data);
            }
        }
    }

    public static readonly BindableProperty TeamProperty =
        BindableProperty.Create(nameof(Team), typeof(Club_Dto), typeof(PageOfActions));

    // Cuando se asigne un equipo, se carga su información
    public Club_Dto Team
    {
        get => (Club_Dto)GetValue(TeamProperty);
        set
        {
            SetValue(TeamProperty, value);
            if (value != null)
            {
                lblTitle.Text = value.Name;
                LoadDataTeam(value.IdPlayers);
            }
        }
    }

    public PageOfActions()
    {
        InitializeComponent();
        this.BindingContext = this;
        
        // Escuchar cambios en el BindingContext

        this.BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object? sender, EventArgs? e)
    {
        base.OnBindingContextChanged(); // Siempre es recomendable llamar al método base

        if (BindingContext is PageOfActions pageOfActions)
        {
            // Verifica si se ha asignado un jugador y actualiza sus datos
            if (pageOfActions.IdPlayer != Guid.Empty)
            {
                Console.WriteLine($"Player ID: {pageOfActions.IdPlayer}");
                var result = Simulo_BdD.GetOnePlayer(pageOfActions.IdPlayer);
                if (result.Success)
                {
                    lblTitle.Text = result.Data.Number + " " + result.Data.Name;
                    LoadDataPlayer(result.Data);
                }
            }

            // Verifica si se ha asignado un equipo y actualiza sus datos
            if (pageOfActions.Team != null)
            {
                Console.WriteLine($"Team: {pageOfActions.Team.Name}");
                LoadDataTeam(pageOfActions.Team.IdPlayers);
            }
        }
    }


    private void LoadDataTeam(List<Guid> idPlayers)
    {
        lblTitle.Text = Team.Name;

        var cantidadBlockeds = 0;
        var cantidadGoals = 0;
        var cantidadSteals_W = 0;
        var cantidadSaves = 0;
        var cantidadSteals_L = 0;
        var cantidadMisses = 0;
        var cantidadFoules = 0;
        var cantidad2Minutos = 0;
        var cantidadRojas = 0;
        var cantidadAzules = 0;

        foreach (var idPlayer in idPlayers)
        {
            cantidadBlockeds += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Blocked);
            cantidadGoals += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Goal);
            cantidadSteals_W += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Steal_W);
            cantidadSaves += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Save);
            cantidadSteals_L += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Steal_L);
            cantidadMisses += GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Miss);
            var variableASeparar = GetQuantityAndPlaceOfActionForPlayer(idPlayer, Ending.Foul);
            cantidadFoules += variableASeparar / 1000;
            cantidad2Minutos += (variableASeparar % 1000) / 100;
            cantidadRojas += (variableASeparar % 100) / 10;
            cantidadAzules += variableASeparar % 10;
        }

        lblBlockeds.Text = cantidadBlockeds.ToString();
        lblGoals.Text = cantidadGoals.ToString();
        lblSteals_W.Text = cantidadSteals_W.ToString();
        lblSaves.Text = cantidadSaves.ToString();
        lblSteals_L.Text = cantidadSteals_L.ToString();
        lblMisses.Text = cantidadMisses.ToString();
        lblTwoMinutes.Text = cantidad2Minutos.ToString();
        lblRedCards.Text = cantidadRojas.ToString();
        lblBlueCards.Text = cantidadAzules.ToString();

    }

    private void LoadDataPlayer(Player_Dto player)
    {
        lblTitle.Text = player.Number + " " + player.Name;

        lblBlockeds.Text = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Blocked).ToString();
        lblGoals.Text = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Goal).ToString();
        lblSteals_W.Text = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Steal_W).ToString();
        lblSaves.Text = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Save).ToString();
        lblSteals_L.Text = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Steal_L).ToString();
        lblMisses.Text = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Miss).ToString();

        var variableASeparar = GetQuantityAndPlaceOfActionForPlayer(player.Id, Ending.Foul);
        var cantidadFoules = variableASeparar / 1000;
        var cantidad2Minutos = (variableASeparar % 1000) / 100;
        var cantidadRojas = (variableASeparar % 100) / 10;
        var cantidadAzules = variableASeparar % 10;
        lblBlockeds.Text = cantidadFoules.ToString();
        lblTwoMinutes.Text = cantidad2Minutos.ToString();
        lblRedCards.Text = cantidadRojas.ToString();
        lblBlueCards.Text = cantidadAzules.ToString();
    }

    private int GetQuantityAndPlaceOfActionForPlayer(Guid idPlayer, Ending ending)
    {
        var result = Functions.GetActionCountForPlayer(idPlayer, ending);
        if (!result.Success) return 0;

        var container = new AbsoluteLayout();

        switch (ending)
        {
            case Ending.Blocked:
                container = markContainerBlockeds;
                break;
            case Ending.Goal:
                container = markContainerGoalsField;
                break;
            case Ending.Steal_W:
                container = markContainerSteals_W;
                break;
            case Ending.Save:
                container = markContainerSavesField;
                break;
            case Ending.Steal_L:
                container = markContainerSteals_L;
                break;
            case Ending.Miss:
                container = markContainerMissesField;
                break;
            case Ending.Foul:
                container = markContainerFouls;
                break;
        }

        AddMarkToImage(container, result.CooField);

        if (ending == Ending.Goal || ending == Ending.Miss || ending == Ending.Save)
        {
            AddMarkToImage(markContainerGoalsGoal, result.CooGoal);
        }

        if (ending == Ending.Foul)
        {
            var cantidadFoules = result.QuantityEnding;
            var cantidadRojas = result.Red ?? 0;
            var cantidadAzules = result.Blue ?? 0;
            var cantidad2Minutos = result.Quantity2min ?? 0;

            var valorTransformado = cantidadFoules * 1000 + cantidad2Minutos * 100 + cantidadRojas * 10 + cantidadAzules;
            return valorTransformado;
        }

        return result.QuantityEnding;
    }

    private void AddMarkToImage(AbsoluteLayout container, List<Coordenates> coordenadas)
    {
        foreach (var coo in coordenadas)
        {
            // Crea una nueva marca (círculo)
            var circle = new BoxView
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Colors.Red,
                CornerRadius = 10,
                Opacity = 0.6
            };

            // Calcula la posición en la pantalla
            AbsoluteLayout.SetLayoutBounds(circle, new Rect(coo.X, coo.Y, 20, 20));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

            // Añade el círculo al contenedor de marcas
            container.Children.Add(circle);
        }
    }
}
using Frontend.Resources;
using Frontend.Resources.Modelos;
using Frontend.Resources.DTOs;
using Microsoft.Maui.Layouts;

namespace Frontend.Pages;

public partial class ActionsHUB : ContentPage
{
    private bool IsPlayerLocal { get; set; }
    private Player PlayerSeleccionado { get; set; } = new Player();
    private Ending EndingSeleccionado { get; set; } = new Ending();
    private Sanction? SanctionSeleccionada { get; set; }

    private float ActionPositionX { get; set; }
    private float ActionPositionY { get; set; }

    private float? DefinitionPlaceX { get; set; }
    private float? DefinitionPlaceY { get; set; }

    public ActionsHUB()
    {
        InitializeComponent();
    }

    private void OnPlayerAwayTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Player player)
        {
            IsPlayerLocal = false;
            PlayerSeleccionado = player;

            //DisplayAlert("Jugador seleccionado", $"Seleccionaste a {player.Name}", "OK");
        }
    }

    private void OnPlayerLocalTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Player player)
        {
            IsPlayerLocal = true;
            PlayerSeleccionado = player;

            //DisplayAlert("Jugador seleccionado", $"Seleccionaste a {player.Name}", "OK");
        }
    }

    private void OnActionPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedAction = "";
            switch (picker.SelectedItem.ToString())
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
                EndingSeleccionado = actionValue;
            }
        }
    }
    
    private void OnSanctionPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        if (picker != null && picker.SelectedItem != null)
        {
            string selectedSanction = "";
            switch (picker.SelectedItem.ToString())
            {
                case "2 Minutos":
                    selectedSanction = "Two_Minutes";
                    break;
                case "Roja":
                    selectedSanction = "Red";
                    break;
                case "Azul":
                    selectedSanction = "Blue";
                    break;
                default:
                    break;
            }
            if (Enum.TryParse(selectedSanction, out Sanction sanctionValue))
            {
                SanctionSeleccionada = sanctionValue;
            }
        }
    }

    private void OnImageTapped_Field(object sender, TappedEventArgs e)
    {
        MarkContainerField.Children.Clear();
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            ActionPositionX = (float)touchPosition.Value.X;
            ActionPositionY = (float)touchPosition.Value.Y;

            // Crea una nueva marca (círculo)
            var circle = new BoxView
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Colors.Red,
                CornerRadius = 10,
                Opacity = 1
            };

            var coordenadasReal = new List<Coordenates>
            {
                new Coordenates { X = (float)touchPosition.Value.X, Y = (float)touchPosition.Value.Y }
            };

            var coordenadasActualizadas = Functions.TranslateCoordenates(coordenadasReal, 30, -10);

            // Calcula la posición en la pantalla
            AbsoluteLayout.SetLayoutBounds(circle,
                new Rect(coordenadasActualizadas[0].X, coordenadasActualizadas[0].Y, 20, 20));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

            // Añade el círculo al contenedor de marcas
            MarkContainerField.Children.Add(circle);

            //ViewModel.DidFieldPlace = true;
        }
    }

    private void OnImageTapped_Goal(object sender, TappedEventArgs e)
    {
        MarkContainerGoal.Children.Clear();
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            DefinitionPlaceX = (float)touchPosition.Value.X;
            DefinitionPlaceY = (float)touchPosition.Value.Y;

            // Crea una nueva marca (círculo)
            var circle = new BoxView
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Colors.YellowGreen,
                CornerRadius = 10,
                Opacity = 1
            };

            var coordenadasReal = new List<Coordenates>
            {
                new Coordenates { X = (float)touchPosition.Value.X, Y = (float)touchPosition.Value.Y }
            };

            var coordenadasActualizadas = Functions.TranslateCoordenates(coordenadasReal, -7, 5);

            // Calcula la posición en la pantalla
            AbsoluteLayout.SetLayoutBounds(circle,
                new Rect(coordenadasActualizadas[0].X, coordenadasActualizadas[0].Y, 20, 20));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

            // Añade el círculo al contenedor de marcas
            MarkContainerGoal.Children.Add(circle);

            //ViewModel.DidGoalPlace = true;
        }
    }


    private void AgregarAccion(object sender, EventArgs e)
    {
        var nuevaAction = new PlayerAction_Dto
        {
            WhichHalf = swtHalfIndicator.IsToggled,
            EndingA = EndingSeleccionado,
            ActionPositionX = ActionPositionX,
            ActionPositionY = ActionPositionY,
            DefinitionPlaceX = DefinitionPlaceX,
            DefinitionPlaceY = DefinitionPlaceY,
            SanctionA = SanctionSeleccionada
        };

        var color = new Color();
        // Restaurar el color del último Label si ya hay alguno
        if (AccionesContainer.Children.Count > 0)
        {
            var ultimoLabel = AccionesContainer.Children.Last() as Label;
            if (ultimoLabel != null)
            {
                ultimoLabel.TextColor = Colors.Black;
            }
        }

        if (IsPlayerLocal)
        {
            color = Colors.Orange;
        }
        else
        {
            color = Colors.Blue;
        }

        // Crear el nuevo Label con el color resaltado
        var labelNuevaAccion = new Label
        {
            Text = $"{EndingSeleccionado} - {PlayerSeleccionado.Number} {PlayerSeleccionado.Name}",
            FontSize = 16,
            TextColor = color,
            HorizontalOptions = LayoutOptions.Start
        };

        // Agregar el nuevo Label a la lista
        AccionesContainer.Children.Add(labelNuevaAccion);
        Services.AddPlayerAction(nuevaAction);

        var actualizarPlayerMatch = SpecialServices.GetPlayerMatchWithIdPlayer(PlayerSeleccionado.Id);
        if (actualizarPlayerMatch != null)
        {
            actualizarPlayerMatch.IdActions.Add(SpecialServices.GetLastAction());
            Services.UpdatePlayerMatch(actualizarPlayerMatch);
        }
    }
}
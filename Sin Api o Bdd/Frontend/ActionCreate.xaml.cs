using Microsoft.Maui.Controls;
using Frontend.Resources;
using Frontend.Resources.Entities;
using System.Threading.Tasks;

namespace Frontend;

public partial class ActionCreate : ContentPage
{
    private PlayerAction_Dto action1 { get; set; }
    private TaskCompletionSource<bool> _taskCompletionSource;

    public ActionCreate(PlayerAction_Dto newAction)
	{
		InitializeComponent();
        action1 = newAction;
        BindingContext = this;
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            action1.Ending = (Ending)selectedIndex;
            DisplayAlert("Elemento Seleccionado", "Has seleccionado: " + action1.Ending, "OK");          // Cambiar, pero no me sale el nombre de la funcion o metodo.
        }
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        bool isToggled = e.Value;
        if (isToggled)
        {
            action1.WhichHalf = true;
            DisplayAlert("Interruptor", "El interruptor está activado", "OK");          // Cambiar, pero no me sale el nombre de la funcion o metodo.
        }
        else
        {
            action1.WhichHalf = false;
            DisplayAlert("Interruptor", "El interruptor está desactivado", "OK");          // Cambiar, pero no me sale el nombre de la funcion o metodo.
        }
    }

    private void OnBoxViewTapped_Field(object sender, TappedEventArgs e)
    {
        var touchPosition = e.GetPosition((VisualElement)sender);

        if (touchPosition is not null)
        {
            action1.ActionPositionX = touchPosition.Value.X;
            action1.ActionPositionY = touchPosition.Value.Y;
            DisplayAlert("Coordenadas", $"X: {action1.ActionPositionX}, Y: {action1.ActionPositionY}", "OK");
        }
    }

    private void OnBoxViewTapped_Goal(object sender, TappedEventArgs e)
    {
        var touchPosition = e.GetPosition((VisualElement)sender);

        if (touchPosition is not null)
        {
            action1.DefinitionPlaceX = touchPosition.Value.X;
            action1.DefinitionPlaceY = touchPosition.Value.Y;
            DisplayAlert("Coordenadas", $"X: {action1.DefinitionPlaceX}, Y: {action1.DefinitionPlaceY}", "OK");
        }
    }

    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        action1.Description = e.NewTextValue;
        DisplayAlert("Descripción", $"Descripción actualizada: {action1.Description}", "OK");
    }

    private void OnSubmit(object sender, TextChangedEventArgs e)
    {
        //Guardo Id de la PlayerAction en el Match
    }

    public Task<bool> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }
}
using Microsoft.Maui.Controls;

namespace Frontend;
public partial class ActionData : ContentPage
{
    public ActionData()
    {
        //InitializeComponent();
    }

    public enum Ending
    {
        Goal,
        Save,
        Miss,
        Blocked,
        Steal_W,
        Steal_L,
        Foul
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    { 
        var picker = (Picker)sender; 
        int selectedIndex = picker.SelectedIndex; 
        if (selectedIndex != -1) 
        { 
            Ending selectedEnding = (Ending)selectedIndex; 
            DisplayAlert("Elemento Seleccionado", "Has seleccionado: " + selectedEnding, "OK");
        } 
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e) 
    { 
        bool isToggled = e.Value; 
        if (isToggled) 
        { 
            DisplayAlert("Interruptor", "El interruptor está activado", "OK"); 
        }
        else 
        {
            DisplayAlert("Interruptor", "El interruptor está desactivado", "OK"); 
        } 
    }

    private void OnBoxViewTapped(object sender, TappedEventArgs e)
    {
        var touchPosition = e.GetPosition((VisualElement)sender);
        
        if(touchPosition is not null)
        {
            double x = touchPosition.Value.X;
            double y = touchPosition.Value.Y;
            DisplayAlert("Coordenadas", $"X: {x}, Y: {y}", "OK");
        }
    }
}


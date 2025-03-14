using Microsoft.Maui.Storage;

namespace Frontend.Resources.Components;

public partial class SwitchHalf : ContentView
{
    private const string SwitchStateKey = "SwitchHalfState";

    public SwitchHalf()
    {
        InitializeComponent();

        // Carga el estado guardado
        bool lastState = Preferences.Get(SwitchStateKey, false);
        CustomSwitch.IsToggled = lastState;

        // Anima la restauración de la posición del switch
        AnimateThumb(lastState);
    }

    private void OnCustomToggleChanged(object sender, ToggledEventArgs e)
    {
        // Guarda el estado actual
        Preferences.Set(SwitchStateKey, e.Value);

        // Anima el cambio del thumb
        AnimateThumb(e.Value);
    }

    private async void AnimateThumb(bool isToggled)
    {
        // Anima el propio Switch (que sí es VisualElement)
        await CustomSwitch.TranslateTo(isToggled ? 30 : 0, 0, 200, Easing.CubicInOut);

        // Cambia el color del thumb según el estado
        CustomSwitch.ThumbColor = isToggled ? Color.FromArgb("#f3b519") : Colors.White;
    }

    public bool IsToggled
    {
        get => CustomSwitch.IsToggled;
        set
        {
            CustomSwitch.IsToggled = value;
            AnimateThumb(value);
            Preferences.Set(SwitchStateKey, value);
        }
    }
}

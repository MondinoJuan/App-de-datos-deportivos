namespace Frontend.Resources.Components;

public partial class SwitchHalf : ContentView
{
	public SwitchHalf()
	{
		InitializeComponent();
	}

    private void OnCustomToggleChanged(object sender, ToggledEventArgs e)
    {
        // Cambia el color de la bolita según el estado
        CustomSwitch.ThumbColor = e.Value ? Color.FromArgb("#f3b519") : Colors.White;
    }

    // Agrego una propiedad para acceder al estado del toggle desde fuera
    public bool IsToggled
    {
        get => CustomSwitch.IsToggled;
        set => CustomSwitch.IsToggled = value;
    }
}
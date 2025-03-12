using Frontend.Resources.Modelos;

namespace Frontend.Resources.Components;

public partial class PlayerCircle : ContentView
{
    public static readonly BindableProperty CircleSizeProperty =
            BindableProperty.Create(nameof(CircleSize), typeof(double), typeof(PlayerCircle), 100.0);

    public double CircleSize
    {
        get => (double)GetValue(CircleSizeProperty);
        set => SetValue(CircleSizeProperty, value);
    }

    public double NumberFontSize => CircleSize * 0.4;
    public double NameFontSize => CircleSize * 0.2;

    public PlayerCircle(Player player)
	{
		InitializeComponent();
        BindingContext = player;

        this.SizeChanged += (s, e) =>
        {
            CircleSize = Math.Min(Width, Height);
            OnPropertyChanged(nameof(NumberFontSize));
            OnPropertyChanged(nameof(NameFontSize));
        };
    }
}
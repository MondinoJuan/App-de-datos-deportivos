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

    public static readonly BindableProperty PlayerProperty =
        BindableProperty.Create(nameof(Player), typeof(Player), typeof(PlayerItemView));

    public Player Player
    {
        get => (Player)GetValue(PlayerProperty);
        set => SetValue(PlayerProperty, value);
    }


    public static readonly BindableProperty ColorTextoProperty =
        BindableProperty.Create(nameof(Color), typeof(Player), typeof(PlayerItemView));

    public Color ColorTexto
    {
        get => (Color)GetValue(ColorTextoProperty);
        set => SetValue(ColorTextoProperty, value);
    }

    public static readonly BindableProperty ColorFondoProperty =
        BindableProperty.Create(nameof(Color), typeof(Player), typeof(PlayerItemView));

    public Color ColorFondo
    {
        get => (Color)GetValue(ColorFondoProperty);
        set => SetValue(ColorFondoProperty, value);
    }

    public PlayerCircle()
	{
		InitializeComponent();
        BindingContext = Player;

        this.SizeChanged += (s, e) =>
        {
            CircleSize = Math.Min(Width, Height);
            OnPropertyChanged(nameof(NumberFontSize));
            OnPropertyChanged(nameof(NameFontSize));
        };
    }

    private void OnTapped(object sender, EventArgs e)
    {
        ColorTexto = Colors.Red;
        ColorFondo = Colors.Pink;
    }
}
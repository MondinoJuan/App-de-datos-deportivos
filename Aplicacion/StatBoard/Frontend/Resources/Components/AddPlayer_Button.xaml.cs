using Frontend.Pages;

namespace Frontend.Resources.Components;

public partial class AddPlayer_Button : ContentView
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


    public static readonly BindableProperty EsLocalProperty =
            BindableProperty.Create(nameof(EsLocal), typeof(bool), typeof(AddPlayer_Button), false);

    public bool EsLocal
    {
        get => (bool)GetValue(EsLocalProperty);
        set => SetValue(EsLocalProperty, value);
    }

    public AddPlayer_Button()
	{
        InitializeComponent();

        this.SizeChanged += (s, e) =>
        {
            CircleSize = Math.Min(Width, Height);
            OnPropertyChanged(nameof(NumberFontSize));
            OnPropertyChanged(nameof(NameFontSize));
        };
    }

    private async void OnTapped(object sender, EventArgs e)
    {
        //Redirigir a la pagina de creacion de Player
        await Navigation.PushAsync(new CreateModify_Player(EsLocal));
    }
}
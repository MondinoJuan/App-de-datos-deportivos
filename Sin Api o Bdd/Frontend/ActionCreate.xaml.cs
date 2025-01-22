using Microsoft.Maui.Controls;
using Frontend.Resources;
using Frontend.Resources.Entities;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend;

public partial class ActionCreate : ContentPage
{
    private PlayerAction_Dto action1 { get; set; }
    private Guid IdPlayerPasado { get; set; }
    private Match_Dto MatchActual { get; set; }
    private TaskCompletionSource<bool> _taskCompletionSource;

    public ActionCreateViewModel ViewModel { get; set; }

    public ActionCreate(Guid id_Player)
    {
        InitializeComponent();
        action1 = new PlayerAction_Dto { Id = Guid.NewGuid() };
        _taskCompletionSource = new TaskCompletionSource<bool>();
        IdPlayerPasado = id_Player;

        MatchActual = Simulo_BdD.GetAllMatches().Data.First();

        ViewModel = new ActionCreateViewModel(action1, IdPlayerPasado, MatchActual, _taskCompletionSource);
        BindingContext = ViewModel;
    }

    private void OnPickerActionSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            action1.Ending = (Ending)selectedIndex;
        }
        ViewModel.DidPckAction = true;
    }

    private void OnPickerSanctionSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            action1.Sanction = ((Sanction)selectedIndex);
        }
        ViewModel.DidPckSanction = true;
    }

    private void OnImageTapped_Field(object sender, TappedEventArgs e)
    {
        var touchPosition = (e as TappedEventArgs)?.GetPosition((View)sender);
        if (touchPosition is not null)
            return;

        action1.ActionPositionX = touchPosition.Value.X;
        action1.ActionPositionY = touchPosition.Value.Y;

        // Crea una nueva marca (círculo)
        var circle = new BoxView
        {
            WidthRequest = 20,
            HeightRequest = 20,
            BackgroundColor = Microsoft.Maui.Graphics.Colors.Blue,
            CornerRadius = 10, // Hace que sea un círculo
            Opacity = 0.8 // Un poco de transparencia
        };

        // Calcula la posición en la pantalla
        AbsoluteLayout.SetLayoutBounds(circle, new Rect(touchPosition.Value.X - 10, touchPosition.Value.Y - 10, 20, 20));
        AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

        // Añade el círculo al contenedor de marcas
        MarkContainerField.Children.Add(circle);

        ViewModel.DidFieldPlace = true;
    }

    private void OnImageTapped_Goal(object sender, TappedEventArgs e)
    {
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            action1.DefinitionPlaceX = touchPosition.Value.X;
            action1.DefinitionPlaceY = touchPosition.Value.Y;

            // Crea una nueva marca (círculo)
            var circle = new BoxView
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Microsoft.Maui.Graphics.Colors.Red,
                CornerRadius = 10, // Hace que sea un círculo
                Opacity = 0.8 // Un poco de transparencia
            };

            // Calcula la posición en la pantalla
            AbsoluteLayout.SetLayoutBounds(circle, new Rect(touchPosition.Value.X - 10, touchPosition.Value.Y - 10, 20, 20));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

            // Añade el círculo al contenedor de marcas
            MarkContainerGoal.Children.Add(circle);

            ViewModel.DidGoalPlace = true;
        }
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        action1.WhichHalf = e.Value;

        ViewModel.DidSwtHalf = true;
    }

    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        action1.Description = e.NewTextValue;
    }

    public Task<bool> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }
}

public class ActionCreateViewModel : BaseViewModel
{
    private PlayerAction_Dto action1;
    private Guid IdPlayerPasado;
    private Match_Dto MatchActual;
    private TaskCompletionSource<bool> _taskCompletionSource;

    public ActionCreateViewModel(PlayerAction_Dto action1, Guid idPlayerPasado, Match_Dto matchActual, TaskCompletionSource<bool> taskCompletionSource)
    {
        this.action1 = action1;
        this.IdPlayerPasado = idPlayerPasado;
        this.MatchActual = matchActual;
        this._taskCompletionSource = taskCompletionSource;
    }

    private bool _didPckAction;
    public bool DidPckAction
    {
        get => _didPckAction;
        set
        {
            if (_didPckAction != value)
            {
                _didPckAction = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DidBtnSubmit));
            }
        }
    }

    private bool _didPckSanction;
    public bool DidPckSanction
    {
        get => _didPckSanction;
        set
        {
            if (_didPckSanction != value)
            {
                _didPckSanction = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DidBtnSubmit));
            }
        }
    }

    private bool _didSwtHalf;
    public bool DidSwtHalf
    {
        get => _didSwtHalf;
        set
        {
            if (_didSwtHalf != value)
            {
                _didSwtHalf = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DidBtnSubmit));
            }
        }
    }

    private bool _didFieldPlace;
    public bool DidFieldPlace
    {
        get => _didFieldPlace;
        set
        {
            if (_didFieldPlace != value)
            {
                _didFieldPlace = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DidBtnSubmit));
            }
        }
    }

    private bool _didGoalPlace;
    public bool DidGoalPlace
    {
        get => _didGoalPlace;
        set
        {
            if (_didGoalPlace != value)
            {
                _didGoalPlace = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DidBtnSubmit));
            }
        }
    }

    public bool DidBtnSubmit => DidPckAction && DidSwtHalf && DidFieldPlace && DidGoalPlace;

    public Command SubmitCommand => new Command(async () => await OnSubmit());
    public Command CancelCommand => new Command(async () => await OnCancel());

    private async Task OnSubmit()
    {
        var result = Simulo_BdD.AddAction(action1);
        Console.WriteLine(result.Message);

        if (result.Success)
        {
            var result1 = Simulo_BdD.GetAllPlayerMatches();
            Console.WriteLine(result1.Message);

            if (result1.Success)
            {
                var playerMatch = result1.Data.FirstOrDefault(pm => pm.IdPlayer == IdPlayerPasado && pm.IdMatch == MatchActual.Id);

                if (playerMatch != null)
                {
                    playerMatch.IdActions.Add(action1.Id);
                    Simulo_BdD.ReplacePlayerMatch(playerMatch);
                }
            }

            _taskCompletionSource.SetResult(true);
        }
        else
        {
            _taskCompletionSource.SetResult(false);
        }

        await Application.Current.MainPage.Navigation.PushAsync(new MatchView());
    }

    private async Task OnCancel()
    {
        action1 = null;
        IdPlayerPasado = Guid.Empty;
        MatchActual = null;

        _taskCompletionSource.SetResult(false);
        await Application.Current.MainPage.Navigation.PushAsync(new MatchView());
    }
}

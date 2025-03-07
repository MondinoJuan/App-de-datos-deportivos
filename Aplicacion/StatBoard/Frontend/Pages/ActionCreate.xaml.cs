using Frontend.Resources;
using Frontend.Resources.Entities;
using Microsoft.Maui.Layouts;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.Pages;

public partial class ActionCreate : ContentPage, INotifyPropertyChanged
{
    private PlayerAction_Dto Action1 { get; set; }
    private Guid IdPlayerPasado { get; set; }
    private Match_Dto MatchActual { get; set; } = new();
    private TaskCompletionSource<bool> _taskCompletionSource;

    public ActionCreate_ViewModel ViewModel { get; set; }


    public ActionCreate(Guid id_Player)
    {
        InitializeComponent();
        Action1 = new PlayerAction_Dto { Id = Guid.NewGuid() };
        _taskCompletionSource = new TaskCompletionSource<bool>();
        IdPlayerPasado = id_Player;

        var result = Simulo_BdD.GetAllMatches();
        if (!result.Success) return;
        MatchActual = result.Data.First();

        ViewModel = new ActionCreate_ViewModel(Action1, IdPlayerPasado, MatchActual, _taskCompletionSource);
        BindingContext = ViewModel;
    }

    private void OnPickerActionSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            Action1.Ending = (Ending)selectedIndex;
        }
        ViewModel.DidPckAction = true;

        if (Action1.Ending == Ending.Foul)
        {
            ViewModel.ActionNeedsSanction = true;
        }
        else
        {
            ViewModel.ActionNeedsSanction = false;
        }

        if (Action1.Ending == Ending.Goal || Action1.Ending == Ending.Miss || Action1.Ending == Ending.Save)
        {
            ViewModel.ActionNeedsGoal = true;
        }
        else
        {
            ViewModel.ActionNeedsGoal = false;
        }
    }

    private void OnPickerSanctionSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            Action1.Sanction = ((Sanction)selectedIndex);
        }
        ViewModel.DidPckSanction = true;
    }

    private void OnImageTapped_Field(object sender, TappedEventArgs e)
    {
        MarkContainerField.Children.Clear();
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            Action1.ActionPositionX = (float)touchPosition.Value.X;
            Action1.ActionPositionY = (float)touchPosition.Value.Y;

            // Crea una nueva marca (c�rculo)
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

            // Calcula la posici�n en la pantalla
            AbsoluteLayout.SetLayoutBounds(circle,
                new Rect(coordenadasActualizadas[0].X, coordenadasActualizadas[0].Y, 20, 20));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

            // A�ade el c�rculo al contenedor de marcas
            MarkContainerField.Children.Add(circle);

            ViewModel.DidFieldPlace = true;
        }
    }

    private void OnImageTapped_Goal(object sender, TappedEventArgs e)
    {
        MarkContainerGoal.Children.Clear();
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            Action1.DefinitionPlaceX = (float)touchPosition.Value.X;
            Action1.DefinitionPlaceY = (float)touchPosition.Value.Y;

            // Crea una nueva marca (c�rculo)
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

            // Calcula la posici�n en la pantalla
            AbsoluteLayout.SetLayoutBounds(circle,
                new Rect(coordenadasActualizadas[0].X, coordenadasActualizadas[0].Y, 20, 20));
            AbsoluteLayout.SetLayoutFlags(circle, AbsoluteLayoutFlags.None);

            // A�ade el c�rculo al contenedor de marcas
            MarkContainerGoal.Children.Add(circle);

            ViewModel.DidGoalPlace = true;
        }
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        Action1.WhichHalf = e.Value;

        ViewModel.DidSwtHalf = true;
    }

    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        Action1.Description = e.NewTextValue;
    }

    public Task<bool> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }
}

public class ActionCreate_ViewModel : BaseViewModel
{
    private PlayerAction_Dto? action1;
    private Guid IdPlayerPasado;
    private Match_Dto? MatchActual;
    private TaskCompletionSource<bool> _taskCompletionSource;

    public ActionCreate_ViewModel(PlayerAction_Dto action1, Guid idPlayerPasado, Match_Dto matchActual, TaskCompletionSource<bool> taskCompletionSource)
    {
        this.action1 = action1;
        this.IdPlayerPasado = idPlayerPasado;
        this.MatchActual = matchActual;
        this._taskCompletionSource = taskCompletionSource;
    }

    private bool _actionNeedsSanction { get; set; } = false;
    public bool ActionNeedsSanction
    {
        get => _actionNeedsSanction;
        set
        {
            if (_actionNeedsSanction != value)
            {
                _actionNeedsSanction = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _actionNeedsGoal { get; set; } = false;
    public bool ActionNeedsGoal
    {
        get => _actionNeedsGoal;
        set
        {
            if (_actionNeedsGoal != value)
            {
                _actionNeedsGoal = value;
                OnPropertyChanged();
            }
        }
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

    public bool DidBtnSubmit => DidPckAction && DidFieldPlace;

    public Command SubmitCommand => new Command(async () => await OnSubmit());
    public Command CancelCommand => new Command(async () => await OnCancel());

    private async Task OnSubmit()
    {
        if (action1 == null) return;
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

        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0].Page is NavigationPage navPage)
        {
            await navPage.PushAsync(new MatchView());
        }
    }

    private async Task OnCancel()
    {
        action1 = null;
        IdPlayerPasado = Guid.Empty;
        MatchActual = null;

        _taskCompletionSource.SetResult(false);
        if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0].Page is NavigationPage navPage)
        {
            await navPage.PushAsync(new MatchView());
        }
    }
}

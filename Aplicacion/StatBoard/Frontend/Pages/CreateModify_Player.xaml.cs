using Frontend.Resources.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontend.Resources;
using Frontend.Resources.Modelos;
using Frontend.Resources.ViewModels;

namespace Frontend.Pages;

public partial class CreateModify_Player : ContentPage, INotifyPropertyChanged
{
    private TaskCompletionSource<int> _taskCompletionSource;
    private TaskCompletionSource<Player_Dto> _playerAGuardar;
    public Player PlayerModificar { get; private set; }
    public Player_Dto PlayerCrear { get; private set; }
    public bool ModifyWarning { get; private set; }
    public bool InvModifyWarning { get; private set; }

    private bool _enableSaveButton = false;
    public bool EnableSaveButton
    {
        get => _enableSaveButton;
        set
        {
            if (_enableSaveButton != value)
            {
                _enableSaveButton = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _enableNumberErrorLabel = false;
    public bool EnableNumberErrorLabel
    {
        get => _enableNumberErrorLabel;
        set
        {
            if (_enableNumberErrorLabel != value)
            {
                _enableNumberErrorLabel = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _enableNameErrorLabel = false;
    public bool EnableNameErrorLabel
    {
        get => _enableNameErrorLabel;
        set
        {
            if (_enableNameErrorLabel != value)
            {
                _enableNameErrorLabel = value;
                OnPropertyChanged();
            }
        }
    }

    public CreateModify_Player()
    {
        InitializeComponent();
        ModifyWarning = false;
        EnableSaveButton = false;
        InvModifyWarning = !ModifyWarning;
        PlayerCrear = new Player_Dto();
        _taskCompletionSource = new TaskCompletionSource<int>();
        BindingContext = this;
    }

    public CreateModify_Player(Player player)
    {
        InitializeComponent();
        ModifyWarning = true;
        EnableSaveButton = false;
        InvModifyWarning = !ModifyWarning;
        PlayerModificar = player;
        CompleteFields();
        _taskCompletionSource = new TaskCompletionSource<int>();
        BindingContext = this;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPlayerNumber.Text))
        {
            EnableNumberErrorLabel = !Validations.ValidateNumber(txtPlayerNumber.Text);

        }

        if (!string.IsNullOrEmpty(txtPlayerName.Text))
        {
            EnableNameErrorLabel = !Validations.ValidateAlphabeticString(txtPlayerName.Text);

        }

        EnableSaveButton = !EnableNumberErrorLabel && !EnableNameErrorLabel &&
                      !string.IsNullOrWhiteSpace(txtPlayerName.Text) &&
                      !string.IsNullOrWhiteSpace(txtPlayerNumber.Text);
    }

    private void CompleteFields()
    {
        txtPlayerName.Text = PlayerModificar.Name;
        txtPlayerNumber.Text = PlayerModificar.Number.ToString();
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        lblStatus.Text = $"{(e.Value ? "Visitante" : "Local")}";
    }

    private async void OnSave(object sender, EventArgs e)
    {   
        if (ModifyWarning)
        {
            PlayerModificar.Name = txtPlayerName.Text;
            PlayerModificar.Number = int.Parse(txtPlayerNumber.Text);

            Services.UpdatePlayer(PlayerModificar);
            // Manejo del error por si no se reemplaza.
        }
        else
        {
            PlayerCrear.Name = txtPlayerName.Text;
            PlayerCrear.Number = int.Parse(txtPlayerNumber.Text);

            Services.AddPlayer(PlayerCrear);
            // Manejo del error por si no se guarda.
        }

        if (!_taskCompletionSource.Task.IsCompleted)
        {
            if (swtLocalAway.IsToggled)
            {
                _taskCompletionSource.SetResult(2); // Visitante
            }
            else
            {
                _taskCompletionSource.SetResult(1); // Local
            }
        }

        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        await Navigation.PushAsync(new MatchView());
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        if (!_taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource.SetResult(0);
        }
        await Navigation.PopModalAsync();
        await Navigation.PushAsync(new MatchView());
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
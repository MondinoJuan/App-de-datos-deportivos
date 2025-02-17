using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontend.Resources;

namespace Frontend.Pages;

public partial class CreateModify_PlayerModal : ContentPage, INotifyPropertyChanged
{
    private TaskCompletionSource<int> _taskCompletionSource;
    private TaskCompletionSource<Player_Dto> _playerAGuardar;
    public Player_Dto Player { get; private set; }
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

    public CreateModify_PlayerModal()
    {
        InitializeComponent();
        ModifyWarning = false;
        EnableSaveButton = false;
        InvModifyWarning = !ModifyWarning;
        Player = new Player_Dto();
        Player.Id = Guid.NewGuid();
        _taskCompletionSource = new TaskCompletionSource<int>();
        _playerAGuardar = new TaskCompletionSource<Player_Dto>();
        BindingContext = this;
    }

    public CreateModify_PlayerModal(Player_Dto player)
    {
        InitializeComponent();
        ModifyWarning = true;
        EnableSaveButton = false;
        InvModifyWarning = !ModifyWarning;
        Player = player;
        CompleteFields();
        _taskCompletionSource = new TaskCompletionSource<int>();
        _playerAGuardar = new TaskCompletionSource<Player_Dto>();
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
        txtPlayerName.Text = Player.Name;
        txtPlayerNumber.Text = Player.Number.ToString();
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        lblStatus.Text = $"{(e.Value ? "Visitante" : "Local")}";
    }

    private async void OnSave(object sender, EventArgs e)
    {
        Player.Name = txtPlayerName.Text;
        Player.Number = int.Parse(txtPlayerNumber.Text);

        if (ModifyWarning)
        {
            var result = Simulo_BdD.ReplacePlayer(Player);
            // Manejo del error por si no se reemplaza.
        }
        else
        {
            var result = Simulo_BdD.AddPlayer(Player);
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

        if (!_playerAGuardar.Task.IsCompleted)
        {
            _playerAGuardar.SetResult(Player);
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

    public Task<int> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }

    public Task<Player_Dto> GetPlayerAsync()
    {
        return _playerAGuardar.Task;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
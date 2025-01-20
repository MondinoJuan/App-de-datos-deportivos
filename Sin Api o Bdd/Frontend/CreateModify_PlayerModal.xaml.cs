using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using System.Threading.Tasks;

namespace Frontend;

public partial class CreateModify_PlayerModal : ContentPage
{
    private TaskCompletionSource<int> _taskCompletionSource;
    private TaskCompletionSource<Player_Dto> _playerAGuardar;
    public Player_Dto Player { get; private set; }
    public bool ModifyWarning { get; private set; }
    public bool InvModifyWarning { get; private set; }

    public CreateModify_PlayerModal()
    {
        InitializeComponent();
        ModifyWarning = false;
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
        InvModifyWarning = !ModifyWarning;
        Player = player;
        CompleteFields();
        _taskCompletionSource = new TaskCompletionSource<int>();
        _playerAGuardar = new TaskCompletionSource<Player_Dto>();
        BindingContext = this;
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

    private void OnSave(object sender, EventArgs e)
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

        Navigation.PopModalAsync();
        Navigation.PushAsync(new MatchView());
    }

    private void OnCancel(object sender, EventArgs e)
    {
        if (!_taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource.SetResult(0);
        }
        Navigation.PopModalAsync();
        Navigation.PushAsync(new MatchView());
    }

    public Task<int> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }

    public Task<Player_Dto> GetPlayerAsync()
    {
        return _playerAGuardar.Task;
    }
}
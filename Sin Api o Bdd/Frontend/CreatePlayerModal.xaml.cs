using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;
using System.Threading.Tasks;

namespace Frontend;

public partial class CreatePlayerModal : ContentPage
{
    private TaskCompletionSource<int> _taskCompletionSource;
    public Player_Dto Player { get; private set; }

    public CreatePlayerModal(Player_Dto player)
    {
        InitializeComponent();
        Player = player;
        BindingContext = this;
        _taskCompletionSource = new TaskCompletionSource<int>();       // lo hago int para saber si es local o visitante?
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        lblStatus.Text = $"{(e.Value ? "Visitante" : "Local")}";
    }

    private void OnSave(object sender, EventArgs e)
    {
        Player.Name = txtPlayerName.Text;
        Player.Number = int.Parse(txtPlayerNumber.Text);

        if (swtLocalAway.IsToggled)
        {
            _taskCompletionSource.SetResult(2); // Visitante
        }
        else
        {
            _taskCompletionSource.SetResult(1); // Local
        }

        Navigation.PopModalAsync();
    }

    private void OnCancel(object sender, EventArgs e)
    {
        _taskCompletionSource.SetResult(0);
        Navigation.PopModalAsync();
    }

    public Task<int> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }
}
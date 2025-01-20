using Microsoft.Maui.Controls;
using Frontend.Resources;
using Frontend.Resources.Entities;
using System.Threading.Tasks;

namespace Frontend;

public partial class ActionCreate : ContentPage
{
    private PlayerAction_Dto action1 { get; set; }
    private Guid IdPlayerPasado { get; set; }
    private Match_Dto MatchActual { get; set; }
    private TaskCompletionSource<bool> _taskCompletionSource;
    private bool DidBtnSubmit { get; set; } = false;
    private bool DidPckAction { get; set; } = false;
    private bool DidSwtHalf { get; set; } = false;
    private bool DidFieldPlace { get; set; } = false;
    private bool DidGoalPlace { get; set; } = false;

    public ActionCreate(Guid id_Player)
    {
        InitializeComponent();
        action1 = new PlayerAction_Dto { Id = Guid.NewGuid() };
        _taskCompletionSource = new TaskCompletionSource<bool>();
        IdPlayerPasado = id_Player;

        MatchActual = Simulo_BdD.GetAllMatches().Data.First();

        BindingContext = this;
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;
        if (selectedIndex != -1)
        {
            action1.Ending = (Ending)selectedIndex;
        }
        DidPckAction = true;
        EnableSubmitButton();
    }

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        action1.WhichHalf = e.Value;

        DidSwtHalf = true;
        EnableSubmitButton();
    }

    private void OnBoxViewTapped_Field(object sender, TappedEventArgs e)
    {
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            action1.ActionPositionX = touchPosition.Value.X;
            action1.ActionPositionY = touchPosition.Value.Y;
        }
        DidFieldPlace = true;
        EnableSubmitButton();
    }

    private void OnBoxViewTapped_Goal(object sender, TappedEventArgs e)
    {
        var touchPosition = e.GetPosition((VisualElement)sender);
        if (touchPosition is not null)
        {
            action1.DefinitionPlaceX = touchPosition.Value.X;
            action1.DefinitionPlaceY = touchPosition.Value.Y;

            DidGoalPlace = true;
            EnableSubmitButton();
        }
    }

    private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
    {
        action1.Description = e.NewTextValue;
    }

    private async void OnSubmit(object sender, EventArgs e)
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
                }
            }

            _taskCompletionSource.SetResult(true);
        }
        else
        {
            _taskCompletionSource.SetResult(false);
        }

        //await Navigation.PopModalAsync();
        await Navigation.PushAsync(new MatchView());
    }

    public Task<bool> GetResultAsync()
    {
        return _taskCompletionSource.Task;
    }

    public async void OnCancel(object sender, EventArgs e)
    {
        action1 = null;
        IdPlayerPasado = Guid.Empty;
        MatchActual = null;

        _taskCompletionSource.SetResult(false);
        //await Navigation.PopModalAsync();
        await Navigation.PushAsync(new MatchView());
        await Navigation.PushAsync(new MatchView());
    }

    public void EnableSubmitButton()
    {
        DidBtnSubmit = DidPckAction & DidSwtHalf & DidFieldPlace & DidGoalPlace;
    }
}
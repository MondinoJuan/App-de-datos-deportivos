using Frontend.Resources.DTOs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frontend.Resources;
using Frontend.Resources.Modelos;
//using Frontend.Resources.ViewModels;

namespace Frontend.Pages;

public partial class CreateModify_Player : ContentPage, INotifyPropertyChanged
{
    private Player PlayerModificar { get; set; }
    private Player_Dto PlayerCrear { get; set; }

    private bool Modify => PlayerModificar != null;

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

    public CreateModify_Player(bool localidad)
    {
        InitializeComponent();
        EnableSaveButton = false;
        PlayerCrear = new Player_Dto();
        BindingContext = this;
    }

    public CreateModify_Player(int idPlayer, bool localidad)
    {
        InitializeComponent();
        EnableSaveButton = false;
        PlayerModificar = Services.GetPlayer(idPlayer);
        CompleteFields();
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

    private async void OnSave(object sender, EventArgs e)
    {   
        if (Modify)
        {
            PlayerModificar.Name = txtPlayerName.Text;
            PlayerModificar.Number = int.Parse(txtPlayerNumber.Text);

            Services.UpdatePlayer(PlayerModificar);
        }
        else
        {
            PlayerCrear.Name = txtPlayerName.Text;
            PlayerCrear.Number = int.Parse(txtPlayerNumber.Text);

            Services.AddPlayer(PlayerCrear);
        }
                
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    private async void OnCancel(object sender, EventArgs e)
    {
        if (Navigation.ModalStack.Count > 0)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            await Navigation.PopAsync();
        }
    }

    public new event PropertyChangedEventHandler? PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
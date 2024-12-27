namespace Frontend;
public partial class MainPage : ContentPage
{

    private bool userNotExists = false;
    private bool passwordWrong = false;
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCreateAccount(object sender, EventArgs e)
    {
        // Redirigir a la pagina de crear cuenta cuando esté hecha.
    }

    private void OnForget(object sender, EventArgs e)
    {
        // Redirigir a la pagina de olvidar contraseña cuando esté hecha.
    }

    private void OnLogin(object sender, EventArgs e)
    {
        string username = UsernameText.Text;
        string password = PasswordText.Text;

        var user = getUserByUsername(username);

        if (user != null)
        {
            if (user.Password == password)
            {
                // Redirigir a la pagina de inicio cuando esté hecha.
            }
            else
            {
                passwordWrong = true;
                DisplayAlert("Error", "Contraseña incorrecta", "OK");
            }
        }
        else
        {
            userNotExists = true;
            DisplayAlert("Error", "Usuario no encontrado", "OK");
        }
    }


    // PhoneWord ejemplo.
    string translatedNumber;

    private void OnTranslate(object sender, EventArgs e)
    {
        string enteredNumber = PhoneNumberText.Text;
        translatedNumber = Core.FrontendTranslator.ToNumber(enteredNumber);

        if (!string.IsNullOrEmpty(translatedNumber))
        {
            CallButton.IsEnabled = true;
            CallButton.Text = "Call " + translatedNumber;
        }
        else
        {
            CallButton.IsEnabled = false;
            CallButton.Text = "Call";
        }
    }

    async void OnCall (object sender, System.EventArgs e)
    {
        if (await this.DisplayAlert(
        "Dial a Number",
        "Would you like to call " + translatedNumber + "?",
        "Yes",
        "No"))
        {
            try
            {
                if (PhoneDialer.Default.IsSupported)
                    PhoneDialer.Default.Open(translatedNumber);
            }
            catch (ArgumentNullException)
            {
                await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
            }
            catch (Exception)
            {
                // Other error has occurred.
                await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
            }
        }
    }
}


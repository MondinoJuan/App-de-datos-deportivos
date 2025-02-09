using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Frontend.Resources.Components
{
    public partial class AnimacionPensar : ContentView
    {
        public AnimacionPensar()
        {
            InitializeComponent();
            StartAnimation(); // Inicia la animación
        }

        private async void StartAnimation()
        {
            while (true) // Animación infinita
                         // No se que tan mal esta esto, porque es un componente que se va a estar mostrando hasta que en la pagina se llame a cerrar 
                         // la aplicacion. Pero como no es buena idea tener un while true podria cambiarlo por una variable que cuando termine de
                         // crearse el PDF le cambie el valor.
            {
                await Loader.RotateTo(360, 1000); // Rota en 1 segundo
                Loader.Rotation = 0; // Reinicia rotación
            }
        }
    }
}

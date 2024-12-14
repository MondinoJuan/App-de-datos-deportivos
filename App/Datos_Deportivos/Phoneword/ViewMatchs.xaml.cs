using Microsoft.Maui.Controls;
using Frontend.Resources.Entities;

namespace Frontend
{
    public partial class ViewMatchs : ContentPage
    {
        public ViewMatchs()
        {
            //InitializeComponent();
            matchSummary.BindData(match);               // Falta que busque todos los partidos existentes y muestre un loop (for?)
        }

        
    }
}

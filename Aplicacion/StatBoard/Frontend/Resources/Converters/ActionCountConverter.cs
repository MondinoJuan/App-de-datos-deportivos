using Frontend.Resources.DTOs;
using System.Globalization;
using Frontend.Pages;
using Frontend.Resources.Components;

namespace Frontend.Resources.Converters
{
    public class ActionCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int playerId && parameter is string actionType)
            {
                //var matchSummary = Application.Current.MainPage as MatchSummary;          //Error

                //var matchView = Application.Current.MainPage?.FindByName<MatchSummary>("matchSummary"); // Solucion
                //if (matchView == null) return "-";

                if (!Enum.TryParse(actionType, out Ending ending)) return "-";
                return Functions.GetActionCountForPlayer(playerId, ending).QuantityEnding.ToString() ?? "-";
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }   
}

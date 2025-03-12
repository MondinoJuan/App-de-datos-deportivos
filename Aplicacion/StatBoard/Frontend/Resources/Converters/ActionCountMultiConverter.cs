using Frontend.Resources.DTOs;
using System.Globalization;
using Frontend.Pages;
using Frontend.Resources.Components;

namespace Frontend.Resources.Converters
{
    public class ActionCountMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int playerId && values[1] is string actionType)
            {
                //var matchSummary = Application.Current.MainPage as MatchSummary;          //ERROR

                //var matchView = Application.Current.MainPage?.FindByName<MatchSummary>("matchSummary"); // Solucion
                //if (matchView == null) return "-";

                if (!Enum.TryParse(actionType, out Ending ending)) return "-";
                return Functions.GetActionCountForPlayer(playerId, ending).QuantityEnding.ToString() ?? "-";
            }
            return "-";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
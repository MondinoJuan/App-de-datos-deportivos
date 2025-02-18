using Frontend.Resources.Entities;
using System.Globalization;
using Frontend.Pages;

namespace Frontend.Resources
{
    public class ActionCountMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is Guid playerId && values[1] is string actionType)
            {
                var matchView = Application.Current.MainPage as MatchView;
                if(!Enum.TryParse(actionType, out Ending ending)) return "-";
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
using Frontend.Resources.Entities;
using System.Globalization;
using Frontend.Pages;

namespace Frontend.Resources.Converters
{
    public class ActionCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Guid playerId && parameter is string actionType)
            {
                var matchView = Application.Current.MainPage as MatchView;
                if(!Enum.TryParse(actionType, out Ending ending)) return "-";
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

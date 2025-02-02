using Frontend.Resources.Entities;
using System.Globalization;

namespace Frontend.Resources
{
    public class ActionCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Guid playerId && parameter is string actionType)
            {
                var matchView = Application.Current.MainPage as MatchView;
                return GetActionCountForPlayer(playerId, actionType).ToString() ?? "-";
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private int GetActionCountForPlayer(Guid playerId, string actionType)
        {
            var result = Simulo_BdD.GetAllPlayerMatches();
            if (!result.Success) return 0;

            var playerMatch = result.Data.FirstOrDefault(a => a.IdPlayer == playerId);
            if (playerMatch?.IdActions == null) return 0;

            if (Enum.TryParse(actionType, out Ending actionValue))
            {
                return playerMatch.IdActions
                    .Select(idAction => Simulo_BdD.GetOneAction(idAction))
                    .Count(result1 => result1.Success && result1.Data.Ending == actionValue);
            }

            return 0;
        }
    }   
}

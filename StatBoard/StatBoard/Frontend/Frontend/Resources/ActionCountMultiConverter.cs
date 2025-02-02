// ActionCountMultiConverter.cs
using Frontend.Resources.Entities;
using System.Globalization;

namespace Frontend.Resources
{
    public class ActionCountMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is Guid playerId && values[1] is string actionType)
            {
                var matchView = Application.Current.MainPage as MatchView;
                return GetActionCountForPlayer(playerId, actionType) ?? "-";
            }
            return "-";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetActionCountForPlayer(Guid playerId, string actionType)
        {
            var result = Simulo_BdD.GetAllPlayerMatches();
            if (!result.Success) return "-";

            var playerMatch = result.Data.FirstOrDefault(a => a.IdPlayer == playerId);
            if (playerMatch?.IdActions == null) return "-";

            if (Enum.TryParse(actionType, out Ending actionValue))
            {
                return playerMatch.IdActions
                    .Select(idAction => Simulo_BdD.GetOneAction(idAction))
                    .Count(result1 => result1.Success && result1.Data.Ending == actionValue).ToString();
            }

            return "-";
        }
    }
}
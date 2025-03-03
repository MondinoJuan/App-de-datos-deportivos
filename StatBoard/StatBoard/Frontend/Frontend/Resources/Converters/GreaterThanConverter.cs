using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Resources.Converters
{
    public class GreaterThanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (!double.TryParse(value?.ToString(), out double parsedValue) || 
                !double.TryParse(parameter?.ToString(), out double parsedParameter))
            {
                return false;
            }

            return parsedValue > parsedParameter;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

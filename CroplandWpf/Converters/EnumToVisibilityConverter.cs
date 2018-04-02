using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CroplandWpf.Converters
{
    public class EnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (!(value is Enum)) throw new ArgumentException("Value must be of Enum type.", nameof(value));
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));
            if (!(parameter is string)) throw new ArgumentException("Parameter must be a string.", nameof(parameter));

            return parameter.ToString().Split(',').Any(
                state => string.Equals(value.ToString(), state.Trim(), StringComparison.OrdinalIgnoreCase))
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

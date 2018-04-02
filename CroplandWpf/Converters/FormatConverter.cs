using System;
using System.Globalization;
using System.Windows.Data;

namespace CroplandWpf.Converters
{
    public class FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatterString = parameter?.ToString();

            return !string.IsNullOrEmpty(formatterString)
                ? string.Format(culture, formatterString, value)
                : value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

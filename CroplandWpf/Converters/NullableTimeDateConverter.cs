using System;
using System.Globalization;
using System.Windows.Data;

namespace CroplandWpf.Converters
{
    public class NullableTimeDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime?)value)?.ToShortDateString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value?.ToString();
            DateTime resultDateTime;
            return strValue != null && DateTime.TryParse(strValue, out resultDateTime) 
                ? resultDateTime 
                : (DateTime?)null;
        }
    }
}

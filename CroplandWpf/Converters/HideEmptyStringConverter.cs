using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CroplandWpf.Converters
{
    public class HideEmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            return string.IsNullOrEmpty(strValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CroplandWpf.Converters
{

    public class BooleanToVisibilityConverter : IValueConverter
    {

        public bool TriggerValue { get; set; }
        public bool IsHidden { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            var objValue = (bool)value;

            if (objValue && TriggerValue && IsHidden || !objValue && !TriggerValue && IsHidden)
                return Visibility.Hidden;

            if (objValue && TriggerValue && !IsHidden || !objValue && !TriggerValue && !IsHidden)
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

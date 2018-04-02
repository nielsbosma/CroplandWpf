using System;
using System.Windows;
using System.Windows.Data;

namespace CroplandWpf.Converters
{
    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = value?.GetType();
            if (type == null || !type.IsEnum || !Enum.IsDefined(type, value))
                throw new ArgumentException("Value must be a valid enum member.", nameof(value));

            var parameterValue = ParseParameter(parameter, type);

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                throw new ArgumentException("Value must be a boolean.", nameof(value));

            return (bool)value ? ParseParameter(parameter, targetType) : DependencyProperty.UnsetValue;
        }

        private object ParseParameter(object parameter, Type enumType)
        {
            var parameterValue = parameter is string
                ? Enum.Parse(enumType, (string)parameter)
                : parameter as Enum;

            if (parameterValue == null || !Enum.IsDefined(enumType, parameterValue))
                throw new ArgumentException("Parameter must be a valid enum member.", nameof(parameter));

            return parameterValue;
        }
    }
}

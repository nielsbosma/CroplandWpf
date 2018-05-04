using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CroplandWpf.Converters
{
	public class EnumToValuesListConverter : IValueConverter
	{
		public static EnumToValuesListConverter Instance
		{
			get { return _instance; }
		}
		private static EnumToValuesListConverter _instance = new EnumToValuesListConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;
			if (value as Enum != null)
				return Enum.GetValues(value.GetType());
			else
				return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

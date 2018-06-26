using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.Attached
{
	public sealed class Slider
	{
		public static string GetValueStringFormat(DependencyObject obj)
		{
			return (string)obj.GetValue(ValueStringFormatProperty);
		}
		public static void SetValueStringFormat(DependencyObject obj, string value)
		{
			obj.SetValue(ValueStringFormatProperty, value);
		}
		public static readonly DependencyProperty ValueStringFormatProperty =
			DependencyProperty.RegisterAttached("ValueStringFormat", typeof(string), typeof(Slider), new PropertyMetadata());
	}
}

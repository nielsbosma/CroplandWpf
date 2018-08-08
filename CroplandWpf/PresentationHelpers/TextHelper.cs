using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.PresentationHelpers
{
    public class TextHelper : FrameworkElement
    {
		public static bool GetConvertToUpper(DependencyObject obj)
		{
			return (bool)obj.GetValue(ConvertToUpperProperty);
		}
		public static void SetConvertToUpper(DependencyObject obj, bool value)
		{
			obj.SetValue(ConvertToUpperProperty, value);
		}
		public static readonly DependencyProperty ConvertToUpperProperty =
			DependencyProperty.RegisterAttached("ConvertToUpper", typeof(bool), typeof(TextHelper), new PropertyMetadata());
	}
}
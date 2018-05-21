using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.Attached
{
	public sealed class FocusHelper
	{
		public static bool GetIsFocused(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsFocusedProperty);
		}
		public static void SetIsFocused(DependencyObject obj, bool value)
		{
			obj.SetValue(IsFocusedProperty, value);
		}
		public static readonly DependencyProperty IsFocusedProperty =
			DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusHelper), new PropertyMetadata());

		public static bool GetIsFocusable(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsFocusableProperty);
		}
		public static void SetIsFocusable(DependencyObject obj, bool value)
		{
			obj.SetValue(IsFocusableProperty, value);
		}
		public static readonly DependencyProperty IsFocusableProperty =
			DependencyProperty.RegisterAttached("IsFocusable", typeof(bool), typeof(FocusHelper), new PropertyMetadata());
	}
}

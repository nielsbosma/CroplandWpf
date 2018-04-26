using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Attached
{
    public sealed class ItemsControlHelper
	{
		public static Orientation GetOrientation(DependencyObject obj)
		{
			return (Orientation)obj.GetValue(OrientationProperty);
		}
		public static void SetOrientation(DependencyObject obj, Orientation value)
		{
			obj.SetValue(OrientationProperty, value);
		}
		public static readonly DependencyProperty OrientationProperty =
			DependencyProperty.RegisterAttached("Orientation", typeof(Orientation), typeof(ItemsControlHelper), new PropertyMetadata());
	}
}
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

		public static Control GetDataItemContainer(DependencyObject obj)
		{
			return (Control)obj.GetValue(DataItemContainerProperty);
		}
		public static void SetDataItemContainer(DependencyObject obj, Control value)
		{
			obj.SetValue(DataItemContainerProperty, value);
		}
		public static readonly DependencyProperty DataItemContainerProperty =
			DependencyProperty.RegisterAttached("DataItemContainer", typeof(Control), typeof(ItemsControlHelper), new PropertyMetadata());

		public static Thickness GetFirstItemMargin(DependencyObject obj)
		{
			return (Thickness)obj.GetValue(FirstItemMarginProperty);
		}
		public static void SetFirstItemMargin(DependencyObject obj, Thickness value)
		{
			obj.SetValue(FirstItemMarginProperty, value);
		}
		public static readonly DependencyProperty FirstItemMarginProperty =
			DependencyProperty.RegisterAttached("FirstItemMargin", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata());

		public static Thickness GetRegularItemMargin(DependencyObject obj)
		{
			return (Thickness)obj.GetValue(RegularItemMarginProperty);
		}
		public static void SetRegularItemMargin(DependencyObject obj, Thickness value)
		{
			obj.SetValue(RegularItemMarginProperty, value);
		}
		public static readonly DependencyProperty RegularItemMarginProperty =
			DependencyProperty.RegisterAttached("RegularItemMargin", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata());

		public static Thickness GetLastItemMargin(DependencyObject obj)
		{
			return (Thickness)obj.GetValue(LastItemMarginProperty);
		}
		public static void SetLastItemMargin(DependencyObject obj, Thickness value)
		{
			obj.SetValue(LastItemMarginProperty, value);
		}
		public static readonly DependencyProperty LastItemMarginProperty =
			DependencyProperty.RegisterAttached("LastItemMargin", typeof(Thickness), typeof(ItemsControlHelper), new PropertyMetadata());
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CroplandWpf.Attached
{
	public sealed class VisualHelper
	{
		public static Transform GetInheritedTransform(DependencyObject obj)
		{
			return (Transform)obj.GetValue(InheritedTransformProperty);
		}
		public static void SetInheritedTransform(DependencyObject obj, Transform value)
		{
			obj.SetValue(InheritedTransformProperty, value);
		}
		public static readonly DependencyProperty InheritedTransformProperty =
			DependencyProperty.RegisterAttached("InheritedTransform", typeof(Transform), typeof(VisualHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static GridLength GetRightColumnGap(DependencyObject obj)
		{
			return (GridLength)obj.GetValue(RightColumnGapProperty);
		}
		public static void SetRightColumnGap(DependencyObject obj, GridLength value)
		{
			obj.SetValue(RightColumnGapProperty, value);
		}
		public static readonly DependencyProperty RightColumnGapProperty =
			DependencyProperty.RegisterAttached("RightColumnGap", typeof(GridLength), typeof(VisualHelper), new PropertyMetadata());

		public static double GetRightColumnGapSource(DependencyObject obj)
		{
			return (double)obj.GetValue(RightColumnGapSourceProperty);
		}
		public static void SetRightColumnGapSource(DependencyObject obj, double value)
		{
			obj.SetValue(RightColumnGapSourceProperty, value);
		}
		public static readonly DependencyProperty RightColumnGapSourceProperty =
			DependencyProperty.RegisterAttached("RightColumnGapSource", typeof(double), typeof(VisualHelper), new PropertyMetadata((o, e) =>
			{
				double dValue = (double)e.NewValue;
				SetRightColumnGap(o, new GridLength(dValue, GridUnitType.Pixel));
			}));
	}
}
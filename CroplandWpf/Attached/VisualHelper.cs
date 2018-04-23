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
	}
}
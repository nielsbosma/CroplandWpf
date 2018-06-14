using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CroplandWpf.Attached
{
	public sealed class ToggleButtonHelper
	{
		public static object GetCheckedContent(DependencyObject obj)
		{
			return (object)obj.GetValue(CheckedContentProperty);
		}
		public static void SetCheckedContent(DependencyObject obj, object value)
		{
			obj.SetValue(CheckedContentProperty, value);
		}
		public static readonly DependencyProperty CheckedContentProperty =
			DependencyProperty.RegisterAttached("CheckedContent", typeof(object), typeof(ToggleButtonHelper), new PropertyMetadata((o, e) =>
			{
				ToggleButton tb = o as ToggleButton;
				if (tb == null)
					return;
				SetHasCheckedContent(tb, e.NewValue != null);
			}));

		public static bool GetHasCheckedContent(DependencyObject obj)
		{
			return (bool)obj.GetValue(HasCheckedContentProperty);
		}
		private static void SetHasCheckedContent(DependencyObject obj, bool value)
		{
			obj.SetValue(HasCheckedContentProperty, value);
		}
		public static readonly DependencyProperty HasCheckedContentProperty =
			DependencyProperty.RegisterAttached("HasCheckedContent", typeof(bool), typeof(ToggleButtonHelper), new PropertyMetadata());
	}
}
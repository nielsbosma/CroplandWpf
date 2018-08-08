using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace CroplandWpf.Attached
{
	public class InternalTargetStyle : DependencyObject
	{
		public Type TargetType
		{
			get { return (Type)GetValue(TargetTypeProperty); }
			set { SetValue(TargetTypeProperty, value); }
		}
		public static readonly DependencyProperty TargetTypeProperty =
			DependencyProperty.Register("TargetType", typeof(Type), typeof(InternalTargetStyle), new PropertyMetadata());

		public object StyleKey
		{
			get { return (object)GetValue(StyleKeyProperty); }
			set { SetValue(StyleKeyProperty, value); }
		}
		public static readonly DependencyProperty StyleKeyProperty =
			DependencyProperty.Register("StyleKey", typeof(object), typeof(InternalTargetStyle), new PropertyMetadata());
	}

	public class InternalTargetStyleCollection:List<InternalTargetStyle>
	{

	}


	public sealed class InternalStyleApplicator
    {
		public static InternalTargetStyleCollection GetTargetStyles(DependencyObject obj)
		{
			return (InternalTargetStyleCollection)obj.GetValue(TargetStylesProperty);
		}
		public static void SetTargetStyles(DependencyObject obj, InternalTargetStyleCollection value)
		{
			obj.SetValue(TargetStylesProperty, value);
		}
		public static readonly DependencyProperty TargetStylesProperty =
			DependencyProperty.RegisterAttached("TargetStyles", typeof(InternalTargetStyleCollection), typeof(InternalStyleApplicator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, TargetStylesChanged));

		private static void TargetStylesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (!(e.NewValue is IEnumerable<InternalTargetStyle> styles))
				return;

			if (!(o is FrameworkElement target))
				return;

			Type targetType = o.GetType();
			InternalTargetStyle styleItem = styles.FirstOrDefault(its => its.TargetType.Equals(targetType));
			if (styleItem != null)
				target.SetResourceReference(FrameworkElement.StyleProperty, styleItem.StyleKey);
		}
	}
}
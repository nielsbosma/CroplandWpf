using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CroplandWpf.Attached
{
	public enum HeaderAlignment
	{
		Left,
		Top
	}

	public enum FooterAlignment
	{
		Right,
		Bottom
	}

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

		public static CornerRadius GetCornerRadius(DependencyObject obj)
		{
			return (CornerRadius)obj.GetValue(CornerRadiusProperty);
		}
		public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
		{
			obj.SetValue(CornerRadiusProperty, value);
		}
		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(VisualHelper), new PropertyMetadata());

		public static object GetHeader(DependencyObject obj)
		{
			return (object)obj.GetValue(HeaderProperty);
		}
		public static void SetHeader(DependencyObject obj, object value)
		{
			obj.SetValue(HeaderProperty, value);
		}
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.RegisterAttached("Header", typeof(object), typeof(VisualHelper), new PropertyMetadata());

		public static DataTemplate GetHeaderTemplate(DependencyObject obj)
		{
			return (DataTemplate)obj.GetValue(HeaderTemplateProperty);
		}
		public static void SetHeaderTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(HeaderTemplateProperty, value);
		}
		public static readonly DependencyProperty HeaderTemplateProperty =
			DependencyProperty.RegisterAttached("HeaderTemplate", typeof(DataTemplate), typeof(VisualHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static HeaderAlignment GetHeaderAlignment(DependencyObject obj)
		{
			return (HeaderAlignment)obj.GetValue(HeaderAlignmentProperty);
		}
		public static void SetHeaderAlignment(DependencyObject obj, HeaderAlignment value)
		{
			obj.SetValue(HeaderAlignmentProperty, value);
		}
		public static readonly DependencyProperty HeaderAlignmentProperty =
			DependencyProperty.RegisterAttached("HeaderAlignment", typeof(HeaderAlignment), typeof(VisualHelper), new PropertyMetadata(HeaderAlignment.Top));

		public static string GetHeaderSharedGroupName(DependencyObject obj)
		{
			return (string)obj.GetValue(HeaderSharedGroupNameProperty);
		}
		public static void SetHeaderSharedGroupName(DependencyObject obj, string value)
		{
			obj.SetValue(HeaderSharedGroupNameProperty, value);
		}
		public static readonly DependencyProperty HeaderSharedGroupNameProperty =
			DependencyProperty.RegisterAttached("HeaderSharedGroupName", typeof(string), typeof(VisualHelper), new PropertyMetadata());

		public static VerticalAlignment GetHeaderVerticalAlignment(DependencyObject obj)
		{
			return (VerticalAlignment)obj.GetValue(HeaderVerticalAlignmentProperty);
		}
		public static void SetHeaderVerticalAlignment(DependencyObject obj, VerticalAlignment value)
		{
			obj.SetValue(HeaderVerticalAlignmentProperty, value);
		}
		public static readonly DependencyProperty HeaderVerticalAlignmentProperty =
			DependencyProperty.RegisterAttached("HeaderVerticalAlignment", typeof(VerticalAlignment), typeof(VisualHelper), new PropertyMetadata());

		public static HorizontalAlignment GetHeaderHorizontalAlignment(DependencyObject obj)
		{
			return (HorizontalAlignment)obj.GetValue(HeaderHorizontalAlignmentProperty);
		}
		public static void SetHeaderHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
		{
			obj.SetValue(HeaderHorizontalAlignmentProperty, value);
		}
		public static readonly DependencyProperty HeaderHorizontalAlignmentProperty =
			DependencyProperty.RegisterAttached("HeaderHorizontalAlignment", typeof(HorizontalAlignment), typeof(VisualHelper), new PropertyMetadata());

		public static object GetFooter(DependencyObject obj)
		{
			return (object)obj.GetValue(FooterProperty);
		}
		public static void SetFooter(DependencyObject obj, object value)
		{
			obj.SetValue(FooterProperty, value);
		}
		public static readonly DependencyProperty FooterProperty =
			DependencyProperty.RegisterAttached("Footer", typeof(object), typeof(VisualHelper), new PropertyMetadata());

		public static DataTemplate GetFooterTemplate(DependencyObject obj)
		{
			return (DataTemplate)obj.GetValue(FooterTemplateProperty);
		}
		public static void SetFooterTemplate(DependencyObject obj, DataTemplate value)
		{
			obj.SetValue(FooterTemplateProperty, value);
		}
		public static readonly DependencyProperty FooterTemplateProperty =
			DependencyProperty.RegisterAttached("FooterTemplate", typeof(DataTemplate), typeof(VisualHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static FooterAlignment GetFooterAlignment(DependencyObject obj)
		{
			return (FooterAlignment)obj.GetValue(FooterAlignmentProperty);
		}
		public static void SetFooterAlignment(DependencyObject obj, FooterAlignment value)
		{
			obj.SetValue(FooterAlignmentProperty, value);
		}
		public static readonly DependencyProperty FooterAlignmentProperty =
			DependencyProperty.RegisterAttached("FooterAlignment", typeof(FooterAlignment), typeof(VisualHelper), new PropertyMetadata(FooterAlignment.Bottom));

		public static string GetFooterSharedGroupName(DependencyObject obj)
		{
			return (string)obj.GetValue(FooterSharedGroupNameProperty);
		}
		public static void SetFooterSharedGroupName(DependencyObject obj, string value)
		{
			obj.SetValue(FooterSharedGroupNameProperty, value);
		}
		public static readonly DependencyProperty FooterSharedGroupNameProperty =
			DependencyProperty.RegisterAttached("FooterSharedGroupName", typeof(string), typeof(VisualHelper), new PropertyMetadata());

		public static VerticalAlignment GetFooterVerticalAlignment(DependencyObject obj)
		{
			return (VerticalAlignment)obj.GetValue(FooterVerticalAlignmentProperty);
		}
		public static void SetFooterVerticalAlignment(DependencyObject obj, VerticalAlignment value)
		{
			obj.SetValue(FooterVerticalAlignmentProperty, value);
		}
		public static readonly DependencyProperty FooterVerticalAlignmentProperty =
			DependencyProperty.RegisterAttached("FooterVerticalAlignment", typeof(VerticalAlignment), typeof(VisualHelper), new PropertyMetadata());

		public static HorizontalAlignment GetFooterHorizontalAlignment(DependencyObject obj)
		{
			return (HorizontalAlignment)obj.GetValue(FooterHorizontalAlignmentProperty);
		}
		public static void SetFooterHorizontalAlignment(DependencyObject obj, HorizontalAlignment value)
		{
			obj.SetValue(FooterHorizontalAlignmentProperty, value);
		}
		public static readonly DependencyProperty FooterHorizontalAlignmentProperty =
			DependencyProperty.RegisterAttached("FooterHorizontalAlignment", typeof(HorizontalAlignment), typeof(VisualHelper), new PropertyMetadata());

		public static Brush GetIconBrush(DependencyObject obj)
		{
			return (Brush)obj.GetValue(IconBrushProperty);
		}
		public static void SetIconBrush(DependencyObject obj, Brush value)
		{
			obj.SetValue(IconBrushProperty, value);
		}
		public static readonly DependencyProperty IconBrushProperty =
			DependencyProperty.RegisterAttached("IconBrush", typeof(Brush), typeof(VisualHelper), new PropertyMetadata());

		public static object GetTestObject(DependencyObject obj)
		{
			return (object)obj.GetValue(TestObjectProperty);
		}
		public static void SetTestObject(DependencyObject obj, object value)
		{
			obj.SetValue(TestObjectProperty, value);
		}
		public static readonly DependencyProperty TestObjectProperty =
			DependencyProperty.RegisterAttached("TestObject", typeof(object), typeof(VisualHelper), new PropertyMetadata((o, e) =>
			{
				DependencyObject t = o;
				object n = e.NewValue;
			}));

		public static FrameworkElement GetWidthConstraintSource(DependencyObject obj)
		{
			return (FrameworkElement)obj.GetValue(WidthConstraintSourceProperty);
		}
		public static void SetWidthConstraintSource(DependencyObject obj, FrameworkElement value)
		{
			obj.SetValue(WidthConstraintSourceProperty, value);
		}
		public static readonly DependencyProperty WidthConstraintSourceProperty =
			DependencyProperty.RegisterAttached("WidthConstraintSource", typeof(FrameworkElement), typeof(VisualHelper), new PropertyMetadata((o, e) =>
			{
				FrameworkElement source = e.NewValue as FrameworkElement;
				if (o is FrameworkElement target && source != null)
					target.SetBinding(FrameworkElement.MaxWidthProperty, new Binding { Source = source, Path = new PropertyPath(FrameworkElement.ActualWidthProperty), Mode = BindingMode.OneWay });
			}));

		public static bool GetIsFirstItem(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsFirstItemProperty);
		}
		public static void SetIsFirstItem(DependencyObject obj, bool value)
		{
			obj.SetValue(IsFirstItemProperty, value);
		}
		public static readonly DependencyProperty IsFirstItemProperty =
			DependencyProperty.RegisterAttached("IsFirstItem", typeof(bool), typeof(VisualHelper), new PropertyMetadata());

		public static bool GetIsLastItem(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsLastItemProperty);
		}
		public static void SetIsLastItem(DependencyObject obj, bool value)
		{
			obj.SetValue(IsLastItemProperty, value);
		}
		public static readonly DependencyProperty IsLastItemProperty =
			DependencyProperty.RegisterAttached("IsLastItem", typeof(bool), typeof(VisualHelper), new PropertyMetadata());

		public static bool GetDisableFocusVisualStyle(DependencyObject obj)
		{
			return (bool)obj.GetValue(DisableFocusVisualStyleProperty);
		}
		public static void SetDisableFocusVisualStyle(DependencyObject obj, bool value)
		{
			obj.SetValue(DisableFocusVisualStyleProperty, value);
		}
		public static readonly DependencyProperty DisableFocusVisualStyleProperty =
			DependencyProperty.RegisterAttached("DisableFocusVisualStyle", typeof(bool), typeof(VisualHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, (o, e) =>
			{
				if (o is FrameworkElement fe && fe.FocusVisualStyle != null)
				{
					fe.FocusVisualStyle = null;
					//Trace.WriteLine("FocusVisualStyle disabled on " + fe);
				}
			}));
	}
}
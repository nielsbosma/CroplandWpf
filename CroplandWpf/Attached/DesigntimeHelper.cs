using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace CroplandWpf.Attached
{
	public sealed class DesigntimeHelper
	{
		public static ResourceDictionary GetResources(DependencyObject obj)
		{
			return (ResourceDictionary)obj.GetValue(ResourcesProperty);
		}
		public static void SetResources(DependencyObject obj, ResourceDictionary value)
		{
			obj.SetValue(ResourcesProperty, value);
		}
		public static readonly DependencyProperty ResourcesProperty =
			DependencyProperty.RegisterAttached("Resources", typeof(ResourceDictionary), typeof(DesigntimeHelper), new PropertyMetadata((o, e) =>
			{
				if (DesignerProperties.GetIsInDesignMode(o) && o is FrameworkElement f && e.NewValue is ResourceDictionary d && !f.Resources.MergedDictionaries.Any(md => md.Source == d.Source))
				{
					f.Resources.MergedDictionaries.Add(d);
				}
			}));

		public static Uri GetResourcesUri(DependencyObject obj)
		{
			return (Uri)obj.GetValue(ResourcesUriProperty);
		}
		public static void SetResourcesUri(DependencyObject obj, Uri value)
		{
			obj.SetValue(ResourcesUriProperty, value);
		}
		public static readonly DependencyProperty ResourcesUriProperty =
			DependencyProperty.RegisterAttached("ResourcesUri", typeof(Uri), typeof(DesigntimeHelper), new PropertyMetadata((o, e) =>
			{
				if (DesignerProperties.GetIsInDesignMode(o) && o is FrameworkElement f && e.NewValue is Uri uri && !f.Resources.MergedDictionaries.Any(md => md.Source == uri))
				{
					ResourceDictionary d = new ResourceDictionary();
					d.Source = uri;
					f.Resources.MergedDictionaries.Add(d);
				}
			}));

		//public static string GetResourceUriString(DependencyObject obj)
		//{
		//	return (string)obj.GetValue(ResourceUriStringProperty);
		//}
		//public static void SetResourceUriString(DependencyObject obj, string value)
		//{
		//	obj.SetValue(ResourceUriStringProperty, value);
		//}
		//public static readonly DependencyProperty ResourceUriStringProperty =
		//	DependencyProperty.RegisterAttached("ResourceUriString", typeof(string), typeof(DesigntimeHelper), new PropertyMetadata((o, e) =>
		//		{
		//			MessageBox.Show(e.NewValue.ToString());

		//		}));

		public static string GetResourceUriString(DependencyObject obj)
		{
			return (string)obj.GetValue(ResourceUriStringProperty);
		}
		public static void SetResourceUriString(DependencyObject obj, string value)
		{
			obj.SetValue(ResourceUriStringProperty, value);
		}
		public static readonly DependencyProperty ResourceUriStringProperty =
			DependencyProperty.RegisterAttached("ResourceUriString", typeof(string), typeof(DesigntimeHelper), new PropertyMetadata((o, e) =>
			{
				if (DesignerProperties.GetIsInDesignMode(o) && o is FrameworkElement f && e.NewValue is string uriString && !f.Resources.MergedDictionaries.Any(md => md.Source.OriginalString == uriString))
				{
					Trace.WriteLine(e.NewValue);
					ResourceDictionary d = new ResourceDictionary();
					d.Source = new Uri(uriString);
					f.Resources.MergedDictionaries.Add(d);
				}
			}));
	}
}

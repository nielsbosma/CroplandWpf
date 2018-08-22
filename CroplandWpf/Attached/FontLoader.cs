using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CroplandWpf.Attached
{
    public class FontLoader
    {
		public static string GetCustomFontFamilySource(DependencyObject obj)
		{
			return (string)obj.GetValue(CustomFontFamilySourceProperty);
		}
		public static void SetCustomFontFamilySource(DependencyObject obj, string value)
		{
			obj.SetValue(CustomFontFamilySourceProperty, value);
		}
		public static readonly DependencyProperty CustomFontFamilySourceProperty =
			DependencyProperty.RegisterAttached("CustomFontFamilySource", typeof(string), typeof(FontLoader), new PropertyMetadata((o,e)=>
			{
				try
				{
					FontFamily ff = new FontFamily();
					string[] fontAddress = e.NewValue.ToString().Split('$');
					Uri fontUri = new Uri(fontAddress[0], UriKind.Absolute);
					ff = new FontFamily(fontUri, fontAddress[1]);
					//(o as Window).FontFamily = ff;
					o.SetValue(Control.FontFamilyProperty, ff);
				}
				catch { }
			}));
	}
}

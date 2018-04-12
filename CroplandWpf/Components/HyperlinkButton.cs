using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Components
{
    public class HyperlinkButton : Button
    {
		static HyperlinkButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HyperlinkButton), new FrameworkPropertyMetadata(typeof(HyperlinkButton)));
		}

		public bool IsUnderlined
		{
			get { return (bool)GetValue(IsUnderlinedProperty); }
			set { SetValue(IsUnderlinedProperty, value); }
		}
		public static readonly DependencyProperty IsUnderlinedProperty =
			DependencyProperty.Register("IsUnderlined", typeof(bool), typeof(HyperlinkButton), new PropertyMetadata(true));

		public HyperlinkButton()
		{

		}
    }
}
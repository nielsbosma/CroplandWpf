using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.MVVM
{
	public class SliderInfo : DependencyObject
	{
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(SliderInfo), new PropertyMetadata());

		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(double), typeof(SliderInfo), new PropertyMetadata());

		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(SliderInfo), new PropertyMetadata());

		public string DisplayValueFormat
		{
			get { return (string)GetValue(DisplayValueFormatProperty); }
			set { SetValue(DisplayValueFormatProperty, value); }
		}
		public static readonly DependencyProperty DisplayValueFormatProperty =
			DependencyProperty.Register("DisplayValueFormat", typeof(string), typeof(SliderInfo), new PropertyMetadata());
	}
}
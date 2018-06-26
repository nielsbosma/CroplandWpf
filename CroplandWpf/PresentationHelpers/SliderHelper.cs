using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CroplandWpf.PresentationHelpers
{
	public class SliderHelper : FrameworkElement
	{
		public static SliderHelper GetAttachedHelper(DependencyObject obj)
		{
			return (SliderHelper)obj.GetValue(AttachedHelperProperty);
		}
		public static void SetAttachedHelper(DependencyObject obj, SliderHelper value)
		{
			obj.SetValue(AttachedHelperProperty, value);
		}
		public static readonly DependencyProperty AttachedHelperProperty =
			DependencyProperty.RegisterAttached("AttachedHelper", typeof(SliderHelper), typeof(SliderHelper), new PropertyMetadata());

		public Slider Target
		{
			get { return (Slider)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Slider), typeof(SliderHelper), new PropertyMetadata());

		public double TargetValue
		{
			get { return (double)GetValue(TargetValueProperty); }
			set { SetValue(TargetValueProperty, value); }
		}
		public static readonly DependencyProperty TargetValueProperty =
			DependencyProperty.Register("TargetValue", typeof(double), typeof(SliderHelper), new PropertyMetadata());

		public string FormattedValue
		{
			get { return (string)GetValue(FormattedValueProperty); }
			set { SetValue(FormattedValueProperty, value); }
		}
		public static readonly DependencyProperty FormattedValueProperty =
			DependencyProperty.Register("FormattedValue", typeof(string), typeof(SliderHelper), new PropertyMetadata());

		private string targetValueFormatString
		{
			get { return Attached.Slider.GetValueStringFormat(this); }
		}

		public SliderHelper()
		{
			Visibility = Visibility.Hidden;
			IsHitTestVisible = false;
			Opacity = 0.0;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TargetProperty && e.NewValue != null)
			{
				SetAttachedHelper(Target, this);
				SetBinding(Attached.Slider.ValueStringFormatProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(Attached.Slider.ValueStringFormatProperty), Mode = BindingMode.OneWay });
				SetBinding(TargetValueProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(Slider.ValueProperty), Mode = BindingMode.OneWay });
			}
			if (e.Property == Attached.Slider.ValueStringFormatProperty || e.Property == TargetValueProperty)
			{
				if (!String.IsNullOrEmpty(targetValueFormatString))
					FormattedValue = String.Format(targetValueFormatString, TargetValue);
				else
					FormattedValue = TargetValue.ToString();
			}
		}
	}
}

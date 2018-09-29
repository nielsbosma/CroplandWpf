using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.Helpers
{
	public class MessageBoxContentWidthHelper : FrameworkElement
	{
		public double FooterSectionWidth
		{
			get { return (double)GetValue(FooterSectionWidthProperty); }
			set { SetValue(FooterSectionWidthProperty, value); }
		}
		public static readonly DependencyProperty FooterSectionWidthProperty =
			DependencyProperty.Register("FooterSectionWidth", typeof(double), typeof(MessageBoxContentWidthHelper), new PropertyMetadata());

		public double CalculatedContentMaxWidth
		{
			get { return (double)GetValue(CalculatedContentMaxWidthProperty); }
			private set { SetValue(CalculatedContentMaxWidthProperty, value); }
		}
		public static readonly DependencyProperty CalculatedContentMaxWidthProperty =
			DependencyProperty.Register("CalculatedContentMaxWidth", typeof(double), typeof(MessageBoxContentWidthHelper), new PropertyMetadata());

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == FooterSectionWidthProperty)
			{
				if(FooterSectionWidth != 0.0 && !FooterSectionWidth.Equals(double.NaN))
				{
					CalculatedContentMaxWidth = Math.Max(FooterSectionWidth, MinWidth);
				}
			}
		}
	}
}

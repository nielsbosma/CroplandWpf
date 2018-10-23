using System;
using System.Windows;
using System.Windows.Media;

namespace CroplandWpf.PresentationHelpers
{
	public class CroppedWindowBackgroundHelper : FrameworkElement
	{
		public Color HeaderBackgroundColor
		{
			get { return (Color)GetValue(HeaderBackgroundColorProperty); }
			set { SetValue(HeaderBackgroundColorProperty, value); }
		}
		public static readonly DependencyProperty HeaderBackgroundColorProperty =
			DependencyProperty.Register("HeaderBackgroundColor", typeof(Color), typeof(CroppedWindowBackgroundHelper), new PropertyMetadata());

		public Color ContentBackgroundColor
		{
			get { return (Color)GetValue(ContentBackgroundColorProperty); }
			set { SetValue(ContentBackgroundColorProperty, value); }
		}
		public static readonly DependencyProperty ContentBackgroundColorProperty =
			DependencyProperty.Register("ContentBackgroundColor", typeof(Color), typeof(CroppedWindowBackgroundHelper), new PropertyMetadata());

		public double HeaderSectionHeight
		{
			get { return (double)GetValue(HeaderSectionHeightProperty); }
			set { SetValue(HeaderSectionHeightProperty, value); }
		}
		public static readonly DependencyProperty HeaderSectionHeightProperty =
			DependencyProperty.Register("HeaderSectionHeight", typeof(double), typeof(CroppedWindowBackgroundHelper), new PropertyMetadata());

		public LinearGradientBrush WindowBackgroundBrush
		{
			get { return (LinearGradientBrush)GetValue(WindowBackgroundBrushProperty); }
			set { SetValue(WindowBackgroundBrushProperty, value); }
		}
		public static readonly DependencyProperty WindowBackgroundBrushProperty =
			DependencyProperty.Register("WindowBackgroundBrush", typeof(LinearGradientBrush), typeof(CroppedWindowBackgroundHelper), new PropertyMetadata());

		public double TotalHeight
		{
			get { return (double)GetValue(TotalHeightProperty); }
			set { SetValue(TotalHeightProperty, value); }
		}
		public static readonly DependencyProperty TotalHeightProperty =
			DependencyProperty.Register("TotalHeight", typeof(double), typeof(CroppedWindowBackgroundHelper), new PropertyMetadata());

		private double finalTotalHeight
		{
			get { return Math.Max(1, TotalHeight); }
		}

		private double finalHeaderHeight
		{
			get
			{
				return Math.Max(1, HeaderSectionHeight);
			}
		}

		private double headerStopOffset
		{
			get { return finalHeaderHeight / finalTotalHeight; }
		}

		public CroppedWindowBackgroundHelper()
		{
			Visibility = Visibility.Collapsed;
			IsHitTestVisible = false;
			WindowBackgroundBrush = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
			WindowBackgroundBrush.GradientStops.Add(new GradientStop(HeaderBackgroundColor, 0.0));
			WindowBackgroundBrush.GradientStops.Add(new GradientStop(HeaderBackgroundColor, headerStopOffset));
			WindowBackgroundBrush.GradientStops.Add(new GradientStop(HeaderBackgroundColor, headerStopOffset));
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == HeaderSectionHeightProperty || e.Property == TotalHeightProperty)
			{
				WindowBackgroundBrush.GradientStops[1].Offset = headerStopOffset;
				WindowBackgroundBrush.GradientStops[2].Offset = headerStopOffset;
			}
			if (e.Property == HeaderBackgroundColorProperty)
			{
				WindowBackgroundBrush.GradientStops[0].Color = (Color)e.NewValue;
				WindowBackgroundBrush.GradientStops[1].Color = (Color)e.NewValue;
			}
			if (e.Property == ContentBackgroundColorProperty)
			{
				WindowBackgroundBrush.GradientStops[2].Color = (Color)e.NewValue;
			}
		}
	}
}

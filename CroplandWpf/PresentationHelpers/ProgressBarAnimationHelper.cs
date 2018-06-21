using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CroplandWpf.PresentationHelpers
{
	public class ProgressBarAnimationHelper : FrameworkElement
	{
		public static double GetIndeterminateIndicatorPercentageWidth(DependencyObject obj)
		{
			return (double)obj.GetValue(IndeterminateIndicatorPercentageWidthProperty);
		}
		public static void SetIndeterminateIndicatorPercentageWidth(DependencyObject obj, double value)
		{
			obj.SetValue(IndeterminateIndicatorPercentageWidthProperty, value);
		}
		public static readonly DependencyProperty IndeterminateIndicatorPercentageWidthProperty =
			DependencyProperty.RegisterAttached("IndeterminateIndicatorPercentageWidth", typeof(double), typeof(ProgressBarAnimationHelper), new FrameworkPropertyMetadata(0.3, FrameworkPropertyMetadataOptions.Inherits, null, CoerceIndeterminateIndicatorPercentageWidth));

		private static object CoerceIndeterminateIndicatorPercentageWidth(DependencyObject target, object value)
		{
			double dValue = (double)value;
			if (dValue > 0.9)
				dValue = 0.9;
			else if (dValue < 0.1)
				dValue = 0.1;
			return dValue;
		}

		public FrameworkElement Indicator
		{
			get { return (FrameworkElement)GetValue(IndicatorProperty); }
			set { SetValue(IndicatorProperty, value); }
		}
		public static readonly DependencyProperty IndicatorProperty =
			DependencyProperty.Register("Indicator", typeof(FrameworkElement), typeof(ProgressBarAnimationHelper), new PropertyMetadata());

		public ProgressBar Container
		{
			get { return (ProgressBar)GetValue(ContainerProperty); }
			set { SetValue(ContainerProperty, value); }
		}
		public static readonly DependencyProperty ContainerProperty =
			DependencyProperty.Register("Container", typeof(ProgressBar), typeof(ProgressBarAnimationHelper), new PropertyMetadata());

		private Storyboard animationStoryboard;

		private bool readyForAnimation
		{
			get { return Indicator != null && Container != null; }
		}

		public ProgressBarAnimationHelper()
		{

		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == IsEnabledProperty)
			{
				if (readyForAnimation && GetIndeterminateIndicatorPercentageWidth(Container) > 0.0)
				{
					if ((bool)e.NewValue)
						StartAnimation();
					else
						StopAnimation();
				}
			}
		}

		private void StartAnimation()
		{
			animationStoryboard = new Storyboard();
			animationStoryboard.RepeatBehavior = RepeatBehavior.Forever;
			double indicatorWidth = GetIndeterminateIndicatorPercentageWidth(Container) * Container.ActualWidth;
			double indicatorStartWidth = indicatorWidth * 0.3;

			Indicator.Visibility = Visibility.Visible;
			Indicator.Width = indicatorStartWidth;

			ThicknessAnimation marginAnimation0 = new ThicknessAnimation(new Thickness(-indicatorStartWidth, 0, 0, 0), new Thickness(Container.ActualWidth, 0, 0, 0), new Duration(TimeSpan.FromSeconds(2)));
			Storyboard.SetTarget(marginAnimation0, Indicator);
			Storyboard.SetTargetProperty(marginAnimation0, new PropertyPath(MarginProperty));
			animationStoryboard.Children.Add(marginAnimation0);

			ThicknessAnimation marginAnimation1 = new ThicknessAnimation(new Thickness(-indicatorWidth, 0, 0, 0), new Thickness(Container.ActualWidth, 0, 0, 0), new Duration(TimeSpan.FromSeconds(2)));
			marginAnimation1.BeginTime = TimeSpan.FromSeconds(2.0);
			Storyboard.SetTarget(marginAnimation1, Indicator);
			Storyboard.SetTargetProperty(marginAnimation1, new PropertyPath(MarginProperty));
			animationStoryboard.Children.Add(marginAnimation1);

			DoubleAnimation widthAnimation1 = new DoubleAnimation(indicatorStartWidth, indicatorWidth, new Duration(TimeSpan.FromSeconds(2.0)));
			widthAnimation1.AutoReverse = true;
			Storyboard.SetTarget(widthAnimation1, Indicator);
			Storyboard.SetTargetProperty(widthAnimation1, new PropertyPath(WidthProperty));
			animationStoryboard.Children.Add(widthAnimation1);

			animationStoryboard.Begin();
		}

		private void StopAnimation()
		{
			if (animationStoryboard != null)
			{
				Indicator.Visibility = Visibility.Hidden;
				animationStoryboard.Stop(Indicator);
				animationStoryboard = null;
			}
		}
	}
}
using CroplandWpf.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CroplandWpf.PresentationHelpers
{
	public class CalendarHelper : FrameworkElement
	{
		public static bool GetIsForceLooseMouseCaptureEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsForceLooseMouseCaptureEnabledProperty);
		}
		public static void SetIsForceLooseMouseCaptureEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsForceLooseMouseCaptureEnabledProperty, value);
		}
		public static readonly DependencyProperty IsForceLooseMouseCaptureEnabledProperty =
			DependencyProperty.RegisterAttached("IsForceLooseMouseCaptureEnabled", typeof(bool), typeof(CalendarHelper), new PropertyMetadata());

		public Calendar Target
		{
			get { return (Calendar)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Calendar), typeof(CalendarHelper), new PropertyMetadata());

		public CalendarHelper()
		{
			Loaded += CalendarHelper_Loaded;
			Unloaded += CalendarHelper_Unloaded;
		}

		private void CalendarHelper_Loaded(object sender, RoutedEventArgs e)
		{
			Mouse.AddGotMouseCaptureHandler(Target, GlobalMouseCapture);
		}

		private void GlobalMouseCapture(object sender, MouseEventArgs e)
		{
			if (e.Source == Target)
			{
				if (e.OriginalSource is UIElement el)
					Dispatcher.Invoke(new Action(() => el.ReleaseMouseCapture()), System.Windows.Threading.DispatcherPriority.Background);
			}
		}

		private void CalendarHelper_Unloaded(object sender, RoutedEventArgs e)
		{
			Mouse.RemoveGotMouseCaptureHandler(Target, GlobalMouseCapture);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			//if (IsEnabled && TargetHasMouseCaptured)
			//	Mouse.Capture(null);
		}
	}
}

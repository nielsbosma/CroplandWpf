using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CroplandWpf.Components
{
    public class WindowResizer : Control
    {
		public WindowState WindowState
		{
			get { return (WindowState)GetValue(WindowStateProperty); }
			set { SetValue(WindowStateProperty, value); }
		}
		public static readonly DependencyProperty WindowStateProperty =
			DependencyProperty.Register("WindowState", typeof(WindowState), typeof(WindowResizer), new PropertyMetadata());

		public bool IsResizeEnabled
		{
			get { return (bool)GetValue(IsResizeEnabledProperty); }
			set { SetValue(IsResizeEnabledProperty, value); }
		}
		public static readonly DependencyProperty IsResizeEnabledProperty =
			DependencyProperty.Register("IsResizeEnabled", typeof(bool), typeof(WindowResizer), new PropertyMetadata());

		static WindowResizer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowResizer), new FrameworkPropertyMetadata(typeof(WindowResizer)));
		}

		public WindowResizer()
		{
			Loaded += WindowResizer_Loaded;
		}

		private void WindowResizer_Loaded(object sender, RoutedEventArgs e)
		{
			SetBinding(IsResizeEnabledProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1), Path = new PropertyPath(Helpers.WindowHelper.AllowResizeProperty), Mode = BindingMode.OneWay });
			SetBinding(WindowStateProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1), Path = new PropertyPath(Window.WindowStateProperty), Mode = BindingMode.OneWay });
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public enum WindowControlThumbRole
	{
		NA,
		Move
	}

    public class WindowControlThumb : Thumb
    {
		public WindowControlThumbRole Role
		{
			get { return (WindowControlThumbRole)GetValue(RoleProperty); }
			set { SetValue(RoleProperty, value); }
		}
		public static readonly DependencyProperty RoleProperty =
			DependencyProperty.Register("Role", typeof(WindowControlThumbRole), typeof(WindowControlThumb), new PropertyMetadata());

		public Window Target
		{
			get { return (Window)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Window), typeof(WindowControlThumb), new PropertyMetadata());

		private double targetLeft_Restore = 0.0;
		private double targetTop_Restore = 0.0;

		public WindowControlThumb()
		{
			Loaded += WindowControlThumb_Loaded;
			DragDelta += WindowControlThumb_DragDelta;
		}

		private void WindowControlThumb_Loaded(object sender, RoutedEventArgs e)
		{
			Target = Window.GetWindow(this);
		}

		private void WindowControlThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if (Target == null)
				return;
			switch(Role)
			{
				case WindowControlThumbRole.Move:
					Target.Left += e.HorizontalChange;
					Target.Top += e.VerticalChange;
					break;

				default:
					break;
			}
		}

		protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseDoubleClick(e);

			if (Target == null)
				return;
			if(Role == WindowControlThumbRole.Move)
			{
				if (Target.WindowState == WindowState.Normal)
				{
					targetLeft_Restore = Target.Left;
					targetTop_Restore = Target.Top;
					Target.WindowState = WindowState.Maximized;
				}
				else if (Target.WindowState == WindowState.Maximized)
				{
					Target.WindowState = WindowState.Normal;
					Dispatcher.BeginInvoke(new Action(() => {
						Target.Left = targetLeft_Restore;
						Target.Top = targetTop_Restore;
					}), System.Windows.Threading.DispatcherPriority.Background);
				}
			}
		}
	}
}
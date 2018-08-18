using CroplandWpf.Attached;
using CroplandWpf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public enum ResizeThumbRole
	{
		NA,
		Move,
		ResizeLeftTop,
		ResizeTop,
		ResizeRightTop,
		ResizeLeft,
		ResizeRight,
		ResizeLeftBottom,
		ResizeBottom,
		ResizeRightBottom
	}

	public class WindowControlThumb : Thumb
	{
		public ResizeThumbRole Role
		{
			get { return (ResizeThumbRole)GetValue(RoleProperty); }
			set { SetValue(RoleProperty, value); }
		}
		public static readonly DependencyProperty RoleProperty =
			DependencyProperty.Register("Role", typeof(ResizeThumbRole), typeof(WindowControlThumb), new PropertyMetadata());

		public Window Target
		{
			get { return (Window)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Window), typeof(WindowControlThumb), new PropertyMetadata());

		public bool AllowMaximize
		{
			get { return (bool)GetValue(AllowMaximizeProperty); }
			set { SetValue(AllowMaximizeProperty, value); }
		}
		public static readonly DependencyProperty AllowMaximizeProperty =
			DependencyProperty.Register("AllowMaximize", typeof(bool), typeof(WindowControlThumb), new PropertyMetadata(true));

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
			if (Role == ResizeThumbRole.Move)
				SetBinding(VisualHelper.TestObjectProperty, new Binding { Source = Target, Path = new PropertyPath(Window.WindowStateProperty), Mode = BindingMode.OneWay });
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == RoleProperty)
				SetCursor((ResizeThumbRole)e.NewValue);
			if (e.Property == VisualHelper.TestObjectProperty)
			{
				WindowState ws = (WindowState)e.NewValue;
				if (Role == ResizeThumbRole.Move)
					Cursor = ws == WindowState.Maximized ? Cursors.Arrow : Cursors.SizeAll;
			}
		}

		private void SetCursor(ResizeThumbRole role)
		{
			switch (role)
			{
				case ResizeThumbRole.Move:
					Cursor = Cursors.SizeAll;
					break;

				case ResizeThumbRole.ResizeLeftTop:
				case ResizeThumbRole.ResizeRightBottom:
					Cursor = Cursors.SizeNWSE;
					break;

				case ResizeThumbRole.ResizeTop:
				case ResizeThumbRole.ResizeBottom:
					Cursor = Cursors.SizeNS;
					break;

				case ResizeThumbRole.ResizeLeft:
				case ResizeThumbRole.ResizeRight:
					Cursor = Cursors.SizeWE;
					break;

				case ResizeThumbRole.ResizeLeftBottom:
				case ResizeThumbRole.ResizeRightTop:
					Cursor = Cursors.SizeNESW;
					break;

				default:
					break;
			}
		}

		private void WindowControlThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if (Target == null || WindowHelper.GetIsMaximizing(Target) || Target.WindowState == WindowState.Maximized)
				return;

			double newLeft = Target.Left, newTop = Target.Top, newWidth = Target.Width, newHeight = Target.Height, minWidth = Target.MinWidth, minHeight = Target.MinHeight;

			switch (Role)
			{
				case ResizeThumbRole.Move:
					newLeft += e.HorizontalChange;
					newTop += e.VerticalChange;
					break;

				case ResizeThumbRole.ResizeLeftTop:
					newLeft += e.HorizontalChange;
					newTop += e.VerticalChange;
					newWidth -= e.HorizontalChange;
					newHeight -= e.VerticalChange;
					break;

				case ResizeThumbRole.ResizeTop:
					newTop += e.VerticalChange;
					newHeight -= e.VerticalChange;
					break;

				case ResizeThumbRole.ResizeRightTop:
					newTop += e.VerticalChange;
					newHeight -= e.VerticalChange;
					newWidth += e.HorizontalChange;
					break;

				case ResizeThumbRole.ResizeLeft:
					newLeft += e.HorizontalChange;
					newWidth -= e.HorizontalChange;
					break;

				case ResizeThumbRole.ResizeRight:
					newWidth += e.HorizontalChange;
					break;

				case ResizeThumbRole.ResizeLeftBottom:
					newLeft += e.HorizontalChange;
					newWidth -= e.HorizontalChange;
					newHeight += e.VerticalChange;
					break;

				case ResizeThumbRole.ResizeBottom:
					newHeight += e.VerticalChange;
					break;

				case ResizeThumbRole.ResizeRightBottom:
					newWidth += e.HorizontalChange;
					newHeight += e.VerticalChange;
					break;

				default:
					break;
			}

			if (newWidth < Target.MinWidth || newWidth < 0)
			{
				newWidth = Target.MinWidth;
				newLeft = Target.Left;
			}
			if (newHeight < Target.MinHeight || newHeight < 0)
			{
				newHeight = Target.MinHeight;
				newTop = Target.Top;
			}

			WindowHelper.ResizeWindow(Target, newLeft, newTop, newWidth, newHeight);
		}

		protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseDoubleClick(e);

			if (Target == null)
				return;
			if (Role == ResizeThumbRole.Move && AllowMaximize)
			{
				WindowHelper.SetIsMaximizing(Target, true);
				if (Target.WindowState == WindowState.Normal)
				{
					targetLeft_Restore = Target.Left;
					targetTop_Restore = Target.Top;
					Target.WindowState = WindowState.Maximized;
					Dispatcher.BeginInvoke(new Action(() => WindowHelper.SetIsMaximizing(Target, false)), System.Windows.Threading.DispatcherPriority.Background);
				}
				else if (Target.WindowState == WindowState.Maximized)
				{
					Target.WindowState = WindowState.Normal;
					Dispatcher.BeginInvoke(new Action(() =>
					{
						Target.Left = targetLeft_Restore;
						Target.Top = targetTop_Restore;
						WindowHelper.SetIsMaximizing(Target, false);
					}), System.Windows.Threading.DispatcherPriority.Background);
				}
			}
		}
	}
}
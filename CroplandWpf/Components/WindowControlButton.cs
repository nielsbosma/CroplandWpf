using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public enum WindowControlButtonRole
	{
		NA,
		MinimizeMaximize,
		Close
	}

    public class WindowControlButton : Button
    {
		public WindowControlButtonRole Role
		{
			get { return (WindowControlButtonRole)GetValue(RoleProperty); }
			set { SetValue(RoleProperty, value); }
		}
		public static readonly DependencyProperty RoleProperty =
			DependencyProperty.Register("Role", typeof(WindowControlButtonRole), typeof(WindowControlButton), new PropertyMetadata());

		public Window Target
		{
			get { return (Window)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Window), typeof(WindowControlButton), new PropertyMetadata());

		static WindowControlButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowControlButton), new FrameworkPropertyMetadata(typeof(WindowControlButton)));
		}

		public WindowControlButton()
		{
			Loaded += WindowControlButton_Loaded;
		}

		private void WindowControlButton_Loaded(object sender, RoutedEventArgs e)
		{
			Target = Window.GetWindow(this);
		}

		protected override void OnClick()
		{
			base.OnClick();

			if (Target == null)
				return;

			if(Role == WindowControlButtonRole.Close)
			{
				//TODO
				Target.Close();
			}
		}

		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			if (Target == null)
				return;
			if(Role == WindowControlButtonRole.MinimizeMaximize)
			{

			}
		}
	}
}
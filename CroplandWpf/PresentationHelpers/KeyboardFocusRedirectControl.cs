using CroplandWpf.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CroplandWpf.PresentationHelpers
{
	public class KeyboardFocusRedirectControl : Control
	{
		static KeyboardFocusRedirectControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyboardFocusRedirectControl), new FrameworkPropertyMetadata(typeof(KeyboardFocusRedirectControl)));
		}

		public IInputElement FocusTarget
		{
			get { return (IInputElement)GetValue(FocusTargetProperty); }
			set { SetValue(FocusTargetProperty, value); }
		}
		public static readonly DependencyProperty FocusTargetProperty =
			DependencyProperty.Register("FocusTarget", typeof(IInputElement), typeof(KeyboardFocusRedirectControl), new PropertyMetadata());

		public KeyboardFocusRedirectControl()
		{
			Focusable = false;
		}

		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);
			if (IsMouseOver && FocusTarget != null && !FocusTarget.IsKeyboardFocused)
				Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => FocusManager.SetFocusedElement(WindowHelper.GetActiveWindowInstance(), FocusTarget)));
		}
	}
}
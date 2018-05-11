using CroplandWpf.Attached;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CroplandWpf.Components
{
	public class OverlayContentControl : HeaderedContentControl
	{
		public OverlayHost ParentOverlay { get; set; }

		static OverlayContentControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayContentControl), new FrameworkPropertyMetadata(typeof(OverlayContentControl)));
		}

		public OverlayContentControl()
		{
			Loaded += MessageBoxPresenter_Loaded;
			Unloaded += MessageBoxPresenter_Unloaded;
		}

		private void MessageBoxPresenter_Loaded(object sender, RoutedEventArgs e)
		{
			KeyboardNavigationMode m = KeyboardNavigation.GetTabNavigation(ParentOverlay.OwnerWindow);
			KeyboardNavigation.SetTabNavigation(ParentOverlay.OwnerWindow, KeyboardNavigationMode.None);
			Keyboard.Focus(this);
		}

		private void MessageBoxPresenter_Unloaded(object sender, RoutedEventArgs e)
		{
			KeyboardNavigation.SetTabNavigation(ParentOverlay.OwnerWindow, KeyboardNavigationMode.Cycle);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
		}
	}
}
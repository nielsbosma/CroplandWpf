using CroplandWpf.Attached;
using CroplandWpf.Exceptions;
using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CroplandWpf.Components
{
	public class OverlayHost : Control
	{
		private static List<OverlayHost> registeredOverlays = new List<OverlayHost>();

		public string ScopeName
		{
			get { return (string)GetValue(ScopeNameProperty); }
			set { SetValue(ScopeNameProperty, value); }
		}
		public static readonly DependencyProperty ScopeNameProperty =
			DependencyProperty.Register("ScopeName", typeof(string), typeof(OverlayHost), new PropertyMetadata());

		public bool HasChildren
		{
			get { return (bool)GetValue(HasChildrenProperty); }
			private set { SetValue(HasChildrenProperty, value); }
		}
		public static readonly DependencyProperty HasChildrenProperty =
			DependencyProperty.Register("HasChildren", typeof(bool), typeof(OverlayHost), new PropertyMetadata());

		public Window OwnerWindow { get; private set; }

		private Canvas hostCanvas;

		static OverlayHost()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayHost), new FrameworkPropertyMetadata(typeof(OverlayHost)));
		}

		public OverlayHost()
		{
			Loaded += Overlay_Loaded;
			Unloaded += Overlay_Unloaded;
		}

		public OverlayContentControl ShowContent(object content, Rect placementRect, object contentTemplateKey = null)
		{
			OverlayContentControl existingOCC = hostCanvas.Children.OfType<OverlayContentControl>().SingleOrDefault(o => o.Content == content);
			if (existingOCC != null)
				return existingOCC;
			OverlayContentControl occ = new OverlayContentControl();
			occ.ParentOverlay = this;
			occ.Content = content;
			if (contentTemplateKey != null)
				occ.SetResourceReference(OverlayContentControl.ContentTemplateProperty, contentTemplateKey);
			hostCanvas.Children.Add(occ);
			occ.TargetRect = placementRect;
			HasChildren = true;
			occ.IsRendering = true;
			return occ;
		}

		public void HideContent(object content)
		{
			OverlayContentControl occ = hostCanvas.Children.OfType<OverlayContentControl>().SingleOrDefault(o => o.Content == content);
			if (occ != null)
			{
				hostCanvas.Children.Remove(occ);
				occ.IsRendering = false;
			}
			HasChildren = hostCanvas.Children.Count > 0;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
		}

		private void Overlay_Loaded(object sender, RoutedEventArgs e)
		{
			Register(this);
			OwnerWindow = Window.GetWindow(this);
		}

		private void Overlay_Unloaded(object sender, RoutedEventArgs e)
		{
			Unregister(this);
			OwnerWindow = null;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			hostCanvas = Template.FindName("PART_HostCanvas", this) as Canvas;
			if (hostCanvas == null)
				throw new TemplatePartNotFoundException("PART_HostCanvas", GetType());
		}

		#region Static methods
		private static void Register(OverlayHost overlay)
		{
			if (!registeredOverlays.Contains(overlay) && !registeredOverlays.Any(o => o.ScopeName == overlay.ScopeName))
				registeredOverlays.Add(overlay);
		}

		private static void Unregister(OverlayHost overlay)
		{
			if (registeredOverlays.Contains(overlay))
				registeredOverlays.Remove(overlay);
		}

		public static OverlayHost GetOverlay(string scopeName = "")
		{
			return registeredOverlays.SingleOrDefault(o => String.IsNullOrEmpty(scopeName) ? o.OwnerWindow == WindowHelper.GetActiveWindowInstance() : o.ScopeName == scopeName);
		}
		#endregion
	}
}
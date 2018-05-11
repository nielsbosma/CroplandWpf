using CroplandWpf.Attached;
using CroplandWpf.Exceptions;
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
	public enum MessageBoxCloseRequestArgument
	{
		None,
		Positive,
		Negative
	}

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

		public Window OwnerWindow { get; private set; }

		private OverlayContentControl _presenter;

		private DispatcherTimer showDelayTimer;

		static OverlayHost()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayHost), new FrameworkPropertyMetadata(typeof(OverlayHost)));
		}

		public OverlayHost()
		{
			Loaded += MessageBoxOverlay_Loaded;
			Unloaded += MessageBoxOverlay_Unloaded;
			showDelayTimer = new DispatcherTimer(DispatcherPriority.Background);
			showDelayTimer.Interval = TimeSpan.FromMilliseconds(500);
			showDelayTimer.Tick += ShowDelayTimer_Tick;
		}

		private void ShowDelayTimer_Tick(object sender, EventArgs e)
		{
			
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
		}

		private void MessageBoxOverlay_Loaded(object sender, RoutedEventArgs e)
		{
			Register(this);
			OwnerWindow = Window.GetWindow(this);
		}

		private void MessageBoxOverlay_Unloaded(object sender, RoutedEventArgs e)
		{
			Unregister(this);
			OwnerWindow = null;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_presenter = Template.FindName("PART_Presenter", this) as OverlayContentControl;
			if (_presenter == null)
				throw new TemplatePartNotFoundException("PART_Presenter", GetType());
			_presenter.ParentOverlay = this;
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

		private static OverlayHost GetOverlayByScopeName(string scopeName)
		{
			if (String.IsNullOrEmpty(scopeName))
				return null;
			return registeredOverlays.FirstOrDefault(o => o.ScopeName == scopeName);
		}
		#endregion
	}
}
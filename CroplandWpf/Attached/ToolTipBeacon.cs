using CroplandWpf.Components;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace CroplandWpf.Attached
{ 
	public class ToolTipGroupVisibility
	{
		public string GroupName { get; set; }
		public bool IsEnabled { get; set; }
	}

	public enum ToolTipHideMode
	{
		Timer,
		Explicit
	}

	public class ToolTipBeacon : Control
	{
		#region Static commands
		public static DelegateCommand DisableToolTipsGroupCommand { get; } = new DelegateCommand(DisableToolTipsGroupCommand_Execute);

		public static DelegateCommand EnableToolTipsGroupCommand { get; } = new DelegateCommand(EnableToolTipsGroupCommand_Execute);

		private static ResourceDictionary disabledToolTipGroupsDictionary
		{
			get
			{
				if (_disabledToolTipGroupsDictionary == null)
				{
					XmlReader reader = XmlReader.Create(new StringReader(Properties.Settings.Default.xamlToolTipsVisibilityDictionary));
					_disabledToolTipGroupsDictionary = null;
					if (reader.HasValue)
					{
						try
						{
							_disabledToolTipGroupsDictionary = (ResourceDictionary)XamlReader.Parse(Properties.Settings.Default.xamlToolTipsVisibilityDictionary);
						}
						catch { }
					}
					if (_disabledToolTipGroupsDictionary == null)
					{
						_disabledToolTipGroupsDictionary = new ResourceDictionary();
						SaveDisabledToolTipGroups();
					}
				}
				return _disabledToolTipGroupsDictionary;
			}
		}
		private static ResourceDictionary _disabledToolTipGroupsDictionary;

		private static void SaveDisabledToolTipGroups()
		{
			Properties.Settings.Default.xamlToolTipsVisibilityDictionary = XamlWriter.Save(disabledToolTipGroupsDictionary);
			Properties.Settings.Default.Save();
		}

		private static void DisableToolTipsGroupCommand_Execute(object parameter)
		{
			ToolTipBeacon beacon = parameter as ToolTipBeacon;
			string groupName = null;
			if (beacon != null)
			{
				beacon.HideToolTip();
				groupName = GetInheritedGroupName(beacon);
			}
			else
				groupName = parameter as string;
			if (String.IsNullOrWhiteSpace(groupName))
				return;
			ToolTipGroupVisibility ttgv = new ToolTipGroupVisibility();
			if (disabledToolTipGroupsDictionary.Contains(groupName))
				(disabledToolTipGroupsDictionary[groupName] as ToolTipGroupVisibility).IsEnabled = false;
			else
				disabledToolTipGroupsDictionary.Add(groupName, new ToolTipGroupVisibility { GroupName = groupName, IsEnabled = false });
			SaveDisabledToolTipGroups();
		}

		private static void EnableToolTipsGroupCommand_Execute(object parameter)
		{

		}

		public static bool IsToolTipsGroupEnabled(string groupName)
		{
			return !disabledToolTipGroupsDictionary.Contains(groupName);
		}
		#endregion

		#region APs
		public static object GetToolTipContent(DependencyObject obj)
		{
			return (object)obj.GetValue(ToolTipContentProperty);
		}
		public static void SetToolTipContent(DependencyObject obj, object value)
		{
			obj.SetValue(ToolTipContentProperty, value);
		}
		public static readonly DependencyProperty ToolTipContentProperty =
			DependencyProperty.RegisterAttached("ToolTipContent", typeof(object), typeof(ToolTipBeacon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static object GetToolTipTemplateKey(DependencyObject obj)
		{
			return (object)obj.GetValue(ToolTipTemplateKeyProperty);
		}
		public static void SetToolTipTemplateKey(DependencyObject obj, object value)
		{
			obj.SetValue(ToolTipTemplateKeyProperty, value);
		}
		public static readonly DependencyProperty ToolTipTemplateKeyProperty =
			DependencyProperty.RegisterAttached("ToolTipTemplateKey", typeof(object), typeof(ToolTipBeacon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static string GetInheritedGroupName(DependencyObject obj)
		{
			return (string)obj.GetValue(InheritedGroupNameProperty);
		}
		public static void SetInheritedGroupName(DependencyObject obj, string value)
		{
			obj.SetValue(InheritedGroupNameProperty, value);
		}
		public static readonly DependencyProperty InheritedGroupNameProperty =
			DependencyProperty.RegisterAttached("InheritedGroupName", typeof(string), typeof(ToolTipBeacon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static ToolTipPlacement GetPlacementPriority(DependencyObject obj)
		{
			return (ToolTipPlacement)obj.GetValue(PlacementPriorityProperty);
		}
		public static void SetPlacementPriority(DependencyObject obj, ToolTipPlacement value)
		{
			obj.SetValue(PlacementPriorityProperty, value);
		}
		public static readonly DependencyProperty PlacementPriorityProperty =
			DependencyProperty.RegisterAttached("PlacementPriority", typeof(ToolTipPlacement), typeof(ToolTipBeacon), new PropertyMetadata());

		public static double GetShowDelay(DependencyObject obj)
		{
			return (double)obj.GetValue(ShowDelayProperty);
		}
		public static void SetShowDelay(DependencyObject obj, int value)
		{
			obj.SetValue(ShowDelayProperty, value);
		}
		public static readonly DependencyProperty ShowDelayProperty =
			DependencyProperty.RegisterAttached("ShowDelay", typeof(double), typeof(ToolTipBeacon), new PropertyMetadata(500.0));

		public static double GetGetHideDelay(DependencyObject obj)
		{
			return (double)obj.GetValue(GetHideDelayProperty);
		}
		public static void SetGetHideDelay(DependencyObject obj, double value)
		{
			obj.SetValue(GetHideDelayProperty, value);
		}
		public static readonly DependencyProperty GetHideDelayProperty =
			DependencyProperty.RegisterAttached("GetHideDelay", typeof(double), typeof(ToolTipBeacon), new PropertyMetadata(5000.0));

		public static ToolTipBeacon GetAttachedBeacon(DependencyObject obj)
		{
			return (ToolTipBeacon)obj.GetValue(AttachedBeaconProperty);
		}
		public static void SetAttachedBeacon(DependencyObject obj, ToolTipBeacon value)
		{
			obj.SetValue(AttachedBeaconProperty, value);
		}
		public static readonly DependencyProperty AttachedBeaconProperty =
			DependencyProperty.RegisterAttached("AttachedBeacon", typeof(ToolTipBeacon), typeof(ToolTipBeacon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static ToolTipHideMode GetHideMode(DependencyObject obj)
		{
			return (ToolTipHideMode)obj.GetValue(HideModeProperty);
		}
		public static void SetHideMode(DependencyObject obj, ToolTipHideMode value)
		{
			obj.SetValue(HideModeProperty, value);
		}
		public static readonly DependencyProperty HideModeProperty =
			DependencyProperty.RegisterAttached("HideMode", typeof(ToolTipHideMode), typeof(ToolTipBeacon), new FrameworkPropertyMetadata(ToolTipHideMode.Timer, FrameworkPropertyMetadataOptions.Inherits));
		#endregion

		#region DPs
		public bool TargetHasMouseOver
		{
			get { return (bool)GetValue(TargetHasMouseOverProperty); }
			set { SetValue(TargetHasMouseOverProperty, value); }
		}
		public static readonly DependencyProperty TargetHasMouseOverProperty =
			DependencyProperty.Register("TargetHasMouseOver", typeof(bool), typeof(ToolTipBeacon), new PropertyMetadata());

		public FrameworkElement Target
		{
			get { return (FrameworkElement)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(ToolTipBeacon), new PropertyMetadata());

		public Rect TargetRect
		{
			get { return (Rect)GetValue(TargetRectProperty); }
			set { SetValue(TargetRectProperty, value); }
		}
		public static readonly DependencyProperty TargetRectProperty =
			DependencyProperty.Register("TargetRect", typeof(Rect), typeof(ToolTipBeacon), new PropertyMetadata());
		#endregion

		private DispatcherTimer showTimer;
		private DispatcherTimer hideTimer;
		private DispatcherTimer stayOpenTimer;
		private OverlayContentControl tooltipPresenter;
		private OverlayHost overlayHost;

		public bool ToolTipHasMouseOver
		{
			get { return (bool)GetValue(ToolTipHasMouseOverProperty); }
			set { SetValue(ToolTipHasMouseOverProperty, value); }
		}
		public static readonly DependencyProperty ToolTipHasMouseOverProperty =
			DependencyProperty.Register("ToolTipHasMouseOver", typeof(bool), typeof(ToolTipBeacon), new PropertyMetadata());

		public DelegateCommand ToolTipCloseCommand
		{
			get { return (DelegateCommand)GetValue(ToolTipCloseCommandProperty); }
			private set { SetValue(ToolTipCloseCommandProperty, value); }
		}
		public static readonly DependencyProperty ToolTipCloseCommandProperty =
			DependencyProperty.Register("ToolTipCloseCommand", typeof(DelegateCommand), typeof(ToolTipBeacon), new PropertyMetadata());

		static ToolTipBeacon()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolTipBeacon), new FrameworkPropertyMetadata(typeof(ToolTipBeacon)));
		}

		public ToolTipBeacon()
		{
			Opacity = 1.0;
			Visibility = Visibility.Visible;
			IsHitTestVisible = false;
			showTimer = new DispatcherTimer();
			hideTimer = new DispatcherTimer();
			stayOpenTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(300) };
			Loaded += ToolTipBeacon_Loaded;
			Unloaded += ToolTipBeacon_Unloaded;
			ToolTipCloseCommand = new DelegateCommand(ToolTipCloseCommand_Execute);
		}

		private void ToolTipCloseCommand_Execute(object obj)
		{
			HideToolTip();
		}

		private Rect GetTargetRect()
		{
			OverlayHost host = OverlayHost.GetOverlay();
			if (host == null || Target == null)
				return default(Rect);
			Point leftTopCornerPoint = Target.TranslatePoint(new Point(0, 0), host);
			Rect result = new Rect(leftTopCornerPoint, new Size(Target.ActualWidth, Target.ActualHeight));
			return result;
		}

		private void ToolTipBeacon_Loaded(object sender, RoutedEventArgs e)
		{
			showTimer.Tick += ShowTimer_Tick;
			hideTimer.Tick += HideTimer_Tick;
			stayOpenTimer.Tick += StayOpenTimer_Tick;
			if (TemplatedParent as FrameworkElement != null && Target == null)
				Target = TemplatedParent as FrameworkElement;
			overlayHost = OverlayHost.GetOverlay();
		}

		private void ToolTipBeacon_Unloaded(object sender, RoutedEventArgs e)
		{
			showTimer.Tick -= ShowTimer_Tick;
			hideTimer.Tick -= HideTimer_Tick;
			stayOpenTimer.Tick += StayOpenTimer_Tick;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TargetHasMouseOverProperty)
			{
				if ((bool)e.NewValue)
				{
					showTimer.Interval = TimeSpan.FromMilliseconds(GetShowDelay(Target));
					showTimer.Start();
				}
				else if(GetHideMode(Target) == ToolTipHideMode.Timer)
				{
					showTimer.Stop();
					stayOpenTimer.Start();
				}
			}
			if (e.Property == ToolTipHasMouseOverProperty)
			{
				if ((bool)e.NewValue && !TargetHasMouseOver)
				{
					hideTimer.Stop();
					stayOpenTimer.Stop();
				}
				else if (!(bool)e.NewValue && !TargetHasMouseOver)
				{
					stayOpenTimer.Start();
				}
			}
			if (e.Property == TargetProperty && e.NewValue != null)
			{
				SetBinding(TargetHasMouseOverProperty, new Binding { Source = Target, Path = new PropertyPath(IsMouseOverProperty), Mode = BindingMode.OneWay });
			}
		}

		#region Timers tick
		private void ShowTimer_Tick(object sender, EventArgs e)
		{
			showTimer.Stop();
			if (TargetHasMouseOver)
				ShowToolTip();
		}

		private void StayOpenTimer_Tick(object sender, EventArgs e)
		{
			stayOpenTimer.Stop();
			if (!TargetHasMouseOver && !ToolTipHasMouseOver)
				HideToolTip();
		}

		private void HideTimer_Tick(object sender, EventArgs e)
		{
			hideTimer.Stop();
			if (ToolTipHasMouseOver)
				return;
			HideToolTip();
		}
		#endregion

		private void ShowToolTip()
		{
			string groupName = GetInheritedGroupName(this);
			if (!String.IsNullOrWhiteSpace(groupName) && !IsToolTipsGroupEnabled(groupName))
			{
				return;
			}
			overlayHost = OverlayHost.GetOverlay();
			if (overlayHost != null)
			{
				overlayHost.OwnerWindow.Deactivated += OwnerWindow_Deactivated;
				tooltipPresenter = overlayHost.ShowContent(GetToolTipContent(Target), GetTargetRect(), GetToolTipTemplateKey(Target));
				tooltipPresenter.PlacementPriority = GetPlacementPriority(Target);
				SetAttachedBeacon(tooltipPresenter, this);
				SetBinding(ToolTipHasMouseOverProperty, new Binding { Source = tooltipPresenter, Path = new PropertyPath(IsMouseOverProperty), Mode = BindingMode.OneWay });
				tooltipPresenter.MouseLeftButtonDown += OverlayControl_MouseLeftButtonDown;
				hideTimer.Interval = TimeSpan.FromMilliseconds(GetGetHideDelay(Target));
				hideTimer.Start();
			}
		}

		private void OwnerWindow_Deactivated(object sender, EventArgs e)
		{
			HideToolTip();
		}

		private void OverlayControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			HideToolTip();
		}

		private void HideToolTip()
		{
			StopTimers();
			if (tooltipPresenter == null)
				return;
			OverlayHost overlay = OverlayHost.GetOverlay();
			if (overlay != null)
			{
				overlay.HideContent(GetToolTipContent(Target));
				overlayHost.OwnerWindow.Deactivated += OwnerWindow_Deactivated;
			}
			tooltipPresenter.MouseLeftButtonDown -= OverlayControl_MouseLeftButtonDown;
			tooltipPresenter = null;
		}

		private void StopTimers()
		{
			showTimer.Stop();
			hideTimer.Stop();
			stayOpenTimer.Stop();
		}
	}

	public class ToolTipTimerShowTrigger : ToolTipShowTrigger
	{

	}

	public class ToolTipShowTrigger : ToolTipTriggerBase
	{

	}

	public class ToolTipHideTrigger : ToolTipTriggerBase
	{

	}

	public class ToolTipTriggerBase : FrameworkElement
	{

		public static ToolTipHideTrigger GetAttachedHideTrigger(DependencyObject obj)
		{
			return (ToolTipHideTrigger)obj.GetValue(AttachedHideTriggerProperty);
		}
		public static void SetAttachedHideTrigger(DependencyObject obj, ToolTipHideTrigger value)
		{
			obj.SetValue(AttachedHideTriggerProperty, value);
		}
		public static readonly DependencyProperty AttachedHideTriggerProperty =
			DependencyProperty.RegisterAttached("AttachedHideTrigger", typeof(ToolTipHideTrigger), typeof(ToolTipTriggerBase), new PropertyMetadata());

		public static ToolTipShowTrigger GetAttachedShowTrigger(DependencyObject obj)
		{
			return (ToolTipShowTrigger)obj.GetValue(AttachedShowTriggerProperty);
		}
		public static void SetAttachedShowTrigger(DependencyObject obj, ToolTipShowTrigger value)
		{
			obj.SetValue(AttachedShowTriggerProperty, value);
		}
		public static readonly DependencyProperty AttachedShowTriggerProperty =
			DependencyProperty.RegisterAttached("AttachedShowTrigger", typeof(ToolTipShowTrigger), typeof(ToolTipTriggerBase), new PropertyMetadata());

		public UIElement Target
		{
			get { return (UIElement)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(UIElement), typeof(ToolTipShowTrigger), new PropertyMetadata());

		public Action Action { get; set; }

	}
}
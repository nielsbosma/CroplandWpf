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
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public class DateInervalControl : Control
	{
		#region DPs
		public DateTime DateTime1
		{
			get { return (DateTime)GetValue(DateTime1Property); }
			set { SetValue(DateTime1Property, value); }
		}
		public static readonly DependencyProperty DateTime1Property =
			DependencyProperty.Register("DateTime1", typeof(DateTime), typeof(DateInervalControl), new PropertyMetadata(default(DateTime), null, CoerceDateTime1));

		public DateTime DateTime2
		{
			get { return (DateTime)GetValue(DateTime2Property); }
			set { SetValue(DateTime2Property, value); }
		}
		public static readonly DependencyProperty DateTime2Property =
			DependencyProperty.Register("DateTime2", typeof(DateTime), typeof(DateInervalControl), new PropertyMetadata(default(DateTime), null, CoerceDateTime2));

		public DateInterval SelectedInterval
		{
			get { return (DateInterval)GetValue(SelectedIntervalProperty); }
			set { SetValue(SelectedIntervalProperty, value); }
		}
		public static readonly DependencyProperty SelectedIntervalProperty =
			DependencyProperty.Register("SelectedInterval", typeof(DateInterval), typeof(DateInervalControl), new PropertyMetadata());

		public DelegateCommand ApplyPresetCommand
		{
			get { return (DelegateCommand)GetValue(ApplyPresetCommandProperty); }
			private set { SetValue(ApplyPresetCommandProperty, value); }
		}
		public static readonly DependencyProperty ApplyPresetCommandProperty =
			DependencyProperty.Register("ApplyPresetCommand", typeof(DelegateCommand), typeof(DateInervalControl), new PropertyMetadata());

		public bool IsPopupVisible
		{
			get { return (bool)GetValue(IsPopupVisibleProperty); }
			set { SetValue(IsPopupVisibleProperty, value); }
		}
		public static readonly DependencyProperty IsPopupVisibleProperty =
			DependencyProperty.Register("IsPopupVisible", typeof(bool), typeof(DateInervalControl), new PropertyMetadata());

		public string DateStringFormat
		{
			get { return (string)GetValue(DateStringFormatProperty); }
			set { SetValue(DateStringFormatProperty, value); }
		}
		public static readonly DependencyProperty DateStringFormatProperty =
			DependencyProperty.Register("DateStringFormat", typeof(string), typeof(DateInervalControl), new PropertyMetadata("dd MMMM yyyy"));

		public string DisplayString
		{
			get { return (string)GetValue(DisplayStringProperty); }
			set { SetValue(DisplayStringProperty, value); }
		}
		public static readonly DependencyProperty DisplayStringProperty =
			DependencyProperty.Register("DisplayString", typeof(string), typeof(DateInervalControl), new PropertyMetadata());

		public List<DateIntervalPreset> Presets
		{
			get { return (List<DateIntervalPreset>)GetValue(PresetsProperty); }
			set { SetValue(PresetsProperty, value); }
		}
		public static readonly DependencyProperty PresetsProperty =
			DependencyProperty.Register("Presets", typeof(List<DateIntervalPreset>), typeof(DateInervalControl), new PropertyMetadata());

		public DateInterval FormerInterval
		{
			get { return (DateInterval)GetValue(FormerIntervalProperty); }
			set { SetValue(FormerIntervalProperty, value); }
		}
		public static readonly DependencyProperty FormerIntervalProperty =
			DependencyProperty.Register("FormerInterval", typeof(DateInterval), typeof(DateInervalControl), new PropertyMetadata());

		public DelegateCommand AbortCommand
		{
			get { return (DelegateCommand)GetValue(AbortCommandProperty); }
			private set { SetValue(AbortCommandProperty, value); }
		}
		public static readonly DependencyProperty AbortCommandProperty =
			DependencyProperty.Register("AbortCommand", typeof(DelegateCommand), typeof(DateInervalControl), new PropertyMetadata());

		public DelegateCommand UpdateCommand
		{
			get { return (DelegateCommand)GetValue(UpdateCommandProperty); }
			private set { SetValue(UpdateCommandProperty, value); }
		}
		public static readonly DependencyProperty UpdateCommandProperty =
			DependencyProperty.Register("UpdateCommand", typeof(DelegateCommand), typeof(DateInervalControl), new PropertyMetadata());

		public string InitialIntervalString
		{
			get { return (string)GetValue(InitialIntervalStringProperty); }
			private set { SetValue(InitialIntervalStringProperty, value); }
		}
		public static readonly DependencyProperty InitialIntervalStringProperty =
			DependencyProperty.Register("InitialIntervalString", typeof(string), typeof(DateInervalControl), new PropertyMetadata());

		public DateIntervalPreset SelectedIntervalPreset
		{
			get { return (DateIntervalPreset)GetValue(SelectedIntervalPresetProperty); }
			private set { SetValue(SelectedIntervalPresetProperty, value); }
		}
		public static readonly DependencyProperty SelectedIntervalPresetProperty =
			DependencyProperty.Register("SelectedIntervalPreset", typeof(DateIntervalPreset), typeof(DateInervalControl), new PropertyMetadata());
		#endregion

		#region Coercing
		private static object CoerceDateTime1(DependencyObject o, object value)
		{
			if (value is DateTime dt1 && o is DateInervalControl dic)
			{
				dt1 = DateTimeHelper.NormalizeIntervalStart(dt1);
				if (dic.DateTime2 < dt1)
					dic.DateTime2 = dt1.AddDays(1.0);
				return dt1;
			}
			return value;
		}

		private static object CoerceDateTime2(DependencyObject o, object value)
		{
			if (value is DateTime dt2 && o is DateInervalControl dic)
			{
				dt2 = DateTimeHelper.NormalizeIntervalEnd(dt2);
				if (dic.DateTime1 > dt2)
					dic.DateTime1 = dt2.AddDays(-1.0);
				return dt2;
			}
			return value;
		}
		#endregion

		#region Properties
		private DateInterval editingInterval
		{
			get
			{
				return _editingInterval;
			}
			set
			{
				_editingInterval = value;
				DisplayString = GetIntervalDisplayString(value);
			}
		}
		private DateInterval _editingInterval;
		#endregion

		#region Fields
		private Popup popup;
		#endregion

		static DateInervalControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(DateInervalControl), new FrameworkPropertyMetadata(typeof(DateInervalControl)));
		}

		public DateInervalControl()
		{
			Loaded += DateInervalControl_Loaded;
			Unloaded += DateInervalControl_Unloaded;
			ApplyPresetCommand = new DelegateCommand(ApplyPresetCommand_Execute);
			Presets = new List<DateIntervalPreset>
			{
				new DateIntervalPreset(DateIntervalType.Today, "Today"),
				new DateIntervalPreset(DateIntervalType.Yesterday, "Yesterday"),
				new DateIntervalPreset(DateIntervalType.Last7Days, "Last 7 Days"),
				new DateIntervalPreset(DateIntervalType.Last14Days, "Last 14 Days"),
				new DateIntervalPreset(DateIntervalType.Last30Days, "Last 30 Days"),
				new DateIntervalPreset(DateIntervalType.ThisWeek, "This Week"),
				new DateIntervalPreset(DateIntervalType.LastWeek, "Last Week"),
				new DateIntervalPreset(DateIntervalType.ThisMonth, "This Month"),
				new DateIntervalPreset(DateIntervalType.LastMonth, "Last Month")
			};
			UpdateCommand = new DelegateCommand(UpdateCommand_Execute);
			AbortCommand = new DelegateCommand(AbortCommand_Execute);

			#region Dates test
			//DateTime dt1 = default(DateTime), dt2 = default(DateTime);
			////Today
			//DateTimeHelper.GetInterval(DateIntervalType.Today, out dt1, out dt2);

			////Yesteday
			//DateTimeHelper.GetInterval(DateIntervalType.Yesterday, out dt1, out dt2);

			////Last seven days
			//DateTimeHelper.GetInterval(DateIntervalType.Last7Days, out dt1, out dt2);

			////Last 14 days
			//DateTimeHelper.GetInterval(DateIntervalType.Last14Days, out dt1, out dt2);

			////Last 30 days
			//DateTimeHelper.GetInterval(DateIntervalType.Last30Days, out dt1, out dt2);

			////This week
			//DateTimeHelper.GetInterval(DateIntervalType.LastWeek, out dt1, out dt2);

			////This month
			//DateTimeHelper.GetInterval(DateIntervalType.ThisMonth, out dt1, out dt2);

			////Last month
			//DateTimeHelper.GetInterval(DateIntervalType.LastMonth, out dt1, out dt2); 
			#endregion
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			popup = Template.FindName("PART_Popup", this) as Popup;
			if (popup == null)
				throw new TemplatePartNotFoundException("PART_Popup", GetType());
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DateTime1Property || e.Property == DateTime2Property)
			{
				editingInterval = new DateInterval(DateTime1, DateTime2);
				RefreshSelectedInterval();
			}
			if (e.Property == IsPopupVisibleProperty)
				popup.IsOpen = (bool)e.NewValue;
			if (e.Property == SelectedIntervalProperty)
			{
				editingInterval = SelectedInterval;
				DateTime1 = SelectedInterval.Date1;
				DateTime2 = SelectedInterval.Date2;
				InitialIntervalString = GetIntervalDisplayString(SelectedInterval);
			}
		}

		private void RefreshSelectedInterval()
		{
			SelectedIntervalPreset = Presets.FirstOrDefault(p => p.Equals(DateTime1, DateTime2));
		}

		private void ApplyPresetCommand_Execute(object obj)
		{
			if (obj is DateIntervalPreset preset)
			{
				preset.GetValues(out DateTime dt1, out DateTime dt2);
				DateTime1 = dt1;
				DateTime2 = dt2;
			}
		}

		private void DateInervalControl_Loaded(object sender, RoutedEventArgs e)
		{
			DateTime1 = DateTime.Now.Date;
			DateTime2 = DateTime1.AddDays(1.0).AddSeconds(-1.0);
			WindowHelper.RegisterHandler(this, PreviewMouseLeftButtonDownEvent, Window_MouseDown);
			WindowHelper.GetWindow(this).Deactivated += DateInervalControl_Deactivated;
		}

		private void DateInervalControl_Unloaded(object sender, RoutedEventArgs e)
		{
			WindowHelper.UnregisterHandler(this, PreviewMouseLeftButtonDownEvent);
			WindowHelper.GetWindow(this).Deactivated -= DateInervalControl_Deactivated;
		}

		private void DateInervalControl_Deactivated(object sender, EventArgs e)
		{
			IsPopupVisible = false;
		}

		private void Window_MouseDown(object sender, RoutedEventArgs e)
		{
			if (popup == null)
				return;
			MouseButtonEventArgs args = e as MouseButtonEventArgs;
			if (args != null)
			{
				if (!IsMouseOver && !popup.IsMouseOver && popup.IsOpen)
					IsPopupVisible = false;
			}
		}

		private string GetIntervalDisplayString(DateInterval interval)
		{
			return interval.GetFormattedValue(DateStringFormat);
		}

		private void AbortCommand_Execute(object obj)
		{
			Reset();
		}

		private void UpdateCommand_Execute(object obj)
		{
			SelectedInterval = editingInterval;
			Reset();
		}

		private void Reset()
		{
			IsPopupVisible = false;
			DateTime1 = SelectedInterval.Date1;
			DateTime2 = SelectedInterval.Date2;
		}
	}
}

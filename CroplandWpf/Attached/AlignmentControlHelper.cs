using CroplandWpf.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CroplandWpf.Attached
{
	public class AlignmentControlHelper : FrameworkElement
	{
		public static ICommand GetCheckedCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(CheckedCommandProperty);
		}
		public static void SetCheckedCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(CheckedCommandProperty, value);
		}
		public static readonly DependencyProperty CheckedCommandProperty =
			DependencyProperty.RegisterAttached("CheckedCommand", typeof(ICommand), typeof(AlignmentControlHelper), new PropertyMetadata());

		public static object GetCheckedCommandParameter(DependencyObject obj)
		{
			return (object)obj.GetValue(CheckedCommandParameterProperty);
		}
		public static void SetCheckedCommandParameter(DependencyObject obj, object value)
		{
			obj.SetValue(CheckedCommandParameterProperty, value);
		}
		public static readonly DependencyProperty CheckedCommandParameterProperty =
			DependencyProperty.RegisterAttached("CheckedCommandParameter", typeof(object), typeof(AlignmentControlHelper), new PropertyMetadata());

		public static List<RadioButton> GetLocalTargets(DependencyObject obj)
		{
			return (List<RadioButton>)obj.GetValue(LocalTargetsProperty);
		}
		public static void SetLocalTargets(DependencyObject obj, List<RadioButton> value)
		{
			obj.SetValue(LocalTargetsProperty, value);
		}
		public static readonly DependencyProperty LocalTargetsProperty =
			DependencyProperty.RegisterAttached("LocalTargets", typeof(List<RadioButton>), typeof(AlignmentControlHelper), new PropertyMetadata());

		private static void InitLocalTargets(DependencyObject o)
		{
			if (GetLocalTargets(o) == null)
				SetLocalTargets(o, new List<RadioButton>());
		}

		private static void AddLocalTarget(DependencyObject o, RadioButton localTarget)
		{
			InitLocalTargets(o);
			if (!GetLocalTargets(o).Contains(localTarget))
				GetLocalTargets(o).Add(localTarget);
		}

		private static void RemoveLocalTarget(DependencyObject o, RadioButton localTarget)
		{
			InitLocalTargets(o);
			if (GetLocalTargets(o).Contains(localTarget))
				GetLocalTargets(o).Remove(localTarget);
		}

		public bool IsTargetChecked
		{
			get { return (bool)GetValue(IsTargetCheckedProperty); }
			set { SetValue(IsTargetCheckedProperty, value); }
		}
		public static readonly DependencyProperty IsTargetCheckedProperty =
			DependencyProperty.Register("IsTargetChecked", typeof(bool), typeof(AlignmentControlHelper), new PropertyMetadata());

		public RadioButton Target
		{
			get { return (RadioButton)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(RadioButton), typeof(AlignmentControlHelper), new PropertyMetadata());

		public DependencyObject LocalTargetsHost
		{
			get { return (DependencyObject)GetValue(LocalTargetsHostProperty); }
			set { SetValue(LocalTargetsHostProperty, value); }
		}
		public static readonly DependencyProperty LocalTargetsHostProperty =
			DependencyProperty.Register("LocalTargetsHost", typeof(DependencyObject), typeof(AlignmentControlHelper), new PropertyMetadata());

		private bool? isTargetChecked;

		public AlignmentControlHelper()
		{
			Loaded += AlignmentControlHelper_Loaded;
			Unloaded += AlignmentControlHelper_Unloaded;
		}

		private void AlignmentControlHelper_Loaded(object sender, RoutedEventArgs e)
		{
			if (Target != null)
			{
				Target.PreviewMouseLeftButtonDown += Target_PreviewMouseLeftButtonDown;
				Target.Click += Target_Click;
				if (Target != null && LocalTargetsHost != null)
				{
					AddLocalTarget(LocalTargetsHost, Target);
					if (GetLocalTargets(LocalTargetsHost).Count == 9 && LocalTargetsHost as AlignmentEditor != null)
						(LocalTargetsHost as AlignmentEditor).CheckTargetForCurrentAlignmentValues();
				}
			}
		}

		private void Target_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			isTargetChecked = Target.IsChecked;
		}

		private void Target_Click(object sender, RoutedEventArgs e)
		{
			if (Target.IsChecked.HasValue && Target.IsChecked.Value && isTargetChecked != Target.IsChecked)
				if (GetCheckedCommand(Target) != null)
					GetCheckedCommand(Target).Execute(GetCheckedCommandParameter(Target));
		}

		private void AlignmentControlHelper_Unloaded(object sender, RoutedEventArgs e)
		{
			if (Target != null)
			{
				Target.PreviewMouseLeftButtonDown -= Target_PreviewMouseLeftButtonDown;
				Target.Click -= Target_Click;
				RemoveLocalTarget(LocalTargetsHost, Target);
			}
		}
	}
}
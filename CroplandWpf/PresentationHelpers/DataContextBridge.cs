using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CroplandWpf.PresentationHelpers
{
	public class DataContextBridge : FrameworkElement
	{
		private static List<DataContextBridge> registeredBridges = new List<DataContextBridge>();
		private static List<FrameworkElement> registeredTargets = new List<FrameworkElement>();

		public FrameworkElement Source
		{
			get { return (FrameworkElement)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register("Source", typeof(FrameworkElement), typeof(DataContextBridge), new PropertyMetadata());

		public FrameworkElement Target
		{
			get { return (FrameworkElement)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(DataContextBridge), new PropertyMetadata());

		public string ScopeName
		{
			get { return (string)GetValue(ScopeNameProperty); }
			set { SetValue(ScopeNameProperty, value); }
		}
		public static readonly DependencyProperty ScopeNameProperty =
			DependencyProperty.Register("ScopeName", typeof(string), typeof(DataContextBridge), new PropertyMetadata());

		public static string GetSourceScopeName(DependencyObject obj)
		{
			return (string)obj.GetValue(SourceScopeNameProperty);
		}
		public static void SetSourceScopeName(DependencyObject obj, string value)
		{
			obj.SetValue(SourceScopeNameProperty, value);
		}
		public static readonly DependencyProperty SourceScopeNameProperty =
			DependencyProperty.RegisterAttached("SourceScopeName", typeof(string), typeof(DataContextBridge), new PropertyMetadata(SourceScopeNameChanged));

		private static void SourceScopeNameChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement target = sender as FrameworkElement;
			string name = e.NewValue as string;
			if (target != null && !String.IsNullOrWhiteSpace(name))
			{
				DataContextBridge bridge = registeredBridges.FirstOrDefault(b => b.ScopeName == name);
				if (bridge != null)
					target.SetBinding(DataContextProperty, new Binding { Source = bridge, Path = new PropertyPath(DataContextProperty), Mode = BindingMode.OneWay });
				else
					registeredTargets.Add(target);
			}
		}

		public DataContextBridge()
		{
			registeredBridges.Add(this);
			Unloaded += DataContextBridge_Unloaded;
		}

		private void DataContextBridge_Loaded(object sender, RoutedEventArgs e)
		{
			Register(this);
		}

		private void DataContextBridge_Unloaded(object sender, RoutedEventArgs e)
		{
			Unregister(this);
		}

		private static void Register(DataContextBridge bridge)
		{
			if (!registeredBridges.Contains(bridge))
				registeredBridges.Add(bridge);
		}

		private static void Unregister(DataContextBridge bridge)
		{
			if (registeredBridges.Contains(bridge))
				registeredBridges.Remove(bridge);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DataContextProperty && ScopeName != null && e.NewValue != null || e.Property == ScopeNameProperty && DataContext != null && e.NewValue != null)
			{
				List<FrameworkElement> targets = registeredTargets.Where(rt => GetSourceScopeName(rt) == ScopeName).ToList();
				targets.ForEach(rt =>
				{
					rt.SetBinding(DataContextProperty, new Binding { Source = this, Path = new PropertyPath(DataContextProperty), Mode = BindingMode.OneWay }); registeredTargets.Remove(rt);
				});
			}
		}
	}
}
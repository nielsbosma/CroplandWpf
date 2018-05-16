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

			}
		}

		public DataContextBridge()
		{
			Loaded += DataContextBridge_Loaded;
			Unloaded += DataContextBridge_Unloaded;
		}

		private void DataContextBridge_Loaded(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(ScopeName))
				Register(this);
		}

		private void DataContextBridge_Unloaded(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(ScopeName))
				Unregister(this);
		}

		private static void Register(DataContextBridge bridge)
		{

		}

		private static void Unregister(DataContextBridge bridge)
		{

		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == SourceProperty || e.Property == TargetProperty)
			{
				if (Source != null && Target != null)
					Target.SetBinding(DataContextProperty, new Binding { Source = Source, Path = new PropertyPath(DataContextProperty) });
			}
		}


	}
}

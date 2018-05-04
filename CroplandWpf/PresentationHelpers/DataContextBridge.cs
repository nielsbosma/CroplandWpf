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

		public DataContextBridge()
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

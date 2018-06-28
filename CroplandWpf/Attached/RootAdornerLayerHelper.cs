using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace CroplandWpf.Attached
{
	public class RootAdornerLayerHelper : FrameworkElement
	{
		public Window Window
		{
			get { return (Window)GetValue(WindowProperty); }
			set { SetValue(WindowProperty, value); }
		}
		public static readonly DependencyProperty WindowProperty =
			DependencyProperty.Register("Window", typeof(Window), typeof(RootAdornerLayerHelper), new PropertyMetadata());

		public AdornerLayer Layer
		{
			get { return (AdornerLayer)GetValue(LayerProperty); }
			set { SetValue(LayerProperty, value); }
		}
		public static readonly DependencyProperty LayerProperty =
			DependencyProperty.Register("Layer", typeof(AdornerLayer), typeof(RootAdornerLayerHelper), new PropertyMetadata());

		private static List<RootAdornerLayerHelper> registeredHelpers = new List<RootAdornerLayerHelper>();

		public static AdornerLayer GetRootLayer(DependencyObject element)
		{
			Window window = Window.GetWindow(element);
			RootAdornerLayerHelper rh = registeredHelpers.FirstOrDefault(h => h.Window == window);
			return rh?.Layer;
		}

		public RootAdornerLayerHelper()
		{
			Unloaded += RootAdornerLayerHelper_Unloaded;
		}

		private void RootAdornerLayerHelper_Unloaded(object sender, RoutedEventArgs e)
		{
			if (registeredHelpers.Contains(this))
				registeredHelpers.Remove(this);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == WindowProperty)
			{
				if (Window != null && !registeredHelpers.Contains(this))
				{
					Layer = AdornerLayer.GetAdornerLayer(this);
					registeredHelpers.Add(this);
				}
			}
		}
	}
}
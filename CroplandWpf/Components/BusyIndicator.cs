using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Components
{
	public class BusyIndicator : Control
	{
		static BusyIndicator()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
		}

		public BusyIndicator()
		{
			IsEnabled = false;
			IsHitTestVisible = false;
		}
	}
}

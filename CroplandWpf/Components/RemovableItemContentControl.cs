using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Components
{
	public class RemovableItemContentControl : ContentControl
	{
		static RemovableItemContentControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RemovableItemContentControl), new FrameworkPropertyMetadata(typeof(RemovableItemContentControl)));
		}

		public RemovableItemContentControl()
		{

		}
	}
}
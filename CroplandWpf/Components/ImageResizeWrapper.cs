using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public class ImageResizeWrapper : Control
	{
		public ICommand ThumbDragCommand
		{
			get { return (ICommand)GetValue(ThumbDragCommandProperty); }
			set { SetValue(ThumbDragCommandProperty, value); }
		}
		public static readonly DependencyProperty ThumbDragCommandProperty =
			DependencyProperty.Register("ThumbDragCommand", typeof(ICommand), typeof(ImageResizeWrapper), new PropertyMetadata());

		public double ScaleFactor
		{
			get { return (double)GetValue(ScaleFactorProperty); }
			set { SetValue(ScaleFactorProperty, value); }
		}
		public static readonly DependencyProperty ScaleFactorProperty =
			DependencyProperty.Register("ScaleFactor", typeof(double), typeof(ImageResizeWrapper), new PropertyMetadata(1.0));

		static ImageResizeWrapper()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageResizeWrapper), new FrameworkPropertyMetadata(typeof(ImageResizeWrapper)));
		}

		public ImageResizeWrapper()
		{

		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public class ImageCropResizeThumb : Thumb
	{
		public ResizeThumbRole Role
		{
			get { return (ResizeThumbRole)GetValue(RoleProperty); }
			set { SetValue(RoleProperty, value); }
		}
		public static readonly DependencyProperty RoleProperty =
			DependencyProperty.Register("Role", typeof(ResizeThumbRole), typeof(ImageCropResizeThumb), new PropertyMetadata());

		public ICommand DragCommand
		{
			get { return (ICommand)GetValue(DragCommandProperty); }
			set { SetValue(DragCommandProperty, value); }
		}
		public static readonly DependencyProperty DragCommandProperty =
			DependencyProperty.Register("DragCommand", typeof(ICommand), typeof(ImageCropResizeThumb), new PropertyMetadata());

		static ImageCropResizeThumb()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageCropResizeThumb), new FrameworkPropertyMetadata(typeof(ImageCropResizeThumb)));
		}

		public ImageCropResizeThumb()
		{
			DragDelta += ImageResizeThumb_DragDelta;
		}

		private void ImageResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			if(DragCommand!= null)
			{
				DragCommand.Execute(new ImageResizeThumbDragDelta { Role = Role, hChange = e.HorizontalChange, vChange = e.VerticalChange });
			}
		}
	}

	public struct ImageResizeThumbDragDelta
	{
		public ResizeThumbRole Role;
		public double hChange;
		public double vChange;
	}
}
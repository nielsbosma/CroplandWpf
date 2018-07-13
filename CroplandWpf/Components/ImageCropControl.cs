using CroplandWpf.Exceptions;
using CroplandWpf.MVVM;
using CroplandWpf.PresentationHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CroplandWpf.Components
{
	public class ImageCropControl : Control
	{
		#region DPs
		public FileInfo SourceFileInfo
		{
			get { return (FileInfo)GetValue(SourceFileInfoProperty); }
			set { SetValue(SourceFileInfoProperty, value); }
		}
		public static readonly DependencyProperty SourceFileInfoProperty =
			DependencyProperty.Register("SourceFileInfo", typeof(FileInfo), typeof(ImageCropControl), new PropertyMetadata());

		public BitmapSource ImageSource
		{
			get { return (BitmapSource)GetValue(ImageSourceProperty); }
			set { SetValue(ImageSourceProperty, value); }
		}
		public static readonly DependencyProperty ImageSourceProperty =
			DependencyProperty.Register("ImageSource", typeof(BitmapSource), typeof(ImageCropControl), new PropertyMetadata());

		public BitmapSource DisplayImageSource
		{
			get { return (BitmapSource)GetValue(DisplayImageSourceProperty); }
			private set { SetValue(DisplayImageSourceProperty, value); }
		}
		public static readonly DependencyProperty DisplayImageSourceProperty =
			DependencyProperty.Register("DisplayImageSource", typeof(BitmapSource), typeof(ImageCropControl), new PropertyMetadata());

		public Stretch ImageStretch
		{
			get { return (Stretch)GetValue(ImageStretchProperty); }
			set { SetValue(ImageStretchProperty, value); }
		}
		public static readonly DependencyProperty ImageStretchProperty =
			DependencyProperty.Register("ImageStretch", typeof(Stretch), typeof(ImageCropControl), new PropertyMetadata(Stretch.None));

		public DelegateCommand DragActionCommand
		{
			get { return (DelegateCommand)GetValue(DragActionCommandProperty); }
			private set { SetValue(DragActionCommandProperty, value); }
		}
		public static readonly DependencyProperty DragActionCommandProperty =
			DependencyProperty.Register("DragActionCommand", typeof(DelegateCommand), typeof(ImageCropControl), new PropertyMetadata());

		public double AverageBrightness
		{
			get { return (double)GetValue(AverageBrightnessProperty); }
			private set { SetValue(AverageBrightnessProperty, value); }
		}
		public static readonly DependencyProperty AverageBrightnessProperty =
			DependencyProperty.Register("AverageBrightness", typeof(double), typeof(ImageCropControl), new PropertyMetadata());

		public Brush CropOverlayBrush
		{
			get { return (Brush)GetValue(CropOverlayBrushProperty); }
			private set { SetValue(CropOverlayBrushProperty, value); }
		}
		public static readonly DependencyProperty CropOverlayBrushProperty =
			DependencyProperty.Register("CropOverlayBrush", typeof(Brush), typeof(ImageCropControl), new PropertyMetadata());

		public double OverlayWidth
		{
			get { return (double)GetValue(OverlayWidthProperty); }
			private set { SetValue(OverlayWidthProperty, value); }
		}
		public static readonly DependencyProperty OverlayWidthProperty =
			DependencyProperty.Register("OverlayWidth", typeof(double), typeof(ImageCropControl), new PropertyMetadata());

		public double OverlayHeight
		{
			get { return (double)GetValue(OverlayHeightProperty); }
			private set { SetValue(OverlayHeightProperty, value); }
		}
		public static readonly DependencyProperty OverlayHeightProperty =
			DependencyProperty.Register("OverlayHeight", typeof(double), typeof(ImageCropControl), new PropertyMetadata());

		public Point DragStartPoint
		{
			get { return (Point)GetValue(DragStartPointProperty); }
			private set { SetValue(DragStartPointProperty, value); }
		}
		public static readonly DependencyProperty DragStartPointProperty =
			DependencyProperty.Register("DragStartPoint", typeof(Point), typeof(ImageCropControl), new PropertyMetadata());

		public Point CurrentDragPoint
		{
			get { return (Point)GetValue(CurrentDragPointProperty); }
			private set { SetValue(CurrentDragPointProperty, value); }
		}
		public static readonly DependencyProperty CurrentDragPointProperty =
			DependencyProperty.Register("CurrentDragPoint", typeof(Point), typeof(ImageCropControl), new PropertyMetadata());

		public Point DragCompletedPoint
		{
			get { return (Point)GetValue(DragCompletedPointProperty); }
			private set { SetValue(DragCompletedPointProperty, value); }
		}
		public static readonly DependencyProperty DragCompletedPointProperty =
			DependencyProperty.Register("DragCompletedPoint", typeof(Point), typeof(ImageCropControl), new PropertyMetadata());

		public Geometry OverlayClip
		{
			get { return (Geometry)GetValue(OverlayClipProperty); }
			private set { SetValue(OverlayClipProperty, value); }
		}
		public static readonly DependencyProperty OverlayClipProperty =
			DependencyProperty.Register("OverlayClip", typeof(Geometry), typeof(ImageCropControl), new PropertyMetadata());

		public double ImageScaleFactor
		{
			get { return (double)GetValue(ImageScaleFactorProperty); }
			set { SetValue(ImageScaleFactorProperty, value); }
		}
		public static readonly DependencyProperty ImageScaleFactorProperty =
			DependencyProperty.Register("ImageScaleFactor", typeof(double), typeof(ImageResizeControl), new PropertyMetadata(1.0));

		public Int32Rect CropResultRect
		{
			get { return (Int32Rect)GetValue(CropResultRectProperty); }
			private set { SetValue(CropResultRectProperty, value); }
		}
		public static readonly DependencyProperty CropResultRectProperty =
			DependencyProperty.Register("CropResultRect", typeof(Int32Rect), typeof(ImageResizeControl), new PropertyMetadata());

		public string DebugString
		{
			get { return (string)GetValue(DebugStringProperty); }
			set { SetValue(DebugStringProperty, value); }
		}
		public static readonly DependencyProperty DebugStringProperty =
			DependencyProperty.Register("DebugString", typeof(string), typeof(ImageResizeControl), new PropertyMetadata());
		#endregion

		#region Fields
		private Rectangle cropOverlay;

		private Image image;

		private CombinedGeometry overlayClipGeometry;

		private Grid imageHost;

		private ScaleTransform imageHostScaleTransform;

		private RectangleGeometry clipGeometry1;

		private RectangleGeometry clipGeometry2;
		TextBlock tb;
		#endregion

		#region Ctor/init
		static ImageCropControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageCropControl), new FrameworkPropertyMetadata(typeof(ImageCropControl)));
		}

		public ImageCropControl()
		{
			ImageStretch = Stretch.Uniform;
			DragActionCommand = new DelegateCommand(DragActionCommand_Execute);
			overlayClipGeometry = new CombinedGeometry();
			overlayClipGeometry.GeometryCombineMode = GeometryCombineMode.Exclude;
			OverlayClip = overlayClipGeometry;
			imageHostScaleTransform = new ScaleTransform(1.0, 1.0);
			clipGeometry1 = new RectangleGeometry();
			clipGeometry2 = new RectangleGeometry();
			overlayClipGeometry.Geometry1 = clipGeometry1;
			overlayClipGeometry.Geometry2 = clipGeometry2;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			cropOverlay = Template.FindName("PART_CropOverlay", this) as Rectangle;
			if (cropOverlay == null)
				throw new TemplatePartNotFoundException("PART_CropOverlay", GetType());
			cropOverlay.Visibility = Visibility.Collapsed;

			image = Template.FindName("PART_Image", this) as Image;
			if (image == null)
				throw new TemplatePartNotFoundException("PART_Image", GetType());

			imageHost = Template.FindName("PART_ImageHost", this) as Grid;
			if (imageHost == null)
				throw new TemplatePartNotFoundException("PART_ImageHost", GetType());
			imageHost.RenderTransform = imageHostScaleTransform;

			tb = Template.FindName("tb", this) as TextBlock;
		}
		#endregion

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == SourceFileInfoProperty)
			{
				if (e.NewValue == null)
					Clear();
				else
					LoadImageSource();
			}
			if(e.Property == ImageSourceProperty)
			{
				DisplayImageSource = GenerateDisplaySource(e.NewValue as BitmapSource);
			}
			if (e.Property == ActualWidthProperty)
				OverlayWidth = ActualWidth / 2.0;
			if (e.Property == ActualHeightProperty)
				OverlayHeight = ActualHeight / 2.0;
			if (e.Property == DragCompletedPointProperty)
				DragStartPoint = DragCompletedPoint;
			if (e.Property == DragStartPointProperty || e.Property == CurrentDragPointProperty || e.Property == DragCompletedPointProperty)
				RefreshOverlayClip();
			if (e.Property == DragCompletedPointProperty || e.Property == DragStartPointProperty)
				cropOverlay.Visibility = DragStartPoint == DragCompletedPoint ? Visibility.Collapsed : Visibility.Visible;
		}

		private void RefreshOverlayClip()
		{
			if (DragStartPoint == CurrentDragPoint)
				return;
			clipGeometry1.Rect = new Rect(0, 0, image.ActualWidth, image.ActualHeight);
			double x1 = Math.Min(DragStartPoint.X, CurrentDragPoint.X);
			double x2 = Math.Max(DragStartPoint.X, CurrentDragPoint.X);
			double y1 = Math.Min(DragStartPoint.Y, CurrentDragPoint.Y);
			double y2 = Math.Max(DragStartPoint.Y, CurrentDragPoint.Y);
			clipGeometry2.Rect = new Rect(x1, y1, x2 - x1, y2 - y1);
		}

		private void LoadImageSource()
		{
			BitmapImage source = new BitmapImage();
			source.BeginInit();
			source.UriSource = new Uri(SourceFileInfo.FullName);
			source.EndInit();
			ImageSource = source;
			AverageBrightness = ImageHelper.CalculateAverageLightness(ImageSource);
			Color overlayColor = default(Color);
			float overlayAlpha = (float)(1.0 - AverageBrightness);
			float colorValue = AverageBrightness > 0.5 ? 0.0f : 1.0f;
			overlayColor = Color.FromScRgb(overlayAlpha, colorValue, colorValue, colorValue);
			CropOverlayBrush = new SolidColorBrush(overlayColor);
			ImageScaleFactor = Math.Min(ActualHeight / source.Height, ActualWidth / source.Width);
		}

		private BitmapSource GenerateDisplaySource(BitmapSource originalSource)
		{
			if (originalSource == null)
				return null;
			double width = originalSource.PixelWidth, height = originalSource.PixelHeight;
			double hostWidth = imageHost.ActualWidth, hostHeight = imageHost.ActualHeight;

			if (hostWidth > hostHeight)
			{
				if (width > height)
				{
					width = hostWidth;
					height = width / originalSource.PixelWidth * originalSource.PixelHeight;
				}
				else
				{
					height = hostHeight;
					width = height / originalSource.PixelHeight * originalSource.PixelWidth;
				}
			}
			else
			{
				height = hostHeight;
				width = height / originalSource.PixelWidth * originalSource.PixelHeight;
			}

			return ImageResizeControl.ResizeImage(originalSource, (int)width, (int)height);
		}

		private void Clear()
		{
			ImageSource = null;
		}

		private void DragActionCommand_Execute(object obj)
		{
			if (obj is DragActionInfo dragInfo)
			{
				if (dragInfo.ActionType == DragActionType.Started)
				{
					DragStartPoint = image.TranslatePoint(Mouse.GetPosition(image), image);
					CurrentDragPoint = new Point(DragStartPoint.X + 1, DragStartPoint.Y + 1);
				}
				if (dragInfo.ActionType == DragActionType.Delta)
				{
					CurrentDragPoint = image.TranslatePoint(Mouse.GetPosition(image), image);
				}
				if (dragInfo.ActionType == DragActionType.Completed)
				{
					DragCompletedPoint = image.TranslatePoint(Mouse.GetPosition(image), image);
					CurrentDragPoint = image.TranslatePoint(Mouse.GetPosition(image), image);
					CropResultRect = GenerateCropResultRect();
					CropSource();
				}
			}
		}

		private Int32Rect GenerateCropResultRect()
		{
			double originalWidth = ImageSource.PixelWidth,
					originalHeight = ImageSource.PixelHeight,
					scaleFactor = originalWidth / image.ActualWidth,
					cropX = clipGeometry2.Rect.X * scaleFactor,
					cropY = clipGeometry2.Rect.Y * scaleFactor,
					cropWidth = clipGeometry2.Rect.Width * scaleFactor,
					cropHeight = clipGeometry2.Rect.Height * scaleFactor;
			return new Int32Rect((int)cropX, (int)cropY, (int)cropWidth, (int)cropHeight);
		}

		private void CropSource()
		{
			if (CropResultRect.X + CropResultRect.Width > ImageSource.PixelWidth || CropResultRect.Y + CropResultRect.Height > ImageSource.PixelHeight)
				return;
			CroppedBitmap cb = new CroppedBitmap(ImageSource, CropResultRect);
			ImageSource = cb;
		}

		//private void RebuildDebugString()
		//{
		//	DebugString = String.Format("drag start point: {0:0.00}x{1:0.00}\n\r " +
		//								"current drag point: {2:0.00}x{3:0.00}\n\r" +
		//								"drag completed point: {4:0.00}x{5:0.00}" +
		//								"crop result rect: {5}", DragStartPoint.X, DragStartPoint.Y, CurrentDragPoint.X, CurrentDragPoint.Y, DragCompletedPoint.X, DragCompletedPoint.Y, CropResultRect);
		//}
	}

	public class ImageCropThumb : Thumb
	{
		public ICommand DragActionCommand
		{
			get { return (ICommand)GetValue(DragActionCommandProperty); }
			set { SetValue(DragActionCommandProperty, value); }
		}
		public static readonly DependencyProperty DragActionCommandProperty =
			DependencyProperty.Register("DragActionCommand", typeof(ICommand), typeof(ImageCropThumb), new PropertyMetadata());

		static ImageCropThumb()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageCropThumb), new FrameworkPropertyMetadata(typeof(ImageCropThumb)));
		}

		public ImageCropThumb()
		{
			DragStarted += ImageCropThumb_DragStarted;
			DragCompleted += ImageCropThumb_DragCompleted;
			DragDelta += ImageCropThumb_DragDelta;
		}

		private void ImageCropThumb_DragStarted(object sender, DragStartedEventArgs e)
		{
			ExecuteDragActionCommand(e);
		}

		private void ImageCropThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			ExecuteDragActionCommand(e);
		}

		private void ImageCropThumb_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			ExecuteDragActionCommand(e);
		}

		private void ExecuteDragActionCommand(RoutedEventArgs e)
		{
			if (DragActionCommand == null)
				return;
			if (e is DragStartedEventArgs)
			{
				DragStartedEventArgs dsea = e as DragStartedEventArgs;
				DragActionCommand.Execute(new DragActionInfo(DragActionType.Started, dsea));
			}
			else if (e is DragDeltaEventArgs)
			{
				DragDeltaEventArgs ddea = e as DragDeltaEventArgs;
				DragActionCommand.Execute(new DragActionInfo(DragActionType.Delta, ddea, new Point(ddea.HorizontalChange, ddea.VerticalChange)));
			}
			else if (e is DragCompletedEventArgs)
			{
				DragCompletedEventArgs dcea = e as DragCompletedEventArgs;
				DragActionCommand.Execute(new DragActionInfo(DragActionType.Completed, dcea));
			}
		}
	}

	public struct DragActionInfo
	{
		public DragActionType ActionType;
		public Point? Point;
		public RoutedEventArgs EventArgs;

		public bool HasPoint
		{
			get { return Point != null && Point.HasValue; }
		}

		public DragActionInfo(DragActionType actionType, RoutedEventArgs args, Point? point = null)
		{
			ActionType = actionType;
			EventArgs = args;
			Point = point;
		}
	}

	public enum DragActionType
	{
		Started,
		Delta,
		Completed
	}
}
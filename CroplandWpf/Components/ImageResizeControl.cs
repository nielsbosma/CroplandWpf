using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;

namespace CroplandWpf.Components
{
	public class ImageResizeControl : Control
	{
		public static Guid GetUniqueSourceId(DependencyObject obj)
		{
			return (Guid)obj.GetValue(UniqueSourceIdProperty);
		}
		private static void SetUniqueSourceId(DependencyObject obj, Guid value)
		{
			obj.SetValue(UniqueSourceIdProperty, value);
		}
		private static readonly DependencyProperty UniqueSourceIdProperty =
			DependencyProperty.RegisterAttached("UniqueSourceId", typeof(Guid), typeof(ImageResizeControl), new PropertyMetadata());

		public BitmapSource Target
		{
			get { return (BitmapSource)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(BitmapSource), typeof(ImageResizeControl), new PropertyMetadata());

		public double TargetWidth
		{
			get { return (double)GetValue(TargetWidthProperty); }
			set { SetValue(TargetWidthProperty, value); }
		}
		public static readonly DependencyProperty TargetWidthProperty =
			DependencyProperty.Register("TargetWidth", typeof(double), typeof(ImageResizeControl), new PropertyMetadata(0.0, null, CoerceTargetDimension));

		public double TargetHeight
		{
			get { return (double)GetValue(TargetHeightProperty); }
			set { SetValue(TargetHeightProperty, value); }
		}
		public static readonly DependencyProperty TargetHeightProperty =
			DependencyProperty.Register("TargetHeight", typeof(double), typeof(ImageResizeControl), new PropertyMetadata(0.0, null, CoerceTargetDimension));

		public double MaxTargetDimenstion
		{
			get { return (double)GetValue(MaxTargetDimenstionProperty); }
			set { SetValue(MaxTargetDimenstionProperty, value); }
		}
		public static readonly DependencyProperty MaxTargetDimenstionProperty =
			DependencyProperty.Register("MaxTargetDimenstion", typeof(double), typeof(ImageResizeControl), new PropertyMetadata(16368.0));

		private static object CoerceTargetDimension(DependencyObject o, object value)
		{
			ImageResizeControl irc = o as ImageResizeControl;
			double dValue = (double)value;
			if (dValue <= 0.0)
				return irc.HasTarget ? 1.0 : 0.0;
			else if (dValue > irc.MaxTargetDimenstion)
				return irc.MaxTargetDimenstion;
			return Math.Round(dValue, 0);
		}

		public ResizeMode ResizeMode
		{
			get { return (ResizeMode)GetValue(ResizeModeProperty); }
			set { SetValue(ResizeModeProperty, value); }
		}
		public static readonly DependencyProperty ResizeModeProperty =
			DependencyProperty.Register("ResizeMode", typeof(ResizeMode), typeof(ImageResizeControl), new PropertyMetadata());

		public ImageResizeInfo ResizeInfo
		{
			get { return (ImageResizeInfo)GetValue(ResizeInfoProperty); }
			private set { SetValue(ResizeInfoProperty, value); }
		}
		public static readonly DependencyProperty ResizeInfoProperty =
			DependencyProperty.Register("ResizeInfo", typeof(ImageResizeInfo), typeof(ImageResizeControl), new PropertyMetadata());

		public ICommand ResizeOverrideCommand
		{
			get { return (ICommand)GetValue(ResizeOverrideCommandProperty); }
			set { SetValue(ResizeOverrideCommandProperty, value); }
		}
		public static readonly DependencyProperty ResizeOverrideCommandProperty =
			DependencyProperty.Register("ResizeOverrideCommand", typeof(ICommand), typeof(ImageResizeControl), new PropertyMetadata());

		public DelegateCommand SetTargetCommand
		{
			get { return (DelegateCommand)GetValue(SetTargetCommandProperty); }
			private set { SetValue(SetTargetCommandProperty, value); }
		}
		public static readonly DependencyProperty SetTargetCommandProperty =
			DependencyProperty.Register("SetTargetCommand", typeof(DelegateCommand), typeof(ImageResizeControl), new PropertyMetadata());

		public DelegateCommand ApplyCommand
		{
			get { return (DelegateCommand)GetValue(ApplyCommandProperty); }
			private set { SetValue(ApplyCommandProperty, value); }
		}
		public static readonly DependencyProperty ApplyCommandProperty =
			DependencyProperty.Register("ApplyCommand", typeof(DelegateCommand), typeof(ImageResizeControl), new PropertyMetadata());

		public bool KeepConstraint
		{
			get { return (bool)GetValue(KeepConstraintProperty); }
			set { SetValue(KeepConstraintProperty, value); }
		}
		public static readonly DependencyProperty KeepConstraintProperty =
			DependencyProperty.Register("KeepConstraint", typeof(bool), typeof(ImageResizeControl), new PropertyMetadata());

		public List<ResizeMode> AvailableResizeModes
		{
			get { return (List<ResizeMode>)GetValue(AvailableResizeModesProperty); }
			private set { SetValue(AvailableResizeModesProperty, value); }
		}
		public static readonly DependencyProperty AvailableResizeModesProperty =
			DependencyProperty.Register("AvailableResizeModes", typeof(List<ResizeMode>), typeof(ImageResizeControl), new PropertyMetadata());

		public bool HasTarget
		{
			get { return (bool)GetValue(HasTargetProperty); }
			private set { SetValue(HasTargetProperty, value); }
		}
		public static readonly DependencyProperty HasTargetProperty =
			DependencyProperty.Register("HasTarget", typeof(bool), typeof(ImageResizeControl), new PropertyMetadata());

		private bool canResize
		{
			get
			{

				return Target != null && (newTargetWidth != Target.PixelWidth || newTargetHeight != Target.PixelHeight);
			}
		}

		private double newTargetWidth
		{
			get
			{
				if (ResizeMode == ResizeMode.Absolute)
					return TargetWidth;
				else
					return Target.PixelWidth / 100.0 * TargetWidth;
			}
		}

		private double newTargetHeight
		{
			get
			{
				if (ResizeMode == ResizeMode.Absolute)
					return TargetHeight;
				else
					return Target.PixelHeight / 100.0 * TargetHeight;
			}
		}

		private double absoluteSizeAspectRatio = 1.0;

		private BitmapSource targetBackup;
		private bool blockTargetBackupCreation;
		private bool blockResizeInfoRefresh;
		private bool isConstraintRefresing;
		private ImageResizeInfo? temporalResizeInfo;

		static ImageResizeControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageResizeControl), new FrameworkPropertyMetadata(typeof(ImageResizeControl)));
		}

		private static void TrySetUniqueId(BitmapSource source)
		{
			if (GetUniqueSourceId(source) == default(Guid))
				SetUniqueSourceId(source, Guid.NewGuid());
		}

		public ImageResizeControl()
		{
			AvailableResizeModes = new List<ResizeMode> { ResizeMode.Absolute, ResizeMode.Relative };
			ResizeMode = ResizeMode.Absolute;
			ApplyCommand = new DelegateCommand(ApplyCommand_Execute, ApplyCommand_CanExecute);
			SetTargetCommand = new DelegateCommand(SetTargetCommand_Execute, SetTargetCommand_CanExecute);
			//KeepConstraint = true;
		}

		private void SetTargetCommand_Execute(object obj)
		{
			blockTargetBackupCreation = false;
			Target = (BitmapImage)obj;
		}

		private bool SetTargetCommand_CanExecute(object arg)
		{
			return arg as BitmapImage != null;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TargetProperty)
			{
				AcceptNewTarget();
				HasTarget = e.NewValue != null;
			}
			if (KeepConstraint)
			{
				if (e.Property == TargetWidthProperty && !isConstraintRefresing)
					AcceptWidthInput(TargetWidth);
				if (e.Property == TargetHeightProperty && !isConstraintRefresing)
					AcceptHeightInput(TargetHeight);
			}
			if (e.Property == TargetWidthProperty || e.Property == TargetHeightProperty || e.Property == ResizeModeProperty)
			{
				if (!blockResizeInfoRefresh)
					ResizeInfo = GenerateResultInfo();
			}
			if (e.Property == ResizeModeProperty)
			{
				isConstraintRefresing = true;
				if ((ResizeMode)e.NewValue == ResizeMode.Absolute)
				{
					TargetWidth = targetBackup.PixelWidth;
					TargetHeight = targetBackup.PixelHeight;
				}
				else
				{
					TargetWidth = 100.0;
					TargetHeight = 100.0;
				}
				isConstraintRefresing = false;
			}
		}

		private void AcceptWidthInput(double newWidth)
		{
			isConstraintRefresing = true;
			if (ResizeMode == ResizeMode.Absolute)
				TargetHeight = Math.Round(newWidth / absoluteSizeAspectRatio, 0);
			else
				TargetHeight = newWidth;
			isConstraintRefresing = false;
		}

		private void AcceptHeightInput(double newHeight)
		{
			isConstraintRefresing = true;
			if (ResizeMode == ResizeMode.Absolute)
				TargetWidth = Math.Round(newHeight * absoluteSizeAspectRatio, 0);
			else
				TargetWidth = newHeight;
			isConstraintRefresing = false;
		}

		private ImageResizeInfo GenerateResultInfo()
		{
			return new ImageResizeInfo(Target, new DimensionalResizeInfo(ResizeMode, newTargetWidth), new DimensionalResizeInfo(ResizeMode, newTargetHeight));
		}

		private void AcceptNewTarget()
		{
			IsEnabled = Target != null;
			if (IsEnabled)
			{
				blockResizeInfoRefresh = true;
				if (targetBackup != null && GetUniqueSourceId(Target) == GetUniqueSourceId(targetBackup) && temporalResizeInfo != null)
				{
					Guid g = GetUniqueSourceId(Target);
					Guid g2 = GetUniqueSourceId(targetBackup);
					ResizeInfo = temporalResizeInfo.Value;
				}
				else
				{
					TrySetUniqueId(Target);
					ResizeMode = ResizeMode.Absolute;
					TargetWidth = Target.PixelWidth;
					TargetHeight = Target.PixelHeight;
					targetBackup = null;
					ResizeInfo = GenerateResultInfo();
					absoluteSizeAspectRatio = (double)Target.PixelWidth / (double)Target.PixelHeight;
					if (!blockTargetBackupCreation)
					{
						targetBackup = Target.Clone();
						SetUniqueSourceId(targetBackup, GetUniqueSourceId(Target));
					}
				}
				blockResizeInfoRefresh = false;
			}
			else
				Cleanup();
		}

		private void Cleanup()
		{
			TargetWidth = 0.0;
			TargetHeight = 0.0;
			ResizeInfo = ImageResizeInfo.GetDefaultInfo(null);
			targetBackup = null;
			absoluteSizeAspectRatio = 1.0;
			temporalResizeInfo = null;
		}

		private void ApplyCommand_Execute(object obj)
		{
			if (ResizeOverrideCommand != null)
				ResizeOverrideCommand.Execute(obj);
			else
			{
				blockTargetBackupCreation = true;
				temporalResizeInfo = ResizeInfo;
				Target = CreateResizedImage(targetBackup, (int)newTargetWidth, (int)newTargetHeight, 0);
				blockTargetBackupCreation = false;
			}
		}

		private bool ApplyCommand_CanExecute(object arg)
		{
			//TODO
			if (ResizeOverrideCommand != null)
				return ResizeOverrideCommand.CanExecute(arg);
			return canResize;
		}

		private BitmapSource CreateResizedImage(BitmapSource source, int width, int height, int margin)
		{
			Rect rect = new Rect(margin, margin, width - margin * 2, height - margin * 2);

			DrawingGroup group = new DrawingGroup();
			RenderOptions.SetBitmapScalingMode(group, BitmapScalingMode.HighQuality);
			group.Children.Add(new ImageDrawing(source, rect));

			DrawingVisual drawingVisual = new DrawingVisual();
			DrawingContext dc = drawingVisual.RenderOpen();
			dc.DrawDrawing(group);
			dc.Close();

			RenderTargetBitmap resizedImage = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
			resizedImage.Render(drawingVisual);

			BitmapSource resizedResult = BitmapFrame.Create(resizedImage);
			Guid g = GetUniqueSourceId(source);
			SetUniqueSourceId(resizedResult, GetUniqueSourceId(source));
			return resizedResult;
		}
	}

	public enum ResizeMode
	{
		Absolute,
		Relative
	}

	public struct ImageResizeInfo
	{
		public BitmapSource SourceImage;
		public DimensionalResizeInfo WidthResizeInfo;
		public DimensionalResizeInfo HeightResizeInfo;

		public ImageResizeInfo(BitmapSource source, DimensionalResizeInfo widthResizeInfo, DimensionalResizeInfo heightResizeInfo)
		{
			SourceImage = source;
			WidthResizeInfo = widthResizeInfo;
			HeightResizeInfo = heightResizeInfo;
		}

		public static ImageResizeInfo GetDefaultInfo(BitmapSource source)
		{
			return new ImageResizeInfo(source, new DimensionalResizeInfo(ResizeMode.Absolute, source.PixelWidth), new DimensionalResizeInfo(ResizeMode.Absolute, source.PixelHeight));
		}

		public static bool operator ==(ImageResizeInfo info1, ImageResizeInfo info2)
		{
			return info1.Equals(info2);
		}

		public static bool operator !=(ImageResizeInfo info1, ImageResizeInfo info2)
		{
			return !info1.Equals(info2);
		}

		public override bool Equals(object obj)
		{
			ImageResizeInfo other = (ImageResizeInfo)obj;
			if ((SourceImage == null || other.SourceImage == null) && SourceImage != other.SourceImage)
				return false;
			return ImageResizeControl.GetUniqueSourceId(SourceImage) == ImageResizeControl.GetUniqueSourceId(other.SourceImage) && WidthResizeInfo.Equals(other.WidthResizeInfo) && HeightResizeInfo.Equals(other.HeightResizeInfo);
		}

		public override string ToString()
		{
			return String.Format("Source: {0}; {1} - {2}", SourceImage, WidthResizeInfo, HeightResizeInfo);
		}
	}

	public struct DimensionalResizeInfo
	{
		public ResizeMode Mode;
		public double Value;

		public DimensionalResizeInfo(ResizeMode mode, double value)
		{
			Mode = mode;
			Value = value;
		}

		public override bool Equals(object obj)
		{
			DimensionalResizeInfo other = (DimensionalResizeInfo)obj;
			return Mode == other.Mode && Value == other.Value;
		}

		public override string ToString()
		{
			return String.Format("{0} ({1})", Value, Mode);
		}
	}
}

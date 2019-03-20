using CroplandWpf.Exceptions;
using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using CroplandWpf.PresentationHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CroplandWpf.Components
{
    public class ImageCropControl : Control
    {
        #region Constants
        private const string AR_FREEFORM = "Freeform";
        private const string AR_SAMPLE_AS_ORIGINAL = "Sample as original";
        private const string AR_SQUARE = "Square";
        private const string AR_GOLDEN_RADIO = "Golden Radio";
        private const string AR_2X3_IPHONE = "2 X 3 (iPhone)";
        private const string AR_3X5 = "3 X 5";
        private const string AR_4X3 = "4 X 3";
        private const string AR_4X6_POSTCARD = "4 X 6 (Postcard)";
        private const string AR_5X7 = "5 X 7";
        private const string AR_8X10 = "8 X 10";
        private const string AR_16X9 = "16 X 9";
        private const string AR_FACEBOOK_COVER_PHOTO = "Facebook Cover Photo";
        private const string AR_640X1136_IPHONE5 = "640 X 1136 (iPhone 5)";
        private const string AR_1080X1920_IPHONE6PLUS = "1080 X 1920 (iPhone 6 plus)";
        private const string AR_YOUR_SCREEN = "Your Screen";
        #endregion

        #region DPs
        public FileInfo SourceFileInfo
        {
            get { return (FileInfo)GetValue(SourceFileInfoProperty); }
            set { SetValue(SourceFileInfoProperty, value); }
        }
        public static readonly DependencyProperty SourceFileInfoProperty =
            DependencyProperty.Register("SourceFileInfo", typeof(FileInfo), typeof(ImageCropControl), new PropertyMetadata());

        public bool AutoCrop
        {
            get { return (bool)GetValue(AutoCropProperty); }
            set { SetValue(AutoCropProperty, value); }
        }
        public static readonly DependencyProperty AutoCropProperty =
            DependencyProperty.Register("AutoCrop", typeof(bool), typeof(ImageCropControl), new PropertyMetadata(false));

        public Size OriginalImageSize
        {
            get { return (Size)GetValue(OriginalImageSizeProperty); }
            private set { SetValue(OriginalImageSizeProperty, value); }
        }
        public static readonly DependencyProperty OriginalImageSizeProperty =
            DependencyProperty.Register("OriginalImageSize", typeof(Size), typeof(ImageCropControl), new PropertyMetadata());

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

        public ImageBrush ImageBrush
        {
            get { return (ImageBrush)GetValue(ImageBrushProperty); }
            set { SetValue(ImageBrushProperty, value); }
        }
        public static readonly DependencyProperty ImageBrushProperty =
            DependencyProperty.Register("ImageBrush", typeof(ImageBrush), typeof(ImageCropControl), new PropertyMetadata());

        public double ImageScaleFactor
        {
            get { return (double)GetValue(ImageScaleFactorProperty); }
            set { SetValue(ImageScaleFactorProperty, value); }
        }
        public static readonly DependencyProperty ImageScaleFactorProperty =
            DependencyProperty.Register("ImageScaleFactor", typeof(double), typeof(ImageCropControl), new PropertyMetadata(1.0, null, CoerceImageScaleFactor));

        private static object CoerceImageScaleFactor(DependencyObject o, object value)
        {
            if (value is double dValue)
            {
                if (dValue < 0.05)
                    dValue = 0.05;
                else if (dValue > 5.0)
                    dValue = 5.0;
                return dValue;
            }
            return value;
        }

        public ImageCropRect CropResultRect
        {
            get { return (ImageCropRect)GetValue(CropResultRectProperty); }
            private set { SetValue(CropResultRectProperty, value); }
        }
        public static readonly DependencyProperty CropResultRectProperty =
            DependencyProperty.Register("CropResultRect", typeof(ImageCropRect), typeof(ImageCropControl), new PropertyMetadata());

        public bool IsRectEditable
        {
            get { return (bool)GetValue(IsRectEditableProperty); }
            private set { SetValue(IsRectEditableProperty, value); }
        }
        public static readonly DependencyProperty IsRectEditableProperty =
            DependencyProperty.Register("IsRectEditable", typeof(bool), typeof(ImageCropControl), new PropertyMetadata());

        public int CropX
        {
            get { return (int)GetValue(CropXProperty); }
            set
            {
                SetValue(CropXProperty, value);
            }
        }
        public static readonly DependencyProperty CropXProperty =
            DependencyProperty.Register("CropX", typeof(int), typeof(ImageCropControl), new PropertyMetadata(0, null, CoerceImageCropX));

        public int CropY
        {
            get { return (int)GetValue(CropYProperty); }
            set { SetValue(CropYProperty, value); }
        }
        public static readonly DependencyProperty CropYProperty =
            DependencyProperty.Register("CropY", typeof(int), typeof(ImageCropControl), new PropertyMetadata(0, null, CoerceImageCropY));

        public int CropWidth
        {
            get { return (int)GetValue(CropWidthProperty); }
            set { SetValue(CropWidthProperty, value); }
        }
        public static readonly DependencyProperty CropWidthProperty =
            DependencyProperty.Register("CropWidth", typeof(int), typeof(ImageCropControl), new PropertyMetadata(0, null, CoerceImageCropWidth));

        public int CropHeight
        {
            get { return (int)GetValue(CropHeightProperty); }
            set { SetValue(CropHeightProperty, value); }
        }
        public static readonly DependencyProperty CropHeightProperty =
            DependencyProperty.Register("CropHeight", typeof(int), typeof(ImageCropControl), new PropertyMetadata(0, null, CoerceImageCropHeight));

        private static object CoerceImageCropX(DependencyObject o, object value)
        {
            if (coerceImageCrop && value is int intValue && o is ImageCropControl c)
            {
                if (intValue < 0)
                    return 0;
                if (intValue + c.CropWidth > c.OriginalImageSize.Width)
                    return Math.Max(0, (int)(c.OriginalImageSize.Width - c.CropWidth));
            }
            return value;
        }

        private static object CoerceImageCropY(DependencyObject o, object value)
        {
            if (coerceImageCrop && value is int intValue && o is ImageCropControl c)
            {
                if (intValue < 0)
                    return 0;
                if (intValue + c.CropHeight > c.OriginalImageSize.Height)
                    return Math.Max(0, (int)(c.OriginalImageSize.Height - c.CropHeight));
            }
            return value;
        }

        private static object CoerceImageCropWidth(DependencyObject o, object value)
        {
            if (coerceImageCrop && value is int intValue && o is ImageCropControl c)
            {
                if (c.IsRectEditable)
                {
                    if (intValue <= 0)
                        return 1;
                }
                else if (intValue < 0)
                    return 0;
                if (intValue + c.CropX > c.OriginalImageSize.Width)
                    return Math.Max(0, (int)(c.OriginalImageSize.Width - c.CropX));
            }
            return value;
        }

        private static object CoerceImageCropHeight(DependencyObject o, object value)
        {
            if (coerceImageCrop && value is int intValue && o is ImageCropControl c)
            {
                if (c.IsRectEditable)
                {
                    if (intValue <= 0)
                        return 1;
                }
                else if (intValue < 0)
                    return 0;
                if (intValue + c.CropY > c.OriginalImageSize.Height)
                    return Math.Max(0, (int)(c.OriginalImageSize.Height - c.CropY));
            }
            return value;
        }

        public Visibility ResizeWrapperVisibility
        {
            get { return (Visibility)GetValue(ResizeWrapperVisibilityProperty); }
            private set { SetValue(ResizeWrapperVisibilityProperty, value); }
        }
        public static readonly DependencyProperty ResizeWrapperVisibilityProperty =
            DependencyProperty.Register("ResizeWrapperVisibility", typeof(Visibility), typeof(ImageCropControl), new PropertyMetadata(Visibility.Collapsed));

        public double ResizeThumbScaleFactor
        {
            get { return (double)GetValue(ResizeThumbScaleFactorProperty); }
            private set { SetValue(ResizeThumbScaleFactorProperty, value); }
        }
        public static readonly DependencyProperty ResizeThumbScaleFactorProperty =
            DependencyProperty.Register("ResizeThumbScaleFactor", typeof(double), typeof(ImageCropControl), new PropertyMetadata(1.0));

        public List<string> AspectRatioItemsSource
        {
            get { return (List<string>)GetValue(AspectRatioItemsSourceProperty); }
            private set { SetValue(AspectRatioItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty AspectRatioItemsSourceProperty =
            DependencyProperty.Register("AspectRatioItemsSource", typeof(List<string>), typeof(ImageCropControl), new PropertyMetadata());

        #endregion

        #region Commands
        public DelegateCommand ResizeThumbDragCommand
        {
            get { return (DelegateCommand)GetValue(ResizeThumbDragCommandProperty); }
            private set { SetValue(ResizeThumbDragCommandProperty, value); }
        }
        public static readonly DependencyProperty ResizeThumbDragCommandProperty =
            DependencyProperty.Register("ResizeThumbDragCommand", typeof(DelegateCommand), typeof(ImageCropControl), new PropertyMetadata());
        public ICommand SubmitCropResultRectCommand
        {
            get { return (ICommand)GetValue(SubmitCropResultRectCommandProperty); }
            set { SetValue(SubmitCropResultRectCommandProperty, value); }
        }
        public static readonly DependencyProperty SubmitCropResultRectCommandProperty =
            DependencyProperty.Register("SubmitCropResultRectCommand", typeof(ICommand), typeof(ImageCropControl), new PropertyMetadata());

        public DelegateCommand ApplyCommandInternal
        {
            get { return (DelegateCommand)GetValue(ApplyCommandInternalProperty); }
            private set { SetValue(ApplyCommandInternalProperty, value); }
        }
        public static readonly DependencyProperty ApplyCommandInternalProperty =
            DependencyProperty.Register("ApplyCommandInternal", typeof(DelegateCommand), typeof(ImageCropControl), new PropertyMetadata());

        public DelegateCommand DragActionCommand
        {
            get { return (DelegateCommand)GetValue(DragActionCommandProperty); }
            private set { SetValue(DragActionCommandProperty, value); }
        }
        public static readonly DependencyProperty DragActionCommandProperty =
            DependencyProperty.Register("DragActionCommand", typeof(DelegateCommand), typeof(ImageCropControl), new PropertyMetadata());

        public DelegateCommand CropValueUpdatedCommand
        {
            get { return (DelegateCommand)GetValue(CropValueUpdatedCommandProperty); }
            private set { SetValue(CropValueUpdatedCommandProperty, value); }
        }
        public static readonly DependencyProperty CropValueUpdatedCommandProperty =
            DependencyProperty.Register("CropValueUpdatedCommand", typeof(DelegateCommand), typeof(ImageCropControl), new PropertyMetadata());

        public DelegateCommand AspectRatioUpdatedCommand
        {
            get => (DelegateCommand)GetValue(AspectRationUpdatedCommandProperty);
            private set => SetValue(AspectRationUpdatedCommandProperty, value);
        }
        public static readonly DependencyProperty AspectRationUpdatedCommandProperty =
            DependencyProperty.Register("AspectRatioUpdatedCommand", typeof(DelegateCommand), typeof(ImageCropControl), new PropertyMetadata());
        #endregion

        #region Fields
        private Rectangle cropOverlay;

        private Canvas imageCanvas;

        private ImageCropThumb imageCropThumb;

        private CombinedGeometry overlayClipGeometry;

        private Grid imageHost;

        private ScaleTransform imageHostScaleTransform;

        private RectangleGeometry clipGeometry1;

        private RectangleGeometry clipGeometry2;

        private bool refreshCropElementsPosition;

        private ScrollViewer imageScrollViewer;

        private ImageCropRect lastSubmittedRect = default(ImageCropRect);

        private ComboBox aspectRatioComboBox;

        private static bool coerceImageCrop = true;

        private bool keepConstraint;

        private double absoluteSizeAspectRatio = 1.0;
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
            ApplyCommandInternal = new DelegateCommand(ApplyCommandInternal_Execute, ApplyCommandInternal_CanExecute);
            overlayClipGeometry = new CombinedGeometry();
            overlayClipGeometry.GeometryCombineMode = GeometryCombineMode.Exclude;
            OverlayClip = overlayClipGeometry;
            imageHostScaleTransform = new ScaleTransform(1.0, 1.0);
            clipGeometry1 = new RectangleGeometry();
            clipGeometry2 = new RectangleGeometry();
            overlayClipGeometry.Geometry1 = clipGeometry1;
            overlayClipGeometry.Geometry2 = clipGeometry2;
            ResizeWrapperVisibility = Visibility.Collapsed;
            ResizeThumbDragCommand = new DelegateCommand(ResizeThumbDragCommand_Execute);
            Loaded += ImageCropControl_Loaded;
            Unloaded += ImageCropControl_Unloaded;
            CropValueUpdatedCommand = new DelegateCommand(CropValueUpdatedCommand_Execute);
            AspectRatioUpdatedCommand = new DelegateCommand(AspectRatioUpdatedCommand_Execute);

            AspectRatioItemsSource = new List<string>
            {
                AR_FREEFORM,
                AR_SAMPLE_AS_ORIGINAL,
                AR_SQUARE,
                AR_GOLDEN_RADIO,
                AR_2X3_IPHONE,
                AR_3X5,
                AR_4X3,
                AR_4X6_POSTCARD,
                AR_5X7,
                AR_8X10,
                AR_16X9,
                AR_FACEBOOK_COVER_PHOTO,
                AR_640X1136_IPHONE5,
                AR_1080X1920_IPHONE6PLUS,
                AR_YOUR_SCREEN
            };
        }

        private void ImageCropControl_Loaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.RegisterHandler(this, Window.PreviewMouseWheelEvent, OnMouseWheel);
        }

        private void ImageCropControl_Unloaded(object sender, RoutedEventArgs e)
        {
            WindowHelper.UnregisterHandler(this, Window.PreviewMouseWheelEvent);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            cropOverlay = Template.FindName("PART_CropOverlay", this) as Rectangle;
            if (cropOverlay == null)
                throw new TemplatePartNotFoundException("PART_CropOverlay", GetType());
            cropOverlay.Visibility = Visibility.Collapsed;

            imageHost = Template.FindName("PART_ImageHost", this) as Grid;
            if (imageHost == null)
                throw new TemplatePartNotFoundException("PART_ImageHost", GetType());
            imageHost.LayoutTransform = imageHostScaleTransform;
            BindingOperations.SetBinding(imageHostScaleTransform, ScaleTransform.ScaleXProperty, new Binding { Source = this, Path = new PropertyPath(ImageScaleFactorProperty), Mode = BindingMode.OneWay });
            BindingOperations.SetBinding(imageHostScaleTransform, ScaleTransform.ScaleYProperty, new Binding { Source = this, Path = new PropertyPath(ImageScaleFactorProperty), Mode = BindingMode.OneWay });

            imageCanvas = Template.FindName("PART_ImageCanvas", this) as Canvas;
            if (imageCanvas == null)
                throw new TemplatePartNotFoundException("PART_ImageCanvas", GetType());

            imageCropThumb = Template.FindName("cropThumb", this) as ImageCropThumb;
            if (imageCropThumb == null)
                throw new TemplatePartNotFoundException("cropThumb", GetType());

            imageScrollViewer = Template.FindName("PART_ScrollViewer", this) as ScrollViewer;
            if (imageScrollViewer == null)
                throw new TemplatePartNotFoundException("PART_ScrollViewer", GetType());

            aspectRatioComboBox = Template.FindName("ComboBoxAspectRatio", this) as ComboBox;
            if (aspectRatioComboBox == null)
                throw new TemplatePartNotFoundException("ComboBoxAspectRatio", GetType());
        }
        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SourceFileInfoProperty)
            {
                Clear();
                if (e.NewValue != null)
                    LoadImageSource();
            }
            if (e.Property == ImageSourceProperty)
            {
                if (e.NewValue == null)
                    Clear();
                else
                {
                    DisplayImageSource = GenerateDisplaySource(e.NewValue as BitmapSource);
                    OriginalImageSize = GetImageSize(e.NewValue as BitmapSource);
                    ImageBrush = new ImageBrush(ImageSource) { AlignmentX = AlignmentX.Left, AlignmentY = AlignmentY.Top, Stretch = Stretch.Fill };
                    imageCanvas.Width = ImageSource.PixelWidth;
                    imageCanvas.Height = ImageSource.PixelHeight;
                    imageCanvas.MouseDown += (sender, args) =>
                    {
                        cropOverlay.Visibility = Visibility.Collapsed;
                        ResizeWrapperVisibility = Visibility.Collapsed;
                        CropX = 0;
                        CropY = 0;
                        CropWidth = 0;
                        CropHeight = 0;
                        lastSubmittedRect = default(ImageCropRect);
                        IsRectEditable = false;
                        CropResultRect = default(ImageCropRect);

                        var pos = args.GetPosition(imageCanvas);
                        imageCropThumb.RaiseEvent(new DragStartedEventArgs(pos.X, pos.Y));
                    };
                    ImageScaleFactor = Math.Min((imageScrollViewer.ViewportWidth - 10) / ImageSource.PixelWidth,
                        (imageScrollViewer.ViewportHeight - 10) / ImageSource.PixelHeight);

                    CropResultRect = new ImageCropRect(0, 0, imageCanvas.Width, imageCanvas.Height);
                    CropX = CropResultRect.X;
                    CropY = CropResultRect.Y;
                    CropWidth = CropResultRect.Width;
                    CropHeight = CropResultRect.Height;
                    clipGeometry1.Rect = new Rect(0, 0, imageCanvas.Width, imageCanvas.Height);
                    clipGeometry2.Rect = new Rect(0, 0, imageCanvas.Width, imageCanvas.Height);
                    IsRectEditable = true;
                    ResizeWrapperVisibility = Visibility.Visible;
                    cropOverlay.Visibility = Visibility.Visible;
                }
            }
            if (e.Property == DragStartPointProperty || e.Property == CurrentDragPointProperty || e.Property == DragCompletedPointProperty)
                InitialRefreshOverlayClip();
            if (e.Property == DragCompletedPointProperty || e.Property == DragStartPointProperty)
                cropOverlay.Visibility = DragStartPoint == DragCompletedPoint ? Visibility.Collapsed : Visibility.Visible;
            if (e.Property == CropResultRectProperty)
            {
                IsRectEditable = (ImageCropRect)e.NewValue != default(ImageCropRect);
                if (refreshCropElementsPosition)
                {
                    CropX = CropResultRect.X;
                    CropY = CropResultRect.Y;
                    CropWidth = CropResultRect.Width;
                    CropHeight = CropResultRect.Height;
                    clipGeometry2.Rect = CropResultRect.ToRect();
                }
            }
            if (e.Property == ImageScaleFactorProperty)
                ResizeThumbScaleFactor = 1.0 / ImageScaleFactor;
        }

        private Size GetImageSize(BitmapSource source)
        {
            if (source == null)
                return Size.Empty;
            else
                return new Size(source.PixelWidth, source.PixelHeight);
        }

        private void ResizeThumbDragCommand_Execute(object obj)
        {
            ImageResizeThumbDragDelta deltaInfo = (ImageResizeThumbDragDelta)obj;
            refreshCropElementsPosition = true;
            //TODO --validation
            double hDelta = deltaInfo.hChange, vDelta = deltaInfo.vChange;


            switch (deltaInfo.Role)
            {
                case ResizeThumbRole.Move:
                    if ((hDelta < 0 && CropResultRect.X + hDelta < 0) || (hDelta > 0 && (CropResultRect.X + CropResultRect.Width + hDelta) > imageCanvas.Width))
                        hDelta = 0;

                    if ((vDelta < 0 && CropResultRect.Y + vDelta < 0) || (vDelta > 0 && (CropResultRect.Y + CropResultRect.Height + vDelta) > imageCanvas.Height))
                        vDelta = 0;

                    CropResultRect = CropResultRect.AddX(hDelta).AddY(vDelta);
                    break;

                case ResizeThumbRole.ResizeLeftTop:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddX(vDelta * absoluteSizeAspectRatio).AddY(vDelta).AddWidth(-vDelta * absoluteSizeAspectRatio).AddHeight(-vDelta)
                        : CropResultRect.AddX(hDelta).AddY(vDelta).AddWidth(-hDelta).AddHeight(-vDelta);
                    break;

                case ResizeThumbRole.ResizeTop:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddX(vDelta * absoluteSizeAspectRatio * .5).AddY(vDelta).AddWidth(-vDelta * absoluteSizeAspectRatio).AddHeight(-vDelta)
                        : CropResultRect.AddY(vDelta).AddHeight(-vDelta);
                    break;

                case ResizeThumbRole.ResizeRightTop:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddY(vDelta).AddWidth(-vDelta * absoluteSizeAspectRatio).AddHeight(-vDelta)
                        : CropResultRect.AddY(vDelta).AddHeight(-vDelta).AddWidth(hDelta);
                    break;

                case ResizeThumbRole.ResizeLeft:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddX(hDelta).AddY(hDelta * .5 / absoluteSizeAspectRatio).AddWidth(-hDelta).AddHeight(-hDelta / absoluteSizeAspectRatio)
                        : CropResultRect.AddX(hDelta).AddWidth(-hDelta);
                    break;

                case ResizeThumbRole.ResizeRight:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddWidth(hDelta).AddY(-hDelta * .5 / absoluteSizeAspectRatio).AddHeight(hDelta / absoluteSizeAspectRatio)
                        : CropResultRect.AddWidth(hDelta);
                    break;

                case ResizeThumbRole.ResizeLeftBottom:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddX(-vDelta * absoluteSizeAspectRatio).AddWidth(vDelta * absoluteSizeAspectRatio).AddHeight(vDelta)
                        : CropResultRect.AddX(hDelta).AddWidth(-hDelta).AddHeight(vDelta);
                    break;

                case ResizeThumbRole.ResizeBottom:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddX(-vDelta * absoluteSizeAspectRatio * .5).AddWidth(vDelta * absoluteSizeAspectRatio).AddHeight(vDelta)
                        : CropResultRect.AddHeight(vDelta);
                    break;

                case ResizeThumbRole.ResizeRightBottom:
                    CropResultRect = keepConstraint
                        ? CropResultRect.AddWidth(vDelta * absoluteSizeAspectRatio).AddHeight(vDelta)
                        : CropResultRect.AddWidth(hDelta).AddHeight(vDelta);
                    break;

                default:
                    break;
            }
            if (keepConstraint) NormalizeImageProportions();
            refreshCropElementsPosition = false;
        }

        private void InitialRefreshOverlayClip()
        {
            if (DragStartPoint == CurrentDragPoint)
                return;
            clipGeometry1.Rect = new Rect(0, 0, imageCanvas.ActualWidth, imageCanvas.ActualHeight);
            double x1 = Math.Min(DragStartPoint.X, CurrentDragPoint.X);
            double x2 = Math.Max(DragStartPoint.X, CurrentDragPoint.X);
            double y1 = Math.Min(DragStartPoint.Y, CurrentDragPoint.Y);
            double y2 = Math.Max(DragStartPoint.Y, CurrentDragPoint.Y);
            clipGeometry2.Rect = new Rect(x1, y1, x2 - x1, y2 - y1);
        }

        private void LoadImageSource()
        {
            Clear();
            BitmapImage source = new BitmapImage();
            using (var stream = File.OpenRead(SourceFileInfo.FullName))
            {
                source.BeginInit();
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.StreamSource = stream;
                source.EndInit();
            }
            ImageSource = source;
            AverageBrightness = ImageHelper.CalculateAverageLightness(ImageSource);
            Color overlayColor = default(Color);
            float overlayAlpha = (float)(1.0 - AverageBrightness);
            float colorValue = AverageBrightness > 0.5 ? 0.0f : 1.0f;
            overlayColor = Color.FromScRgb(overlayAlpha, colorValue, colorValue, colorValue);
            CropOverlayBrush = new SolidColorBrush(overlayColor);
            imageCanvas.Width = ImageSource.PixelWidth;
            imageCanvas.Height = ImageSource.PixelHeight;
            if (imageScrollViewer.ViewportHeight < imageCanvas.Height || imageScrollViewer.ViewportWidth < imageCanvas.Width)
            {
                double
                    sourceWidth = imageCanvas.Width,
                    sourceHeight = imageCanvas.Height,
                    viewportWidth = imageScrollViewer.ViewportWidth,
                    viewportHeight = imageScrollViewer.ViewportHeight,
                    sourceAspect = sourceWidth / sourceHeight,
                    viewportAspect = viewportWidth / viewportHeight;
                //TODO
                if (sourceAspect > viewportAspect)
                {

                }
            }
        }

        #region Event handlers
        private void OnMouseWheel(object sender, RoutedEventArgs e)
        {
            if (!IsVisible || !IsLoaded)
                return;
            if (imageScrollViewer.IsMouseOver && Keyboard.IsKeyDown(Key.LeftCtrl) && e is MouseWheelEventArgs we)
            {
                e.Handled = true;
                ImageScaleFactor += we.Delta > 0.0 ? 0.02 : -0.02;
            }
        }
        #endregion

        #region Commanding
        private void DragActionCommand_Execute(object obj)
        {
            if (ImageSource == null)
                return;
            if (obj is DragActionInfo dragInfo)
            {
                if (dragInfo.ActionType == DragActionType.Started)
                {
                    DragStartPoint = imageCanvas.TranslatePoint(Mouse.GetPosition(imageCanvas), imageCanvas);
                    CurrentDragPoint = new Point(DragStartPoint.X + 1, DragStartPoint.Y + 1);
                }
                if (dragInfo.ActionType == DragActionType.Delta)
                {
                    CurrentDragPoint = imageCanvas.TranslatePoint(Mouse.GetPosition(imageCanvas), imageCanvas);
                }
                if (dragInfo.ActionType == DragActionType.Completed)
                {
                    DragCompletedPoint = imageCanvas.TranslatePoint(Mouse.GetPosition(imageCanvas), imageCanvas);
                    CurrentDragPoint = imageCanvas.TranslatePoint(Mouse.GetPosition(imageCanvas), imageCanvas);
                    CropResultRect = GenerateCropResultRect();
                    ResizeWrapperVisibility = Visibility.Visible;
                    CropWidth = CropResultRect.Width;
                    CropHeight = CropResultRect.Height;
                    CropX = CropResultRect.X;
                    CropY = CropResultRect.Y;
                }
            }
        }

        private void ApplyCommandInternal_Execute(object obj)
        {
            if (SubmitCropResultRectCommand != null)
            {
                SubmitCropResultRectCommand.Execute(CropResultRect);
                lastSubmittedRect = CropResultRect;
            }
        }

        private bool ApplyCommandInternal_CanExecute(object arg)
        {
            return CropResultRect != default(ImageCropRect) && CropResultRect != lastSubmittedRect;
        }

        private void CropValueUpdatedCommand_Execute(object obj)
        {
            refreshCropElementsPosition = true;
            CropResultRect = new ImageCropRect(CropX, CropY, CropWidth, CropHeight);
            refreshCropElementsPosition = false;
        }

        private void AspectRatioUpdatedCommand_Execute(object obj)
        {
            var width = (int)imageCanvas.Width;
            var height = (int)imageCanvas.Height;
            var hCoef = 1;
            var wCoef = 1;
            keepConstraint = true;
            coerceImageCrop = false;

            switch (aspectRatioComboBox.SelectedValue)
            {
                case AR_FREEFORM:
                    CropX = 0;
                    CropY = 0;
                    CropWidth = width;
                    CropHeight = height;
                    keepConstraint = false;
                    break;

                case AR_SAMPLE_AS_ORIGINAL:
                    CropX = 0;
                    CropY = 0;
                    CropWidth = width;
                    CropHeight = height;
                    absoluteSizeAspectRatio = (double)width / height;
                    break;

                case AR_SQUARE:
                    GenerateCropResultByAspectRatio(1, 1);
                    break;

                case AR_GOLDEN_RADIO:
                    GenerateCropResultByAspectRatio(62, 38);
                    break;

                case AR_2X3_IPHONE:
                    GenerateCropResultByAspectRatio(2, 3);
                    break;

                case AR_3X5:
                    GenerateCropResultByAspectRatio(3, 5);
                    break;

                case AR_4X3:
                    GenerateCropResultByAspectRatio(4, 3);
                    break;

                case AR_4X6_POSTCARD:
                    GenerateCropResultByAspectRatio(4, 6);
                    break;

                case AR_5X7:
                    GenerateCropResultByAspectRatio(5, 7);
                    break;

                case AR_8X10:
                    GenerateCropResultByAspectRatio(8, 10);
                    break;

                case AR_16X9:
                    GenerateCropResultByAspectRatio(16, 9);
                    break;

                case AR_FACEBOOK_COVER_PHOTO:
                    GenerateCropResultByAspectRatio(851, 315);
                    break;

                case AR_640X1136_IPHONE5:
                    GenerateCropResultByAspectRatio(640, 1136);
                    break;

                case AR_1080X1920_IPHONE6PLUS:
                    GenerateCropResultByAspectRatio(9, 16);
                    break;

                case AR_YOUR_SCREEN:
                    GenerateCropResultByAspectRatio((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
                    break;
            }
            CropValueUpdatedCommand_Execute(null);
            coerceImageCrop = true;
        }
        #endregion

        #region Private methods
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
        private ImageCropRect GenerateCropResultRect()
        {
            double originalWidth = ImageSource.PixelWidth,
                    originalHeight = ImageSource.PixelHeight,
                    scaleFactor = originalWidth / imageCanvas.ActualWidth,
                    cropX = clipGeometry2.Rect.X * scaleFactor,
                    cropY = clipGeometry2.Rect.Y * scaleFactor,
                    cropWidth = clipGeometry2.Rect.Width * scaleFactor,
                    cropHeight = clipGeometry2.Rect.Height * scaleFactor;
            if (cropX < 0.0)
            {
                cropWidth -= Math.Abs(cropX);
                cropX = 0.0;
            }
            if (cropY < 0.0)
            {
                cropHeight -= Math.Abs(cropY);
                cropY = 0.0;
            }

            if (cropWidth + cropX > originalWidth)
                cropWidth = originalWidth - cropX;
            if (cropHeight + cropY > originalHeight)
                cropHeight = originalHeight - cropY;

            if (cropWidth < 0.0)
                cropWidth = 0.0;
            if (cropHeight < 0.0)
                cropHeight = 0.0;
            return new ImageCropRect((int)cropX, (int)cropY, (int)cropWidth, (int)cropHeight);
        }

        private void CropSource()
        {
            if (CropResultRect.X + CropResultRect.Width > ImageSource.PixelWidth || CropResultRect.Y + CropResultRect.Height > ImageSource.PixelHeight)
                return;
            CroppedBitmap cb = new CroppedBitmap(ImageSource, CropResultRect.ToInt32Rect());
            ImageSource = cb;
        }

        private void Clear()
        {
            ImageSource = null;
            cropOverlay.Visibility = Visibility.Collapsed;
            ResizeWrapperVisibility = Visibility.Collapsed;
            CropX = 0;
            CropY = 0;
            CropWidth = 0;
            CropHeight = 0;
            OriginalImageSize = Size.Empty;
            ImageScaleFactor = 1.0;
            lastSubmittedRect = default(ImageCropRect);
            IsRectEditable = false;
            CropResultRect = default(ImageCropRect);
        }

        private void GenerateCropResultByAspectRatio(float wRatio, float hRatio)
        {
            var width = (int)imageCanvas.Width;
            var height = (int)imageCanvas.Height;
            absoluteSizeAspectRatio = (double)wRatio / hRatio;

            if (width / wRatio < height / hRatio)
            {
                CropWidth = width;
                CropHeight = (int)(width / wRatio * hRatio);
                CropX = 0;
                CropY = (int)((height - CropHeight) * .5);
            }
            else
            {
                CropWidth = (int)(height / hRatio * wRatio);
                CropHeight = height;
                CropX = (int)((width - CropWidth) * .5);
                CropY = 0;
            }
        }

        private void NormalizeImageProportions()
        {
            var widthOffset = CropResultRect.Height * absoluteSizeAspectRatio - CropResultRect.Width;
            CropResultRect = CropResultRect.AddWidth(widthOffset);
        }
        #endregion
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

    public struct ImageCropRect
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public ImageCropRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public ImageCropRect(double x, double y, double width, double height)
        {
            X = (int)x;
            Y = (int)y;
            Width = (int)width;
            Height = (int)height;
        }

        public ImageCropRect AddX(double xChange)
        {
            return new ImageCropRect { X = X + (int)xChange, Y = Y, Width = Width, Height = Height };
        }

        public ImageCropRect AddY(double yChange)
        {
            return new ImageCropRect { X = X, Y = Y + (int)yChange, Width = Width, Height = Height };
        }

        public ImageCropRect AddWidth(double widthChange)
        {
            return new ImageCropRect { X = X, Y = Y, Width = NormalzeDimension(Width + (int)widthChange), Height = Height };
        }

        public ImageCropRect AddHeight(double heightChange)
        {
            return new ImageCropRect { X = X, Y = Y, Width = Width, Height = NormalzeDimension(Height + (int)heightChange) };
        }

        private static int NormalzeDimension(double width)
        {
            return (int)Math.Max(1, width);
        }

        public Int32Rect ToInt32Rect()
        {
            return new Int32Rect(X, Y, Width, Height);
        }

        public override bool Equals(object obj)
        {
            if (obj is ImageCropRect other)
                return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
            else return base.Equals(obj);
        }

        public Rect ToRect()
        {
            return new Rect(X, Y, Width, Height);
        }

        public static bool operator !=(ImageCropRect rect1, ImageCropRect rect2)
        {
            return !rect1.Equals(rect2);
        }

        public static bool operator ==(ImageCropRect rect1, ImageCropRect rect2)
        {
            return rect1.Equals(rect2);
        }

        public override string ToString()
        {
            return String.Format("{0}:{1} - {2}x{3}", X, Y, Width, Height);
        }
    }
}
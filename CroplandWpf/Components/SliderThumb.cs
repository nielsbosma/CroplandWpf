using CroplandWpf.Attached;
using CroplandWpf.PresentationHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CroplandWpf.Components
{
	public class SliderThumb : Thumb
	{
		public SliderHelper SliderHelper
		{
			get { return (SliderHelper)GetValue(SliderHelperProperty); }
			set { SetValue(SliderHelperProperty, value); }
		}
		public static readonly DependencyProperty SliderHelperProperty =
			DependencyProperty.Register("SliderHelper", typeof(SliderHelper), typeof(SliderThumb), new PropertyMetadata());

		public DataTemplate ValueToolTipTemplate
		{
			get { return (DataTemplate)GetValue(ValueToolTipTemplateProperty); }
			set { SetValue(ValueToolTipTemplateProperty, value); }
		}
		public static readonly DependencyProperty ValueToolTipTemplateProperty =
			DependencyProperty.Register("ValueToolTipTemplate", typeof(DataTemplate), typeof(SliderThumb), new PropertyMetadata());

		public Rect ToolTipTargetRect
		{
			get { return (Rect)GetValue(ToolTipTargetRectProperty); }
			set { SetValue(ToolTipTargetRectProperty, value); }
		}
		public static readonly DependencyProperty ToolTipTargetRectProperty =
			DependencyProperty.Register("ToolTipTargetRect", typeof(Rect), typeof(SliderThumb), new PropertyMetadata());

		private ContentControl toolTipPresenter;

		private SliderTumbToolTipAdorner toolTipAdorner
		{
			get
			{
				if (_toolTipAdorner == null)
					_toolTipAdorner = new SliderTumbToolTipAdorner(toolTipPresenter, this);
				return _toolTipAdorner;
			}
		}
		private SliderTumbToolTipAdorner _toolTipAdorner;

		private AdornerLayer toolTipLayer
		{
			get
			{
				if (!IsLoaded)
					return null;
				if (_toolTipLayer == null)
					_toolTipLayer = RootAdornerLayerHelper.GetRootLayer(this);
				return _toolTipLayer;
			}
		}
		private AdornerLayer _toolTipLayer;

		private bool _adornerAdded = false;

		private DoubleAnimation toolTipFadeInAnimation = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromMilliseconds(100)), FillBehavior.HoldEnd);
		private DoubleAnimation toolTipFadeOutAnimation = new DoubleAnimation(1.0, 0.0, new Duration(TimeSpan.FromMilliseconds(100)), FillBehavior.HoldEnd);

		static SliderThumb()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderThumb), new FrameworkPropertyMetadata(typeof(SliderThumb)));
		}

		public SliderThumb()
		{
			toolTipPresenter = new ContentControl();
			toolTipPresenter.Opacity = 0.0;
			toolTipPresenter.SetBinding(DataContextProperty, new Binding { Source = this, Path = new PropertyPath(SliderHelperProperty), Mode = BindingMode.OneWay });
			toolTipPresenter.SetBinding(ContentControl.ContentTemplateProperty, new Binding { Source = this, Path = new PropertyPath(ValueToolTipTemplateProperty), Mode = BindingMode.OneWay });
			toolTipAdorner.SetBinding(SliderTumbToolTipAdorner.TargetRectProperty, new Binding { Source = this, Path = new PropertyPath(ToolTipTargetRectProperty), Mode = BindingMode.OneWay });
			Loaded += SliderThumb_Loaded;
			Unloaded += SliderThumb_Unloaded;
			DragDelta += SliderThumb_DragDelta;
			toolTipFadeOutAnimation.Completed += ToolTipFadeOutAnimation_Completed;
		}

		private void SliderThumb_Loaded(object sender, RoutedEventArgs e)
		{
			ToolTipTargetRect = GetToolTipTargetRect();
			toolTipFadeOutAnimation.Completed += ToolTipFadeOutAnimation_Completed;
		}

		private void SliderThumb_Unloaded(object sender, RoutedEventArgs e)
		{
			toolTipFadeOutAnimation.Completed -= ToolTipFadeOutAnimation_Completed;
		}

		private void SliderThumb_DragDelta(object sender, DragDeltaEventArgs e)
		{
			ToolTipTargetRect = GetToolTipTargetRect();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == IsMouseOverProperty)
			{
				if ((bool)e.NewValue)
					ShowValueToolTip();
				else if (!IsMouseCaptured)
					HideValueToolTip();
			}
			if (e.Property == IsMouseCapturedProperty)
			{
				if (!(bool)e.NewValue && !IsMouseOver)
					HideValueToolTip();
			}
		}

		private void ShowValueToolTip()
		{
			if (_adornerAdded)
				return;
			if (toolTipLayer != null)
			{
				toolTipLayer.Add(toolTipAdorner);
				_adornerAdded = true;
				toolTipPresenter.BeginAnimation(ContentControl.OpacityProperty, toolTipFadeInAnimation);
			}
		}

		private void HideValueToolTip()
		{
			if (!_adornerAdded)
				return;
			toolTipPresenter.BeginAnimation(ContentControl.OpacityProperty, toolTipFadeOutAnimation);
		}

		private Rect GetToolTipTargetRect()
		{
			Point p = TranslatePoint(new Point(0, 0), toolTipLayer);
			return new Rect(p.X, p.Y, ActualWidth, ActualHeight);
		}

		private void ToolTipFadeOutAnimation_Completed(object sender, EventArgs e)
		{
			toolTipLayer.Remove(toolTipAdorner);
			_adornerAdded = false;
		}
	}

	public class SliderTumbToolTipAdorner : Adorner
	{
		public Rect TargetRect
		{
			get { return (Rect)GetValue(TargetRectProperty); }
			set { SetValue(TargetRectProperty, value); }
		}
		public static readonly DependencyProperty TargetRectProperty =
			DependencyProperty.Register("TargetRect", typeof(Rect), typeof(SliderTumbToolTipAdorner), new FrameworkPropertyMetadata(default(Rect), FrameworkPropertyMetadataOptions.AffectsArrange));

		public FrameworkElement Content { get; private set; }

		protected override int VisualChildrenCount => 1;

		public SliderTumbToolTipAdorner(FrameworkElement content, UIElement adornedElement) : base(adornedElement)
		{
			IsHitTestVisible = false;
			Content = content;
			AddVisualChild(content);
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			Content.Measure(constraint);
			return Content.DesiredSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			Content.Arrange(new Rect(-Content.DesiredSize.Width / 2.0 + TargetRect.Width / 2, -Content.DesiredSize.Height, Content.DesiredSize.Width, Content.DesiredSize.Height));
			return base.ArrangeOverride(finalSize);
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
				throw new ArgumentOutOfRangeException();
			return Content;
		}
	}
}
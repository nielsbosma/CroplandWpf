using CroplandWpf.Attached;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CroplandWpf.Components
{
	public enum ToolTipPlacement
	{
		Top,
		Left,
		Right,
		Bottom
	}

	public class OverlayContentControl : HeaderedContentControl
	{
		public OverlayHost ParentOverlay { get; set; }

		public UIElement Target
		{
			get { return (UIElement)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(UIElement), typeof(OverlayContentControl), new PropertyMetadata());

		public ToolTipPlacement PlacementPriority
		{
			get { return (ToolTipPlacement)GetValue(PlacementPriorityProperty); }
			set { SetValue(PlacementPriorityProperty, value); }
		}
		public static readonly DependencyProperty PlacementPriorityProperty =
			DependencyProperty.Register("PlacementPriority", typeof(ToolTipPlacement), typeof(OverlayContentControl), new PropertyMetadata());

		public Rect TargetRect
		{
			get { return (Rect)GetValue(TargetRectProperty); }
			set { SetValue(TargetRectProperty, value); }
		}
		public static readonly DependencyProperty TargetRectProperty =
			DependencyProperty.Register("TargetRect", typeof(Rect), typeof(OverlayContentControl), new PropertyMetadata());

		public bool IsRendering
		{
			get { return (bool)GetValue(IsRenderingProperty); }
			set { SetValue(IsRenderingProperty, value); }
		}
		public static readonly DependencyProperty IsRenderingProperty =
			DependencyProperty.Register("IsRendering", typeof(bool), typeof(OverlayContentControl), new PropertyMetadata());

		public ToolTipPlacement CalculatedPlacement
		{
			get { return (ToolTipPlacement)GetValue(CalculatedPlacementProperty); }
			private set { SetValue(CalculatedPlacementProperty, value); }
		}
		public static readonly DependencyProperty CalculatedPlacementProperty =
			DependencyProperty.Register("CalculatedPlacement", typeof(ToolTipPlacement), typeof(OverlayContentControl), new PropertyMetadata());

		static OverlayContentControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayContentControl), new FrameworkPropertyMetadata(typeof(OverlayContentControl)));
		}

		public OverlayContentControl()
		{

		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TargetRectProperty)
			{
				//TODO
			}
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			double width = RenderSize.Width;
			double height = RenderSize.Height;
			if (width == 0.0 || height == 0.0)
				return;
			double top = 0.0;
			double left = 0.0;
			//TODO
			switch (PlacementPriority)
			{
				case ToolTipPlacement.Bottom:
					left = TargetRect.Left + TargetRect.Width / 2.0 - width / 2.0;
					top = TargetRect.Top + TargetRect.Height;
					if (top + height > ParentOverlay.ActualHeight)
					{
						top = TargetRect.Top - height;
						CalculatedPlacement = ToolTipPlacement.Top;
					}
					else
						CalculatedPlacement = ToolTipPlacement.Bottom;
					break;

				case ToolTipPlacement.Top:
					left = TargetRect.Left + TargetRect.Width / 2.0 - width / 2.0;
					top = TargetRect.Top - height;
					if (top < 0.0)
					{
						top = TargetRect.Top + TargetRect.Height;
						CalculatedPlacement = ToolTipPlacement.Bottom;
					}
					else
						CalculatedPlacement = ToolTipPlacement.Top;
					break;

				case ToolTipPlacement.Left:
					left = TargetRect.Left - width;
					if (left < 0.0)
					{
						left = 0.0;
						CalculatedPlacement = ToolTipPlacement.Right;
					}
					else
						CalculatedPlacement = ToolTipPlacement.Left;
					top = TargetRect.Top + TargetRect.Height / 2.0 - height / 2.0;
					break;

				case ToolTipPlacement.Right:
					left = TargetRect.Left + TargetRect.Width;
					if (left + width > ParentOverlay.ActualWidth)
					{
						left = TargetRect.Left - width;
						CalculatedPlacement = ToolTipPlacement.Left;
					}
					else
						CalculatedPlacement = ToolTipPlacement.Right;
					top = TargetRect.Top + TargetRect.Height / 2.0 - height / 2.0;
					break;

				default:
					break;
			}
			Canvas.SetLeft(this, left);
			Canvas.SetTop(this, top);
		}

		protected override Size MeasureOverride(Size constraint)
		{
			Size result = base.MeasureOverride(constraint);

			return result;
		}
	}
}
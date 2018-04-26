using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CroplandWpf.Components
{
	public class ClippingBorder : Decorator
	{
		public Thickness BorderThickness
		{
			get { return (Thickness)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}
		public static readonly DependencyProperty BorderThicknessProperty =
			DependencyProperty.Register("BorderThickness", typeof(Thickness), typeof(ClippingBorder), new FrameworkPropertyMetadata(new Thickness(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public Brush BorderBrush
		{
			get { return (Brush)GetValue(BorderBrushProperty); }
			set { SetValue(BorderBrushProperty, value); }
		}
		public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ClippingBorder), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ClippingBorder), new FrameworkPropertyMetadata(new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public Brush Background
		{
			get { return (Brush)GetValue(BackgroundProperty); }
			set { SetValue(BackgroundProperty, value); }
		}
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(ClippingBorder), new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

		private Size? arrangeResult;

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			base.InvalidateVisual();
		}

		protected override Size MeasureOverride(Size constraint)
		{
			arrangeResult = null;
			bool unconstrained = double.IsInfinity(constraint.Width) || double.IsInfinity(constraint.Height);
			Size expectedSize;
			if (Child != null)
			{
				Thickness th = AdjustThickness(BorderThickness, constraint.Width, constraint.Height);
				if (!unconstrained)
					Child.Measure(new Size(constraint.Width - th.Right - th.Left, constraint.Height - th.Bottom - th.Top));
				else
					Child.Measure(constraint);
				expectedSize = new Size(Child.DesiredSize.Width + th.Right + th.Left, Child.DesiredSize.Height + th.Bottom + th.Top);
			}
			else
				expectedSize = unconstrained ? new Size() : constraint;
			return expectedSize;
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (Child != null)
			{
				double w = arrangeSize.Width;
				double h = arrangeSize.Height;
				Thickness th = AdjustThickness(BorderThickness, w, h);
				Child.Arrange(new Rect(th.Left, th.Top, arrangeSize.Width - th.Right - th.Left, arrangeSize.Height - th.Bottom - th.Top));
			}
			arrangeResult = arrangeSize;
			return arrangeSize;
		}

		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			Geometry inner = null, outer = null;
			if (Background != null)
			{
				inner = GetInnerGeometry(BorderThickness);
				dc.DrawGeometry(Background, null, inner);
			}
			if (BorderThickness != new Thickness() && BorderBrush != null)
			{
				outer = GetOuterGeometry();
				dc.DrawGeometry(BorderBrush, null, outer);
			}
			if (base.Child != null)
			{
				var clip = GetClipGeometry();
				base.Child.Clip = clip;
			}
		}

		private Geometry GetOuterGeometry()
		{
			var geometry = new StreamGeometry();
			using (StreamGeometryContext context = geometry.Open())
			{
				double w = arrangeResult.Value.Width;
				double h = arrangeResult.Value.Height;
				var th = AdjustThickness(BorderThickness, w, h);
				var router = AdjustRadius(CornerRadius, new Thickness(), w, h);
				var rinner = AdjustRadius(CornerRadius, th, w, h);
				if (router.TopLeftLeft > th.Top + rinner.TopLeftLeft)
					router.TopLeftLeft = th.Top + rinner.TopLeftLeft;
				if (router.TopLeftTop > th.Left + rinner.TopLeftTop)
					router.TopLeftTop = th.Left + rinner.TopLeftTop;
				if (router.TopRightTop > th.Right + rinner.TopRightTop)
					router.TopRightTop = th.Right + rinner.TopRightTop;
				if (router.TopRightRight > th.Top + rinner.TopRightRight)
					router.TopRightRight = th.Top + rinner.TopRightRight;
				if (router.BottomRightRight > th.Bottom + rinner.BottomRightRight)
					router.BottomRightRight = th.Bottom + rinner.BottomRightRight;
				if (router.BottomRightBottom > th.Right + rinner.BottomRightBottom)
					router.BottomRightBottom = th.Right + rinner.BottomRightBottom;
				if (router.BottomLeftBottom > th.Left + rinner.BottomLeftBottom)
					router.BottomLeftBottom = th.Left + rinner.BottomLeftBottom;
				if (router.BottomLeftLeft > th.Bottom + rinner.BottomLeftLeft)
					router.BottomLeftLeft = th.Bottom + rinner.BottomLeftLeft;
				DefineRectangle(context, new Thickness(), router, w, h);
				DefineRectangle(context, th, rinner, w, h);
			}
			return geometry;
		}

		Geometry GetInnerGeometry(Thickness thickness = new Thickness())
		{
			StreamGeometry geometry = new StreamGeometry();
			using (StreamGeometryContext context = geometry.Open())
			{
				double w = arrangeResult.Value.Width;
				double h = arrangeResult.Value.Height;
				var th = AdjustThickness(thickness, w, h);
				var r = AdjustRadius(CornerRadius, th, w, h);
				DefineRectangle(context, th, r, w, h);
			}
			return geometry;
		}

		Geometry GetClipGeometry()
		{
			StreamGeometry geometry = new StreamGeometry();
			using (StreamGeometryContext context = geometry.Open())
			{
				double w = arrangeResult.Value.Width;
				double h = arrangeResult.Value.Height;
				var th = AdjustThickness(BorderThickness, w, h);
				var r = AdjustRadius(CornerRadius, th, w, h);
				DefineRectangle(context, new Thickness(), r, w - th.Left - th.Right, h - th.Top - th.Bottom);
			}
			return geometry;
		}

		private static void DefineRectangle(StreamGeometryContext context, Thickness th, CornerRadiusExt r, double w, double h)
		{
			context.BeginFigure(new Point(th.Left, th.Top + r.TopLeftLeft), true, true);
			context.QuadraticBezierTo(new Point(th.Left, th.Top), new Point(th.Left + r.TopLeftTop, th.Top), false, false);
			context.LineTo(new Point(w - th.Right - r.TopRightTop, th.Top), false, false);
			context.QuadraticBezierTo(new Point(w - th.Right, th.Top), new Point(w - th.Right, th.Top + r.TopRightRight), false, false);
			context.LineTo(new Point(w - th.Right, h - th.Bottom - r.BottomRightRight), false, false);
			context.QuadraticBezierTo(new Point(w - th.Right, h - th.Bottom), new Point(w - th.Right - r.BottomRightBottom, h - th.Bottom), false, false);
			context.LineTo(new Point(th.Left + r.BottomLeftBottom, h - th.Bottom), false, false);
			context.QuadraticBezierTo(new Point(th.Left, h - th.Bottom), new Point(th.Left, h - th.Bottom - r.BottomLeftLeft), false, false);
		}

		/// <summary>Proportional scaling of thicknesses larger than width/height</summary>
		public static Thickness AdjustThickness(Thickness th, double w, double h)
		{
			Thickness thickness = new Thickness();
			if (h < th.Top + th.Bottom)
			{
				thickness.Top = h * (th.Top / (th.Top + th.Bottom));
				thickness.Bottom = h * (th.Bottom / (th.Top + th.Bottom));
			}
			else
			{
				thickness.Top = th.Top;
				thickness.Bottom = th.Bottom;
			}
			if (w < th.Left + th.Right)
			{
				thickness.Left = w * (th.Left / (th.Left + th.Right));
				thickness.Right = w * (th.Right / (th.Left + th.Right));
			}
			else
			{
				thickness.Left = th.Left;
				thickness.Right = th.Right;
			}
			return thickness;
		}

		public static CornerRadiusExt AdjustRadius(CornerRadius r, Thickness th, double w, double h)
		{
			CornerRadiusExt radius = new CornerRadiusExt();
			double lside = h - th.Top - th.Bottom;
			if (lside < r.TopLeft + r.BottomLeft)
			{
				radius.TopLeftLeft = lside * r.TopLeft / (r.TopLeft + r.BottomLeft + double.Epsilon);
				radius.BottomLeftLeft = lside * r.BottomLeft / (r.TopLeft + r.BottomLeft + double.Epsilon);
			}
			else
			{
				radius.TopLeftLeft = r.TopLeft;
				radius.BottomLeftLeft = r.BottomLeft;
			}
			double rside = h - th.Top - th.Bottom;
			if (rside < r.TopRight + r.BottomRight)
			{
				radius.TopRightRight = rside * r.TopRight / (r.TopRight + r.BottomRight + double.Epsilon);
				radius.BottomRightRight = rside * r.BottomRight / (r.TopRight + r.BottomRight + double.Epsilon);
			}
			else
			{
				radius.TopRightRight = r.TopRight;
				radius.BottomRightRight = r.BottomRight;
			}
			double tside = w - th.Left - th.Right;
			if (tside < r.TopLeft + r.TopRight)
			{
				radius.TopLeftTop = tside * r.TopLeft / (r.TopLeft + r.TopRight + double.Epsilon);
				radius.TopRightTop = tside * r.TopRight / (r.TopLeft + r.TopRight + double.Epsilon);
			}
			else
			{
				radius.TopLeftTop = r.TopLeft;
				radius.TopRightTop = r.TopRight;
			}
			double bside = w - th.Left - th.Right;
			if (bside < r.BottomRight + r.BottomLeft)
			{
				radius.BottomLeftBottom = bside * r.BottomLeft / (r.BottomLeft + r.BottomRight + double.Epsilon);
				radius.BottomRightBottom = bside * r.BottomRight / (r.BottomLeft + r.BottomRight + double.Epsilon);
			}
			else
			{
				radius.BottomLeftBottom = r.BottomLeft;
				radius.BottomRightBottom = r.BottomRight;
			}
			return radius;
		}
	}

	public class CornerRadiusExt
	{
		public double TopLeftLeft { get; set; }
		public double TopLeftTop { get; set; }
		public double TopRightTop { get; set; }
		public double TopRightRight { get; set; }
		public double BottomRightRight { get; set; }
		public double BottomRightBottom { get; set; }
		public double BottomLeftBottom { get; set; }
		public double BottomLeftLeft { get; set; }

		public CornerRadiusExt()
		{

		}

		public CornerRadiusExt(double uniform) :
			this(uniform, uniform, uniform, uniform, uniform, uniform, uniform, uniform)
		{

		}

		public CornerRadiusExt(double topLeftLeft, double topLeftTop, double topRightTop, double topRightRight, double bottomRightRight, double bottomRightBottom, double bottomLeftBottom, double bottomLeftLeft)
		{
			TopLeftLeft = topLeftLeft;
			TopLeftTop = topLeftTop;
			TopRightTop = topRightTop;
			TopRightRight = topRightRight;
			BottomRightRight = bottomRightRight;
			BottomRightBottom = bottomRightBottom;
			BottomLeftBottom = bottomLeftBottom;
			BottomLeftLeft = bottomLeftLeft;
		}

		public override int GetHashCode()
		{
			return (int)((long)TopLeftLeft ^ (long)TopLeftTop ^ (long)TopRightTop ^ (long)TopRightRight ^ (long)BottomRightRight ^ (long)BottomRightBottom ^ (long)BottomLeftBottom ^ (long)BottomLeftLeft);
		}

		public override bool Equals(object obj)
		{
			CornerRadiusExt other = obj as CornerRadiusExt;
			if (other == null)
				return false;
			bool equals = TopLeftLeft == other.TopLeftLeft && TopLeftTop == other.TopLeftTop && TopRightTop == other.TopRightTop && TopRightRight == other.TopRightRight && BottomRightRight == other.BottomRightRight && BottomRightBottom == other.BottomRightBottom && BottomLeftBottom == other.BottomLeftBottom && BottomLeftLeft == other.BottomLeftLeft;
			return equals;
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", TopLeftLeft, TopLeftTop, TopRightTop, TopRightRight, BottomRightRight, BottomRightBottom, BottomLeftBottom, BottomLeftLeft);
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CroplandWpf.PresentationHelpers
{
    public class DynamicColorAnimator : FrameworkElement
    {
		public object FromColorKey
		{
			get { return (object)GetValue(FromColorKeyProperty); }
			set { SetValue(FromColorKeyProperty, value); }
		}
		public static readonly DependencyProperty FromColorKeyProperty =
			DependencyProperty.Register("FromColorKey", typeof(object), typeof(DynamicColorAnimator), new PropertyMetadata());

		public object ToColorKey
		{
			get { return (object)GetValue(ToColorKeyProperty); }
			set { SetValue(ToColorKeyProperty, value); }
		}
		public static readonly DependencyProperty ToColorKeyProperty =
			DependencyProperty.Register("ToColorKey", typeof(object), typeof(DynamicColorAnimator), new PropertyMetadata());

		public Color FromColor
		{
			get { return (Color)GetValue(FromColorProperty); }
			private set { SetValue(FromColorProperty, value); }
		}
		public static readonly DependencyProperty FromColorProperty =
			DependencyProperty.Register("FromColor", typeof(Color), typeof(DynamicColorAnimator), new PropertyMetadata());

		public Color FromColorFallback
		{
			get { return (Color)GetValue(FromColorFallbackProperty); }
			set { SetValue(FromColorFallbackProperty, value); }
		}
		public static readonly DependencyProperty FromColorFallbackProperty =
			DependencyProperty.Register("FromColorFallback", typeof(Color), typeof(DynamicColorAnimator), new PropertyMetadata());

		public Color ToColor
		{
			get { return (Color)GetValue(ToColorProperty); }
			private set { SetValue(ToColorProperty, value); }
		}
		public static readonly DependencyProperty ToColorProperty =
			DependencyProperty.Register("ToColor", typeof(Color), typeof(DynamicColorAnimator), new PropertyMetadata());

		public Color ToColorFallback
		{
			get { return (Color)GetValue(ToColorFallbackProperty); }
			set { SetValue(ToColorFallbackProperty, value); }
		}
		public static readonly DependencyProperty ToColorFallbackProperty =
			DependencyProperty.Register("ToColorFallback", typeof(Color), typeof(DynamicColorAnimator), new PropertyMetadata());

		public bool IsStateSwitched
		{
			get { return (bool)GetValue(IsStateSwitchedProperty); }
			set { SetValue(IsStateSwitchedProperty, value); }
		}
		public static readonly DependencyProperty IsStateSwitchedProperty =
			DependencyProperty.Register("IsStateSwitched", typeof(bool), typeof(DynamicColorAnimator), new PropertyMetadata());

		public Animatable AnimationTarget
		{
			get { return (Animatable)GetValue(AnimationTargetProperty); }
			set { SetValue(AnimationTargetProperty, value); }
		}
		public static readonly DependencyProperty AnimationTargetProperty =
			DependencyProperty.Register("AnimationTarget", typeof(Animatable), typeof(DynamicColorAnimator), new PropertyMetadata());

		public DependencyProperty TargetProperty
		{
			get { return (DependencyProperty)GetValue(TargetPropertyProperty); }
			set { SetValue(TargetPropertyProperty, value); }
		}
		public static readonly DependencyProperty TargetPropertyProperty =
			DependencyProperty.Register("TargetProperty", typeof(DependencyProperty), typeof(DynamicColorAnimator), new PropertyMetadata());

		public Duration Duration
		{
			get { return (Duration)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}
		public static readonly DependencyProperty DurationProperty =
			DependencyProperty.Register("Duration", typeof(Duration), typeof(DynamicColorAnimator), new PropertyMetadata());

		private ColorAnimation _animation;

		private bool isReadyToAnimate
		{
			get { return IsLoaded && AnimationTarget != null && TargetProperty != null && FromColor != default(Color) && ToColor != default(Color); }
		}

		public DynamicColorAnimator()
		{
			Visibility = Visibility.Collapsed;
			IsHitTestVisible = false;
			Opacity = 0.0;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == IsStateSwitchedProperty)
			{
				if (isReadyToAnimate)
				{
					if ((bool)e.NewValue)
						Animate();
					else
						ReverseAnimation();
				}
			}
			if (e.Property == FromColorFallbackProperty && FromColor == default(Color))
				FromColor = (Color)e.NewValue;
			if (e.Property == ToColorFallbackProperty && ToColor == default(Color))
				ToColor = (Color)e.NewValue;
			if (e.Property == ToColorKeyProperty && e.NewValue != null)
				SetResourceReference(ToColorProperty, e.NewValue);
			if (e.Property == FromColorKeyProperty && e.NewValue != null)
				SetResourceReference(FromColorProperty, e.NewValue);
		}

		private void Animate()
		{
			_animation = new ColorAnimation();
			_animation.From = FromColor;
			_animation.To = ToColor;
			_animation.Duration = Duration;
			_animation.FillBehavior = FillBehavior.HoldEnd;
			AnimationTarget.BeginAnimation(TargetProperty, _animation);
		}

		private void ReverseAnimation()
		{
			_animation = new ColorAnimation();
			_animation.From = ToColor;
			_animation.To = FromColor;
			_animation.Duration = Duration;
			_animation.FillBehavior = FillBehavior.HoldEnd;
			AnimationTarget.BeginAnimation(TargetProperty, _animation);
		}
	}
}

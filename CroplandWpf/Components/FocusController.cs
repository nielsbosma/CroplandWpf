using System.Windows;

namespace CroplandWpf.Components
{
	public class FocusController : DependencyObject
	{
		private bool isFocusEnqueued = false;

		public DependencyObject Target
		{
			get { return (DependencyObject)GetValue(TargetProperty); }
			private set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(DependencyObject), typeof(FocusController), new PropertyMetadata());

		public static FocusController GetAttachedController(DependencyObject obj)
		{
			return (FocusController)obj.GetValue(AttachedControllerProperty);
		}
		public static void SetAttachedController(DependencyObject obj, FocusController value)
		{
			obj.SetValue(AttachedControllerProperty, value);
		}
		public static readonly DependencyProperty AttachedControllerProperty =
			DependencyProperty.RegisterAttached("AttachedController", typeof(FocusController), typeof(FocusController), new PropertyMetadata((o, e) =>
			{
				DependencyObject target = o as DependencyObject;
				FocusController controller = e.NewValue as FocusController;
				controller.Target = target;
			}));

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TargetProperty && isFocusEnqueued)
				EnqueueKeyboardFocus();
		}

		public void EnqueueKeyboardFocus()
		{
			if (Target is IFocusableElement focusableTarget)
			{
				focusableTarget.KeyboardFocus();
				isFocusEnqueued = false;
			}
			else
				isFocusEnqueued = true;
		}
	}

	public interface IFocusableElement
	{
		void KeyboardFocus();
	}
}

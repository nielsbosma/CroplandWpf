using System.Windows;
using System.Windows.Input;

namespace CroplandWpf.PresentationHelpers
{
	public class EventCommandInvocator : FrameworkElement
	{
		public RoutedEvent Event
		{
			get { return (RoutedEvent)GetValue(EventProperty); }
			set { SetValue(EventProperty, value); }
		}
		public static readonly DependencyProperty EventProperty =
			DependencyProperty.Register("Event", typeof(RoutedEvent), typeof(EventCommandInvocator), new PropertyMetadata());

		public ICommand EventCommand
		{
			get { return (ICommand)GetValue(EventCommandProperty); }
			set { SetValue(EventCommandProperty, value); }
		}
		public static readonly DependencyProperty EventCommandProperty =
			DependencyProperty.Register("EventCommand", typeof(ICommand), typeof(EventCommandInvocator), new PropertyMetadata());

		public UIElement Target
		{
			get { return (UIElement)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(UIElement), typeof(EventCommandInvocator), new PropertyMetadata());

		private readonly RoutedEventHandler eventHandler;

		public EventCommandInvocator()
		{
			IsHitTestVisible = false;
			Opacity = 0.0;
			eventHandler = new RoutedEventHandler(EventHandler);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == EventProperty)
			{
				if (e.OldValue != null)
					RemoveHandler((RoutedEvent)e.OldValue, eventHandler);
				if (e.NewValue != null)
					AddHandler((RoutedEvent)e.NewValue, eventHandler);
			}
		}

		private void EventHandler(object sender, RoutedEventArgs e)
		{
			if (EventCommand != null)
				EventCommand.Execute(Target ?? (this));
		}
	}
}

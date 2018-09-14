using CroplandWpf.Attached;
using CroplandWpf.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CroplandWpf.Components
{
	public class RemovableItemsItemsControl : ItemsControl
	{
		public object CustomPartContent
		{
			get { return (object)GetValue(CustomPartContentProperty); }
			set { SetValue(CustomPartContentProperty, value); }
		}
		public static readonly DependencyProperty CustomPartContentProperty =
			DependencyProperty.Register("CustomPartContent", typeof(object), typeof(RemovableItemsItemsControl), new PropertyMetadata());

		public DataTemplate CustomPartContentTemplate
		{
			get { return (DataTemplate)GetValue(CustomPartContentTemplateProperty); }
			set { SetValue(CustomPartContentTemplateProperty, value); }
		}
		public static readonly DependencyProperty CustomPartContentTemplateProperty =
			DependencyProperty.Register("CustomPartContentTemplate", typeof(DataTemplate), typeof(RemovableItemsItemsControl), new PropertyMetadata());

		public bool HasCustomContent
		{
			get { return (bool)GetValue(HasCustomContentProperty); }
			private set { SetValue(HasCustomContentProperty, value); }
		}
		public static readonly DependencyProperty HasCustomContentProperty =
			DependencyProperty.Register("HasCustomContent", typeof(bool), typeof(RemovableItemsItemsControl), new PropertyMetadata());

		private RemovableItemsPanel itemsHost;

		static RemovableItemsItemsControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RemovableItemsItemsControl), new FrameworkPropertyMetadata(typeof(RemovableItemsItemsControl)));
		}

		public RemovableItemsItemsControl()
		{
			Loaded += RemovableItemsItemsControl_Loaded;
			Unloaded += RemovableItemsItemsControl_Unloaded;
		}

		private void RemovableItemsItemsControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (itemsHost != null && itemsHost.OnVisualAdded == null)
				itemsHost.OnVisualAdded = OnVisualAdded;
		}

		private void RemovableItemsItemsControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (itemsHost != null && itemsHost.OnVisualAdded != null)
				itemsHost.OnVisualAdded = null;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == CustomPartContentProperty || e.Property == CustomPartContentTemplateProperty)
				HasCustomContent = CustomPartContent != null || CustomPartContentTemplate != null;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			itemsHost = Template.FindName("PART_ItemsHost", this) as RemovableItemsPanel;
			if (itemsHost == null)
				throw new TemplatePartNotFoundException("PART_ItemsHost", GetType());
			itemsHost.OnVisualAdded = OnVisualAdded;
		}

		private void OnVisualAdded(DependencyObject addedChild, DependencyObject removedChild)
		{
			RefreshItemsFirstLastIndicator();
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new RemovableItemContentControl();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item != null && item.GetType().Equals(typeof(RemovableItemContentControl));
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			RemovableItemContentControl control = element as RemovableItemContentControl;
			control.ContentTemplate = ItemTemplate;
			control.Content = item;
			base.PrepareContainerForItemOverride(element, item);
		}

		private void RefreshItemsFirstLastIndicator()
		{
			//foreach (UIElement container in itemsHost.Children)
			//	SetFirstOrLastItem(container);
		}

		private void SetFirstOrLastItem(UIElement container)
		{
			if (container == null)
				return;
			int itemsCount = itemsHost.Children.Count, index = itemsHost.Children.IndexOf(container);
			if (index == 0 || index == itemsCount - 1)
			{
				VisualHelper.SetIsFirstItem(container, index == 0);
				VisualHelper.SetIsLastItem(container, index == itemsCount - 1);
			}
			else
			{
				VisualHelper.SetIsFirstItem(container, false);
				VisualHelper.SetIsLastItem(container, false);
			}
		}
	}

	public class RemovableItemsPanel : StackPanel
	{
		public Action<DependencyObject, DependencyObject> OnVisualAdded;

		public RemovableItemsPanel()
		{
			Orientation = Orientation.Horizontal;
		}

		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			return;
			if (OnVisualAdded != null)
				NotifyOnVisualAdded(visualAdded, visualRemoved);
		}

		private async void NotifyOnVisualAdded(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			await Task.Run(() =>
			{
				Dispatcher.BeginInvoke(new Action(() => { OnVisualAdded.Invoke(visualAdded, visualRemoved); }), DispatcherPriority.Background);
			});
		}
	}
}

using CroplandWpf.Attached;
using CroplandWpf.Helpers;
using System.Windows;
using System.Windows.Controls;

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

		private ItemsSourceHelper itemsSourceHelper;

		static RemovableItemsItemsControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RemovableItemsItemsControl), new FrameworkPropertyMetadata(typeof(RemovableItemsItemsControl)));
		}

		public RemovableItemsItemsControl()
		{
			itemsSourceHelper = new ItemsSourceHelper();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == CustomPartContentProperty || e.Property == CustomPartContentTemplateProperty)
				HasCustomContent = CustomPartContent != null || CustomPartContentTemplate != null;
			if (e.Property == ItemsSourceProperty)
				itemsSourceHelper.ItemContainerPairs.Clear();
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
			itemsSourceHelper.RegisterItemContainerPair(item, element as FrameworkElement);
			int itemIndex = itemsSourceHelper.ItemIndex(item);
			if (itemIndex == 0)
			{
				if (itemsSourceHelper.ItemsCount() > 1)
					control.Margin = ItemsControlHelper.GetFirstItemMargin(this);
				else
					control.Margin = ItemsControlHelper.GetLastItemMargin(this);
			}
			else if (itemsSourceHelper.HasItems() && itemIndex + 1 < itemsSourceHelper.ItemsCount())
				control.Margin = ItemsControlHelper.GetRegularItemMargin(this);
			else
				control.Margin = ItemsControlHelper.GetLastItemMargin(this);
			if (itemsSourceHelper.GetPreviousContainer(item) is RemovableItemContentControl previous)
				previous.Margin = ItemsControlHelper.GetRegularItemMargin(this);
			base.PrepareContainerForItemOverride(element, item);
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			FrameworkElement previousContainer = itemsSourceHelper.GetPreviousContainer(item);
			FrameworkElement nextContainer = itemsSourceHelper.GetNextContainer(item);
			if (previousContainer != null)
			{
				if (nextContainer == null)
					previousContainer.Margin = ItemsControlHelper.GetLastItemMargin(this);
				else
					previousContainer.Margin = ItemsControlHelper.GetRegularItemMargin(this);
			}
			itemsSourceHelper.UnregisterItemContainerPair(item);
			base.ClearContainerForItemOverride(element, item);
		}
	}
}
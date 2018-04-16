using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		static RemovableItemsItemsControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RemovableItemsItemsControl), new FrameworkPropertyMetadata(typeof(RemovableItemsItemsControl)));
		}

		public RemovableItemsItemsControl()
		{

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
		}
	}
}

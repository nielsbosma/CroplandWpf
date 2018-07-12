using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace CroplandWpf.Components
{
	[TemplatePart(Name = "PART_ItemsHolder", Type = typeof(Panel))]
	public class TabControlEx : System.Windows.Controls.TabControl
	{
		private Panel _itemsHolder = null;

		private object _deletedObject = null;

		public TabControlEx()
			: base()
		{
			ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
		}

		private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
		{
			if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
				UpdateSelectedItem();
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_itemsHolder = GetTemplateChild("PART_ItemsHolder") as Panel;
			UpdateSelectedItem();
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);

			if (_itemsHolder == null)
			{
				return;
			}

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Reset:
					_itemsHolder.Children.Clear();
					if (base.Items.Count > 0)
					{
						base.SelectedItem = base.Items[0];
						UpdateSelectedItem();
					}
					break;

				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Remove:
					if (e.NewItems != null && _deletedObject != null)
					{
						foreach (var item in e.NewItems)
						{
							if (_deletedObject == item)
							{
								ContentPresenter cp = FindChildContentPresenter(_deletedObject);
								if (cp != null)
								{
									int index = _itemsHolder.Children.IndexOf(cp);

									(_itemsHolder.Children[index] as ContentPresenter).Tag =
										(item is TabItem) ? item : (ItemContainerGenerator.ContainerFromItem(item));
								}
								_deletedObject = null;
							}
						}
					}

					if (e.OldItems != null)
					{
						foreach (var item in e.OldItems)
						{

							_deletedObject = item;
							Dispatcher.BeginInvoke(DispatcherPriority.DataBind,
															new Action(delegate ()
															{
																if (_deletedObject != null)
																{
																	ContentPresenter cp = FindChildContentPresenter(_deletedObject);
																	if (cp != null)
																	{
																		_itemsHolder.Children.Remove(cp);
																	}
																}
															}
														));
						}
					}

					UpdateSelectedItem();
					break;

				case NotifyCollectionChangedAction.Replace:
					throw new NotImplementedException("Replace not implemented yet");
			}
		}

		/// <summary>
		/// update the visible child in the ItemsHolder
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			base.OnSelectionChanged(e);
			UpdateSelectedItem();
		}

		/// <summary>
		/// generate a ContentPresenter for the selected item
		/// </summary>
		private void UpdateSelectedItem()
		{
			if (_itemsHolder == null)
			{
				return;
			}

			// generate a ContentPresenter if necessary
			TabItem item = GetSelectedTabItem();
			if (item != null)
			{
				CreateChildContentPresenter(item);
			}

			// show the right child
			foreach (ContentPresenter child in _itemsHolder.Children)
			{
				child.Visibility = ((child.Tag as TabItem).IsSelected) ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		/// <summary>
		/// create the child ContentPresenter for the given item (could be data or a TabItem)
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private ContentPresenter CreateChildContentPresenter(object item)
		{
			if (item == null)
			{
				return null;
			}

			ContentPresenter cp = FindChildContentPresenter(item);

			if (cp != null)
			{
				return cp;
			}
			
			cp = new ContentPresenter();
			cp.Content = (item is TabItem) ? (item as TabItem).Content : item;
			cp.ContentTemplate = SelectedContentTemplate;
			cp.ContentTemplateSelector = SelectedContentTemplateSelector;
			cp.ContentStringFormat = SelectedContentStringFormat;
			cp.Visibility = Visibility.Collapsed;
			cp.Tag = (item is TabItem) ? item : (ItemContainerGenerator.ContainerFromItem(item));
			_itemsHolder.Children.Add(cp);
			return cp;
		}

		private ContentPresenter FindChildContentPresenter(object data)
		{
			if (data is TabItem)
			{
				data = (data as TabItem).Content;
			}

			if (data == null)
			{
				return null;
			}

			if (_itemsHolder == null)
			{
				return null;
			}

			foreach (ContentPresenter cp in _itemsHolder.Children)
			{
				if (cp.Content == data)
				{
					return cp;
				}
			}

			return null;
		}
		
		protected TabItem GetSelectedTabItem()
		{
			object selectedItem = base.SelectedItem;
			if (selectedItem == null)
			{
				return null;
			}

			if (_deletedObject == selectedItem)
			{

			}

			TabItem item = selectedItem as TabItem;
			if (item == null)
			{
				item = base.ItemContainerGenerator.ContainerFromIndex(base.SelectedIndex) as TabItem;
			}
			return item;
		}
	}
}

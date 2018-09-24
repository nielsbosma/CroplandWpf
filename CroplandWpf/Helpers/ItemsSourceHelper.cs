using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Helpers
{
	public class ItemsSourceHelper
	{
		public ItemContainerPairCollection ItemContainerPairs { get; private set; }

		public ItemsSourceHelper()
		{
			ItemContainerPairs = new ItemContainerPairCollection();
		}

		public int ItemIndex(object item)
		{
			return ItemContainerPairs.IndexOf(item);
		}

		public int ItemsCount()
		{
			return ItemContainerPairs.Count;
		}

		public bool HasItems()
		{
			return ItemContainerPairs.Count > 0;
		}

		public FrameworkElement GetPreviousContainer(object dataItem)
		{
			if (ItemContainerPairs.Contains(dataItem))
			{
				int itemIndex = ItemContainerPairs.IndexOf(dataItem);
				if (itemIndex <= 0)
					return null;
				else
					return ItemContainerPairs[itemIndex - 1].Container;
			}
			return null;
		}

		public FrameworkElement GetNextContainer(object dataItem)
		{
			if (ItemContainerPairs.Contains(dataItem))
			{
				int itemIndex = ItemContainerPairs.IndexOf(dataItem);
				if (itemIndex == ItemsCount() - 1 || itemIndex == -1)
					return null;
				return ItemContainerPairs[itemIndex + 1].Container;
			}
			return null;
		}

		public void RegisterItemContainerPair(object dataItem, FrameworkElement container)
		{
			ItemContainerPairs.Add(dataItem, container);
		}

		public void UnregisterItemContainerPair(object dataItem)
		{
			ItemContainerPairs.Remove(dataItem);
		}
	}

	public class ItemContainerPair
	{
		public FrameworkElement Container;
		public object DataItem;

		public ItemContainerPair(object dataItem, FrameworkElement container)
		{
			DataItem = dataItem;
			Container = container;
		}
	}

	public class ItemContainerPairCollection : List<ItemContainerPair>
	{
		public object this[FrameworkElement container]
		{
			get
			{
				return this.FirstOrDefault(p => p.Container == container) ?? null;
			}
		}

		public bool Contains(object dataItem)
		{
			return GetPair(dataItem) != null;
		}

		public FrameworkElement GetContainerFor(object dataItem)
		{
			ItemContainerPair pair = GetPair(dataItem);
			if (pair == null)
				return null;
			return pair.Container;
		}

		public ItemContainerPair GetPair(object dataItem)
		{
			ItemContainerPair pair = this.FirstOrDefault(p => p.DataItem == dataItem);
			return pair;
		}

		public int IndexOf(object dataItem)
		{
			ItemContainerPair pair = this.FirstOrDefault(p => p.DataItem == dataItem);
			int result = pair == null ? -1 : base.IndexOf(pair);
			return result;
		}

		public void Add(object dataItem, FrameworkElement container)
		{
			if (GetContainerFor(dataItem) != null)
				return;
			Add(new ItemContainerPair(dataItem, container));
		}

		public void Remove(object dataItem)
		{
			ItemContainerPair pair = GetPair(dataItem);
			if (pair != null)
				base.Remove(pair);
		}

		public void Remove(FrameworkElement container)
		{
			if (this[container] == null)
				return;
			Remove(this[container]);
		}

		public void RemoveFor(object dataItem)
		{
			ItemContainerPair p = GetPair(dataItem);
			if (p != null)
				Remove(p);
		}
	}
}

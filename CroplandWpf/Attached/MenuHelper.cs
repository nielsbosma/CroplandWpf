using CroplandWpf.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace CroplandWpf.Attached
{
	public sealed class MenuHelper
	{
		public static IEnumerable GetItemsSource(DependencyObject obj)
		{
			return (IEnumerable)obj.GetValue(ItemsSourceProperty);
		}
		public static void SetItemsSource(DependencyObject obj, IEnumerable value)
		{
			obj.SetValue(ItemsSourceProperty, value);
		}
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable), typeof(MenuHelper), new PropertyMetadata((o, e) =>
			{
				Window window = o as Window;
				MenuItemsCollection mic = e.NewValue as MenuItemsCollection;
				if (window != null)
				{
					if(e.OldValue == null && mic != null && mic.AutoRegisterKeyBindings)
					{
						List<CommandGesturePair> cgps = MenuItemsCollection.GetCommandGesturePairs(mic);
						foreach (CommandGesturePair pair in cgps)
						{
							KeyBinding binding = new KeyBinding(pair.Command, pair.Gesture);
							BindingOperations.SetBinding(binding, KeyBinding.CommandParameterProperty, new Binding { Source = pair.MenuItem, Path = new PropertyPath(vmMenuItem.CommandParameterProperty), Mode = BindingMode.OneWay });
							window.InputBindings.Add(binding);
						}
					}
					if(e.OldValue != null)
					{

					}
				}
				//TODO
			}));

		public static HierarchicalDataTemplate GetHierarchicalTemplate(DependencyObject obj)
		{
			return (HierarchicalDataTemplate)obj.GetValue(HierarchicalTemplateProperty);
		}
		public static void SetHierarchicalTemplate(DependencyObject obj, HierarchicalDataTemplate value)
		{
			obj.SetValue(HierarchicalTemplateProperty, value);
		}
		public static readonly DependencyProperty HierarchicalTemplateProperty =
			DependencyProperty.RegisterAttached("HierarchicalTemplate", typeof(HierarchicalDataTemplate), typeof(MenuHelper), new PropertyMetadata());
	}
}
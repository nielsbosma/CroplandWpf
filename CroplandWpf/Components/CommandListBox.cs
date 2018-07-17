using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CroplandWpf.Attached;

namespace CroplandWpf.Components
{
	public class CommandListBox : ListBox
	{
		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandListBox), new PropertyMetadata());

		public ListBoxItem ClickedListBoxItem
		{
			get { return (ListBoxItem)GetValue(ClickedListBoxItemProperty); }
			set { SetValue(ClickedListBoxItemProperty, value); }
		}
		public static readonly DependencyProperty ClickedListBoxItemProperty =
			DependencyProperty.Register("ClickedListBoxItem", typeof(ListBoxItem), typeof(CommandListBox), new PropertyMetadata());

		public bool ClearSelectionOnCommandExecute
		{
			get { return (bool)GetValue(ClearSelectionOnCommandExecuteProperty); }
			set { SetValue(ClearSelectionOnCommandExecuteProperty, value); }
		}
		public static readonly DependencyProperty ClearSelectionOnCommandExecuteProperty =
			DependencyProperty.Register("ClearSelectionOnCommandExecute", typeof(bool), typeof(CommandListBox), new PropertyMetadata());

		public CommandListBox()
		{
			Loaded += CommandListBox_Loaded;
			Unloaded += CommandListBox_Unloaded;
		}

		private void CommandListBox_Loaded(object sender, RoutedEventArgs e)
		{
			AddHandler(ListBoxItem.PreviewMouseDownEvent, new MouseButtonEventHandler(ChildItem_Clicked));
		}

		private void CommandListBox_Unloaded(object sender, RoutedEventArgs e)
		{
			RemoveHandler(ListBoxItem.PreviewMouseDownEvent, new MouseButtonEventHandler(ChildItem_Clicked));
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == ClickedListBoxItemProperty && e.NewValue != null)
			{
				ExecuteCommand(ClickedListBoxItem);
				BindingOperations.ClearBinding(this, ClickedListBoxItemProperty);
			}
		}

		private void ChildItem_Clicked(object item, MouseButtonEventArgs e)
		{
			if (Command != null)
			{
				ListBoxItem clickedItem = e.OriginalSource as ListBoxItem;
				if (clickedItem == null)
				{
					FrameworkElement clickedFE = e.OriginalSource as FrameworkElement;
					if (clickedFE != null)
					{
						if (ItemsControlHelper.GetDataItemContainer(clickedFE) != null)
						{
							ExecuteCommand(ItemsControlHelper.GetDataItemContainer(clickedFE) as ListBoxItem);
							return;
						}

						clickedFE.SetBinding(ItemsControlHelper.DataItemContainerProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListBoxItem), 1) });
						SetBinding(ClickedListBoxItemProperty, new Binding { Source = clickedFE, Path = new PropertyPath(ItemsControlHelper.DataItemContainerProperty) });
					}
				}
				else
					ExecuteCommand(e.OriginalSource as ListBoxItem);
			}
		}

		private void ExecuteCommand(ListBoxItem clickedItem)
		{
			if (Command != null && clickedItem != null)
				Command.Execute(clickedItem.DataContext);
			if (ClearSelectionOnCommandExecute)
				SelectedItem = null;
		}
	}
}

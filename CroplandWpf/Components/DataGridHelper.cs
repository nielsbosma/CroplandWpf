using CroplandWpf.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public class DataGridHelper : FrameworkElement
	{
		private enum ItemMoveDirection
		{
			Up,
			Down
		}

		#region APs
		public static ICommand GetMoveRowUpCommandOverride(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(MoveRowUpCommandOverrideProperty);
		}
		public static void SetMoveRowUpCommandOverride(DependencyObject obj, ICommand value)
		{
			obj.SetValue(MoveRowUpCommandOverrideProperty, value);
		}
		public static readonly DependencyProperty MoveRowUpCommandOverrideProperty =
			DependencyProperty.RegisterAttached("MoveRowUpCommandOverride", typeof(ICommand), typeof(DataGridHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static ICommand GetMoveRowDownCommandOverride(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(MoveRowDownCommandOverrideProperty);
		}
		public static void SetMoveRowDownCommandOverride(DependencyObject obj, ICommand value)
		{
			obj.SetValue(MoveRowDownCommandOverrideProperty, value);
		}
		public static readonly DependencyProperty MoveRowDownCommandOverrideProperty =
			DependencyProperty.RegisterAttached("MoveRowDownCommandOverride", typeof(ICommand), typeof(DataGridHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static ICommand GetRemoveRowCommandOverride(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(RemoveRowCommandOverrideProperty);
		}
		public static void SetRemoveRowCommandOverride(DependencyObject obj, ICommand value)
		{
			obj.SetValue(RemoveRowCommandOverrideProperty, value);
		}
		public static readonly DependencyProperty RemoveRowCommandOverrideProperty =
			DependencyProperty.RegisterAttached("RemoveRowCommandOverride", typeof(ICommand), typeof(DataGridHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static bool GetShowDefaultRemoveConfirmation(DependencyObject obj)
		{
			return (bool)obj.GetValue(ShowDefaultRemoveConfirmationProperty);
		}
		public static void SetShowDefaultRemoveConfirmation(DependencyObject obj, bool value)
		{
			obj.SetValue(ShowDefaultRemoveConfirmationProperty, value);
		}
		public static readonly DependencyProperty ShowDefaultRemoveConfirmationProperty =
			DependencyProperty.RegisterAttached("ShowDefaultRemoveConfirmation", typeof(bool), typeof(DataGridHelper), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.Inherits));

		public static bool GetLastColumnFill(DependencyObject obj)
		{
			return (bool)obj.GetValue(LastColumnFillProperty);
		}
		public static void SetLastColumnFill(DependencyObject obj, bool value)
		{
			obj.SetValue(LastColumnFillProperty, value);
		}
		public static readonly DependencyProperty LastColumnFillProperty =
			DependencyProperty.RegisterAttached("LastColumnFill", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(false));
		#endregion

		#region DPs
		public DataGrid TargetDataGrid
		{
			get { return (DataGrid)GetValue(TargetDataGridProperty); }
			set { SetValue(TargetDataGridProperty, value); }
		}
		public static readonly DependencyProperty TargetDataGridProperty =
			DependencyProperty.Register("TargetDataGrid", typeof(DataGrid), typeof(DataGridHelper), new PropertyMetadata());

		public DataGridRow TargetDataGridRow
		{
			get { return (DataGridRow)GetValue(TargetDataGridRowProperty); }
			set { SetValue(TargetDataGridRowProperty, value); }
		}
		public static readonly DependencyProperty TargetDataGridRowProperty =
			DependencyProperty.Register("TargetDataGridRow", typeof(DataGridRow), typeof(DataGridHelper), new PropertyMetadata());

		public DelegateCommand MoveRowUpCommand
		{
			get { return (DelegateCommand)GetValue(MoveRowUpCommandProperty); }
			private set { SetValue(MoveRowUpCommandProperty, value); }
		}
		public static readonly DependencyProperty MoveRowUpCommandProperty =
			DependencyProperty.Register("MoveRowUpCommand", typeof(DelegateCommand), typeof(DataGridHelper), new PropertyMetadata());

		public DelegateCommand MoveRowDownCommand
		{
			get { return (DelegateCommand)GetValue(MoveRowDownCommandProperty); }
			private set { SetValue(MoveRowDownCommandProperty, value); }
		}
		public static readonly DependencyProperty MoveRowDownCommandProperty =
			DependencyProperty.Register("MoveRowDownCommand", typeof(DelegateCommand), typeof(DataGridHelper), new PropertyMetadata());

		public DelegateCommand RemoveRowCommand
		{
			get { return (DelegateCommand)GetValue(RemoveRowCommandProperty); }
			private set { SetValue(RemoveRowCommandProperty, value); }
		}
		public static readonly DependencyProperty RemoveRowCommandProperty =
			DependencyProperty.Register("RemoveRowCommand", typeof(DelegateCommand), typeof(DataGridHelper), new PropertyMetadata());
		#endregion

		#region Properties
		private bool HasMoveDownCommandOverride
		{
			get
			{
				return TargetDataGrid != null && GetMoveRowUpCommandOverride(TargetDataGrid) != null;
			}
		}

		private bool HasMoveUpCommandOverride
		{
			get
			{
				return TargetDataGrid != null && GetMoveRowDownCommandOverride(TargetDataGrid) != null;
			}
		}

		private bool HasRemoveCommandOverride
		{
			get
			{
				return TargetDataGrid != null && GetRemoveRowCommandOverride(TargetDataGrid) != null;
			}
		}

		private IList targetItemsSourceAsList
		{
			get
			{
				if (TargetDataGrid == null)
					return null;
				return TargetDataGrid.ItemsSource as IList;
			}
		}
		#endregion

		public DataGridHelper()
		{
			MoveRowUpCommand = new DelegateCommand(MoveRowUpCommand_Execute, MoveRowUpCommand_CanExecute);
			MoveRowDownCommand = new DelegateCommand(MoveRowDownCommand_Execute, MoveRowDownCommand_CanExecute);
			RemoveRowCommand = new DelegateCommand(RemoveRowCommand_Execute, RemoveRowCommand_CanExecute);
			SetBinding(TargetDataGridProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGrid), 1) });
			SetBinding(TargetDataGridRowProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridRow), 1) });
		}

		#region Commanding
		private bool MoveRowUpCommand_CanExecute(object item)
		{
			return TargetDataGrid != null && GetMoveRowUpCommandOverride(TargetDataGrid) != null && GetMoveRowUpCommandOverride(TargetDataGrid).CanExecute(DataContext);
		}

		private void MoveRowUpCommand_Execute(object item)
		{
			if (TargetDataGrid == null)
				return;
			if (HasMoveDownCommandOverride)
			{
				GetMoveRowUpCommandOverride(TargetDataGrid).Execute(DataContext);
				if (TargetDataGridRow != null)
					TargetDataGridRow.BringIntoView();
			}
			else if (targetItemsSourceAsList != null)
			{
				object itemBackup = DataContext;
				int currentIndex = targetItemsSourceAsList.IndexOf(itemBackup);
				targetItemsSourceAsList.RemoveAt(currentIndex);
				targetItemsSourceAsList.Insert(currentIndex - 1, itemBackup);
			}
		}

		private bool MoveRowDownCommand_CanExecute(object item)
		{
			if (HasMoveDownCommandOverride)
				return GetMoveRowDownCommandOverride(TargetDataGrid).CanExecute(DataContext);
			else if (targetItemsSourceAsList != null)
			{
				int currentIndex = targetItemsSourceAsList.IndexOf(DataContext);
				return currentIndex < targetItemsSourceAsList.Count - 1;
			}
			return false;
		}

		private void MoveRowDownCommand_Execute(object item)
		{
			if (TargetDataGrid == null)
				return;
			if (GetMoveRowDownCommandOverride(TargetDataGrid) != null)
			{
				GetMoveRowDownCommandOverride(TargetDataGrid).Execute(DataContext);
				if (TargetDataGridRow != null)
					TargetDataGridRow.BringIntoView();
			}
			else if (targetItemsSourceAsList != null)
			{
				int currentIndex = targetItemsSourceAsList.IndexOf(DataContext);
				object currentItem = DataContext;
				targetItemsSourceAsList.RemoveAt(currentIndex);
				targetItemsSourceAsList.Insert(currentIndex + 1, currentItem);
			}
		}

		private bool RemoveRowCommand_CanExecute(object arg)
		{
			if (HasRemoveCommandOverride)
				return GetRemoveRowCommandOverride(TargetDataGrid).CanExecute(DataContext);
			else
				return targetItemsSourceAsList != null && targetItemsSourceAsList.Contains(DataContext);
		}

		private void RemoveRowCommand_Execute(object obj)
		{
			if (GetShowDefaultRemoveConfirmation(TargetDataGrid))
			{
				MessageBoxResult result = MessageBoxService.Show(new MessageBoxInfo { Header = "Remove Confirmation", Content = String.Format("Remove {0}?", DataContext), Buttons = MessageBoxButton.YesNo });
				if (result == MessageBoxResult.No || result == MessageBoxResult.None)
					return;
			}
			if (HasRemoveCommandOverride)
				GetRemoveRowCommandOverride(TargetDataGrid).Execute(DataContext);
			else if (targetItemsSourceAsList != null)
				targetItemsSourceAsList.Remove(DataContext);
		}
		#endregion

		#region Private methods
		private int GetItemIndex(object item)
		{
			IList c = TargetDataGrid.ItemsSource as IList;
			if (c != null)
				return c.IndexOf(item);
			else
				return -1;
		}
		#endregion
	}
}
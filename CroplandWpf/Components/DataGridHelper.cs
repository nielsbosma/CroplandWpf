﻿using CroplandWpf.MVVM;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace CroplandWpf.Components
{
    public class DataGridHelper : FrameworkElement
    {
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

        public static bool GetShowRowControls(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowRowControlsProperty);
        }
        public static void SetShowRowControls(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowRowControlsProperty, value);
        }
        public static readonly DependencyProperty ShowRowControlsProperty =
            DependencyProperty.RegisterAttached("ShowRowControls", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(true));

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

        public static bool GetDenySelection(DependencyObject obj)
        {
            return (bool)obj.GetValue(DenySelectionProperty);
        }
        public static void SetDenySelection(DependencyObject obj, bool value)
        {
            obj.SetValue(DenySelectionProperty, value);
        }
        public static readonly DependencyProperty DenySelectionProperty =
            DependencyProperty.RegisterAttached("DenySelection", typeof(bool), typeof(DataGridHelper), new PropertyMetadata());

        public static bool GetScrollBarExpandedMode(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollBarExpandedModeProperty);
        }
        public static void SetScrollBarExpandedMode(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollBarExpandedModeProperty, value);
        }
        public static readonly DependencyProperty ScrollBarExpandedModeProperty =
            DependencyProperty.RegisterAttached("ScrollBarExpandedMode", typeof(bool), typeof(DataGridHelper), new PropertyMetadata(false));
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
            SetBinding(DenySelectionProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGrid), 1), Path = new PropertyPath(DenySelectionProperty), Mode = BindingMode.OneWay });
            Loaded += DataGridHelper_Loaded;
            Unloaded += DataGridHelper_Unloaded;
        }

        #region Event hadlers
        private void DataGridHelper_Loaded(object sender, RoutedEventArgs e)
        {
            if (TargetDataGridRow != null)
                TargetDataGridRow.PreviewMouseDown += TargetDataGridRow_PreviewMouseDown;
        }

        private void DataGridHelper_Unloaded(object sender, RoutedEventArgs e)
        {
            if (TargetDataGridRow != null)
                TargetDataGridRow.PreviewMouseDown -= TargetDataGridRow_PreviewMouseDown;
        }

        private void TargetDataGridRow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (GetDenySelection(this))
            {
                if (Mouse.DirectlyOver == TargetDataGridRow)
                    e.Handled = true;
            }
        }
        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            return;
            if (e.Property == DenySelectionProperty)
            {
                if ((bool)e.NewValue)
                    DisableSelection();
                else
                    EnableSelection();
            }
            if (e.Property == TargetDataGridRowProperty && e.NewValue != null && GetDenySelection(this))
            {
                DisableSelection();
            }
        }

        private void DisableSelection()
        {
            if (TargetDataGrid != null)
            {
                KeyboardNavigation.SetTabNavigation(TargetDataGrid, KeyboardNavigationMode.None);
                KeyboardNavigation.SetDirectionalNavigation(TargetDataGrid, KeyboardNavigationMode.None);
                TargetDataGrid.SelectedIndex = -1;
                TargetDataGrid.Focusable = false;
            }
            if (TargetDataGridRow != null)
            {
                //TargetDataGridRow.IsHitTestVisible = false;
                TargetDataGridRow.Focusable = false;
            }
        }

        private void EnableSelection()
        {
            if (TargetDataGrid != null)
            {
                TargetDataGrid.Focusable = true;
                KeyboardNavigation.SetTabNavigation(TargetDataGrid, KeyboardNavigationMode.Continue);
            }
            if (TargetDataGridRow != null)
            {
                //TargetDataGridRow.IsHitTestVisible = true;
                TargetDataGridRow.Focusable = true;
            }
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
                MessageBoxButton result = MessageBoxService.Show(new MessageBoxInfo { Title = "Remove Confirmation", Content = string.Format("Remove {0}?", DataContext), Buttons = MessageBoxButtons.YesNo });
                if (result == MessageBoxButton.No || result == MessageBoxButton.Close)
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

    public class DataGridColumnHelper : FrameworkElement
    {
        public DataGrid TargetDataGrid
        {
            get { return (DataGrid)GetValue(TargetDataGridProperty); }
            set { SetValue(TargetDataGridProperty, value); }
        }
        public static readonly DependencyProperty TargetDataGridProperty =
            DependencyProperty.Register("TargetDataGrid", typeof(DataGrid), typeof(DataGridColumnHelper), new PropertyMetadata());

        public IEnumerable TargetItemsSource
        {
            get { return (IEnumerable)GetValue(TargetItemsSourceProperty); }
            set { SetValue(TargetItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty TargetItemsSourceProperty =
            DependencyProperty.Register("TargetItemsSource", typeof(IEnumerable), typeof(DataGridColumnHeader), new PropertyMetadata());

        private bool columnsChangedSubscribed = false;
        private DataGridColumn lastColumn;
        private DataGridLength lastColumnWidthBackup;

        public DataGridColumnHelper()
        {
            IsEnabled = false;
            IsHitTestVisible = false;
            Visibility = Visibility.Collapsed;
            Loaded += DataGridColumnHelper_Loaded;
            Unloaded += DataGridColumnHelper_Unloaded;
        }

        private void DataGridColumnHelper_Unloaded(object sender, RoutedEventArgs e)
        {
            UnsubscribeColumnsChanged();
        }

        private void DataGridColumnHelper_Loaded(object sender, RoutedEventArgs e)
        {
            if (TargetDataGrid == null)
                SetBinding(TargetDataGridProperty, new Binding { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGrid), 1) });
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == TargetDataGridProperty)
            {
                if (e.NewValue != null)
                {
                    AcquireLastColumn();
                    SetBinding(IsEnabledProperty, new Binding { Source = TargetDataGrid, Path = new PropertyPath(DataGridHelper.LastColumnFillProperty), Mode = BindingMode.OneWay });
                }
                else
                    UnsubscribeColumnsChanged();
            }
            if (e.Property == IsEnabledProperty)
            {
                if ((bool)e.NewValue)
                    AcquireLastColumn();
                else
                    RestoreLastColumn();
            }
        }

        private void SubscribeColumnsChanged()
        {
            if (!columnsChangedSubscribed && TargetDataGrid != null)
            {
                TargetDataGrid.Columns.CollectionChanged += Columns_CollectionChanged;
                columnsChangedSubscribed = true;
            }
        }

        private void UnsubscribeColumnsChanged()
        {
            if (columnsChangedSubscribed && TargetDataGrid != null)
            {
                TargetDataGrid.Columns.CollectionChanged -= Columns_CollectionChanged;
                columnsChangedSubscribed = false;
            }
        }

        private void RestoreLastColumn()
        {
            if (lastColumn != null)
            {
                lastColumn.Width = lastColumnWidthBackup;
                lastColumn = null;
                lastColumnWidthBackup = default(DataGridLength);
            }
        }

        private void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!IsEnabled)
                return;
            if (lastColumn != null)
            {
                RestoreLastColumn();
                AcquireLastColumn();
            }
        }

        private void AcquireLastColumn()
        {
            if (!IsEnabled)
                return;
            lastColumn = TargetDataGrid.Columns.LastOrDefault();
            if (lastColumn != null)
            {
                lastColumnWidthBackup = lastColumn.Width;
                lastColumn.Width = new DataGridLength(100.0, DataGridLengthUnitType.Star);
            }
        }
    }
}
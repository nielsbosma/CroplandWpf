using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using CroplandWpf.Exceptions;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Data;
using CroplandWpf.Attached;

namespace CroplandWpf.Components
{
	public class SearchAutocompleteControl : ItemsControl
	{
		#region Dps
		public int SearchRefreshTimeout
		{
			get { return (int)GetValue(SearchRefreshTimeoutProperty); }
			set { SetValue(SearchRefreshTimeoutProperty, value); }
		}
		public static readonly DependencyProperty SearchRefreshTimeoutProperty =
			DependencyProperty.Register("SearchRefreshTimeout", typeof(int), typeof(SearchAutocompleteControl), new PropertyMetadata(500));

		public string SearchString
		{
			get { return (string)GetValue(SearchStringProperty); }
			set { SetValue(SearchStringProperty, value); }
		}
		public static readonly DependencyProperty SearchStringProperty =
			DependencyProperty.Register("SearchString", typeof(string), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public bool IsIconVisible
		{
			get { return (bool)GetValue(IsIconVisibleProperty); }
			private set { SetValue(IsIconVisibleProperty, value); }
		}
		public static readonly DependencyProperty IsIconVisibleProperty =
			DependencyProperty.Register("IsIconVisible", typeof(bool), typeof(SearchAutocompleteControl), new PropertyMetadata(true));

		public bool IsPopupOpen
		{
			get { return (bool)GetValue(IsPopupOpenProperty); }
			private set { SetValue(IsPopupOpenProperty, value); }
		}
		public static readonly DependencyProperty IsPopupOpenProperty =
			DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public SearchAutocmpleteItem HighlightedItem
		{
			get { return (SearchAutocmpleteItem)GetValue(HighlightedItemProperty); }
			private set { SetValue(HighlightedItemProperty, value); }
		}
		public static readonly DependencyProperty HighlightedItemProperty =
			DependencyProperty.Register("HighlightedItem", typeof(SearchAutocmpleteItem), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public string HighlightedSearchString
		{
			get { return (string)GetValue(HighlightedSearchStringProperty); }
			private set { SetValue(HighlightedSearchStringProperty, value); }
		}
		public static readonly DependencyProperty HighlightedSearchStringProperty =
			DependencyProperty.Register("HighlightedSearchString", typeof(string), typeof(SearchAutocompleteControl), new PropertyMetadata());

		#region Commands
		public ICommand SearchResultRefreshCommand
		{
			get { return (ICommand)GetValue(SearchResultRefreshCommandProperty); }
			set { SetValue(SearchResultRefreshCommandProperty, value); }
		}
		public static readonly DependencyProperty SearchResultRefreshCommandProperty =
			DependencyProperty.Register("SearchResultRefreshCommand", typeof(ICommand), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public ICommand SearchItemCommand
		{
			get { return (ICommand)GetValue(SearchItemCommandProperty); }
			set { SetValue(SearchItemCommandProperty, value); }
		}
		public static readonly DependencyProperty SearchItemCommandProperty =
			DependencyProperty.Register("SearchItemCommand", typeof(ICommand), typeof(SearchAutocompleteControl), new PropertyMetadata());
		#endregion
		#endregion

		#region Fields
		private TextBox _editableTextBox;
		private Popup _popup;
		private DispatcherTimer refreshTimer;
		private List<SearchAutocmpleteItem> focusableItems;
		private string searchStringBackup;
		private Action awaitingHighlightAction = null;
		#endregion

		#region Properties
		private string _textInternal
		{
			get { return _editableTextBox.Text; }
		}
		#endregion

		#region Ctor
		static SearchAutocompleteControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchAutocompleteControl), new FrameworkPropertyMetadata(typeof(SearchAutocompleteControl)));
		}

		public SearchAutocompleteControl()
		{
			focusableItems = new List<SearchAutocmpleteItem>();
			Loaded += SearchAutocompleteControl_Loaded;
			Unloaded += SearchAutocompleteControl_Unloaded;
			refreshTimer = new DispatcherTimer(DispatcherPriority.Send) { Interval = TimeSpan.FromMilliseconds(SearchRefreshTimeout) };
		}
		#endregion

		#region Overrides
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == SearchStringProperty)
			{
				IsPopupOpen = !String.IsNullOrEmpty(e.NewValue as string);
				if (IsPopupOpen)
					focusableItems.ForEach(fi => SetBinding(SearchAutocmpleteItem.HighlightStringProperty, new Binding { Source = this, Path = new PropertyPath(SearchStringProperty), Mode = BindingMode.OneWay }));
				IsIconVisible = String.IsNullOrEmpty(SearchString);
			}
			if (e.Property == HighlightedItemProperty)
			{
				if (e.OldValue == null && e.NewValue != null)
				{
					searchStringBackup = _editableTextBox.Text;
					BindingOperations.ClearBinding(_editableTextBox, TextBox.TextProperty);
					focusableItems.ForEach(fi =>
					{
						BindingOperations.ClearBinding(fi, SearchAutocmpleteItem.HighlightStringProperty);
						fi.HighlightString = searchStringBackup;
					});
				}
				if (e.OldValue != null)
					FocusHelper.SetIsFocused(e.OldValue as SearchAutocmpleteItem, false);
				if (e.NewValue != null)
				{
					FocusHelper.SetIsFocused(e.NewValue as SearchAutocmpleteItem, true);
					(e.NewValue as SearchAutocmpleteItem).BringIntoView();
					_editableTextBox.Text = (e.NewValue as SearchAutocmpleteItem).DisplayString;
				}
				if (e.NewValue == null)
				{
					SetBinding(SearchStringProperty, new Binding { Source = _editableTextBox, Path = new PropertyPath(TextBox.TextProperty), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
					focusableItems.ForEach(fi => SetBinding(SearchAutocmpleteItem.HighlightStringProperty, new Binding { Source = this, Path = new PropertyPath(SearchStringProperty), Mode = BindingMode.OneWay }));
					_editableTextBox.Text = searchStringBackup;
					searchStringBackup = null;
				}
			}
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_editableTextBox = Template.FindName("PART_EditableTextBox", this) as TextBox;
			if (_editableTextBox == null)
				throw new TemplatePartNotFoundException("PART_EditableTextBox", GetType());
			SetBinding(SearchStringProperty, new Binding { Source = _editableTextBox, Path = new PropertyPath(TextBox.TextProperty), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

			_popup = Template.FindName("PART_Popup", this) as Popup;
			if (_popup == null)
				throw new TemplatePartNotFoundException("PART_Popup", GetType());
			SetBinding(IsPopupOpenProperty, new Binding { Source = _popup, Path = new PropertyPath(Popup.IsOpenProperty), Mode = BindingMode.TwoWay });
		}
		#endregion

		#region Init/deinit
		private void SearchAutocompleteControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(this))
				return;
			Window.GetWindow(this).PreviewMouseLeftButtonDown += SearchAutocompleteControl_PreviewMouseLeftButtonDown;
			if (_editableTextBox != null)
			{
				_editableTextBox.TextChanged += EditableTextBox_TextChanged;
				_editableTextBox.PreviewKeyDown += _editableTextBox_PreviewKeyDown;
				refreshTimer.Tick += RefreshTimer_Tick;
			}
			AddHandler(SearchAutocmpleteItem.SearchAutocompleteItemClickedEvent, new RoutedEventHandler(OnSearchAutocompleteItemClicked));
		}

		private void SearchAutocompleteControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(this))
				return;
			Window.GetWindow(this).PreviewMouseLeftButtonDown -= SearchAutocompleteControl_PreviewMouseLeftButtonDown;
			if (_editableTextBox != null)
			{
				_editableTextBox.TextChanged -= EditableTextBox_TextChanged;
				_editableTextBox.PreviewKeyDown -= _editableTextBox_PreviewKeyDown;
				refreshTimer.Tick += RefreshTimer_Tick;
			}
		}
		#endregion

		private void _editableTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
				Unfocus();
			if (e.Key == Key.Down)
				HighlightNextItem();
			if (e.Key == Key.Up)
				HighlightPreviousItem();
			if (e.Key == Key.Enter)
			{
				if (searchStringBackup != null)
				{
					if (SearchItemCommand.CanExecute(_editableTextBox.Text))
						SearchItemCommand.Execute(_editableTextBox.Text);
					Unfocus();
				}
			}
		}

		private void HighlightNextItem()
		{
			if (refreshTimer.IsEnabled)
			{
				awaitingHighlightAction = HighlightNextItem;
				return;
			}
			if (!IsPopupOpen || focusableItems.Count == 0)
				return;
			blockRefresh = true;
			SearchAutocmpleteItem hItem = focusableItems.FirstOrDefault(sai => FocusHelper.GetIsFocused(sai));
			if (hItem == null)
				HighlightedItem = focusableItems.First();
			else if (focusableItems.Last() == hItem)
			{
				HighlightedItem = null;
				//_editableTextBox.Text = searchStringBackup;
			}
			else
				HighlightedItem = focusableItems[focusableItems.IndexOf(hItem) + 1];
			if (HighlightedItem != null)
				HighlightedSearchString = HighlightedItem.DisplayString;
			_editableTextBox.CaretIndex = _editableTextBox.Text.Length;
			blockRefresh = false;
		}

		private void HighlightPreviousItem()
		{
			if(refreshTimer.IsEnabled)
			{
				awaitingHighlightAction = HighlightPreviousItem;
				return;
			}
			if (!IsPopupOpen || focusableItems.Count == 0)
				return;
			blockRefresh = true;
			SearchAutocmpleteItem hItem = focusableItems.FirstOrDefault(sai => FocusHelper.GetIsFocused(sai));
			if (hItem == null)
				HighlightedItem = focusableItems.Last();
			else if (focusableItems.First() == hItem)
				HighlightedItem = null;
			else
				HighlightedItem = focusableItems[focusableItems.IndexOf(hItem) - 1];
			if (HighlightedItem != null)
				HighlightedSearchString = HighlightedItem.DisplayString;
			_editableTextBox.CaretIndex = _editableTextBox.Text.Length;
			blockRefresh = false;
		}

		private void SearchAutocompleteControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!IsMouseOver && _popup.IsOpen)
				Unfocus();
		}

		private void OnSearchAutocompleteItemClicked(object sender, RoutedEventArgs e)
		{
			Unfocus();
		}

		private bool blockRefresh = false;

		private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			refreshTimer.Stop();
			if (!blockRefresh)
			{
				refreshTimer.Start();
			}
		}

		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			refreshTimer.Stop();
			if (blockRefresh)
				return;
			if (SearchResultRefreshCommand != null)
				SearchResultRefreshCommand.Execute(_editableTextBox.Text);
			if (awaitingHighlightAction != null)
			{
				awaitingHighlightAction.Invoke();
				awaitingHighlightAction = null;
			}
		}

		private void Unfocus()
		{
			focusableItems.Clear();
			HighlightedItem = null;
			IsPopupOpen = false;
			//SearchString = null;
			Keyboard.ClearFocus();
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new SearchAutocmpleteItem();
		}

		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			SearchAutocmpleteItem itemContainer = element as SearchAutocmpleteItem;
			if (itemContainer != null)
			{
				FocusHelper.SetIsFocusable(itemContainer, false);
				FocusHelper.SetIsFocused(itemContainer, false);
				if (focusableItems.Contains(itemContainer))
					focusableItems.Remove(itemContainer);
			}
			base.ClearContainerForItemOverride(element, item);
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			SearchAutocmpleteItem saItem = element as SearchAutocmpleteItem;
			saItem.Content = item;
			saItem.SetBinding(SearchAutocmpleteItem.HighlightStringProperty, new Binding { Source = this, Path = new PropertyPath(SearchStringProperty), Mode = BindingMode.OneWay });
			saItem.SetBinding(SearchAutocmpleteItem.CommandProperty, new Binding { Source = this, Path = new PropertyPath(SearchItemCommandProperty), Mode = BindingMode.OneWay });
			FocusHelper.SetIsFocusable(saItem, true);
			focusableItems.Add(saItem);
			base.PrepareContainerForItemOverride(element, item);
		}
	}
}
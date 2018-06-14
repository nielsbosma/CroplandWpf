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
using CroplandWpf.Helpers;

namespace CroplandWpf.Components
{
	public class SearchAutocompleteControl : ItemsControl
	{
		#region Dps
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

		public SearchAutocmpleteItem SelectedItem
		{
			get { return (SearchAutocmpleteItem)GetValue(SelectedItemProperty); }
			private set { SetValue(SelectedItemProperty, value); }
		}
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(SearchAutocmpleteItem), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public bool HasNoMatchesButtonCommand
		{
			get { return (bool)GetValue(HasNoMatchesButtonCommandProperty); }
			private set { SetValue(HasNoMatchesButtonCommandProperty, value); }
		}
		public static readonly DependencyProperty HasNoMatchesButtonCommandProperty =
			DependencyProperty.Register("HasNoMatchesButtonCommand", typeof(bool), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public bool AutoClear
		{
			get { return (bool)GetValue(AutoClearProperty); }
			set { SetValue(AutoClearProperty, value); }
		}
		public static readonly DependencyProperty AutoClearProperty =
			DependencyProperty.Register("AutoClear", typeof(bool), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public string NoMatchesFoundButtonText
		{
			get { return (string)GetValue(NoMatchesFoundButtonTextProperty); }
			set { SetValue(NoMatchesFoundButtonTextProperty, value); }
		}
		public static readonly DependencyProperty NoMatchesFoundButtonTextProperty =
			DependencyProperty.Register("NoMatchesFoundButtonText", typeof(string), typeof(SearchAutocompleteControl), new PropertyMetadata("[no matches found]"));

		public string SeeAllOptionsButtonText
		{
			get { return (string)GetValue(SeeAllOptionsButtonTextProperty); }
			set { SetValue(SeeAllOptionsButtonTextProperty, value); }
		}
		public static readonly DependencyProperty SeeAllOptionsButtonTextProperty =
			DependencyProperty.Register("SeeAllOptionsButtonText", typeof(string), typeof(SearchAutocompleteControl), new PropertyMetadata("See All Options"));

		public bool AutoFocusOnLoad
		{
			get { return (bool)GetValue(AutoFocusOnLoadProperty); }
			set { SetValue(AutoFocusOnLoadProperty, value); }
		}
		public static readonly DependencyProperty AutoFocusOnLoadProperty =
			DependencyProperty.Register("AutoFocusOnLoad", typeof(bool), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public string SearchPropertyBindingPath
		{
			get { return (string)GetValue(SearchPropertyBindingPathProperty); }
			set { SetValue(SearchPropertyBindingPathProperty, value); }
		}
		public static readonly DependencyProperty SearchPropertyBindingPathProperty =
			DependencyProperty.Register("SearchPropertyBindingPath", typeof(string), typeof(SearchAutocompleteControl), new PropertyMetadata());

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

		public ICommand NoMatchesButtonCommand
		{
			get { return (ICommand)GetValue(NoMatchesButtonCommandProperty); }
			set { SetValue(NoMatchesButtonCommandProperty, value); }
		}
		public static readonly DependencyProperty NoMatchesButtonCommandProperty =
			DependencyProperty.Register("NoMatchesButtonCommand", typeof(ICommand), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public ICommand SeeAllOptionsCommand
		{
			get { return (ICommand)GetValue(SeeAllOptionsCommandProperty); }
			set { SetValue(SeeAllOptionsCommandProperty, value); }
		}
		public static readonly DependencyProperty SeeAllOptionsCommandProperty =
			DependencyProperty.Register("SeeAllOptionsCommand", typeof(ICommand), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public DelegateCommand PopupButtonCommandInternal
		{
			get { return (DelegateCommand)GetValue(PopupButtonCommandInternalProperty); }
			set { SetValue(PopupButtonCommandInternalProperty, value); }
		}
		public static readonly DependencyProperty PopupButtonCommandInternalProperty =
			DependencyProperty.Register("PopupButtonCommandInternal", typeof(DelegateCommand), typeof(SearchAutocompleteControl), new PropertyMetadata());
		#endregion
		#endregion

		#region Fields
		private TextBox _editableTextBox;
		private Popup _popup;
		private List<SearchAutocmpleteItem> focusableItems;
		private bool refreshHighlightString = false;
		private Window ownerWindow;
		private bool textBoxAcquired
		{
			get
			{
				return _textBoxAcquired;
			}
			set
			{
				if (_textBoxAcquired == value)
					return;
				_textBoxAcquired = value;
				if (value && IsLoaded && !_isLoaded)
					OnLoad();
			}
		}
		private bool _textBoxAcquired;
		private bool _isLoaded = false;
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
			PopupButtonCommandInternal = new DelegateCommand(PopupButtonCommandInternal_Execute);
		}
		#endregion

		#region Overrides
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == SearchStringProperty)
			{
				IsPopupOpen = !String.IsNullOrEmpty(e.NewValue as string);
			}
			if (e.Property == SelectedItemProperty)
			{
				if (e.OldValue != null)
					(e.OldValue as SearchAutocmpleteItem).IsSelected = false;
				if (e.NewValue != null)
					(e.NewValue as SearchAutocmpleteItem).IsSelected = true;
				if (e.OldValue == null && e.NewValue != null)
					SearchString = _editableTextBox.Text;
			}
			if (e.Property == NoMatchesButtonCommandProperty)
				HasNoMatchesButtonCommand = e.NewValue != null;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_editableTextBox = Template.FindName("PART_EditableTextBox", this) as TextBox;
			if (_editableTextBox == null)
				throw new TemplatePartNotFoundException("PART_EditableTextBox", GetType());
			_popup = Template.FindName("PART_Popup", this) as Popup;
			if (_popup == null)
				throw new TemplatePartNotFoundException("PART_Popup", GetType());
			SetBinding(IsPopupOpenProperty, new Binding { Source = _popup, Path = new PropertyPath(Popup.IsOpenProperty), Mode = BindingMode.TwoWay });
			textBoxAcquired = true;
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
				itemContainer.IsSelected = false;
				if (focusableItems.Contains(itemContainer))
					focusableItems.Remove(itemContainer);
			}
			base.ClearContainerForItemOverride(element, item);
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			SearchAutocmpleteItem saItem = element as SearchAutocmpleteItem;
			saItem.DataContext = item;
			saItem.BindingPath = SearchPropertyBindingPath;
			saItem.SetBinding(SearchAutocmpleteItem.CommandProperty, new Binding { Source = this, Path = new PropertyPath(SearchItemCommandProperty), Mode = BindingMode.OneWay });
			focusableItems.Add(saItem);
			saItem.HighlightString = SearchString;
			base.PrepareContainerForItemOverride(element, item);
		}
		#endregion

		#region Init/deinit
		private void SearchAutocompleteControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(this) || !textBoxAcquired)
				return;
			OnLoad();
		}

		private void OnLoad()
		{
			if (!IsLoaded || _isLoaded)
				return;
			if (_editableTextBox != null)
			{
				_editableTextBox.TextChanged += EditableTextBox_TextChanged;
				_editableTextBox.PreviewKeyDown += EditableTextBox_PreviewKeyDown;
				_editableTextBox.PreviewTextInput += EditableTextBox_PreviewTextInput;
			}
			AddHandler(SearchAutocmpleteItem.ClickedEvent, new RoutedEventHandler(OnSearchAutocompleteItemClicked));
			AddHandler(SearchAutocmpleteItem.SelectedEvent, new RoutedEventHandler(OnSearchAutocompleteItemFocused));
			ownerWindow = Window.GetWindow(this);
			ownerWindow.Deactivated += OwnerWindow_Deactivated;
			_isLoaded = true;
			WindowHelper.RegisterHandler(this, Window.PreviewMouseDownEvent, WindowMouseDownHandler);
		}

		private void SearchAutocompleteControl_Unloaded(object sender, RoutedEventArgs e)
		{
			if (DesignerProperties.GetIsInDesignMode(this))
				return;
			if (_editableTextBox != null)
			{
				_editableTextBox.TextChanged -= EditableTextBox_TextChanged;
				_editableTextBox.PreviewKeyDown -= EditableTextBox_PreviewKeyDown;
				_editableTextBox.PreviewTextInput -= EditableTextBox_PreviewTextInput;
			}
			RemoveHandler(SearchAutocmpleteItem.ClickedEvent, new RoutedEventHandler(OnSearchAutocompleteItemClicked));
			RemoveHandler(SearchAutocmpleteItem.SelectedEvent, new RoutedEventHandler(OnSearchAutocompleteItemFocused));
			if (ownerWindow != null)
			{
				ownerWindow.Deactivated -= OwnerWindow_Deactivated;
				ownerWindow = null;
			}
			if(AutoClear)
			{
				_editableTextBox.Text = "";
			}
			WindowHelper.UnregisterHandler(this, Window.PreviewMouseDownEvent);
		}

		private void WindowMouseDownHandler(object sender, RoutedEventArgs e)
		{
			if (!IsMouseOver && !_popup.IsMouseOver && _popup.IsOpen)
			{
				Unfocus(true);
				Keyboard.ClearFocus();
			}
		}
		#endregion

		private void EditableTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Space && String.IsNullOrWhiteSpace(_editableTextBox.Text))
				e.Handled = true;
			if (e.Key == Key.Delete || e.Key == Key.Back)
				refreshHighlightString = true;
			if (e.Key == Key.Escape)
				Unfocus(true);
			if (e.Key == Key.Down)
				HighlightNextItem();
			if (e.Key == Key.Up)
				HighlightPreviousItem();
			if (e.Key == Key.Enter)
				SearchCompleted();
		}

		private void EditableTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			refreshHighlightString = true;
			if (String.IsNullOrEmpty(_editableTextBox.Text) && e.Text == " ")
				e.Handled = true;
		}

		private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (refreshHighlightString)
			{
				SelectedItem = null;
				SearchResultRefreshCommand.Execute(_editableTextBox.Text.Trim());
				refreshHighlightString = false;
				focusableItems.ForEach(sai => sai.HighlightString = _editableTextBox.Text);
				SearchString = _editableTextBox.Text;
			}
		}

		private void OwnerWindow_Deactivated(object sender, EventArgs e)
		{
			if (IsPopupOpen)
				Unfocus(true);
		}

		private void HighlightNextItem()
		{
			if (!IsPopupOpen || focusableItems.Count == 0)
				return;

			if (SelectedItem == null)
				SelectItem(focusableItems.First());
			else if (focusableItems.Last() == SelectedItem)
				SelectItem(null);
			else
				SelectItem(focusableItems[focusableItems.IndexOf(SelectedItem) + 1]);
		}

		private void HighlightPreviousItem()
		{
			if (!IsPopupOpen || focusableItems.Count == 0)
				return;
			if (SelectedItem == null)
				SelectItem(focusableItems.Last());
			else if (focusableItems.First() == SelectedItem)
				SelectItem(null);
			else
				SelectItem(focusableItems[focusableItems.IndexOf(SelectedItem) - 1]);
		}

		private void SelectItem(SearchAutocmpleteItem item)
		{
			if (item == null)
			{
				refreshHighlightString = false;
				SelectedItem = null;
				_editableTextBox.Text = SearchString;
				refreshHighlightString = true;
			}
			else
			{
				SelectedItem = item;
				refreshHighlightString = false;
				_editableTextBox.Text = SelectedItem.DisplayString;
				refreshHighlightString = true;
				_editableTextBox.CaretIndex = _editableTextBox.Text.Length;
			}
		}

		private void SearchAutocompleteControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			//if (!IsMouseOver && _popup.IsOpen)
			//	Unfocus();
		}

		private void OnSearchAutocompleteItemClicked(object sender, RoutedEventArgs e)
		{
			SearchAutocmpleteItem clickedItem = e.OriginalSource as SearchAutocmpleteItem;
			if (SearchItemCommand != null)
			{
				object selectedItemData = null;
				if (SelectedItem != null)
				{
					selectedItemData = SelectedItem.DataContext;
					_editableTextBox.Text = SelectedItem.DisplayString;
				}
				SearchItemCommand.Execute(selectedItemData);
			}
			Unfocus(false);
		}

		private void OnSearchAutocompleteItemFocused(object sender, RoutedEventArgs e)
		{
			SelectedItem = e.OriginalSource as SearchAutocmpleteItem;
		}

		private void ClearFocusedItem()
		{
			focusableItems.ForEach(sai => sai.IsSelected = false);
		}

		private void Unfocus(bool clearText = false)
		{
			refreshHighlightString = false;
			focusableItems.Clear();
			if (clearText)
			{
				SelectedItem = null;
				_editableTextBox.Text = "";
				SearchString = "";
				refreshHighlightString = true;
				if (SearchItemCommand != null)
					SearchItemCommand.Execute(null);
			}
			IsPopupOpen = false;
			Keyboard.ClearFocus();
		}

		private void SearchCompleted()
		{
			if (SelectedItem != null && SearchItemCommand != null)
				SearchItemCommand.Execute(SelectedItem.Content);
			Unfocus();
		}

		private void PopupButtonCommandInternal_Execute(object obj)
		{
			if (obj == null)
				return;
			Unfocus(true);
			if (obj.ToString() == "PART_ButtonNoMatches" && NoMatchesButtonCommand != null)
				Dispatcher.BeginInvoke(new Action(() => { NoMatchesButtonCommand.Execute(null); }), DispatcherPriority.Background);
			if (obj.ToString() == "PART_ButtonShowAllOptions" && SeeAllOptionsCommand != null)
				Dispatcher.BeginInvoke(new Action(() => { SeeAllOptionsCommand.Execute(null); }), DispatcherPriority.Background);
		}
	}
}
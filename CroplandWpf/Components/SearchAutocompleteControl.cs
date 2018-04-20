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

		public bool IsPopupOpen
		{
			get { return (bool)GetValue(IsPopupOpenProperty); }
			private set { SetValue(IsPopupOpenProperty, value); }
		}
		public static readonly DependencyProperty IsPopupOpenProperty =
			DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(SearchAutocompleteControl), new PropertyMetadata());

		#region Commands
		public DelegateCommand SearchResultRefreshCommand
		{
			get { return (DelegateCommand)GetValue(SearchResultRefreshCommandProperty); }
			set { SetValue(SearchResultRefreshCommandProperty, value); }
		}
		public static readonly DependencyProperty SearchResultRefreshCommandProperty =
			DependencyProperty.Register("SearchResultRefreshCommand", typeof(DelegateCommand), typeof(SearchAutocompleteControl), new PropertyMetadata());

		public DelegateCommand SearchItemCommand
		{
			get { return (DelegateCommand)GetValue(SearchItemCommandProperty); }
			set { SetValue(SearchItemCommandProperty, value); }
		}
		public static readonly DependencyProperty SearchItemCommandProperty =
			DependencyProperty.Register("SearchItemCommand", typeof(DelegateCommand), typeof(SearchAutocompleteControl), new PropertyMetadata());
		#endregion
		#endregion

		#region Fields
		private TextBox _editableTextBox;
		private Popup _popup;
		private DispatcherTimer refreshTimer;
		#endregion

		#region Properties
		private string _textInternal
		{
			get { return _editableTextBox.Text; }
		}
		#endregion

		static SearchAutocompleteControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchAutocompleteControl), new FrameworkPropertyMetadata(typeof(SearchAutocompleteControl)));
		}

		public SearchAutocompleteControl()
		{
			Loaded += SearchAutocompleteControl_Loaded;
			Unloaded += SearchAutocompleteControl_Unloaded;
			refreshTimer = new DispatcherTimer(DispatcherPriority.Send) { Interval = TimeSpan.FromMilliseconds(SearchRefreshTimeout) };
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == SearchStringProperty)
			{
				IsPopupOpen = !String.IsNullOrEmpty(e.NewValue as string);
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

		private void OnSearchAutocompleteItemClicked(object sender, RoutedEventArgs e)
		{
			Unfocus();
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

		private void _editableTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Escape)
				Unfocus();
		}

		private void SearchAutocompleteControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!IsMouseOver && _popup.IsOpen)
				Unfocus();
		}

		private void EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			refreshTimer.Stop();
			refreshTimer.Start();
		}

		private void RefreshTimer_Tick(object sender, EventArgs e)
		{
			refreshTimer.Stop();
			if (SearchResultRefreshCommand != null)
				SearchResultRefreshCommand.Execute(_editableTextBox.Text);
		}

		private void Unfocus()
		{
			SearchString = null;
			Keyboard.ClearFocus();
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new SearchAutocmpleteItem();
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			SearchAutocmpleteItem saItem = element as SearchAutocmpleteItem;
			saItem.Content = item;
			saItem.SetBinding(SearchAutocmpleteItem.HighlightStringProperty, new Binding { Source = this, Path = new PropertyPath(SearchStringProperty), Mode = BindingMode.OneWay });
			saItem.SetBinding(SearchAutocmpleteItem.CommandProperty, new Binding { Source = this, Path = new PropertyPath(SearchItemCommandProperty), Mode = BindingMode.OneWay });
			base.PrepareContainerForItemOverride(element, item);
		}
	}
}
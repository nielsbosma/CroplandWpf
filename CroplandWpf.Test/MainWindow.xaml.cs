using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CroplandWpf.Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region DPs
		public string HyperlinkTest
		{
			get { return (string)GetValue(HyperlinkTestProperty); }
			set { SetValue(HyperlinkTestProperty, value); }
		}
		public static readonly DependencyProperty HyperlinkTestProperty =
			DependencyProperty.Register("HyperlinkTest", typeof(string), typeof(MainWindow), new PropertyMetadata("http://some_link"));

		#region Collections
		public List<string> TestItemsSource
		{
			get { return (List<string>)GetValue(TestItemsSourceProperty); }
			private set { SetValue(TestItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestItemsSourceProperty =
			DependencyProperty.Register("TestItemsSource", typeof(List<string>), typeof(MainWindow), new PropertyMetadata());

		#region RemovableItemsItemsControl
		public ObservableCollection<string> TestRemovableItemsItemsSource
		{
			get { return (ObservableCollection<string>)GetValue(TestRemovableItemsItemsSourceProperty); }
			private set { SetValue(TestRemovableItemsItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestRemovableItemsItemsSourceProperty =
			DependencyProperty.Register("TestRemovableItemsItemsSource", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region SearchAutocompleteControl
		public ObservableCollection<string> SearchAutocompleteTestSource
		{
			get { return (ObservableCollection<string>)GetValue(SearchAutocompleteTestSourceProperty); }
			private set { SetValue(SearchAutocompleteTestSourceProperty, value); }
		}
		public static readonly DependencyProperty SearchAutocompleteTestSourceProperty =
			DependencyProperty.Register("SearchAutocompleteTestSource", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata());

		public ObservableCollection<string> SearchResults
		{
			get { return (ObservableCollection<string>)GetValue(SearchResultsProperty); }
			private set { SetValue(SearchResultsProperty, value); }
		}
		public static readonly DependencyProperty SearchResultsProperty =
			DependencyProperty.Register("SearchResults", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata());

		public object ClickedSearchItem
		{
			get { return (object)GetValue(ClickedSearchItemProperty); }
			set { SetValue(ClickedSearchItemProperty, value); }
		}
		public static readonly DependencyProperty ClickedSearchItemProperty =
			DependencyProperty.Register("ClickedSearchItem", typeof(object), typeof(MainWindow), new PropertyMetadata());

		public string CommandTextBoxString
		{
			get { return (string)GetValue(CommandTextBoxStringProperty); }
			set { SetValue(CommandTextBoxStringProperty, value); }
		}
		public static readonly DependencyProperty CommandTextBoxStringProperty =
			DependencyProperty.Register("CommandTextBoxString", typeof(string), typeof(MainWindow), new PropertyMetadata());
		#endregion
		#endregion

		#region Commands
		public DelegateCommand HyperlinkTestCommand
		{
			get { return (DelegateCommand)GetValue(HyperlinkTestCommandProperty); }
			private set { SetValue(HyperlinkTestCommandProperty, value); }
		}
		public static readonly DependencyProperty HyperlinkTestCommandProperty =
			DependencyProperty.Register("HyperlinkTestCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand RemoveRequestTestCommand
		{
			get { return (DelegateCommand)GetValue(RemoveRequestTestCommandProperty); }
			private set { SetValue(RemoveRequestTestCommandProperty, value); }
		}
		public static readonly DependencyProperty RemoveRequestTestCommandProperty =
			DependencyProperty.Register("RemoveRequestTestCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand AddNewFileTestCommand
		{
			get { return (DelegateCommand)GetValue(AddNewFileTestCommandProperty); }
			private set { SetValue(AddNewFileTestCommandProperty, value); }
		}
		public static readonly DependencyProperty AddNewFileTestCommandProperty =
			DependencyProperty.Register("AddNewFileTestCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ConversionSearchResultRefreshCommand
		{
			get { return (DelegateCommand)GetValue(ConversionSearchResultRefreshCommandProperty); }
			private set { SetValue(ConversionSearchResultRefreshCommandProperty, value); }
		}
		public static readonly DependencyProperty ConversionSearchResultRefreshCommandProperty =
			DependencyProperty.Register("ConversionSearchResultRefreshCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand SearchItemClickCommand
		{
			get { return (DelegateCommand)GetValue(SearchItemClickCommandProperty); }
			set { SetValue(SearchItemClickCommandProperty, value); }
		}
		public static readonly DependencyProperty SearchItemClickCommandProperty =
			DependencyProperty.Register("SearchItemClickCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand CommandTextBoxCommand
		{
			get { return (DelegateCommand)GetValue(CommandTextBoxCommandProperty); }
			private set { SetValue(CommandTextBoxCommandProperty, value); }
		}
		public static readonly DependencyProperty CommandTextBoxCommandProperty =
			DependencyProperty.Register("CommandTextBoxCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion
		#endregion

		private Random random = new Random();

		public MainWindow()
		{
			InitializeComponent();

			TestItemsSource = new List<string>
			{
				"Item 1", "Item 2",
				"Some item with looong looooooong text 3",
				"Item 4", "Item 5", "Item 6", "Item 7",
				"Another list box item with even more loooooooooooooooooooong text 8",
				"Item 9", "Item 10", "Item 11", "Item 12"
			};

			TestRemovableItemsItemsSource = new ObservableCollection<string>
			{
				"File1.xslx",
				"File2.xslx",
				"File3.xslx",
				"File4.xslx",
				"File5.xslx",
			};

			SearchAutocompleteTestSource = new ObservableCollection<string>
			{
				"Convert to bmp",
				"Convert to psd",
				"Convert to 3ds",
				"Convert to ai",
				"Convert to max",
				"Convert to xml",
				"Convert to jpeg",
				"Convert to doc",
				"Convert to xsl",
				"Cnovetr to pdf",
				"Convert to cs",
				"Convert to jpg"
			};

			HyperlinkTestCommand = new DelegateCommand(HyperlinkTestCommand_Execute);
			RemoveRequestTestCommand = new DelegateCommand(RemoveRequestTestCommand_Execute);
			AddNewFileTestCommand = new DelegateCommand(AddNewFileTestCommand_Execute);
			ConversionSearchResultRefreshCommand = new DelegateCommand(ConversionSearchResultRefreshCommand_Execute);
			SearchItemClickCommand = new DelegateCommand(SearchItemClickCommand_Execute);
			CommandTextBoxCommand = new DelegateCommand(CommandTextBoxCommand_Execute, CommandTextBoxCommand_CanExecute);
		}

		#region Commanding
		private void ConversionSearchResultRefreshCommand_Execute(object obj)
		{
			string searchString = obj as string;
			if (String.IsNullOrWhiteSpace(searchString))
				SearchResults = new ObservableCollection<string>();
			else
			{
				searchString = searchString.ToLower();
				SearchResults = new ObservableCollection<string>(from cs in SearchAutocompleteTestSource
																 where cs != null && cs.ToLower().Contains(searchString)
																 select cs);
			}
		}

		private void AddNewFileTestCommand_Execute(object parameter)
		{
			TestRemovableItemsItemsSource.Add("File" + random.Next(5, 20) + ".xlsx");
		}

		private void HyperlinkTestCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter as string);
		}

		private void RemoveRequestTestCommand_Execute(object parameter)
		{
			if (parameter != null && TestRemovableItemsItemsSource.Contains(parameter.ToString()))
			{
				string name = parameter.ToString();
				if (MessageBox.Show("Remove '" + name + "'?", "Confirm Item Removal", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					TestRemovableItemsItemsSource.Remove(parameter.ToString());
			}
		}

		private void SearchItemClickCommand_Execute(object parameter)
		{
			ClickedSearchItem = parameter;
		}

		private bool CommandTextBoxCommand_CanExecute(object parameter)
		{
			bool result = parameter != null && !String.IsNullOrWhiteSpace(parameter.ToString());
			return result;
		}

		private void CommandTextBoxCommand_Execute(object parameter)
		{
			CommandTextBoxString = parameter.ToString();
		}
		#endregion
	}
}
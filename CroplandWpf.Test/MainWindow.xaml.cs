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
		public List<string> TestItemsSource
		{
			get { return (List<string>)GetValue(TestItemsSourceProperty); }
			private set { SetValue(TestItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestItemsSourceProperty =
			DependencyProperty.Register("TestItemsSource", typeof(List<string>), typeof(MainWindow), new PropertyMetadata());

		public ObservableCollection<string> TestRemovableItemsItemsSource
		{
			get { return (ObservableCollection<string>)GetValue(TestRemovableItemsItemsSourceProperty); }
			private set { SetValue(TestRemovableItemsItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestRemovableItemsItemsSourceProperty =
			DependencyProperty.Register("TestRemovableItemsItemsSource", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata());

		public string HyperlinkTest
		{
			get { return (string)GetValue(HyperlinkTestProperty); }
			set { SetValue(HyperlinkTestProperty, value); }
		}
		public static readonly DependencyProperty HyperlinkTestProperty =
			DependencyProperty.Register("HyperlinkTest", typeof(string), typeof(MainWindow), new PropertyMetadata("http://some_link"));

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

			HyperlinkTestCommand = new DelegateCommand(HyperlinkTestCommand_Execute);
			RemoveRequestTestCommand = new DelegateCommand(RemoveRequestTestCommand_Execute);
			AddNewFileTestCommand = new DelegateCommand(AddNewFileTestCommand_Execute);
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
	}
}
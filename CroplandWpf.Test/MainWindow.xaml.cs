using System;
using System.Collections.Generic;
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
			set { SetValue(TestItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestItemsSourceProperty =
			DependencyProperty.Register("TestItemsSource", typeof(List<string>), typeof(MainWindow), new PropertyMetadata());

		public MainWindow()
        {
            InitializeComponent();
			TestItemsSource = new List<string> { "Item 1", "Item 2", "Some item with looong looooooong text 3", "Item 4", "Item 5", "Item 6", "Item 7", "Another list box item with even more loooooooooooooooooooong text 8", "Item 9", "Item 10", "Item 11", "Item 12" };
        }
    }
}
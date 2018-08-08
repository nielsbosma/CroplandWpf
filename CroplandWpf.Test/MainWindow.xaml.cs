using CroplandWpf.Attached;
using CroplandWpf.Components;
using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tk = Xceed.Wpf.Toolkit;

namespace CroplandWpf.Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region DPs
		#region Hyperlink
		public string HyperlinkTest
		{
			get { return (string)GetValue(HyperlinkTestProperty); }
			set { SetValue(HyperlinkTestProperty, value); }
		}
		public static readonly DependencyProperty HyperlinkTestProperty =
			DependencyProperty.Register("HyperlinkTest", typeof(string), typeof(MainWindow), new PropertyMetadata("http://some_link"));
		#endregion

		#region Calendar
		public DateTime DateTimeTest
		{
			get { return (DateTime)GetValue(DateTimeTestProperty); }
			set { SetValue(DateTimeTestProperty, value); }
		}
		public static readonly DependencyProperty DateTimeTestProperty =
			DependencyProperty.Register("DateTimeTest", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));
		#endregion

		#region ToolTip

		#endregion

		#region MessageBox
		public MessageBoxInfo Mbi_Warning
		{
			get { return (MessageBoxInfo)GetValue(Mbi_WarningProperty); }
			private set { SetValue(Mbi_WarningProperty, value); }
		}
		public static readonly DependencyProperty Mbi_WarningProperty =
			DependencyProperty.Register("Mbi_Warning", typeof(MessageBoxInfo), typeof(MainWindow), new PropertyMetadata());

		public MessageBoxInfo Mbi_Question
		{
			get { return (MessageBoxInfo)GetValue(Mbi_QuestionProperty); }
			private set { SetValue(Mbi_QuestionProperty, value); }
		}
		public static readonly DependencyProperty Mbi_QuestionProperty =
			DependencyProperty.Register("Mbi_Question", typeof(MessageBoxInfo), typeof(MainWindow), new PropertyMetadata());

		public MessageBoxInfo Mbi_Exception
		{
			get { return (MessageBoxInfo)GetValue(Mbi_ExceptionProperty); }
			private set { SetValue(Mbi_ExceptionProperty, value); }
		}
		public static readonly DependencyProperty Mbi_ExceptionProperty =
			DependencyProperty.Register("Mbi_Exception", typeof(MessageBoxInfo), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ShowMessageBoxCommand
		{
			get { return (DelegateCommand)GetValue(ShowMessageBoxCommandProperty); }
			private set { SetValue(ShowMessageBoxCommandProperty, value); }
		}
		public static readonly DependencyProperty ShowMessageBoxCommandProperty =
			DependencyProperty.Register("ShowMessageBoxCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public MessageBoxResult MessageBoxResult
		{
			get { return (MessageBoxResult)GetValue(MessageBoxResultProperty); }
			private set { SetValue(MessageBoxResultProperty, value); }
		}
		public static readonly DependencyProperty MessageBoxResultProperty =
			DependencyProperty.Register("MessageBoxResult", typeof(MessageBoxResult), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region Slider
		public string SliderDisplayFormat
		{
			get { return (string)GetValue(SliderDisplayFormatProperty); }
			set { SetValue(SliderDisplayFormatProperty, value); }
		}
		public static readonly DependencyProperty SliderDisplayFormatProperty =
			DependencyProperty.Register("SliderDisplayFormat", typeof(string), typeof(MainWindow), new PropertyMetadata());

		public double SliderValue
		{
			get { return (double)GetValue(SliderValueProperty); }
			set { SetValue(SliderValueProperty, value); }
		}
		public static readonly DependencyProperty SliderValueProperty =
			DependencyProperty.Register("SliderValue", typeof(double), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region Exceed IntegerUpDown
		public int IntegerValueTest
		{
			get { return (int)GetValue(IntegerValueTestProperty); }
			set { SetValue(IntegerValueTestProperty, value); }
		}
		public static readonly DependencyProperty IntegerValueTestProperty =
			DependencyProperty.Register("IntegerValueTest", typeof(int), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region ProgressBar
		public double ProgressBarValueTest
		{
			get { return (double)GetValue(ProgressBarValueTestProperty); }
			set { SetValue(ProgressBarValueTestProperty, value); }
		}
		public static readonly DependencyProperty ProgressBarValueTestProperty =
			DependencyProperty.Register("ProgressBarValueTest", typeof(double), typeof(MainWindow), new PropertyMetadata());

		public bool IsLongOperationInProgress
		{
			get { return (bool)GetValue(IsLongOperationInProgressProperty); }
			private set { SetValue(IsLongOperationInProgressProperty, value); }
		}
		public static readonly DependencyProperty IsLongOperationInProgressProperty =
			DependencyProperty.Register("IsLongOperationInProgress", typeof(bool), typeof(MainWindow), new PropertyMetadata());

		public bool IsVeryLongOperationInProgress
		{
			get { return (bool)GetValue(IsVeryLongOperationInProgressProperty); }
			private set { SetValue(IsVeryLongOperationInProgressProperty, value); }
		}
		public static readonly DependencyProperty IsVeryLongOperationInProgressProperty =
			DependencyProperty.Register("IsVeryLongOperationInProgress", typeof(bool), typeof(MainWindow), new PropertyMetadata());

		public bool IsInProgress
		{
			get { return (bool)GetValue(IsInProgressProperty); }
			private set { SetValue(IsInProgressProperty, value); }
		}
		public static readonly DependencyProperty IsInProgressProperty =
			DependencyProperty.Register("IsInProgress", typeof(bool), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand StartProgressTestCommand
		{
			get { return (DelegateCommand)GetValue(StartProgressTestCommandProperty); }
			private set { SetValue(StartProgressTestCommandProperty, value); }
		}
		public static readonly DependencyProperty StartProgressTestCommandProperty =
			DependencyProperty.Register("StartProgressTestCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand StartLongOperationCommand
		{
			get { return (DelegateCommand)GetValue(StartLongOperationCommandProperty); }
			private set { SetValue(StartLongOperationCommandProperty, value); }
		}
		public static readonly DependencyProperty StartLongOperationCommandProperty =
			DependencyProperty.Register("StartLongOperationCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand StartVeryLongOperationCommand
		{
			get { return (DelegateCommand)GetValue(StartVeryLongOperationCommandProperty); }
			private set { SetValue(StartVeryLongOperationCommandProperty, value); }
		}
		public static readonly DependencyProperty StartVeryLongOperationCommandProperty =
			DependencyProperty.Register("StartVeryLongOperationCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region CommandListBox

		public List<DateIntervalType> DateIntervalPresets
		{
			get { return (List<DateIntervalType>)GetValue(DateIntervalPresetsProperty); }
			private set { SetValue(DateIntervalPresetsProperty, value); }
		}
		public static readonly DependencyProperty DateIntervalPresetsProperty =
			DependencyProperty.Register("DateIntervalPresets", typeof(List<DateIntervalType>), typeof(MainWindow), new PropertyMetadata());

		public DateInterval SelectedDateInterval
		{
			get { return (DateInterval)GetValue(SelectedDateIntervalProperty); }
			set { SetValue(SelectedDateIntervalProperty, value); }
		}
		public static readonly DependencyProperty SelectedDateIntervalProperty =
			DependencyProperty.Register("SelectedDateInterval", typeof(DateInterval), typeof(MainWindow), new PropertyMetadata());

		public DateIntervalType SelectedDateIntervalPreset
		{
			get { return (DateIntervalType)GetValue(SelectedDateIntervalPresetProperty); }
			private set { SetValue(SelectedDateIntervalPresetProperty, value); }
		}
		public static readonly DependencyProperty SelectedDateIntervalPresetProperty =
			DependencyProperty.Register("SelectedDateIntervalPreset", typeof(DateIntervalType), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand CommandListBoxCommand
		{
			get { return (DelegateCommand)GetValue(CommandListBoxCommandProperty); }
			private set { SetValue(CommandListBoxCommandProperty, value); }
		}
		public static readonly DependencyProperty CommandListBoxCommandProperty =
			DependencyProperty.Register("CommandListBoxCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata()); 
		#endregion

		#region Collections
		public List<string> TestItemsSource
		{
			get { return (List<string>)GetValue(TestItemsSourceProperty); }
			private set { SetValue(TestItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestItemsSourceProperty =
			DependencyProperty.Register("TestItemsSource", typeof(List<string>), typeof(MainWindow), new PropertyMetadata());

		public MenuItemsCollection MenuItemsTestSource
		{
			get { return (MenuItemsCollection)GetValue(MenuItemsTestSourceProperty); }
			private set { SetValue(MenuItemsTestSourceProperty, value); }
		}
		public static readonly DependencyProperty MenuItemsTestSourceProperty =
			DependencyProperty.Register("MenuItemsTestSource", typeof(MenuItemsCollection), typeof(MainWindow), new PropertyMetadata());

		public ObservableCollection<Person> PersonsTestSource
		{
			get { return (ObservableCollection<Person>)GetValue(PersonsTestSourceProperty); }
			private set { SetValue(PersonsTestSourceProperty, value); }
		}
		public static readonly DependencyProperty PersonsTestSourceProperty =
			DependencyProperty.Register("PersonsTestSource", typeof(ObservableCollection<Person>), typeof(MainWindow), new PropertyMetadata());

		#region RemovableItemsItemsControl
		public ObservableCollection<FileItem> TestRemovableItemsItemsSource
		{
			get { return (ObservableCollection<FileItem>)GetValue(TestRemovableItemsItemsSourceProperty); }
			private set { SetValue(TestRemovableItemsItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty TestRemovableItemsItemsSourceProperty =
			DependencyProperty.Register("TestRemovableItemsItemsSource", typeof(ObservableCollection<FileItem>), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region SearchAutocompleteControl
		public ObservableCollection<string> SearchAutocompleteTestSource_String
		{
			get { return (ObservableCollection<string>)GetValue(SearchAutocompleteTestSource_StringProperty); }
			private set { SetValue(SearchAutocompleteTestSource_StringProperty, value); }
		}
		public static readonly DependencyProperty SearchAutocompleteTestSource_StringProperty =
			DependencyProperty.Register("SearchAutocompleteTestSource_String", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata());

		public ObservableCollection<CustomSearchItem> SearchAutocompleteTestSource_SearchItem
		{
			get { return (ObservableCollection<CustomSearchItem>)GetValue(SearchAutocompleteTestSource_SearchItemProperty); }
			private set { SetValue(SearchAutocompleteTestSource_SearchItemProperty, value); }
		}
		public static readonly DependencyProperty SearchAutocompleteTestSource_SearchItemProperty =
			DependencyProperty.Register("SearchAutocompleteTestSource_SearchItem", typeof(ObservableCollection<CustomSearchItem>), typeof(MainWindow), new PropertyMetadata());

		public ObservableCollection<string> StringSearchResults
		{
			get { return (ObservableCollection<string>)GetValue(StringSearchResultsProperty); }
			private set { SetValue(StringSearchResultsProperty, value); }
		}
		public static readonly DependencyProperty StringSearchResultsProperty =
			DependencyProperty.Register("StringSearchResults", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata());

		public ObservableCollection<CustomSearchItem> CustomItemSearchResults
		{
			get { return (ObservableCollection<CustomSearchItem>)GetValue(CustomItemSearchResultsProperty); }
			private set { SetValue(CustomItemSearchResultsProperty, value); }
		}
		public static readonly DependencyProperty CustomItemSearchResultsProperty =
			DependencyProperty.Register("CustomItemSearchResults", typeof(ObservableCollection<CustomSearchItem>), typeof(MainWindow), new PropertyMetadata());

		public SearchResultInfo StringSearchResultInfo
		{
			get { return (SearchResultInfo)GetValue(StringSearchResultInfoProperty); }
			set { SetValue(StringSearchResultInfoProperty, value); }
		}
		public static readonly DependencyProperty StringSearchResultInfoProperty =
			DependencyProperty.Register("StringSearchResultInfo", typeof(SearchResultInfo), typeof(MainWindow), new PropertyMetadata());

		public SearchResultInfo CustomItemSearchResult
		{
			get { return (SearchResultInfo)GetValue(CustomItemSearchResultProperty); }
			set { SetValue(CustomItemSearchResultProperty, value); }
		}
		public static readonly DependencyProperty CustomItemSearchResultProperty =
			DependencyProperty.Register("CustomItemSearchResult", typeof(SearchResultInfo), typeof(MainWindow), new PropertyMetadata());
		#endregion
		#endregion

		#region CommandTextBox
		public string CommandTextBoxString
		{
			get { return (string)GetValue(CommandTextBoxStringProperty); }
			set { SetValue(CommandTextBoxStringProperty, value); }
		}
		public static readonly DependencyProperty CommandTextBoxStringProperty =
			DependencyProperty.Register("CommandTextBoxString", typeof(string), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region PasswordBox

		public PasswordController UserPasswordController
		{
			get { return (PasswordController)GetValue(UserPasswordControllerProperty); }
			private set { SetValue(UserPasswordControllerProperty, value); }
		}
		public static readonly DependencyProperty UserPasswordControllerProperty =
			DependencyProperty.Register("UserPasswordController", typeof(PasswordController), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand UpdateUserPasswordCommand
		{
			get { return (DelegateCommand)GetValue(UpdateUserPasswordCommandProperty); }
			private set { SetValue(UpdateUserPasswordCommandProperty, value); }
		}
		public static readonly DependencyProperty UpdateUserPasswordCommandProperty =
			DependencyProperty.Register("UpdateUserPasswordCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region InputWindow
		public InputDialogInfo NewFileInputDialogInfo
		{
			get { return (InputDialogInfo)GetValue(NewFileInputDialogInfoProperty); }
			set { SetValue(NewFileInputDialogInfoProperty, value); }
		}
		public static readonly DependencyProperty NewFileInputDialogInfoProperty =
			DependencyProperty.Register("NewFileInputDialogInfo", typeof(InputDialogInfo), typeof(MainWindow), new PropertyMetadata());

		public Func<InputDialogResultActionType, object, bool> NewFileItemValidator
		{
			get { return (Func<InputDialogResultActionType, object, bool>)GetValue(NewFileItemValidatorProperty); }
			set { SetValue(NewFileItemValidatorProperty, value); }
		}
		public static readonly DependencyProperty NewFileItemValidatorProperty =
			DependencyProperty.Register("NewFileItemValidator", typeof(Func<InputDialogResultActionType, object, bool>), typeof(MainWindow), new PropertyMetadata());

		public Func<InputDialogResultActionType, object, bool> TextInputDialogValidator
		{
			get { return (Func<InputDialogResultActionType, object, bool>)GetValue(TextInputDialogValidatorProperty); }
			set { SetValue(TextInputDialogValidatorProperty, value); }
		}
		public static readonly DependencyProperty TextInputDialogValidatorProperty =
			DependencyProperty.Register("TextInputDialogValidator", typeof(Func<InputDialogResultActionType, object, bool>), typeof(MainWindow), new PropertyMetadata());

		public vmSearchInputDialog SearchInputDialogViewModel
		{
			get { return (vmSearchInputDialog)GetValue(SearchInputDialogViewModelProperty); }
			private set { SetValue(SearchInputDialogViewModelProperty, value); }
		}
		public static readonly DependencyProperty SearchInputDialogViewModelProperty =
			DependencyProperty.Register("SearchInputDialogViewModel", typeof(vmSearchInputDialog), typeof(MainWindow), new PropertyMetadata());

		public vmTextInputDialog TextInputDialogViewModel
		{
			get { return (vmTextInputDialog)GetValue(TextInputDialogViewModelProperty); }
			private set { SetValue(TextInputDialogViewModelProperty, value); }
		}
		public static readonly DependencyProperty TextInputDialogViewModelProperty =
			DependencyProperty.Register("TextInputDialogViewModel", typeof(vmTextInputDialog), typeof(MainWindow), new PropertyMetadata());

		public string TextInputDialogResult
		{
			get { return (string)GetValue(TextInputDialogResultProperty); }
			set { SetValue(TextInputDialogResultProperty, value); }
		}
		public static readonly DependencyProperty TextInputDialogResultProperty =
			DependencyProperty.Register("TextInputDialogResult", typeof(string), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ShowSearchInputBoxCommand
		{
			get { return (DelegateCommand)GetValue(ShowSearchInputBoxCommandProperty); }
			private set { SetValue(ShowSearchInputBoxCommandProperty, value); }
		}
		public static readonly DependencyProperty ShowSearchInputBoxCommandProperty =
			DependencyProperty.Register("ShowSearchInputBoxCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ShowTextInputDialogCommand
		{
			get { return (DelegateCommand)GetValue(ShowTextInputDialogCommandProperty); }
			private set { SetValue(ShowTextInputDialogCommandProperty, value); }
		}
		public static readonly DependencyProperty ShowTextInputDialogCommandProperty =
			DependencyProperty.Register("ShowTextInputDialogCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region ResizeControl
		public BitmapSource ResizeImageSource
		{
			get { return (BitmapSource)GetValue(ResizeImageSourceProperty); }
			set { SetValue(ResizeImageSourceProperty, value); }
		}
		public static readonly DependencyProperty ResizeImageSourceProperty =
			DependencyProperty.Register("ResizeImageSource", typeof(BitmapSource), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ResizeImageSourceCommand
		{
			get { return (DelegateCommand)GetValue(ResizeImageSourceCommandProperty); }
			private set { SetValue(ResizeImageSourceCommandProperty, value); }
		}
		public static readonly DependencyProperty ResizeImageSourceCommandProperty =
			DependencyProperty.Register("ResizeImageSourceCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand SelectImageSourceCommand
		{
			get { return (DelegateCommand)GetValue(SelectImageSourceCommandProperty); }
			private set { SetValue(SelectImageSourceCommandProperty, value); }
		}
		public static readonly DependencyProperty SelectImageSourceCommandProperty =
			DependencyProperty.Register("SelectImageSourceCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region ImageCropControl
		public FileInfo CropImageSourceFileInfo
		{
			get { return (FileInfo)GetValue(CropImageSourceFileInfoProperty); }
			set { SetValue(CropImageSourceFileInfoProperty, value); }
		}
		public static readonly DependencyProperty CropImageSourceFileInfoProperty =
			DependencyProperty.Register("CropImageSourceFileInfo", typeof(FileInfo), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand SelectCropImageSourceCommand
		{
			get { return (DelegateCommand)GetValue(SelectCropImageSourceCommandProperty); }
			private set { SetValue(SelectCropImageSourceCommandProperty, value); }
		}
		public static readonly DependencyProperty SelectCropImageSourceCommandProperty =
			DependencyProperty.Register("SelectCropImageSourceCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand CropImageCommand
		{
			get { return (DelegateCommand)GetValue(CropImageCommandProperty); }
			private set { SetValue(CropImageCommandProperty, value); }
		}
		public static readonly DependencyProperty CropImageCommandProperty =
			DependencyProperty.Register("CropImageCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public Int32Rect CropResultRect
		{
			get { return (Int32Rect)GetValue(CropResultRectProperty); }
			private set { SetValue(CropResultRectProperty, value); }
		}
		public static readonly DependencyProperty CropResultRectProperty =
			DependencyProperty.Register("CropResultRect", typeof(Int32Rect), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region TimePicker
		public DateTime? TimeValue
		{
			get { return (DateTime?)GetValue(TimeValueProperty); }
			set { SetValue(TimeValueProperty, value); }
		}
		public static readonly DependencyProperty TimeValueProperty =
			DependencyProperty.Register("TimeValue", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

		public DateTime? SelectedDateTime
		{
			get { return (DateTime?)GetValue(SelectedDateTimeProperty); }
			set { SetValue(SelectedDateTimeProperty, value); }
		}
		public static readonly DependencyProperty SelectedDateTimeProperty =
			DependencyProperty.Register("SelectedDateTime", typeof(DateTime?), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region ColorPicker
		public Color SelectedColor
		{
			get { return (Color)GetValue(SelectedColorProperty); }
			set { SetValue(SelectedColorProperty, value); }
		}
		public static readonly DependencyProperty SelectedColorProperty =
			DependencyProperty.Register("SelectedColor", typeof(Color), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region AlignmentEditor
		public VerticalAlignment VerticalAlignmentValue
		{
			get { return (VerticalAlignment)GetValue(VerticalAlignmentValueProperty); }
			set { SetValue(VerticalAlignmentValueProperty, value); }
		}
		public static readonly DependencyProperty VerticalAlignmentValueProperty =
			DependencyProperty.Register("VerticalAlignmentValue", typeof(VerticalAlignment), typeof(MainWindow), new PropertyMetadata(VerticalAlignment.Center));

		public HorizontalAlignment HorizontalAlignmentValue
		{
			get { return (HorizontalAlignment)GetValue(HorizontalAlignmentValueProperty); }
			set { SetValue(HorizontalAlignmentValueProperty, value); }
		}
		public static readonly DependencyProperty HorizontalAlignmentValueProperty =
			DependencyProperty.Register("HorizontalAlignmentValue", typeof(HorizontalAlignment), typeof(MainWindow), new PropertyMetadata(HorizontalAlignment.Right));

		public Thickness MarginValue
		{
			get { return (Thickness)GetValue(MarginValueProperty); }
			set { SetValue(MarginValueProperty, value); }
		}
		public static readonly DependencyProperty MarginValueProperty =
			DependencyProperty.Register("MarginValue", typeof(Thickness), typeof(MainWindow), new PropertyMetadata());

		#endregion

		#region Window with tool style
		public DelegateCommand SummonToolWindowCommand
		{
			get { return (DelegateCommand)GetValue(SummonToolWindowCommandProperty); }
			private set { SetValue(SummonToolWindowCommandProperty, value); }
		}
		public static readonly DependencyProperty SummonToolWindowCommandProperty =
			DependencyProperty.Register("SummonToolWindowCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public bool AllowToolWindowResize
		{
			get { return (bool)GetValue(AllowToolWindowResizeProperty); }
			set { SetValue(AllowToolWindowResizeProperty, value); }
		}
		public static readonly DependencyProperty AllowToolWindowResizeProperty =
			DependencyProperty.Register("AllowToolWindowResize", typeof(bool), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region Commands
		public DelegateCommand ShowAddNewFileInputDialogCommand
		{
			get { return (DelegateCommand)GetValue(ShowAddNewFileInputDialogCommandProperty); }
			private set { SetValue(ShowAddNewFileInputDialogCommandProperty, value); }
		}
		public static readonly DependencyProperty ShowAddNewFileInputDialogCommandProperty =
			DependencyProperty.Register("ShowAddNewFileInputDialogCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

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

		public DelegateCommand CommandTextBoxCommand
		{
			get { return (DelegateCommand)GetValue(CommandTextBoxCommandProperty); }
			private set { SetValue(CommandTextBoxCommandProperty, value); }
		}
		public static readonly DependencyProperty CommandTextBoxCommandProperty =
			DependencyProperty.Register("CommandTextBoxCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand PersonItemMoveUpCommand
		{
			get { return (DelegateCommand)GetValue(PersonItemMoveUpCommandProperty); }
			private set { SetValue(PersonItemMoveUpCommandProperty, value); }
		}
		public static readonly DependencyProperty PersonItemMoveUpCommandProperty =
			DependencyProperty.Register("PersonItemMoveUpCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand PersonItemMoveDownCommand
		{
			get { return (DelegateCommand)GetValue(PersonItemMoveDownCommandProperty); }
			private set { SetValue(PersonItemMoveDownCommandProperty, value); }
		}
		public static readonly DependencyProperty PersonItemMoveDownCommandProperty =
			DependencyProperty.Register("PersonItemMoveDownCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand RemovePersonItemCommand
		{
			get { return (DelegateCommand)GetValue(RemovePersonItemCommandProperty); }
			private set { SetValue(RemovePersonItemCommandProperty, value); }
		}
		public static readonly DependencyProperty RemovePersonItemCommandProperty =
			DependencyProperty.Register("RemovePersonItemCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		#region SearchAutocompleteControl
		public DelegateCommand ConversionSearchResultRefreshCommand
		{
			get { return (DelegateCommand)GetValue(ConversionSearchResultRefreshCommandProperty); }
			private set { SetValue(ConversionSearchResultRefreshCommandProperty, value); }
		}
		public static readonly DependencyProperty ConversionSearchResultRefreshCommandProperty =
			DependencyProperty.Register("ConversionSearchResultRefreshCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand CustomSearchItemSearchResultRefreshCommand
		{
			get { return (DelegateCommand)GetValue(CustomSearchItemSearchResultRefreshCommandProperty); }
			private set { SetValue(CustomSearchItemSearchResultRefreshCommandProperty, value); }
		}
		public static readonly DependencyProperty CustomSearchItemSearchResultRefreshCommandProperty =
			DependencyProperty.Register("CustomSearchItemSearchResultRefreshCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand StringSearchCompleteCommand
		{
			get { return (DelegateCommand)GetValue(StringSearchCompleteCommandProperty); }
			private set { SetValue(StringSearchCompleteCommandProperty, value); }
		}
		public static readonly DependencyProperty StringSearchCompleteCommandProperty =
			DependencyProperty.Register("StringSearchCompleteCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand CustomSearchCompleteCommand
		{
			get { return (DelegateCommand)GetValue(CustomSearchCompleteCommandProperty); }
			private set { SetValue(CustomSearchCompleteCommandProperty, value); }
		}
		public static readonly DependencyProperty CustomSearchCompleteCommandProperty =
			DependencyProperty.Register("CustomSearchCompleteCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand SeeMoreSearchOptionsCommand
		{
			get { return (DelegateCommand)GetValue(SeeMoreSearchOptionsCommandProperty); }
			private set { SetValue(SeeMoreSearchOptionsCommandProperty, value); }
		}
		public static readonly DependencyProperty SeeMoreSearchOptionsCommandProperty =
			DependencyProperty.Register("SeeMoreSearchOptionsCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand NoMatchesFoundCommand
		{
			get { return (DelegateCommand)GetValue(NoMatchesFoundCommandProperty); }
			private set { SetValue(NoMatchesFoundCommandProperty, value); }
		}
		public static readonly DependencyProperty NoMatchesFoundCommandProperty =
			DependencyProperty.Register("NoMatchesFoundCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion

		#region Menu commands
		public DelegateCommand OpenCommand
		{
			get { return (DelegateCommand)GetValue(OpenCommandProperty); }
			private set { SetValue(OpenCommandProperty, value); }
		}
		public static readonly DependencyProperty OpenCommandProperty =
			DependencyProperty.Register("OpenCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ShowInExplorerCommand
		{
			get { return (DelegateCommand)GetValue(ShowInExplorerCommandProperty); }
			private set { SetValue(ShowInExplorerCommandProperty, value); }
		}
		public static readonly DependencyProperty ShowInExplorerCommandProperty =
			DependencyProperty.Register("ShowInExplorerCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand AddCommand
		{
			get { return (DelegateCommand)GetValue(AddCommandProperty); }
			private set { SetValue(AddCommandProperty, value); }
		}
		public static readonly DependencyProperty AddCommandProperty =
			DependencyProperty.Register("AddCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand RemoveCommand
		{
			get { return (DelegateCommand)GetValue(RemoveCommandProperty); }
			private set { SetValue(RemoveCommandProperty, value); }
		}
		public static readonly DependencyProperty RemoveCommandProperty =
			DependencyProperty.Register("RemoveCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ResolveNamesCommand
		{
			get { return (DelegateCommand)GetValue(ResolveNamesCommandProperty); }
			private set { SetValue(ResolveNamesCommandProperty, value); }
		}
		public static readonly DependencyProperty ResolveNamesCommandProperty =
			DependencyProperty.Register("ResolveNamesCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand ResolveSurnamesCommand
		{
			get { return (DelegateCommand)GetValue(ResolveSurnamesCommandProperty); }
			private set { SetValue(ResolveSurnamesCommandProperty, value); }
		}
		public static readonly DependencyProperty ResolveSurnamesCommandProperty =
			DependencyProperty.Register("ResolveSurnamesCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());

		public DelegateCommand LaunchCommandLineCommand
		{
			get { return (DelegateCommand)GetValue(LaunchCommandLineCommandProperty); }
			set { SetValue(LaunchCommandLineCommandProperty, value); }
		}
		public static readonly DependencyProperty LaunchCommandLineCommandProperty =
			DependencyProperty.Register("LaunchCommandLineCommand", typeof(DelegateCommand), typeof(MainWindow), new PropertyMetadata());
		#endregion
		#endregion
		#endregion

		#region Private fields/properties
		private Random random = new Random(); 

		private OpenFileDialog openImageForResizeFileDialog
		{
			get
			{
				if(_ofd_ImageForResize == null)
				{
					_ofd_ImageForResize = new OpenFileDialog
					{
						Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp",
						Multiselect = false,
						Title = "Choose An Image for ImageResizeControl Test"
					};
					_ofd_ImageForResize.FileOk += (o, e) =>
					{
						BitmapImage source = new BitmapImage();
						source.BeginInit();
						source.UriSource = new Uri(_ofd_ImageForResize.FileName);
						source.EndInit();
						ResizeImageSource = source;
					};
				}
				return _ofd_ImageForResize;
			}
		}
		private OpenFileDialog _ofd_ImageForResize;

		private OpenFileDialog openImageForCropFileDialog
		{
			get
			{
				if (_ofd_ImageForCrop == null)
				{
					_ofd_ImageForCrop = new OpenFileDialog
					{
						Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp",
						Multiselect = false,
						Title = "Choose An Image for ImageResizeControl Test"
					};
					_ofd_ImageForCrop.FileOk += (o, e) =>
					{
						FileInfo info = new FileInfo(_ofd_ImageForCrop.FileName);
						CropImageSourceFileInfo = info;
					};
				}
				return _ofd_ImageForCrop;
			}
		}
		private OpenFileDialog _ofd_ImageForCrop;
		#endregion

		public MainWindow()
		{
			InitializeComponent();
			#region ItemsSources
			TestItemsSource = new List<string>
			{
				"Item 1", "Item 2",
				"Some item with looong looooooong text 3",
				"Item 4", "Item 5", "Item 6", "Item 7",
				"Another list box item with even more loooooooooooooooooooong text 8",
				"Item 9", "Item 10", "Item 11", "Item 12"
			};

			TestRemovableItemsItemsSource = new ObservableCollection<FileItem>
			{
				new FileItem{ Name = "File1.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) },
				new FileItem{ Name = "File2.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) },
				new FileItem{ Name = "File3.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) },
				new FileItem{ Name = "Some Longer File Name.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) },
				new FileItem{ Name = "File4.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) },
				new FileItem{ Name = "File5.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) },
				new FileItem{ Name = "File6.xslx", Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(1,256000) }
			};

			SearchAutocompleteTestSource_String = new ObservableCollection<string>
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
				"Convert to jpg",
				"Remove ID3 Tags",
				"Rasterize Vector Format",
				"Ask a Question",
				"Get an Answer",
				"Get Multiple Answers",
				"Get Two Answers",
				"Conversion FAQ",
				"Send Conversion Request",
				"Convert",
				"Convert to"
			};

			SearchAutocompleteTestSource_SearchItem = new ObservableCollection<CustomSearchItem>
			{
				new CustomSearchItem { Name = "Convert to bmp", Description = "Conversion to BMP raster image format" },
				new CustomSearchItem { Name = "Convert to psd", Description = "Conversion to Photoshop format" },
				new CustomSearchItem { Name = "Convert to 3ds", Description = "Conversion to 3DS Max format" },
				new CustomSearchItem { Name = "Convert to ai", Description = "Conversion to Adobe Illustrator format" },
				new CustomSearchItem { Name = "Convert to max", Description = "Conversion to 3DS Max Format" },
				new CustomSearchItem { Name = "Convert to xml", Description = "Conversion to BMP raster image format" },
				new CustomSearchItem { Name = "Convert to jpeg", Description = "Conversion to JPEG raster image format" },
				new CustomSearchItem { Name = "Convert to doc", Description = "Conversion to MS Word document format" },
				new CustomSearchItem { Name = "Convert to xls", Description = "Conversion to MS Excel spreadsheet format" },
				new CustomSearchItem { Name = "Cnovetr to pdf", Description = "Cnoversion to Adobe PDF document format" },
				new CustomSearchItem { Name = "Convert to cs", Description = "Conversion to C# code file format" },
				new CustomSearchItem { Name = "Convert to jpg", Description = "Conversion to JPG raster image format" },
				new CustomSearchItem { Name = "Remove ID3 Tags", Description = "Removing of tags from MP3 audio file" },
				new CustomSearchItem { Name = "Rasterize Vector Format", Description = "Rasterizing of vector image" },
				new CustomSearchItem { Name = "Ask a Question", Description = "Ask some question, apparently" },
				new CustomSearchItem { Name = "Get an Answer", Description = "Getting an answer to your question" },
				new CustomSearchItem { Name = "Get Multiple Answers", Description = "The more you know..." },
				new CustomSearchItem { Name = "Get Two Answers", Description = "Only two answers available" },
				new CustomSearchItem { Name = "Conversion FAQ", Description = "General answers and questions about conversion process" },
				new CustomSearchItem { Name = "Send Conversion Request", Description = "Send conversion rquest if you are not proficient enough to do that" },
				new CustomSearchItem { Name = "Convert", Description = "Just convert it" },
				new CustomSearchItem { Name = "Convert to", Description = "Just convert it to something" },
			};

			NoMatchesFoundCommand = new DelegateCommand((o) => { MessageBoxService.Show(new MessageBoxInfo { Buttons = MessageBoxButton.OK, Content = "No matches found command executed." }); });
			SeeMoreSearchOptionsCommand = new DelegateCommand((o) => { MessageBoxService.Show(new MessageBoxInfo { Buttons = MessageBoxButton.OK, Content = "See more search options command executed." }); });

			PersonsTestSource = new ObservableCollection<Person>()
			{
				new Person{ Name = "Laura", IsMedic = false, InternalNumber = 910934, PersonalityType = PersonalityType.Extrovert },
				new Person{ Name = "Vigilo", IsMedic = false, InternalNumber = 001971, PersonalityType = PersonalityType.Introvert },
				new Person{ Name = "C3PO", IsMedic = false, InternalNumber = 1001110, PersonalityType = PersonalityType.NA},
				new Person{ Name = "E2E4", IsMedic = true, InternalNumber = 1001111, PersonalityType = PersonalityType.AnnoyingCryBaby},
				new Person{ Name = "Adam Fielding", IsMedic = false, InternalNumber = 0491991, PersonalityType = PersonalityType.Introvert},
				new Person{ Name = "Mere Mortal", IsMedic = false, InternalNumber = 000000, PersonalityType = PersonalityType.NA },
				new Person{ Name = "Crystal Clear Table-Top Epoxy Resin", IsMedic = false, InternalNumber = 6097661, PersonalityType = PersonalityType.NA },
				new Person{ Name = "Carinthia", IsMedic = true, InternalNumber = 3966511, PersonalityType = PersonalityType.NA }
			};
			PersonItemMoveUpCommand = new DelegateCommand(PersonItemMoveUpCommand_Execute, PersonItemMoveUpCommand_CanExecute);
			PersonItemMoveDownCommand = new DelegateCommand(PersonItemMoveDownCommand_Execute, PersonItemMoveDownCommand_CanExecute);
			RemovePersonItemCommand = new DelegateCommand(RemovePersonItemCommand_Execute, RemovePersonItemCommand_CanExecute);
			#endregion

			UserPasswordController = new PasswordController();
			UserPasswordController.SetPassword("123456");
			UpdateUserPasswordCommand = new DelegateCommand(UpdateUserPasswordCommand_Execute, UpdateUserPasswordCommand_CanExecute);

			HyperlinkTestCommand = new DelegateCommand(HyperlinkTestCommand_Execute);
			RemoveRequestTestCommand = new DelegateCommand(RemoveRequestTestCommand_Execute);
			AddNewFileTestCommand = new DelegateCommand(AddNewFileTestCommand_Execute);
			ConversionSearchResultRefreshCommand = new DelegateCommand(ConversionSearchResultRefreshCommand_Execute);
			CustomSearchItemSearchResultRefreshCommand = new DelegateCommand(CustomSearchItemSearchResultRefreshCommand_Execute);
			StringSearchCompleteCommand = new DelegateCommand(StringSearchCompleteCommand_Execute);
			CustomSearchCompleteCommand = new DelegateCommand(CustomSearchCompleteCommand_Execute);
			CommandTextBoxCommand = new DelegateCommand(CommandTextBoxCommand_Execute, CommandTextBoxCommand_CanExecute);

			#region Menu
			OpenCommand = new DelegateCommand(OpenCommand_Execute);
			ShowInExplorerCommand = new DelegateCommand(ShowInExplorerCommand_Execute, ShowInExplorerCommand_CanExecute);
			AddCommand = new DelegateCommand(AddCommand_Execute);
			RemoveCommand = new DelegateCommand(RemoveCommand_Execute);
			ResolveNamesCommand = new DelegateCommand(ResolveNamesCommand_Execute);
			ResolveSurnamesCommand = new DelegateCommand(ResolveSurnamesCommand_Execute);
			LaunchCommandLineCommand = new DelegateCommand(LaunchCommandLineCommand_Execute);
			MenuItemsCollection mic = new MenuItemsCollection() { AutoRegisterKeyBindings = true };
			mic.Add(new vmMenuItemsContainer()
			{
				Header = "Action",
				Items = new MenuItemsCollection()
				{
					new vmMenuItem(){ Header = "Open", Command = OpenCommand, CommandParameter = "[open]", Gesture = new KeyGesture(Key.O, ModifierKeys.Windows) },
					new vmMenuItem() { Header = "Show in Explorer", Command = ShowInExplorerCommand, CommandParameter = "[show_in_explorer]", Gesture = new KeyGesture(Key.S, ModifierKeys.Alt | ModifierKeys.Control) },
					new vmMenuItemsContainer() { Header = "Convert To..." , Items = new MenuItemsCollection
					{
						new vmMenuItem() { Header = "Resolve Names", Command = ResolveNamesCommand, CommandParameter = "[resolve_names]", Gesture = new KeyGesture(Key.N, ModifierKeys.Control) },
						new vmMenuItem() { Header = "Resolve Surnames", Command = ResolveSurnamesCommand, CommandParameter = "[resolve_surnames]" },
						new vmMenuItemsContainer() { Header = "Additional Actions",
							Items = new MenuItemsCollection()
							{
								new vmMenuItem(){ Header = "Additional Open", Command = OpenCommand, CommandParameter = "additional_open", Gesture = new KeyGesture(Key.W, ModifierKeys.Windows) }
							}
						},
						new vmMenuItem() { IsSeparator = true }
					}},
					new vmMenuItem() { IsSeparator = true },
					new vmMenuItem() { Header = "Add", Command = AddCommand, CommandParameter = "[add]" },
					new vmMenuItem() { Header = "Remove", Command = RemoveCommand, CommandParameter = "[remove]" },
				}
			});
			mic.Add(new vmMenuItemsContainer()
			{
				Header = "Tools",
				Items = new MenuItemsCollection()
				{
					new vmMenuItem() { Header = "Launch Command Line", Command = LaunchCommandLineCommand, Gesture = new KeyGesture(Key.C, ModifierKeys.Alt | ModifierKeys.Control) },
					new vmMenuItemSeparator(),
					new vmMenuItem() { Header = "Video Converter" },
					new vmMenuItem() { Header = "Audio Converter" },
					new vmMenuItem() { Header = "Image Converter" },
					new vmMenuItemSeparator(),
					new vmMenuItem() { Header = "FLAC to MP3 useless converter" }
				}
			});
			mic.Add(new vmMenuItem() { Header = "No Items Here" });
			MenuItemsTestSource = mic;
			#endregion

			#region InputDialog
			NewFileInputDialogInfo = new InputDialogInfo() { Content = new FileItem(), ContentTemplateKey = "templateInputDialog_FileItem" };
			ShowAddNewFileInputDialogCommand = new DelegateCommand(ShowAddNewFileInputDialogCommand_Execute);
			NewFileItemValidator = new Func<InputDialogResultActionType, object, bool>((at, fi) =>
			{
				FileItem item = fi as FileItem;
				if (at == InputDialogResultActionType.Positive)
					return item != null && !String.IsNullOrWhiteSpace(item.Name) && item.Size_Mb > 0;
				else
					return true;
			});
			SearchInputDialogViewModel = new vmSearchInputDialog();
			SearchInputDialogViewModel.RefreshSearchResults_ExecuteFunction = new Func<object, IEnumerable, IEnumerable>((parameter, itemsSource) =>
			{
				string searchString = parameter as string;
				if (String.IsNullOrWhiteSpace(searchString))
					return null;
				else
				{
					searchString = searchString.ToLower();
					return (from cs in itemsSource as IEnumerable<string>
							where cs != null && cs.ToLower().Contains(searchString)
							select cs);
				}
			});
			SearchInputDialogViewModel.ItemsSource = SearchAutocompleteTestSource_String;
			ShowSearchInputBoxCommand = new DelegateCommand(ShowSearchInputBoxCommand_Execute);
			TextInputDialogViewModel = new vmTextInputDialog();
			ShowTextInputDialogCommand = new DelegateCommand(ShowTextInputDialogCommand_Execute);
			TextInputDialogValidator = new Func<InputDialogResultActionType, object, bool>((at, tid) =>
			{
				return tid as vmTextInputDialog != null && !String.IsNullOrEmpty((tid as vmTextInputDialog).Text);
			});
			#endregion

			#region MessageBox
			ShowMessageBoxCommand = new DelegateCommand(ShowMessageBoxCommand_Execute, ShowMessageBoxCommand_CanExecute);
			Mbi_Exception = new MessageBoxInfo()
			{
				Header = "SeoTool",
				Buttons = MessageBoxButton.OK,
				Content = new ExceptionInfo
				{
					Name = "XPathonUrl",
					Exception = "ArgumentException",
					Message = "Missing the 'Url' argument. Check if you supplied the URL, or nothing good will happen.",
					#region StackTrace
					StackTrace = "at CroplandWpf.Components.InputDialog.Show(InputDialogInfo info) in D:\\Visual Studio Projects\\Cropland\\CroplandWpf\\CroplandWpf\\Components\\InputDialog.cs:line 105\r\n   at CroplandWpf.Test.MainWindow.ShowSearchInputBoxCommand_Execute(Object arg) in D:\\Visual Studio Projects\\Cropland\\CroplandWpf\\CroplandWpf.Test\\MainWindow.xaml.cs:line 798\r\n   at CroplandWpf.MVVM.DelegateCommand.Execute(Object parameter) in D:\\Visual Studio Projects\\Cropland\\CroplandWpf\\CroplandWpf\\MVVM\\DelegateCommand.cs:line 72\r\n   at MS.Internal.Commands.CommandHelpers.CriticalExecuteCommandSource(ICommandSource commandSource, Boolean userInitiated)\r\n   at System.Windows.Controls.Primitives.ButtonBase.OnClick()\r\n   at System.Windows.Controls.Button.OnClick()\r\n   at System.Windows.Controls.Primitives.ButtonBase.OnMouseLeftButtonUp(MouseButtonEventArgs e)\r\n   at System.Windows.UIElement.OnMouseLeftButtonUpThunk(Object sender, MouseButtonEventArgs e)\r\n   at System.Windows.Input.MouseButtonEventArgs.InvokeEventHandler(Delegate genericHandler, Object genericTarget)\r\n   at System.Windows.RoutedEventArgs.InvokeHandler(Delegate handler, Object target)\r\n   at System.Windows.RoutedEventHandlerInfo.InvokeHandler(Object target, RoutedEventArgs routedEventArgs)\r\n   at System.Windows.EventRoute.InvokeHandlersImpl(Object source, RoutedEventArgs args, Boolean reRaised)\r\n   at System.Windows.UIElement.ReRaiseEventAs(DependencyObject sender, RoutedEventArgs args, RoutedEvent newEvent)\r\n   at System.Windows.UIElement.OnMouseUpThunk(Object sender, MouseButtonEventArgs e)\r\n   at System.Windows.Input.MouseButtonEventArgs.InvokeEventHandler(Delegate genericHandler, Object genericTarget)\r\n   at System.Windows.RoutedEventArgs.InvokeHandler(Delegate handler, Object target)\r\n   at System.Windows.RoutedEventHandlerInfo.InvokeHandler(Object target, RoutedEventArgs routedEventArgs)\r\n   at System.Windows.EventRoute.InvokeHandlersImpl(Object source, RoutedEventArgs args, Boolean reRaised)\r\n   at System.Windows.UIElement.RaiseEventImpl(DependencyObject sender, RoutedEventArgs args)\r\n   at System.Windows.UIElement.RaiseTrustedEvent(RoutedEventArgs args)\r\n   at System.Windows.UIElement.RaiseEvent(RoutedEventArgs args, Boolean trusted)\r\n   at System.Windows.Input.InputManager.ProcessStagingArea()\r\n   at System.Windows.Input.InputManager.ProcessInput(InputEventArgs input)\r\n   at System.Windows.Input.InputProviderSite.ReportInput(InputReport inputReport)\r\n   at System.Windows.Interop.HwndMouseInputProvider.ReportInput(IntPtr hwnd, InputMode mode, Int32 timestamp, RawMouseActions actions, Int32 x, Int32 y, Int32 wheel)\r\n   at System.Windows.Interop.HwndMouseInputProvider.FilterMessage(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam, Boolean & handled)\r\n   at System.Windows.Interop.HwndSource.InputFilterMessage(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean & handled)\r\n   at MS.Win32.HwndWrapper.WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, Boolean & handled)\r\n   at MS.Win32.HwndSubclass.DispatcherCallbackOperation(Object o)\r\n   at System.Windows.Threading.ExceptionWrapper.InternalRealCall(Delegate callback, Object args, Int32 numArgs)\r\n   at System.Windows.Threading.ExceptionWrapper.TryCatchWhen(Object source, Delegate callback, Object args, Int32 numArgs, Delegate catchHandler)\r\n   at System.Windows.Threading.Dispatcher.LegacyInvokeImpl(DispatcherPriority priority, TimeSpan timeout, Delegate method, Object args, Int32 numArgs)\r\n   at MS.Win32.HwndSubclass.SubclassWndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam)\r\n   at MS.Win32.UnsafeNativeMethods.DispatchMessage(MSG & msg)\r\n   at System.Windows.Threading.Dispatcher.PushFrameImpl(DispatcherFrame frame)\r\n   at System.Windows.Threading.Dispatcher.PushFrame(DispatcherFrame frame)\r\n   at System.Windows.Application.RunDispatcher(Object ignore)\r\n   at System.Windows.Application.RunInternal(Window window)\r\n   at System.Windows.Application.Run(Window window)\r\n   at System.Windows.Application.Run()\r\n   at CroplandWpf.Test.App.Main()"
					#endregion
				},
				AdditionalContentTemplateKey = "templateMessageBoxContent_Exception_AdditionalContent",
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Exception,
				ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Exception
			};
			Mbi_Question = new MessageBoxInfo() { Header = "FileStar", Buttons = MessageBoxButton.YesNo, Content = "Can you answer the question?..", IconBrushKey = MessageBoxIconBrushDefaultKeys.Question };
			Mbi_Warning = new MessageBoxInfo() { Header = "SuperTsar", Buttons = MessageBoxButton.OK, Content = "Congratulations! You received a warning!", IconBrushKey = MessageBoxIconBrushDefaultKeys.Warning };
			#endregion

			#region 
			StartProgressTestCommand = new DelegateCommand(StartProgressTestCommand_Execute, StartProgressTestCommand_CanExecute);
			StartLongOperationCommand = new DelegateCommand(StartLongOperationCommand_Execute, StartLongOperationCommand_CanExecute);
			StartVeryLongOperationCommand = new DelegateCommand(StartVeryLongOperationCommand_Execute, StartVeryLongOperationCommand_CanExecute);
			#endregion

			#region Slider
			SliderValue = 67.04;
			#endregion

			#region AlignmentEditor
			MarginValue = new Thickness(10, 20, 30, 40);
			#endregion

			#region DateTimePicker
			SelectedDateTime = DateTime.Now;
			#endregion

			#region ColorPicker
			SelectedColor = Color.FromArgb(255, 255, 255, 0);
			#endregion

			#region ImageResizeEditor
			BitmapImage biResizeSource = new BitmapImage();
			biResizeSource.BeginInit();
			biResizeSource.UriSource = new Uri("pack://application:,,,/Images/test image.jpg");
			biResizeSource.EndInit();
			ResizeImageSource = biResizeSource;
			ResizeImageSourceCommand = new DelegateCommand(ResizeImageSourceCommand_Execute, ResizeImageSourceCommand_CanExecute);
			SelectImageSourceCommand = new DelegateCommand(SelectImageSourceCommand_Execute);
			#endregion

			#region ImageCropControl
			SelectCropImageSourceCommand = new DelegateCommand(SelectCropImageSourceCommand_Execute);
			CropImageCommand = new DelegateCommand(CropImageCommand_Execute);
			#endregion

			#region CommandListBox
			CommandListBoxCommand = new DelegateCommand(CommandListBoxCommand_Execute);
			DateIntervalPresets = new List<DateIntervalType>(Enum.GetValues(typeof(DateIntervalType)).Cast<DateIntervalType>());
			#endregion

			#region DateIntervalControl
			SelectedDateInterval = new DateInterval(DateTime.Now.Date, DateTime.Now.Date.AddDays(1.0).AddSeconds(-1.0));
			#endregion

			#region Tool window style
			SummonToolWindowCommand = new DelegateCommand(SummonToolWindowCommand_Execute);
			#endregion
		}

		private void SummonToolWindowCommand_Execute(object obj)
		{
			Window window = new Window();
			window.SetResourceReference(Window.StyleProperty, "styleToolWindow_Dark");
			window.Owner = this;
			window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			window.Content = new Button() { Content = "Tool window content button" };
			WindowHelper.SetAllowResize(window, AllowToolWindowResize);
			window.Title = "SeoTools";
			window.ShowDialog();
		}

		#region Overrides
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
		}
		#endregion

		#region Commanding
		#region DataGrid
		private void RemovePersonItemCommand_Execute(object parameter)
		{
			Person person = parameter as Person;
			if (person != null && PersonsTestSource.Contains(person))
				PersonsTestSource.Remove(person);
		}

		private bool RemovePersonItemCommand_CanExecute(object parameter)
		{
			return parameter as Person != null;
		}

		private bool PersonItemMoveDownCommand_CanExecute(object parameter)
		{
			if (!(parameter is Person person))
				return false;
			int currentIndex = PersonsTestSource.IndexOf(person);
			return currentIndex < PersonsTestSource.Count - 1;
		}

		private void PersonItemMoveDownCommand_Execute(object parameter)
		{
			Person person = parameter as Person;
			if (person == null)
				return;
			int currentIndex = PersonsTestSource.IndexOf(person);
			PersonsTestSource.Move(currentIndex, currentIndex + 1);
		}

		private void PersonItemMoveUpCommand_Execute(object parameter)
		{
			Person p = parameter as Person;
			if (p == null)
				return;
			int currentIndex = PersonsTestSource.IndexOf(p);
			PersonsTestSource.Move(currentIndex, currentIndex - 1);
		}

		private bool PersonItemMoveUpCommand_CanExecute(object arg)
		{
			return arg as Person != null && PersonsTestSource.IndexOf((Person)arg) > 0;
		}
		#endregion

		#region MessageBox
		private void ShowMessageBoxCommand_Execute(object obj)
		{
			MessageBoxResult = MessageBoxService.Show(obj as MessageBoxInfo);
		}

		private bool ShowMessageBoxCommand_CanExecute(object arg)
		{
			return arg as MessageBoxInfo != null;
		}
		#endregion

		#region CommandListBox
		private void CommandListBoxCommand_Execute(object obj)
		{
			SelectedDateIntervalPreset = (DateIntervalType)obj;
		}
		#endregion

		#region SearchAutocompleteControl
		private void ConversionSearchResultRefreshCommand_Execute(object obj)
		{
			string searchString = obj as string;
			if (String.IsNullOrWhiteSpace(searchString))
				StringSearchResults = new ObservableCollection<string>();
			else
			{
				searchString = searchString.ToLower();
				StringSearchResults = new ObservableCollection<string>(from cs in SearchAutocompleteTestSource_String
																	   where cs != null && cs.ToLower().Contains(searchString)
																	   select cs);
			}
		}

		private void CustomSearchItemSearchResultRefreshCommand_Execute(object obj)
		{
			string searchString = obj as string;
			if (String.IsNullOrWhiteSpace(searchString))
				CustomItemSearchResults = new ObservableCollection<CustomSearchItem>();
			else
			{
				searchString = searchString.ToLower();
				CustomItemSearchResults = new ObservableCollection<CustomSearchItem>(from cs in SearchAutocompleteTestSource_SearchItem
																					 where cs != null && cs.Description.ToLower().Contains(searchString)
																					 select cs);
			}
		}

		private void StringSearchCompleteCommand_Execute(object parameter)
		{
			SearchResultInfo info = (SearchResultInfo)parameter;
			StringSearchResultInfo = info;
		}

		private void CustomSearchCompleteCommand_Execute(object parameter)
		{
			SearchResultInfo info = (SearchResultInfo)parameter;
			CustomItemSearchResult = info;
		}
		#endregion

		#region InputDialog
		private void ShowAddNewFileInputDialogCommand_Execute(object arg)
		{
			InputDialogResult result = InputDialog.Show(arg as InputDialogInfo);
			if (result.ResultAction == InputDialogResultActionType.Positive)
				TestRemovableItemsItemsSource.Add(result.Value as FileItem);
		}

		private void ShowSearchInputBoxCommand_Execute(object arg)
		{
			InputDialogResult result = InputDialog.Show(arg as InputDialogInfo);
			if (result.ResultAction == InputDialogResultActionType.Positive)
			{
				vmSearchInputDialog resultValue = result.GetValueRefrenceAs<vmSearchInputDialog>();
				if (resultValue != null && resultValue.HasSearchResult)
					StringSearchResultInfo = resultValue.SearchResult;
			}
		}

		private void ShowTextInputDialogCommand_Execute(object parameter)
		{
			InputDialogResult result = InputDialog.Show(parameter as InputDialogInfo);
			if (result.ResultAction == InputDialogResultActionType.Positive)
			{
				vmTextInputDialog resultValue = result.GetValueRefrenceAs<vmTextInputDialog>();
				if (resultValue != null)
					TextInputDialogResult = resultValue.Text;
			}
		}
		#endregion

		#region HyperlinkButton
		private void HyperlinkTestCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter as string);
		}
		#endregion

		#region ImageResizeControl
		private void SelectImageSourceCommand_Execute(object obj)
		{
			openImageForResizeFileDialog.ShowDialog(this);
		}

		private void ResizeImageSourceCommand_Execute(object obj)
		{
			ImageResizeInfo iri = (ImageResizeInfo)obj;
		}

		private bool ResizeImageSourceCommand_CanExecute(object arg)
		{
			return arg is ImageResizeInfo && (ImageResizeInfo)arg != ImageResizeInfo.GetDefaultInfo(((ImageResizeInfo)arg).SourceImage);
		}
		#endregion

		#region ImageCropControl
		private void SelectCropImageSourceCommand_Execute(object obj)
		{
			openImageForCropFileDialog.ShowDialog(this);
		}

		private void CropImageCommand_Execute(object obj)
		{
			CropResultRect = (Int32Rect)obj;
		}
		#endregion

		#region RemovableItemItemsControl
		private void AddNewFileTestCommand_Execute(object parameter)
		{
			TestRemovableItemsItemsSource.Add(new FileItem() { Name = "new file #" + TestRemovableItemsItemsSource.Count, Path = AppDomain.CurrentDomain.BaseDirectory, Size_Mb = random.Next(256000, 512000) });
		}

		private void RemoveRequestTestCommand_Execute(object parameter)
		{
			FileItem fi = parameter as FileItem;
			if (fi != null && TestRemovableItemsItemsSource.Contains(fi))
			{
				string name = fi.Name;
				if (MessageBox.Show("Remove '" + name + "'?", "Confirm Item Removal", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					TestRemovableItemsItemsSource.Remove(parameter as FileItem);
			}
		}
		#endregion

		#region CommandTextBox
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

		#region PasswordBox
		private void UpdateUserPasswordCommand_Execute(object obj)
		{
			MessageBoxService.Show(new MessageBoxInfo { Content = String.Format("User password was changed to '{0}'", UserPasswordController.GetPassword()), Buttons = MessageBoxButton.OK });
		}

		private bool UpdateUserPasswordCommand_CanExecute(object arg)
		{
			return UserPasswordController.IsPasswordAvailable;
		}
		#endregion

		#region ProgressBar
		private async void StartProgressTestCommand_Execute(object obj)
		{
			var progress = new Progress<int>(value => ProgressBarValueTest = value);
			await Task.Run(() =>
			{
				Dispatcher.Invoke(() => { IsInProgress = true; CommandManager.InvalidateRequerySuggested(); }, System.Windows.Threading.DispatcherPriority.Background);
				for (int i = 0; i < 100; i++)
				{
					((IProgress<int>)progress).Report(i);
					Thread.Sleep(50);
				}
				Dispatcher.Invoke(() => { IsInProgress = false; CommandManager.InvalidateRequerySuggested(); }, System.Windows.Threading.DispatcherPriority.Background);
			});
		}

		private bool StartProgressTestCommand_CanExecute(object arg)
		{
			return !IsInProgress;
		}

		private async void StartLongOperationCommand_Execute(object obj)
		{
			IsLongOperationInProgress = true;
			CommandManager.InvalidateRequerySuggested();
			await Task.Run(() =>
			{
				Thread.Sleep(random.Next(5000, 8000));
				Dispatcher.Invoke(() => { IsLongOperationInProgress = false; CommandManager.InvalidateRequerySuggested(); }, System.Windows.Threading.DispatcherPriority.Background);
			});
		}

		private bool StartLongOperationCommand_CanExecute(object arg)
		{
			return !IsLongOperationInProgress;
		}

		private async void StartVeryLongOperationCommand_Execute(object obj)
		{
			IsVeryLongOperationInProgress = true;
			CommandManager.InvalidateRequerySuggested();
			await Task.Run(() =>
			{
				Thread.Sleep(random.Next(15000, 20000));
				Dispatcher.Invoke(() => { IsVeryLongOperationInProgress = false; CommandManager.InvalidateRequerySuggested(); }, System.Windows.Threading.DispatcherPriority.Background);
			});
		}

		private bool StartVeryLongOperationCommand_CanExecute(object arg)
		{
			return !IsVeryLongOperationInProgress;
		}
		#endregion

		#region Menu commands
		private void OpenCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter.ToString());
		}

		private void AddCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter.ToString());
		}

		private void RemoveCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter.ToString());
		}

		private void ShowInExplorerCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter.ToString());
		}

		private bool ShowInExplorerCommand_CanExecute(object parameter)
		{
			return false;
		}

		private void ResolveNamesCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter.ToString());
		}

		private void ResolveSurnamesCommand_Execute(object parameter)
		{
			MessageBox.Show(parameter.ToString());
		}

		private void LaunchCommandLineCommand_Execute(object parameter)
		{
			Process.Start("cmd.exe");
		}
		#endregion
		#endregion
	}
}
using CroplandWpf.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.MVVM
{
	public class vmSearchInputDialog : DependencyObject
	{
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(vmSearchInputDialog), new PropertyMetadata());

		public IEnumerable SearchResults
		{
			get { return (IEnumerable)GetValue(SearchResultsProperty); }
			set { SetValue(SearchResultsProperty, value); }
		}
		public static readonly DependencyProperty SearchResultsProperty =
			DependencyProperty.Register("SearchResults", typeof(IEnumerable), typeof(vmSearchInputDialog), new PropertyMetadata());

		public DelegateCommand RefreshSearchResultsCommand
		{
			get { return (DelegateCommand)GetValue(RefreshSearchResultsCommandProperty); }
			set { SetValue(RefreshSearchResultsCommandProperty, value); }
		}
		public static readonly DependencyProperty RefreshSearchResultsCommandProperty =
			DependencyProperty.Register("RefreshSearchResultsCommand", typeof(DelegateCommand), typeof(vmSearchInputDialog), new PropertyMetadata());

		public DelegateCommand SearchCompleteCommand
		{
			get { return (DelegateCommand)GetValue(SearchCompleteCommandProperty); }
			private set { SetValue(SearchCompleteCommandProperty, value); }
		}
		public static readonly DependencyProperty SearchCompleteCommandProperty =
			DependencyProperty.Register("SearchCompleteCommand", typeof(DelegateCommand), typeof(vmSearchInputDialog), new PropertyMetadata());

		public SearchResultInfo SearchResult
		{
			get { return (SearchResultInfo)GetValue(SearchResultProperty); }
			set { SetValue(SearchResultProperty, value); }
		}
		public static readonly DependencyProperty SearchResultProperty =
			DependencyProperty.Register("SearchResult", typeof(SearchResultInfo), typeof(vmSearchInputDialog), new PropertyMetadata());

		public Func<object, IEnumerable, IEnumerable> RefreshSearchResults_ExecuteFunction
		{
			get { return (Func<object, IEnumerable, IEnumerable>)GetValue(RefreshSearchResults_ExecuteFunctionProperty); }
			set { SetValue(RefreshSearchResults_ExecuteFunctionProperty, value); }
		}
		public static readonly DependencyProperty RefreshSearchResults_ExecuteFunctionProperty =
			DependencyProperty.Register("RefreshSearchResults_ExecuteFunction", typeof(Func<object, IEnumerable, IEnumerable>), typeof(vmSearchInputDialog), new PropertyMetadata());

		public Action<object> SearchCompleteAction
		{
			get { return (Action<object>)GetValue(SearchCompleteActionProperty); }
			set { SetValue(SearchCompleteActionProperty, value); }
		}
		public static readonly DependencyProperty SearchCompleteActionProperty =
			DependencyProperty.Register("SearchCompleteAction", typeof(Action<object>), typeof(vmSearchInputDialog), new PropertyMetadata());

		public bool HasSearchResult
		{
			get { return SearchResult != default(SearchResultInfo); }
		}

		public vmSearchInputDialog()
		{
			RefreshSearchResultsCommand = new DelegateCommand(RefreshSearchResultsCommand_Execute);
			SearchCompleteCommand = new DelegateCommand(SearchCompleteCommand_Execute);
		}

		private void SearchCompleteCommand_Execute(object parameter)
		{
			SearchResult = (SearchResultInfo)parameter;
			if (SearchCompleteAction != null)
				SearchCompleteAction.Invoke(parameter);
		}

		private void RefreshSearchResultsCommand_Execute(object parameter)
		{
			if (RefreshSearchResults_ExecuteFunction != null)
				SearchResults = RefreshSearchResults_ExecuteFunction.Invoke(parameter, ItemsSource);
		}
	}
}
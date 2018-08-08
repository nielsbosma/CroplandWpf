using CroplandWpf.Exceptions;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CroplandWpf.Components
{
	public class SearchAutocmpleteItem : Button
	{
		#region Routed events
		public static readonly RoutedEvent ClickedEvent =
			EventManager.RegisterRoutedEvent("Clicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchAutocmpleteItem));
		public event RoutedEventHandler Clicked
		{
			add { AddHandler(ClickedEvent, value); }
			remove { RemoveHandler(ClickedEvent, value); }
		}

		public static readonly RoutedEvent SelectedEvent =
			EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchAutocmpleteItem));
		public event RoutedEventHandler Selected
		{
			add { AddHandler(SelectedEvent, value); }
			remove { RemoveHandler(SelectedEvent, value); }
		}
		#endregion

		public string BindingPath
		{
			get { return (string)GetValue(BindingPathProperty); }
			set { SetValue(BindingPathProperty, value); }
		}
		public static readonly DependencyProperty BindingPathProperty =
			DependencyProperty.Register("BindingPath", typeof(string), typeof(SearchAutocmpleteItem), new PropertyMetadata());

		public string DisplayString
		{
			get { return (string)GetValue(DisplayStringProperty); }
			set { SetValue(DisplayStringProperty, value); }
		}
		public static readonly DependencyProperty DisplayStringProperty =
			DependencyProperty.Register("DisplayString", typeof(string), typeof(SearchAutocmpleteItem), new PropertyMetadata());

		public Brush HighlightBrush
		{
			get { return (Brush)GetValue(HighlightBrushProperty); }
			set { SetValue(HighlightBrushProperty, value); }
		}
		public static readonly DependencyProperty HighlightBrushProperty =
			DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(SearchAutocmpleteItem), new PropertyMetadata());

		public string HighlightString
		{
			get { return (string)GetValue(HighlightStringProperty); }
			set { SetValue(HighlightStringProperty, value); }
		}
		public static readonly DependencyProperty HighlightStringProperty =
			DependencyProperty.Register("HighlightString", typeof(string), typeof(SearchAutocmpleteItem), new PropertyMetadata());

		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}
		public static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(SearchAutocmpleteItem), new PropertyMetadata());

		private TextBlock displayTextBlock;

		private List<Run> runsToRender
		{
			get
			{
				return _runsToRender;
			}
			set
			{
				_runsToRender = value;
				if (displayTextBlock != null)
				{
					displayTextBlock.Inlines.Clear();
					displayTextBlock.Inlines.AddRange(value);
				}
			}
		}
		private List<Run> _runsToRender;

		static SearchAutocmpleteItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchAutocmpleteItem), new FrameworkPropertyMetadata(typeof(SearchAutocmpleteItem)));
		}

		public SearchAutocmpleteItem()
		{
			SetBinding(CommandParameterProperty, new Binding { Source = this, Path = new PropertyPath(ContentProperty), Mode = BindingMode.OneWay });
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			displayTextBlock = Template.FindName("PART_TextBlock", this) as TextBlock;
			if (displayTextBlock == null)
				throw new TemplatePartNotFoundException("PART_TextBlock", GetType());
			displayTextBlock.Inlines.AddRange(runsToRender);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DataContextProperty)
			{
				if (e.NewValue != null)
				{
					if (BindingPath != null)
						SetBinding(DisplayStringProperty, new Binding { Source = DataContext, Path = new PropertyPath(BindingPath) });
					else
						SetBinding(DisplayStringProperty, new Binding { Source = DataContext });
				}
				else
					DisplayString = "";
			}
			if(e.Property == BindingPathProperty)
			{
				if(DataContext != null)
				{
					if (e.NewValue != null)
						SetBinding(DisplayStringProperty, new Binding { Source = DataContext, Path = new PropertyPath(BindingPath) });
					else
						SetBinding(DisplayStringProperty, new Binding { Source = DataContext });
				}
			}
			if (e.Property == DisplayStringProperty || e.Property == HighlightStringProperty)
				RefreshHighlight();
			if (e.Property == IsMouseOverProperty && (bool)e.NewValue)
				IsSelected = true;
			if (e.Property == IsSelectedProperty && (bool)e.NewValue)
			{
				RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
				BringIntoView();
			}
		}

		protected override void OnClick()
		{
			base.OnClick();
			Dispatcher.Invoke(new Action(() => RaiseEvent(new RoutedEventArgs(ClickedEvent, this))));
		}

		private void RefreshHighlight()
		{
			List<Run> runs = new List<Run>();
			if (DisplayString != null && HighlightString != null)
			{
				string displayStringLower = DisplayString.ToLower();
				string highlightStringLower = HighlightString.ToLower().Trim();
				int highlightStartIndex = displayStringLower.IndexOf(highlightStringLower);
				if (highlightStartIndex > 0)
				{
					runs.Add(GenerateRun(DisplayString.Substring(0, highlightStartIndex)));
					runs.Add(GenerateRun(DisplayString.Substring(highlightStartIndex, highlightStringLower.Length), true));
					if (highlightStartIndex + highlightStringLower.Length <= DisplayString.Length)
						runs.Add(GenerateRun(DisplayString.Substring(highlightStartIndex + highlightStringLower.Length, DisplayString.Length - highlightStartIndex - highlightStringLower.Length)));
				}
				else if (highlightStartIndex == 0)
				{
					runs.Add(GenerateRun(DisplayString.Substring(0, highlightStringLower.Length), true));
					if (highlightStringLower.Length + highlightStartIndex < displayStringLower.Length)
						runs.Add(GenerateRun(DisplayString.Substring(highlightStringLower.Length, DisplayString.Length - highlightStringLower.Length)));
				}
			}
			runsToRender = runs;
		}

		private Run GenerateRun(string text, bool isHighlighted = false)
		{
			Run result = new Run { Text = text };
			if (!isHighlighted)
				result.Foreground = HighlightBrush;
			else
				result.FontWeight = FontWeights.Bold;
			return result;
		}

		public override string ToString()
		{
			return String.Format("{0} -> {1} ({2})", Content, DisplayString, runsToRender.Count);
		}
	}
}
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CroplandWpf.Components
{
	public enum MessageBoxControlButtonActionType
	{
		Positive,
		Negative,
		Close
	}

	public class MessageBoxWindow : Window
	{
		public object IconKey
		{
			get { return (object)GetValue(IconKeyProperty); }
			set { SetValue(IconKeyProperty, value); }
		}
		public static readonly DependencyProperty IconKeyProperty =
			DependencyProperty.Register("IconKey", typeof(object), typeof(MessageBoxWindow), new PropertyMetadata());

		public Brush IconBrush
		{
			get { return (Brush)GetValue(IconBrushProperty); }
			set { SetValue(IconBrushProperty, value); }
		}
		public static readonly DependencyProperty IconBrushProperty =
			DependencyProperty.Register("IconBrush", typeof(Brush), typeof(MessageBoxWindow), new PropertyMetadata());

		public object Footer
		{
			get { return (object)GetValue(FooterProperty); }
			set { SetValue(FooterProperty, value); }
		}
		public static readonly DependencyProperty FooterProperty =
			DependencyProperty.Register("Footer", typeof(object), typeof(MessageBoxWindow), new PropertyMetadata());

		public DataTemplate FooterTemplate
		{
			get { return (DataTemplate)GetValue(FooterTemplateProperty); }
			set { SetValue(FooterTemplateProperty, value); }
		}
		public static readonly DependencyProperty FooterTemplateProperty =
			DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(MessageBoxWindow), new PropertyMetadata());

		public MessageBoxResult Result
		{
			get { return (MessageBoxResult)GetValue(ResultProperty); }
			private set { SetValue(ResultProperty, value); }
		}
		public static readonly DependencyProperty ResultProperty =
			DependencyProperty.Register("Result", typeof(MessageBoxResult), typeof(MessageBoxWindow), new PropertyMetadata());

		public MessageBoxInfo Info
		{
			get { return (MessageBoxInfo)GetValue(InfoProperty); }
			set { SetValue(InfoProperty, value); }
		}
		public static readonly DependencyProperty InfoProperty =
			DependencyProperty.Register("Info", typeof(MessageBoxInfo), typeof(MessageBoxWindow), new PropertyMetadata());

		public DelegateCommand CloseRequestCommand
		{
			get { return (DelegateCommand)GetValue(CloseRequestCommandProperty); }
			private set { SetValue(CloseRequestCommandProperty, value); }
		}
		public static readonly DependencyProperty CloseRequestCommandProperty =
			DependencyProperty.Register("CloseRequestCommand", typeof(DelegateCommand), typeof(MessageBoxWindow), new PropertyMetadata());

		#region Control buttons properties
		public Visibility PositiveActionButtonVisibility
		{
			get { return (Visibility)GetValue(PositiveActionButtonVisibilityProperty); }
			private set { SetValue(PositiveActionButtonVisibilityProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionButtonVisibilityProperty =
			DependencyProperty.Register("PositiveActionButtonVisibility", typeof(Visibility), typeof(MessageBoxWindow), new PropertyMetadata());

		public Visibility NegativeActionButtonVisibility
		{
			get { return (Visibility)GetValue(NegativeActionButtonVisibilityProperty); }
			private set { SetValue(NegativeActionButtonVisibilityProperty, value); }
		}
		public static readonly DependencyProperty NegativeActionButtonVisibilityProperty =
			DependencyProperty.Register("NegativeActionButtonVisibility", typeof(Visibility), typeof(MessageBoxWindow), new PropertyMetadata());

		public Visibility CancelActionButtonVisibility
		{
			get { return (Visibility)GetValue(CancelActionButtonVisibilityProperty); }
			private set { SetValue(CancelActionButtonVisibilityProperty, value); }
		}
		public static readonly DependencyProperty CancelActionButtonVisibilityProperty =
			DependencyProperty.Register("CancelActionButtonVisibility", typeof(Visibility), typeof(MessageBoxWindow), new PropertyMetadata());

		public string PositiveActionButtonHeader
		{
			get { return (string)GetValue(PositiveActionButtonHeaderProperty); }
			private set { SetValue(PositiveActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionButtonHeaderProperty =
			DependencyProperty.Register("PositiveActionButtonHeader", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata());

		public string NegativeActionButtonHeader
		{
			get { return (string)GetValue(NegativeActionButtonHeaderProperty); }
			private set { SetValue(NegativeActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty NegativeActionButtonHeaderProperty =
			DependencyProperty.Register("NegativeActionButtonHeader", typeof(string), typeof(MessageBoxWindow), new PropertyMetadata());

		public DelegateCommand ControlButtonCommand
		{
			get { return (DelegateCommand)GetValue(ControlButtonCommandProperty); }
			private set { SetValue(ControlButtonCommandProperty, value); }
		}
		public static readonly DependencyProperty ControlButtonCommandProperty =
			DependencyProperty.Register("ControlButtonCommand", typeof(DelegateCommand), typeof(MessageBoxWindow), new PropertyMetadata());
		#endregion

		static MessageBoxWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxWindow), new FrameworkPropertyMetadata(typeof(MessageBoxWindow)));
		}

		public MessageBoxWindow()
		{
			ControlButtonCommand = new DelegateCommand(ControlButtonCommand_Execute, ControlButtonCommand_CanExecute);
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == InfoProperty)
			{
				if(e.NewValue != null)
				{
					if (Info.Resources.Count > 0)
						foreach (object key in Info.Resources.Keys)
							Resources.Add(key, Info.Resources[key]);
					Title = Info.Header;
					Content = Info.Content;
					Footer = Info.Footer;
					if (Info.IconBrushKey != null)
						SetResourceReference(IconBrushProperty, Info.IconBrushKey);
					if (Info.ContentTemplateKey != null)
						SetResourceReference(ContentTemplateProperty, Info.ContentTemplateKey);
					if (Info.FooterTemplateKey != null)
						SetResourceReference(FooterTemplateProperty, Info.FooterTemplateKey);
					if(Info.Buttons == MessageBoxButton.OK)
					{
						PositiveActionButtonVisibility = Visibility.Visible;
						NegativeActionButtonVisibility = Visibility.Collapsed;
						CancelActionButtonVisibility = Visibility.Collapsed;
						PositiveActionButtonHeader = "OK";
					}
					else if(Info.Buttons == MessageBoxButton.OKCancel)
					{
						PositiveActionButtonVisibility = Visibility.Visible;
						CancelActionButtonVisibility = Visibility.Visible;
						NegativeActionButtonVisibility = Visibility.Collapsed;
						PositiveActionButtonHeader = "OK";
					}
					else if(Info.Buttons == MessageBoxButton.YesNo)
					{
						PositiveActionButtonHeader = "Yes";
						PositiveActionButtonVisibility = Visibility.Visible;
						NegativeActionButtonHeader = "No";
						NegativeActionButtonVisibility = Visibility.Visible;
						CancelActionButtonVisibility = Visibility.Collapsed;
					}
					else if(Info.Buttons == MessageBoxButton.YesNoCancel)
					{
						PositiveActionButtonHeader = "Yes";
						PositiveActionButtonVisibility = Visibility.Visible;
						NegativeActionButtonHeader = "No";
						NegativeActionButtonVisibility = Visibility.Visible;
						CancelActionButtonVisibility = Visibility.Visible;
					}
				}
				if (e.OldValue != null)
					Resources.Clear();
			}
		}

		private bool ControlButtonCommand_CanExecute(object arg)
		{
			//TODO
			return true;
		}

		private void ControlButtonCommand_Execute(object obj)
		{
			MessageBoxControlButtonActionType actionType = (MessageBoxControlButtonActionType)obj;
			switch (actionType)
			{
				case MessageBoxControlButtonActionType.Close:
					Result = MessageBoxResult.None;
					break;
				case MessageBoxControlButtonActionType.Negative:
					if (Info.Buttons == MessageBoxButton.YesNo || Info.Buttons == MessageBoxButton.YesNoCancel)
						Result = MessageBoxResult.No;
					else if (Info.Buttons == MessageBoxButton.OKCancel)
						Result = MessageBoxResult.Cancel;
					break;
				case MessageBoxControlButtonActionType.Positive:
					if (Info.Buttons == MessageBoxButton.YesNo || Info.Buttons == MessageBoxButton.YesNoCancel)
						Result = MessageBoxResult.Yes;
					else if (Info.Buttons == MessageBoxButton.OK || Info.Buttons == MessageBoxButton.OKCancel)
						Result = MessageBoxResult.OK;
					break;
			}
			Close();
		}

		public void Show(MessageBoxInfo info)
		{
			Info = info;
			ShowDialog();
		}
	}

	public enum MessageBoxType
	{
		None,
		Question,
		Information,
		Warning,
		Exception
	}

	public class MessageBoxInfo : FrameworkElement
	{
		public string Header { get; set; }
		public object Content { get; set; }
		public object Footer { get; set; }
		public object FooterTemplateKey { get; set; }
		public object ContentTemplateKey { get; set; }
		public object IconBrushKey { get; set; }
		public string ScopeName { get; set; }
		public Action OkAction { get; set; }
		public Action CancelAction { get; set; }
		public MessageBoxButton Buttons { get; set; }

		public MessageBoxInfo()
		{
			Header = "Message";
			Content = "Message content";
			Footer = null;
			FooterTemplateKey = null;
			ContentTemplateKey = null;
			Buttons = MessageBoxButton.OK;
		}

		public static MessageBoxInfo DefaultQuestionInfo
		{
			get { return new MessageBoxInfo { IconBrushKey = "" }; }
		}

		public static MessageBoxInfo GetDefaultInfo(MessageBoxType type, string header, string content, Action positiveAction = null, Action negativeAction = null)
		{
			MessageBoxInfo result = new MessageBoxInfo();
			result.Header = header;
			result.Content = content;
			result.OkAction = positiveAction;
			result.CancelAction = negativeAction;
			switch (type)
			{
				case MessageBoxType.Question:
					result.IconBrushKey = MessageBoxIconBrushDefaultKeys.Question;
					result.ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Question;
					break;
			}
			return result;
		}
	}

	public static class MessageBoxIconBrushDefaultKeys
	{
		public static string Warning { get; } = "brushIcon_MessageBox_Info";
		public static string Error { get; } = "brushIcon_MessageBox_Error";
		public static string Exception { get; } = "brushIcon_MessageBox_Error";
		public static string Question { get; } = "brushIcon_MessageBox_Question";
	}

	public static class MessageBoxContentTemplateDefaultKeys
	{
		public static string Info { get; } = "templateMessageBoxContent_Info";
		public static string Warning { get; } = "templateMessageBoxContent_Warning";
		public static string Error { get; } = "templateMessageBoxContent_Error";
		public static string Exception { get; } = "templateMessageBoxContent_Exception";
		public static string Question { get; } = "templateMessageBoxContent_Question";
	}

	public class ExceptionInfo
	{
		public string Name { get; set; }
		public string Exception { get; set; }
		public string Message { get; set; }
	}
}

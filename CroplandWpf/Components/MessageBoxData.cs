﻿using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public enum MessageBoxType
	{
		None,
		Question,
		Information,
		Warning,
		Exception
	}

	public class MessageBoxInfo : Freezable
	{
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(MessageBoxInfo), new PropertyMetadata());

		public object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(MessageBoxInfo), new PropertyMetadata());

		public object AdditionalContentTemplateKey
		{
			get { return (object)GetValue(AdditionalContentTemplateKeyProperty); }
			set { SetValue(AdditionalContentTemplateKeyProperty, value); }
		}
		public static readonly DependencyProperty AdditionalContentTemplateKeyProperty =
			DependencyProperty.Register("AdditionalContentTemplateKey", typeof(object), typeof(MessageBoxInfo), new PropertyMetadata());

		public object Footer
		{
			get { return (object)GetValue(FooterProperty); }
			set { SetValue(FooterProperty, value); }
		}
		public static readonly DependencyProperty FooterProperty =
			DependencyProperty.Register("Footer", typeof(object), typeof(MessageBoxInfo), new PropertyMetadata());

		public object FooterTemplateKey
		{
			get { return (object)GetValue(FooterTemplateKeyProperty); }
			set { SetValue(FooterTemplateKeyProperty, value); }
		}
		public static readonly DependencyProperty FooterTemplateKeyProperty =
			DependencyProperty.Register("FooterTemplateKey", typeof(object), typeof(MessageBoxInfo), new PropertyMetadata());

		public object ContentTemplateKey
		{
			get { return (object)GetValue(ContentTemplateKeyProperty); }
			set { SetValue(ContentTemplateKeyProperty, value); }
		}
		public static readonly DependencyProperty ContentTemplateKeyProperty =
			DependencyProperty.Register("ContentTemplateKey", typeof(object), typeof(MessageBoxInfo), new PropertyMetadata());

		public object IconBrushKey
		{
			get { return (object)GetValue(IconBrushKeyProperty); }
			set { SetValue(IconBrushKeyProperty, value); }
		}
		public static readonly DependencyProperty IconBrushKeyProperty =
			DependencyProperty.Register("IconBrushKey", typeof(object), typeof(MessageBoxInfo), new PropertyMetadata());

		public string ScopeName
		{
			get { return (string)GetValue(ScopeNameProperty); }
			set { SetValue(ScopeNameProperty, value); }
		}
		public static readonly DependencyProperty ScopeNameProperty =
			DependencyProperty.Register("ScopeName", typeof(string), typeof(MessageBoxInfo), new PropertyMetadata());

		public Action<MessageBoxButton> Action
		{
			get { return (Action<MessageBoxButton>)GetValue(ActionProperty); }
			set { SetValue(ActionProperty, value); }
		}
		public static readonly DependencyProperty ActionProperty =
			DependencyProperty.Register("Action", typeof(Action<MessageBoxButton>), typeof(MessageBoxInfo), new PropertyMetadata());

		public MessageBoxButtons Buttons
		{
			get { return (MessageBoxButtons)GetValue(ButtonsProperty); }
			set { SetValue(ButtonsProperty, value); }
		}
		public static readonly DependencyProperty ButtonsProperty =
			DependencyProperty.Register("Buttons", typeof(MessageBoxButtons), typeof(MessageBoxInfo), new PropertyMetadata());

		public MessageBoxFooterButtonsCollection FooterButtons
		{
			get { return (MessageBoxFooterButtonsCollection)GetValue(FooterButtonsProperty); }
			set { SetValue(FooterButtonsProperty, value); }
		}
		public static readonly DependencyProperty FooterButtonsProperty =
			DependencyProperty.Register("FooterButtons", typeof(MessageBoxFooterButtonsCollection), typeof(MessageBoxInfo), new PropertyMetadata());

		public ResourceDictionary Resources
		{
			get { return (ResourceDictionary)GetValue(ResourcesProperty); }
			set { SetValue(ResourcesProperty, value); }
		}
		public static readonly DependencyProperty ResourcesProperty =
			DependencyProperty.Register("Resources", typeof(ResourceDictionary), typeof(MessageBoxInfo), new PropertyMetadata());

		public static MessageBoxInfo New(MessageBoxType type, string header, string content, MessageBoxFooterButtonsCollection footerButtons = null, Action<MessageBoxButton> action = null)
		{
			MessageBoxInfo result = new MessageBoxInfo
			{
				Title = header,
				Content = content,
				Action = action,
				FooterButtons = footerButtons
			};

			switch (type)
			{
				case MessageBoxType.Question:
					result.IconBrushKey = MessageBoxIconBrushDefaultKeys.Question;
					result.Buttons = MessageBoxButtons.YesNo;
					break;
				case MessageBoxType.Exception:
					result.IconBrushKey = MessageBoxIconBrushDefaultKeys.Exception;
					result.ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Exception;
					result.Buttons = MessageBoxButtons.OK;
					break;
				case MessageBoxType.Information:
					result.IconBrushKey = MessageBoxIconBrushDefaultKeys.Information;
                    result.Buttons = MessageBoxButtons.OK;
					break;
				case MessageBoxType.Warning:
					result.IconBrushKey = MessageBoxIconBrushDefaultKeys.Warning;
                    result.Buttons = MessageBoxButtons.OK;
					break;
			}
			return result;
		}

		public MessageBoxInfo()
		{
			Resources = new ResourceDictionary();
			Title = "Message";
			Content = "Message content";
			Footer = null;
			FooterTemplateKey = null;
            ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Simple;
            Buttons = new MessageBoxButtons();
		}

		public void ExecuteAction(MessageBoxButton actionButton)
		{
			if (Action != null)
				Action.Invoke(actionButton);
		}

		public List<MessageBoxFooterButton> GetFooterButtonsFinal()
		{
			if (!IsFrozen && FooterButtons == null)
				FooterButtons = new MessageBoxFooterButtonsCollection();
			return FooterButtons;
		}

		protected override Freezable CreateInstanceCore()
		{
			return new MessageBoxInfo();
		}

		protected override bool FreezeCore(bool isChecking)
		{
			//if (FooterButtons != null)
			//{
			//	if (FooterButtons.Count > 0)
			//		FooterButtons.ForEach(fb => fb.Freeze());
			//}
			if (FooterButtons == null)
				FooterButtons = new MessageBoxFooterButtonsCollection();
			return base.FreezeCore(isChecking);
		}
	}

	public static class MessageBoxIconBrushDefaultKeys
	{
		public static string Warning { get; } = "brushIcon_Alert";
		public static string Error { get; } = "brushIcon_Alert";
		public static string Exception { get; } = "brushIcon_Bug";
		public static string Question { get; } = "brushIcon_Help";
		public static string Information { get; } = "brushIcon_Info";
	}

	public static class MessageBoxContentTemplateDefaultKeys
	{
		public static string Simple { get; } = "templateMessageBoxContent_Simple";
		public static string Exception { get; } = "templateMessageBoxContent_Exception";
	}

	public static class MessageBoxAdditionalContentTemplateDefaultKeys
	{
		public static string Exception { get; } = "templateMessageBoxContent_Exception_AdditionalContent";
	}

	public class ExceptionInfo
	{
		public string Name { get; set; }
		public string Exception { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
        public string FilePath { get; set; }

        public ExceptionInfo()
		{

		}

		public ExceptionInfo(string name, string exception, string message, string stackTrace, string filePath = null)
		{
			Name = name;
			Exception = exception;
			Message = message;
			StackTrace = stackTrace.Trim();
            FilePath = filePath;
        }

		public ExceptionInfo(Exception exception)
		{
			Name = exception.ToString();
			Exception = exception.InnerException.ToString();
			Message = exception.Message;
			StackTrace = exception.StackTrace;
		}
	}

	[TypeConverter(typeof(MessageBoxButtonTypeConverter))]
	public struct MessageBoxButton
	{
		public static bool GetIsPrimaryAttached(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsPrimaryAttachedProperty);
		}
		public static void SetIsPrimaryAttached(DependencyObject obj, bool value)
		{
			obj.SetValue(IsPrimaryAttachedProperty, value);
		}
		public static readonly DependencyProperty IsPrimaryAttachedProperty =
			DependencyProperty.RegisterAttached("IsPrimaryAttached", typeof(bool), typeof(MessageBoxButton), new PropertyMetadata());

		#region Standard buttons
		public static MessageBoxButton OK
		{
			get { return new MessageBoxButton("OK", true); }
		}

		public static MessageBoxButton Cancel
		{
			get { return new MessageBoxButton("Cancel"); }
		}

		public static MessageBoxButton Yes
		{
			get { return new MessageBoxButton("Yes", true); }
		}

		public static MessageBoxButton No
		{
			get { return new MessageBoxButton("No"); }
		}

		public static MessageBoxButton YesToAll
		{
			get { return new MessageBoxButton("Yes To All", true); }
		}

		public static MessageBoxButton NoToAll
		{
			get { return new MessageBoxButton("No To All"); }
		}

		public static MessageBoxButton Close
		{
			get { return new MessageBoxButton("Close"); }
		}
		#endregion

		public string Header { get; private set; }

		public bool IsPrimary { get; private set; }

		public MessageBoxButton(string header = "", bool isPrimary = false)
		{
			Header = String.IsNullOrEmpty(header) ? String.Empty : header;
			IsPrimary = isPrimary;
		}

		public MessageBoxButton MakePrimary()
		{
			return new MessageBoxButton(Header, true);
		}

		public override bool Equals(object obj)
		{
			if (obj is MessageBoxButton other)
			{
				if (Header == null)
					return other.Header == null;
				if (other.Header == null)
					return Header == null;
				return Header.ToLowerInvariant().Equals(other.Header.ToLowerInvariant());
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return Header.GetHashCode();
		}

		public static bool operator !=(object mbb1, MessageBoxButton mbb2)
		{
			if (mbb1 == null)
				return false;
			if (mbb1 is MessageBoxButton mbb_1)
				return !mbb1.Equals(mbb2);
			return false;
		}

		public static bool operator ==(object mbb1, MessageBoxButton mbb2)
		{
			if (mbb1 == null)
				return false;
			if (mbb1 is MessageBoxButton mbb_1)
				return mbb1.Equals(mbb2);
			return false;
		}

		public override string ToString()
		{
			return Header;
		}
	}

	[TypeConverter(typeof(MessageBoxButtonsTypeConverter))]
	public class MessageBoxButtons : List<MessageBoxButton>
	{
		public static MessageBoxButtons OK
		{
			get
			{
				return new MessageBoxButtons { MessageBoxButton.OK };
			}
		}

		public static MessageBoxButtons OKCancel
		{
			get
			{
				return new MessageBoxButtons { MessageBoxButton.OK, MessageBoxButton.Cancel };
			}
		}

		public static MessageBoxButtons YesNo
		{
			get { return new MessageBoxButtons { MessageBoxButton.Yes, MessageBoxButton.No }; }
		}

		public static MessageBoxButtons YesNoCancel
		{
			get { return new MessageBoxButtons { MessageBoxButton.Yes, MessageBoxButton.No, MessageBoxButton.Cancel }; }
		}

		public MessageBoxButtons()
		{

		}

		public MessageBoxButtons(params string[] buttonHeaders)
		{
			foreach (string buttonHeader in buttonHeaders)
				Add(new MessageBoxButton(buttonHeader));
		}

		public MessageBoxButtons(params MessageBoxButton[] buttons)
		{
			foreach (MessageBoxButton button in buttons)
				Add(button);
		}

		public static MessageBoxButtons CreateNew(params string[] buttonHeaders)
		{
			return new MessageBoxButtons(buttonHeaders);
		}

		public static MessageBoxButtons CreateNew(params MessageBoxButton[] buttons)
		{
			return new MessageBoxButtons(buttons);
		}

		public List<MessageBoxButton> GetFinalButtonsList()
		{
			if (Count == 0)
				Add(MessageBoxButton.OK);
			if (!this.Any(mbb => mbb.IsPrimary))
			{
				MessageBoxButton mbb = this.First();
				mbb = mbb.MakePrimary();
				RemoveAt(0);
				Insert(0, mbb);
			}
			return new List<MessageBoxButton>(this);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int count = 0; count < Count; count++)
			{
				sb.Append(this[count].Header);
				if (count > 0 && count < Count - 1)
					sb.Append(';');
			}
			return sb.ToString();
		}
	}

	public class MessageBoxButtonTypeConverter : TypeConverter
	{
		private StandardValuesCollection standardValues
		{
			get
			{
				return new StandardValuesCollection(new[] { MessageBoxButton.OK, MessageBoxButton.Cancel, MessageBoxButton.Yes, MessageBoxButton.No, MessageBoxButton.YesToAll, MessageBoxButton.NoToAll });
			}
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType.Equals(typeof(string));
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType.Equals(typeof(string));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string sValue)
			{
				return new MessageBoxButton(sValue);
			}
			return null;
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is MessageBoxButton mbb)
			{
				return mbb.Header;
			}
			return null;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return standardValues;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}

	public class MessageBoxButtonsTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType.Equals(typeof(string));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string sValue)
				return new MessageBoxButtons(sValue.Split(';'));
			return null;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType.Equals(typeof(string));
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType.Equals(typeof(string)) && value is MessageBoxButtons mbbc)
				return mbbc.ToString();
			else
				return null;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return new StandardValuesCollection(new[] { MessageBoxButtons.OK, MessageBoxButtons.OKCancel, MessageBoxButtons.YesNo, MessageBoxButtons.YesNoCancel });
		}
	}

	public class MessageBoxFooterButton : INotifyPropertyChanged
	{
		public string Header { get; set; }

		public Action<object> CommandExecute { get; set; }

		public ICommand Command { get; set; }

		public object CommandParameter { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

        public bool CloseWindowOnCommand { get; set; }

		public MessageBoxFooterButton()
		{

		}

		public MessageBoxFooterButton(string header, Action<object> commandExecute = null, object commandParameter = null, bool closeWindowOnCommand = true)
		{
			Header = header;
			CommandExecute = commandExecute;
			CommandParameter = commandParameter ?? this;
            CloseWindowOnCommand = closeWindowOnCommand;
			if (CommandExecute != null)
				Command = new DelegateCommand(CommandExecute);
		}

		//protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		//{
		//	base.OnPropertyChanged(e);
		//	if (e.Property == CommandExecuteProperty && e.NewValue != null)
		//		Command = new DelegateCommand(CommandExecute);
		//}

		//protected override Freezable CreateInstanceCore()
		//{
		//	return new MessageBoxFooterButton();
		//}
	}

	public class MessageBoxFooterButtonsCollection : List<MessageBoxFooterButton>
	{
		public static MessageBoxFooterButtonsCollection OptOut(Action<object> optOutAction, string optOutButtonHeader = "Don`t show again", object commandParameter = null)
		{
			return Single(optOutButtonHeader, optOutAction, commandParameter);
		}

		public static MessageBoxFooterButtonsCollection New(params MessageBoxFooterButton[] buttons)
		{
			return new MessageBoxFooterButtonsCollection(buttons);
		}

		public static MessageBoxFooterButtonsCollection Single(string buttonHeader, Action<object> action, object commandParameter = null, bool closeWindowOnCommand = true)
		{
			return new MessageBoxFooterButtonsCollection { new MessageBoxFooterButton(buttonHeader, action, commandParameter, closeWindowOnCommand) };
		}

		public MessageBoxFooterButtonsCollection()
		{

		}

		public MessageBoxFooterButtonsCollection(MessageBoxFooterButton[] buttons) : base(buttons)
		{

		}
	}
}
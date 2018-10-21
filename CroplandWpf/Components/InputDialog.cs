using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using System;
using System.Windows;

namespace CroplandWpf.Components
{
	public enum InputDialogResultActionType
	{
		Close,
		Positive,
		Negative
	}

	public struct InputDialogResult
	{
		public InputDialogResultActionType ResultAction;
		public object Value;

		public InputDialogResult(InputDialogResultActionType actionType, object value)
		{
			ResultAction = actionType;
			Value = value;
		}

		public T GetValueRefrenceAs<T>() where T : class
		{
			if (Value as T != null)
				return (T)Value;
			else
				return null;
		}
	}

	public class InputDialog : Window
	{
		public static string DefaultWindowTitle = "Cropland";

		#region DPs
		public InputDialogInfo Info
		{
			get { return (InputDialogInfo)GetValue(InfoProperty); }
			set { SetValue(InfoProperty, value); }
		}
		public static readonly DependencyProperty InfoProperty =
			DependencyProperty.Register("Info", typeof(InputDialogInfo), typeof(InputDialog), new PropertyMetadata());

		public InputDialogResult Result
		{
			get { return (InputDialogResult)GetValue(ResultProperty); }
			set { SetValue(ResultProperty, value); }
		}
		public static readonly DependencyProperty ResultProperty =
			DependencyProperty.Register("Result", typeof(InputDialogResult), typeof(InputDialog), new PropertyMetadata());

		public object InputResult
		{
			get { return (object)GetValue(InputResultProperty); }
			set { SetValue(InputResultProperty, value); }
		}
		public static readonly DependencyProperty InputResultProperty =
			DependencyProperty.Register("InputResult", typeof(object), typeof(InputDialog), new PropertyMetadata());

		public Func<object, bool> PositiveActionCanExecute
		{
			get { return (Func<object, bool>)GetValue(PositiveActionCanExecuteProperty); }
			set { SetValue(PositiveActionCanExecuteProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionCanExecuteProperty =
			DependencyProperty.Register("PositiveActionCanExecute", typeof(Func<object, bool>), typeof(InputDialog), new PropertyMetadata());

		public string NegativeActionButtonHeader
		{
			get { return (string)GetValue(NegativeActionButtonHeaderProperty); }
			private set { SetValue(NegativeActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty NegativeActionButtonHeaderProperty =
			DependencyProperty.Register("NegativeActionButtonHeader", typeof(string), typeof(InputDialog), new PropertyMetadata());

		public string PositiveActionButtonHeader
		{
			get { return (string)GetValue(PositiveActionButtonHeaderProperty); }
			private set { SetValue(PositiveActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionButtonHeaderProperty =
			DependencyProperty.Register("PositiveActionButtonHeader", typeof(string), typeof(InputDialog), new PropertyMetadata());

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			private set { SetValue(HeaderProperty, value); }
		}
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(InputDialog), new PropertyMetadata());

		#region Commands
		public DelegateCommand ControlButtonCommand
		{
			get { return (DelegateCommand)GetValue(ControlButtonCommandProperty); }
			private set { SetValue(ControlButtonCommandProperty, value); }
		}
		public static readonly DependencyProperty ControlButtonCommandProperty =
			DependencyProperty.Register("ControlButtonCommand", typeof(DelegateCommand), typeof(InputDialog), new PropertyMetadata());

		public DelegateCommand PositiveActionCommand
		{
			get { return (DelegateCommand)GetValue(PositiveActionCommandProperty); }
			private set { SetValue(PositiveActionCommandProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionCommandProperty =
			DependencyProperty.Register("PositiveActionCommand", typeof(DelegateCommand), typeof(InputDialog), new PropertyMetadata());

		public DelegateCommand NegativeActionCommand
		{
			get { return (DelegateCommand)GetValue(NegativeActionCommandProperty); }
			private set { SetValue(NegativeActionCommandProperty, value); }
		}
		public static readonly DependencyProperty NegativeActionCommandProperty =
			DependencyProperty.Register("NegativeActionCommand", typeof(DelegateCommand), typeof(InputDialog), new PropertyMetadata());
		#endregion
		#endregion

		#region Static
		public static InputDialogResult Show(InputDialogInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			InputDialog dialog = new InputDialog
			{
				Info = info,
				Owner = WindowHelper.GetActiveWindowInstance()
			};
			dialog.ShowDialog();
			return dialog.Result;
		}

		public static string ShowStringInputPrompt(string windowTitle = null, string header = null, string defaultValue = null, Func<object, bool> validator = null)
		{
			InputDialogInfo info = new InputDialogInfo
			{
				WindowTitle = windowTitle ?? DefaultWindowTitle,
				Header = header,
				ContentType = typeof(InputDialogTextViewModel),
				ContentTemplateKey = InputDialogDefaultContentTemplateKeys.Text,
				CanPositiveActionExecute = validator,
				DefaultValue = defaultValue
			};
			InputDialog dialog = new InputDialog
			{
				Owner = WindowHelper.GetActiveWindowInstance()
			};
			InputDialogResult result = dialog.ShowDialog(info);

			return result.ResultAction == InputDialogResultActionType.Positive ?
				result.GetValueRefrenceAs<InputDialogTextViewModel>().ResultValue : null;
		}
		#endregion

		#region Ctor
		static InputDialog()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(InputDialog), new FrameworkPropertyMetadata(typeof(InputDialog)));
		}

		public InputDialog()
		{
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			ControlButtonCommand = new DelegateCommand(ControlButtonCommand_Execute, ControlButtonCommand_CanExecute);
			PositiveActionCommand = new DelegateCommand(PositiveActionCommand_Execute, PositiveActionCommand_CanExecute);
			NegativeActionCommand = new DelegateCommand(NegativeActionCommand_Execute, NegativeActionCommand_CanExecute);
		}
		#endregion

		#region Overrides
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == InfoProperty)
			{
				if (e.NewValue != null)
				{
					if (Info.Resources.Count > 0)
						foreach (object key in Info.Resources.Keys)
							Resources.Add(key, Info.Resources[key]);
					DataContext = Info.DataContext;
					Content = Info.GetContentInstance();
					PositiveActionCanExecute = Info.CanPositiveActionExecute;
					if (Info.ContentTemplateKey != null)
						SetResourceReference(ContentTemplateProperty, Info.ContentTemplateKey);
					NegativeActionButtonHeader = Info.NegativeActionButtonHeader;
					PositiveActionButtonHeader = Info.PositiveActionButtonHeader;
					Title = Info.WindowTitle;
					Header = Info.Header;
				}
				if (e.OldValue != null)
					Resources.Clear();
			}
		}
		#endregion

		public InputDialogResult ShowDialog(InputDialogInfo info)
		{
			Info = info;
			ShowDialog();
			return Result;
		}

		#region Commanding
		private void ControlButtonCommand_Execute(object arg)
		{
			InputDialogResultActionType actionType = (InputDialogResultActionType)arg;
			switch (actionType)
			{
				case InputDialogResultActionType.Negative:
					Result = new InputDialogResult(InputDialogResultActionType.Negative, null);
					break;
				case InputDialogResultActionType.Positive:
					Result = new InputDialogResult(InputDialogResultActionType.Positive, Content);
					break;
				default:
					break;
			}
			Close();
		}

		private bool ControlButtonCommand_CanExecute(object arg)
		{
			InputDialogResultActionType actionType = InputDialogResultActionType.Positive;
			try
			{
				actionType = (InputDialogResultActionType)arg;
			}
			catch { }

			if (actionType == InputDialogResultActionType.Positive && PositiveActionCanExecute != null && !PositiveActionCanExecute(Content))
				return false;

			return true;
		}

		private void NegativeActionCommand_Execute(object obj)
		{
			ControlButtonCommand.Execute(InputDialogResultActionType.Negative);
		}

		private bool NegativeActionCommand_CanExecute(object arg)
		{
			return ControlButtonCommand.CanExecute(InputDialogResultActionType.Negative);
		}

		private void PositiveActionCommand_Execute(object obj)
		{
			ControlButtonCommand.Execute(InputDialogResultActionType.Positive);
		}

		private bool PositiveActionCommand_CanExecute(object arg)
		{
			return ControlButtonCommand.CanExecute(InputDialogResultActionType.Positive);
		}
		#endregion
	}

	public static class InputDialogDefaultContentTemplateKeys
	{
		public static readonly string Text = "templateInputDialog_Text";
		public static readonly string Double = "templateInputDialog_Double";
	}

	public class InputDialogTextViewModel : InputDialogTypedViewModelBase<string>
	{

	}

	public class InputDialogTypedViewModelBase<TResultValue> : InputDialogViewModelBase
	{
		public TResultValue ResultValue
		{
			get { return (TResultValue)GetValue(ResultValueProperty); }
			set { SetValue(ResultValueProperty, value); }
		}
		public static readonly DependencyProperty ResultValueProperty =
			DependencyProperty.Register("ResultValue", typeof(TResultValue), typeof(InputDialogTypedViewModelBase<TResultValue>), new PropertyMetadata());

		protected override void DefaultValue_Changed(object newValue)
		{
			try
			{
				ResultValue = (TResultValue)newValue;
			}
			catch { }
		}

		public override object GetValue()
		{
			return ResultValue;
		}
	}

	public class InputDialogViewModelBase : Freezable
	{
		public object DefaultValue
		{
			get { return (object)GetValue(DefaultValueProperty); }
			set { SetValue(DefaultValueProperty, value); }
		}
		public static readonly DependencyProperty DefaultValueProperty =
			DependencyProperty.Register("DefaultValue", typeof(object), typeof(InputDialogViewModelBase), new PropertyMetadata((o, e) =>
			{
				if (o is InputDialogViewModelBase idvb)
					idvb.DefaultValue_Changed(e.NewValue);
			}));

		protected virtual void DefaultValue_Changed(object newValue)
		{

		}

		public virtual object GetValue()
		{
			throw new NotImplementedException();
		}

		protected override Freezable CreateInstanceCore()
		{
			return new InputDialogViewModelBase();
		}
	}

	public class InputDialogInfo : Freezable
	{
		public static InputDialogInfo GetTextInputPromptInfo(string defaultValue = null, Func<object, bool> validator = null)
		{
			InputDialogInfo result = new InputDialogInfo
			{
				ContentTemplateKey = InputDialogDefaultContentTemplateKeys.Text,
				ContentType = typeof(InputDialogTextViewModel),
				CanPositiveActionExecute = validator
			};
			return result;
		}

		public string WindowTitle
		{
			get { return (string)GetValue(WindowTitleProperty); }
			set { SetValue(WindowTitleProperty, value); }
		}
		public static readonly DependencyProperty WindowTitleProperty =
			DependencyProperty.Register("WindowTitle", typeof(string), typeof(InputDialogInfo), new PropertyMetadata("Input Dialog"));

		public object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(InputDialogInfo), new PropertyMetadata());

		public object ContentTemplateKey
		{
			get { return (object)GetValue(ContentTemplateKeyProperty); }
			set { SetValue(ContentTemplateKeyProperty, value); }
		}
		public static readonly DependencyProperty ContentTemplateKeyProperty =
			DependencyProperty.Register("ContentTemplateKey", typeof(object), typeof(InputDialogInfo), new PropertyMetadata());

		public object DefaultValue
		{
			get { return (object)GetValue(DefaultValueProperty); }
			set { SetValue(DefaultValueProperty, value); }
		}
		public static readonly DependencyProperty DefaultValueProperty =
			DependencyProperty.Register("DefaultValue", typeof(object), typeof(InputDialogInfo), new PropertyMetadata());

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(InputDialogInfo), new PropertyMetadata());

		public Type ContentType
		{
			get { return (Type)GetValue(ContentTypeProperty); }
			set { SetValue(ContentTypeProperty, value); }
		}
		public static readonly DependencyProperty ContentTypeProperty =
			DependencyProperty.Register("ContentType", typeof(Type), typeof(InputDialogInfo), new PropertyMetadata());

		public string PositiveActionButtonHeader
		{
			get { return (string)GetValue(PositiveActionButtonHeaderProperty); }
			set { SetValue(PositiveActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionButtonHeaderProperty =
			DependencyProperty.Register("PositiveActionButtonHeader", typeof(string), typeof(InputDialogInfo), new PropertyMetadata());

		public string NegativeActionButtonHeader
		{
			get { return (string)GetValue(NegativeActionButtonHeaderProperty); }
			set { SetValue(NegativeActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty NegativeActionButtonHeaderProperty =
			DependencyProperty.Register("NegativeActionButtonHeader", typeof(string), typeof(InputDialogInfo), new PropertyMetadata());

		public Func<object, bool> CanPositiveActionExecute
		{
			get { return (Func<object, bool>)GetValue(CanPositiveActionExecuteProperty); }
			set { SetValue(CanPositiveActionExecuteProperty, value); }
		}
		public static readonly DependencyProperty CanPositiveActionExecuteProperty =
			DependencyProperty.Register("CanPositiveActionExecute", typeof(Func<object, bool>), typeof(InputDialogInfo), new PropertyMetadata());

		public ResourceDictionary Resources
		{
			get { return (ResourceDictionary)GetValue(ResourcesProperty); }
			set { SetValue(ResourcesProperty, value); }
		}
		public static readonly DependencyProperty ResourcesProperty =
			DependencyProperty.Register("Resources", typeof(ResourceDictionary), typeof(InputDialogInfo), new PropertyMetadata());

		public object DataContext
		{
			get { return (object)GetValue(DataContextProperty); }
			set { SetValue(DataContextProperty, value); }
		}
		public static readonly DependencyProperty DataContextProperty =
			DependencyProperty.Register("DataContext", typeof(object), typeof(InputDialogInfo), new PropertyMetadata());

		public bool HasContentType
		{
			get { return ContentType != null; }
		}

		public InputDialogInfo()
		{
			PositiveActionButtonHeader = "OK";
			NegativeActionButtonHeader = "CANCEL";
			Resources = new ResourceDictionary();
		}

		public object GetContentInstance()
		{
			object result = HasContentType ? Activator.CreateInstance(ContentType):Content;
			if (result is InputDialogViewModelBase idvmb)
				idvmb.DefaultValue = DefaultValue;
			return result;
		}

		protected override Freezable CreateInstanceCore()
		{
			return new InputDialogInfo();
		}
	}
}
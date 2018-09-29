using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Func<InputDialogResultActionType, object, bool> PositiveActionCanExecute
		{
			get { return (Func<InputDialogResultActionType, object, bool>)GetValue(PositiveActionCanExecuteProperty); }
			set { SetValue(PositiveActionCanExecuteProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionCanExecuteProperty =
			DependencyProperty.Register("PositiveActionCanExecute", typeof(Func<InputDialogResultActionType, object, bool>), typeof(InputDialog), new PropertyMetadata());

		public string NegativeActionButtonHeader
		{
			get { return (string)GetValue(NegativeActionButtonHeaderProperty); }
			set { SetValue(NegativeActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty NegativeActionButtonHeaderProperty =
			DependencyProperty.Register("NegativeActionButtonHeader", typeof(string), typeof(InputDialog), new PropertyMetadata());

		public string PositiveActionButtonHeader
		{
			get { return (string)GetValue(PositiveActionButtonHeaderProperty); }
			set { SetValue(PositiveActionButtonHeaderProperty, value); }
		}
		public static readonly DependencyProperty PositiveActionButtonHeaderProperty =
			DependencyProperty.Register("PositiveActionButtonHeader", typeof(string), typeof(InputDialog), new PropertyMetadata());

		#region Commands
		public DelegateCommand ControlButtonCommand
		{
			get { return (DelegateCommand)GetValue(ControlButtonCommandProperty); }
			private set { SetValue(ControlButtonCommandProperty, value); }
		}
		public static readonly DependencyProperty ControlButtonCommandProperty =
			DependencyProperty.Register("ControlButtonCommand", typeof(DelegateCommand), typeof(InputDialog), new PropertyMetadata());
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
					Title = Info.Title;
				}
				if (e.OldValue != null)
					Resources.Clear();
			}
		}
		#endregion

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

			if (actionType == InputDialogResultActionType.Positive && PositiveActionCanExecute != null && !PositiveActionCanExecute(actionType, Content))
				return false;

			return true;
		}
		#endregion
	}

	public class InputDialogInfo : FrameworkElement
	{
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(InputDialogInfo), new PropertyMetadata("Input Dialog"));

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

		public Func<InputDialogResultActionType, object, bool> CanPositiveActionExecute
		{
			get { return (Func<InputDialogResultActionType, object, bool>)GetValue(CanPositiveActionExecuteProperty); }
			set { SetValue(CanPositiveActionExecuteProperty, value); }
		}
		public static readonly DependencyProperty CanPositiveActionExecuteProperty =
			DependencyProperty.Register("CanPositiveActionExecute", typeof(Func<InputDialogResultActionType, object, bool>), typeof(InputDialogInfo), new PropertyMetadata());

		public bool HasContentType
		{
			get { return ContentType != null; }
		}

		public InputDialogInfo()
		{
			PositiveActionButtonHeader = "OK";
			NegativeActionButtonHeader = "CANCEL";
		}

		public object GetContentInstance()
		{
			if (HasContentType)
				return Activator.CreateInstance(ContentType);
			else
				return Content;
		}
	}
}
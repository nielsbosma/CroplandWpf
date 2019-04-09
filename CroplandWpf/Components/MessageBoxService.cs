using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace CroplandWpf.Components
{
	public class MessageBoxService
	{
		public static string DefaultWindowTitle = "Cropland";

		private static MessageBoxWindow window;

		delegate void ShowInfoDelegate(MessageBoxInfo info);

		private static Dispatcher currentDispatcher
		{
			get { return Application.Current.Dispatcher; }
		}

		public static MessageBoxButton Show(MessageBoxInfo info)
		{
			if (info == null)
				return MessageBoxButton.Close;
			if (String.IsNullOrWhiteSpace(info.Title))
				info.Title = GetFinalWindowTitle();
			if (currentDispatcher.Thread != Thread.CurrentThread && info.CanFreeze)
			{
				info.Freeze();
			}
			DispatcherOperation<MessageBoxButton> op = currentDispatcher.InvokeAsync(
				new Func<MessageBoxButton>(() =>
				{
					window = GetWindow();
					window.Show(info);
					return window.Result;
				}),
				DispatcherPriority.Normal);
			return op.Result;
		}

		public static DelegateCommand ShowCommand { get; } = new DelegateCommand(ShowCommand_Execute);

		private static void ShowCommand_Execute(object obj)
		{
			if (obj is MessageBoxInfo info)
			{
				Show(info);
				info.ExecuteAction(window.Result);
			}
		}

		public static MessageBoxButton ShowInformation(string windowTitle, string infoText)
		{
			MessageBoxInfo info = new MessageBoxInfo()
			{
				Title = GetFinalWindowTitle(windowTitle),
				Buttons = MessageBoxButtons.OK,
				Content = infoText,
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Information
			};
			return Show(info);
		}

		public static MessageBoxButton ShowError(string windowTitle, string errorText)
		{
			MessageBoxInfo info = new MessageBoxInfo()
			{
				Title = GetFinalWindowTitle(windowTitle),
				Buttons = MessageBoxButtons.OK,
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Error,
				Content = errorText
			};
			return ShowWindow(info);
		}

		private static MessageBoxButton ShowWindow(MessageBoxInfo info)
		{
			if (String.IsNullOrWhiteSpace(info.Title))
				info.Title = DefaultWindowTitle;
			return Show(info);
		}

		private static string GetFinalWindowTitle(string customTitle = null)
		{
			return customTitle ?? DefaultWindowTitle;
		}

		private static MessageBoxWindow GetWindow()
		{
			window = new MessageBoxWindow
			{
				Owner = WindowHelper.GetActiveWindowInstance()
			};
			return window;
		}

		public static void ShowException(Exception exception, string windowTitle = null, string exceptionHeader = null, string exceptionMessageOverride = null, MessageBoxFooterButtonsCollection footerButtons = null)
		{
			string finalWindowTitle = windowTitle ?? DefaultWindowTitle;
			currentDispatcher.Invoke(new Action(() =>
			{
				MessageBoxInfo info = new MessageBoxInfo
				{
					Title = finalWindowTitle,
					Content = exception != null ?
					new ExceptionInfo(exceptionHeader, exception.GetType().Name, exceptionMessageOverride ?? exception.Message, exception.StackTrace) :
					new ExceptionInfo(exceptionHeader, null, exceptionMessageOverride, null),
					IconBrushKey = MessageBoxIconBrushDefaultKeys.Exception,
					ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Exception,
					AdditionalContentTemplateKey = MessageBoxAdditionalContentTemplateDefaultKeys.Exception,
					FooterButtons = footerButtons,
					Buttons = MessageBoxButtons.OK
				};
				Show(info);
			}));
		}
	}
}
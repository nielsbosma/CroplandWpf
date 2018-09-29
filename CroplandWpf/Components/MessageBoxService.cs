using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;

namespace CroplandWpf.Components
{
	public class MessageBoxService
	{
		public static string DefaultWindowTitle = "Cropland";

		private static MessageBoxWindow window;

		public static MessageBoxButton Show(MessageBoxInfo info)
		{
			window = GetWindow();
			window.Show(info);
			return window.Result;
		}

		public static DelegateCommand ShowCommand { get; } = new DelegateCommand(ShowCommand_Execute);

		private static void ShowCommand_Execute(object obj)
		{
			if (obj is MessageBoxInfo info)
			{
				window = GetWindow();
				window.Show(info);
				info.ExecuteAction(window.Result);
			}
		}

		public static MessageBoxButton ShowInformation(string windowTitle, string infoText)
		{
			string finalWindowTitle = windowTitle ?? DefaultWindowTitle;
			MessageBoxInfo info = new MessageBoxInfo()
			{
				Title = finalWindowTitle,
				Buttons = MessageBoxButtons.OK,
				Content = infoText,
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Information
			};
			return ShowWindow(info);
		}

		public static MessageBoxButton ShowError(string windowHeader, string errorText)
		{
			MessageBoxInfo info = new MessageBoxInfo()
			{
				Title = windowHeader,
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
			window = GetWindow();
			window.Show(info);
			return window.Result;
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
			string finalWIndowTitle = windowTitle ?? DefaultWindowTitle;
			MessageBoxInfo info = new MessageBoxInfo
			{
				Title = finalWIndowTitle,
				Content = exception != null ?
					new ExceptionInfo(exceptionHeader, exception.GetType().Name, exceptionMessageOverride ?? exception.Message, exception.StackTrace) :
					new ExceptionInfo(exceptionHeader, null, exceptionMessageOverride, null),
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Exception,
				ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Exception,
				AdditionalContentTemplateKey = MessageBoxAdditionalContentTemplateDefaultKeys.Exception,
				FooterButtons = footerButtons,
				Buttons = MessageBoxButtons.OK
			};

			window = GetWindow();
			window.Show(info);
		}
	}
}
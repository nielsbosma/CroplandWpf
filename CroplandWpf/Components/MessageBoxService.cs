using CroplandWpf.Helpers;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace CroplandWpf.Components
{
	public class MessageBoxService
	{
		public static string DefaultWindowHeader = "Cropland";

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
			if(obj is MessageBoxInfo info)
			{
				window = GetWindow();
				window.Show(info);
				info.ExecuteAction(window.Result);
			}
		}

		public static MessageBoxButton ShowException(string windowHeader, ExceptionInfo exceptionInfo)
		{
			MessageBoxInfo info = new MessageBoxInfo
			{
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Exception,
				Buttons = MessageBoxButtons.OK
			};
			window = GetWindow();
			window.Show(info);
			return window.Result;
		}

		public static MessageBoxButton ShowException(string windowHeader, string exceptionName, string exception, string stackTrace)
		{
			MessageBoxInfo info = new MessageBoxInfo
			{
				Header = windowHeader,
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Exception,
				Buttons = MessageBoxButtons.OK,
				Content = new ExceptionInfo
				{
					Name = exceptionName,
					Exception = exception,
					StackTrace = stackTrace
				},
				ContentTemplateKey = MessageBoxContentTemplateDefaultKeys.Exception
			};
			window = GetWindow();
			window.Show(info);
			return window.Result;
		}

		public static MessageBoxButton ShowInformation(string windowHeader, string infoText)
		{
			MessageBoxInfo info = new MessageBoxInfo()
			{
				Header = windowHeader,
				Buttons = MessageBoxButtons.OK,
				Content = infoText,
				IconBrushKey = MessageBoxIconBrushDefaultKeys.Information
			};
			return ShowWindow(info);
		}

		private static MessageBoxButton ShowWindow(MessageBoxInfo info)
		{
			if (String.IsNullOrWhiteSpace(info.Header))
				info.Header = DefaultWindowHeader;
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
	}
}
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
		private static MessageBoxWindow window;

		public static MessageBoxResult Show(MessageBoxInfo info)
		{
			window = new MessageBoxWindow();
			window.Owner = WindowHelper.GetActiveWindowInstance();
			window.Show(info);
			return window.Result;
		}

		public static DelegateCommand ShowCommand
		{
			get { return _showCommand; }
		}
		private static DelegateCommand _showCommand = new DelegateCommand(ShowCommand_Execute);

		private static void ShowCommand_Execute(object obj)
		{

		}
	}
}

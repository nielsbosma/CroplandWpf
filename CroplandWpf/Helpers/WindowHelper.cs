using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace CroplandWpf.Helpers
{
	public static class WindowHelper
	{
		public static Window GetActiveWindowInstance()
		{
			IntPtr result = GetActiveWindow();
			Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == result);
			return activeWindow;
		}

		[DllImport("user32.dll")]
		static extern IntPtr GetActiveWindow();
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using WinInterop = System.Windows.Interop;

namespace CroplandWpf.Helpers
{
	public sealed class WindowHelper
	{
		private class EventHandlerObjectInfo
		{
			public DependencyObject Target;
			public RoutedEvent Event;
			public Action<object, RoutedEventArgs> Handler;
		}

		public static WindowHelper GetAttachedHelper(DependencyObject obj)
		{
			return (WindowHelper)obj.GetValue(AttachedHelperProperty);
		}
		public static void SetAttachedHelper(DependencyObject obj, WindowHelper value)
		{
			obj.SetValue(AttachedHelperProperty, value);
		}
		public static readonly DependencyProperty AttachedHelperProperty =
			DependencyProperty.RegisterAttached("AttachedHelper", typeof(WindowHelper), typeof(WindowHelper), new PropertyMetadata());

		#region APs
		public static bool GetAllowResize(DependencyObject obj)
		{
			return (bool)obj.GetValue(AllowResizeProperty);
		}
		public static void SetAllowResize(DependencyObject obj, bool value)
		{
			obj.SetValue(AllowResizeProperty, value);
		}
		public static readonly DependencyProperty AllowResizeProperty =
			DependencyProperty.RegisterAttached("AllowResize", typeof(bool), typeof(WindowHelper), new PropertyMetadata(false));

		public static bool GetMaximizeFixEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(MaximizeFixEnabledProperty);
		}
		public static void SetMaximizeFixEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(MaximizeFixEnabledProperty, value);
		}
		public static readonly DependencyProperty MaximizeFixEnabledProperty =
			DependencyProperty.RegisterAttached("MaximizeFixEnabled", typeof(bool), typeof(WindowHelper), new PropertyMetadata((o, e) =>
			{
				Window target = o as Window;
				if (target != null && !DesignerProperties.GetIsInDesignMode(target))
				{
					target.SourceInitialized += Target_SourceInitialized;
				}
			}));

		public static System.IntPtr GetHwndPointer(DependencyObject obj)
		{
			return (System.IntPtr)obj.GetValue(HwndPointerProperty);
		}
		private static void SetHwndPointer(DependencyObject obj, System.IntPtr value)
		{
			obj.SetValue(HwndPointerProperty, value);
		}
		public static readonly DependencyProperty HwndPointerProperty =
			DependencyProperty.RegisterAttached("HwndPointer", typeof(System.IntPtr), typeof(WindowHelper), new PropertyMetadata());

		public static bool GetIsMaximizing(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsMaximizingProperty);
		}
		public static void SetIsMaximizing(DependencyObject obj, bool value)
		{
			obj.SetValue(IsMaximizingProperty, value);
		}
		public static readonly DependencyProperty IsMaximizingProperty =
			DependencyProperty.RegisterAttached("IsMaximizing", typeof(bool), typeof(WindowHelper), new PropertyMetadata());

		public static Window GetActiveWindowInstance()
		{
			IntPtr result = GetActiveWindow();
			Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == result);
			return activeWindow;
		}

		public static Window GetWindow(DependencyObject target)
		{
			return Window.GetWindow(target);
		}
		#endregion

		public static void RegisterHandler(DependencyObject target, RoutedEvent routedEvent, Action<object, RoutedEventArgs> routedEventHandler)
		{
			if (target == null)
				return;
			Window window = Window.GetWindow(target);
			if (window == null || DesignerProperties.GetIsInDesignMode(window))
				return;
			WindowHelper helper = GetAttachedHelper(window);
			if (helper == null)
			{
				helper = new WindowHelper();
				SetAttachedHelper(window, helper);
				helper.Target = window;
			}
			helper.RegisterHandlerInternal(target, routedEvent, routedEventHandler);
		}

		public static void UnregisterHandler(DependencyObject target, RoutedEvent routedEvent)
		{
			if (target == null)
				return;
			Window window = Window.GetWindow(target);
			if (window == null || DesignerProperties.GetIsInDesignMode(window))
				return;
			WindowHelper helper = GetAttachedHelper(window);
			if (helper != null)
				helper.UnregisterHandlerInternal(target, routedEvent);
		}

		private static void Target_SourceInitialized(object sender, EventArgs e)
		{
			Window target = sender as Window;
			target.SourceInitialized -= Target_SourceInitialized;
			System.IntPtr handle = new WinInterop.WindowInteropHelper(target).Handle;
			WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
			SetHwndPointer(target, handle);
		}

		#region Interop
		[DllImport("user32.dll")]
		static extern IntPtr GetActiveWindow();

		private static System.IntPtr WindowProc(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
		{
			switch (msg)
			{
				case 0x0024:/* WM_GETMINMAXINFO */
					WmGetMinMaxInfo(hwnd, lParam);
					handled = true;
					break;
			}
			return (System.IntPtr)0;
		}

		private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
		{
			MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
			// Adjust the maximized size and position to fit the work area of the correct monitor
			int MONITOR_DEFAULTTONEAREST = 0x00000002;
			System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
			if (monitor != System.IntPtr.Zero)
			{
				MONITORINFO monitorInfo = new MONITORINFO();
				GetMonitorInfo(monitor, monitorInfo);
				RECT rcWorkArea = monitorInfo.rcWork;
				RECT rcMonitorArea = monitorInfo.rcMonitor;
				mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
				mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
				mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
				mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
			}
			Marshal.StructureToPtr(mmi, lParam, true);
		}

		[DllImport("user32")]
		internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

		/// <summary>
		/// 
		/// </summary>
		[DllImport("User32")]
		internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
		#endregion

		private List<EventHandlerObjectInfo> registeredHandlers = new List<EventHandlerObjectInfo>();

		private List<RoutedEvent> registeredEvents = new List<RoutedEvent>();

		public Window Target { get; private set; }

		private void RegisterHandlerInternal(DependencyObject target, RoutedEvent routedEvent, Action<object, RoutedEventArgs> routedEventHandler)
		{
			if (Target == null)
				return;
			if (!registeredEvents.Contains(routedEvent))
			{
				Target.AddHandler(routedEvent, new RoutedEventHandler(LocalEventHandler));
				registeredEvents.Add(routedEvent);
			}
			EventHandlerObjectInfo eh = registeredHandlers.FirstOrDefault(rh => rh.Target == target && rh.Event == routedEvent);
			if (eh != null)
				return;
			eh = new EventHandlerObjectInfo() { Event = routedEvent, Target = target, Handler = routedEventHandler };
			registeredHandlers.Add(eh);
		}

		private void UnregisterHandlerInternal(DependencyObject target, RoutedEvent routedEvent)
		{
			EventHandlerObjectInfo eh = registeredHandlers.FirstOrDefault(rh => rh.Target == target && rh.Event == routedEvent);
			if (eh != null)
				registeredHandlers.Remove(eh);
		}

		private void LocalEventHandler(object sender, RoutedEventArgs e)
		{
			foreach (EventHandlerObjectInfo eh in from reh in registeredHandlers
												  where reh.Event == e.RoutedEvent
												  select reh)
				eh.Handler.Invoke(sender, e);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		public static void ResizeWindow(Window target, double newLeft, double newTop, double newWidth, double newHeight)
		{
			PresentationSource source = PresentationSource.FromVisual(target);
			Matrix transformToDevice = source.CompositionTarget.TransformToDevice;
			Point[] p = new Point[] { new Point(newLeft, newTop), new Point(newWidth, newHeight) };
			transformToDevice.Transform(p);
			SetWindowPos(GetWindowPointer(target), IntPtr.Zero, Convert.ToInt32(p[0].X), Convert.ToInt32(p[0].Y), Convert.ToInt32(p[1].X), Convert.ToInt32(p[1].Y), 0x0040);
		}

		private static IntPtr GetWindowPointer(Window target)
		{
			IntPtr result = GetHwndPointer(target);
			if (result != IntPtr.Zero)
				return GetHwndPointer(target);
			else
			{
				System.IntPtr handle = new WinInterop.WindowInteropHelper(target).Handle;
				SetHwndPointer(target, handle);
				result = GetHwndPointer(target);
			}
			return result;
		}
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class MONITORINFO
	{
		/// <summary>
		/// </summary>            
		public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

		/// <summary>
		/// </summary>            
		public RECT rcMonitor = new RECT();

		/// <summary>
		/// </summary>            
		public RECT rcWork = new RECT();

		/// <summary>
		/// </summary>            
		public int dwFlags = 0;
	}

	/// <summary>
	/// POINT aka POINTAPI
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		/// <summary>
		/// x coordinate of point.
		/// </summary>
		public int x;
		/// <summary>
		/// y coordinate of point.
		/// </summary>
		public int y;

		/// <summary>
		/// Construct a point of coordinates (x,y).
		/// </summary>
		public POINT(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MINMAXINFO
	{
		public POINT ptReserved;
		public POINT ptMaxSize;
		public POINT ptMaxPosition;
		public POINT ptMinTrackSize;
		public POINT ptMaxTrackSize;
	};

	/// <summary> Win32 </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0)]
	public struct RECT
	{
		/// <summary> Win32 </summary>
		public int left;
		/// <summary> Win32 </summary>
		public int top;
		/// <summary> Win32 </summary>
		public int right;
		/// <summary> Win32 </summary>
		public int bottom;

		/// <summary> Win32 </summary>
		public static readonly RECT Empty = new RECT();

		/// <summary> Win32 </summary>
		public int Width
		{
			get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
		}

		/// <summary> Win32 </summary>
		public int Height
		{
			get { return bottom - top; }
		}

		/// <summary> Win32 </summary>
		public RECT(int left, int top, int right, int bottom)
		{
			this.left = left;
			this.top = top;
			this.right = right;
			this.bottom = bottom;
		}


		/// <summary> Win32 </summary>
		public RECT(RECT rcSrc)
		{
			this.left = rcSrc.left;
			this.top = rcSrc.top;
			this.right = rcSrc.right;
			this.bottom = rcSrc.bottom;
		}

		/// <summary> Win32 </summary>
		public bool IsEmpty
		{
			get
			{
				// BUGBUG : On Bidi OS (hebrew arabic) left > right
				return left >= right || top >= bottom;
			}
		}
		/// <summary> Return a user friendly representation of this struct </summary>
		public override string ToString()
		{
			if (this == RECT.Empty) { return "RECT {Empty}"; }
			return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
		}

		/// <summary> Determine if 2 RECT are equal (deep compare) </summary>
		public override bool Equals(object obj)
		{
			if (!(obj is Rect)) { return false; }
			return (this == (RECT)obj);
		}

		/// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
		public override int GetHashCode()
		{
			return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
		}

		/// <summary> Determine if 2 RECT are equal (deep compare)</summary>
		public static bool operator ==(RECT rect1, RECT rect2)
		{
			return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
		}

		/// <summary> Determine if 2 RECT are different(deep compare)</summary>
		public static bool operator !=(RECT rect1, RECT rect2)
		{
			return !(rect1 == rect2);
		}
	}
}

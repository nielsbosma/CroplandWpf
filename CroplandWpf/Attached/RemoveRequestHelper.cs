using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.Attached
{
	public sealed class RemoveRequestHelper
	{
		public static DelegateCommand GetRemoveRequestCommand(DependencyObject obj)
		{
			return (DelegateCommand)obj.GetValue(RemoveRequestCommandProperty);
		}
		public static void SetRemoveRequestCommand(DependencyObject obj, DelegateCommand value)
		{
			obj.SetValue(RemoveRequestCommandProperty, value);
		}
		public static readonly DependencyProperty RemoveRequestCommandProperty =
			DependencyProperty.RegisterAttached("RemoveRequestCommand", typeof(DelegateCommand), typeof(RemoveRequestHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));
	}
}
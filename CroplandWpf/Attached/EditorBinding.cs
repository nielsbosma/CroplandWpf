using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.Attached
{
	public class EditorBinding : FrameworkElement
	{
		public static IPropertyValueAcceptor GetPropertyValueAcceptor(DependencyObject obj)
		{
			return (IPropertyValueAcceptor)obj.GetValue(PropertyValueAcceptorProperty);
		}
		public static void SetPropertyValueAcceptor(DependencyObject obj, IPropertyValueAcceptor value)
		{
			obj.SetValue(PropertyValueAcceptorProperty, value);
		}
		public static readonly DependencyProperty PropertyValueAcceptorProperty =
			DependencyProperty.RegisterAttached("PropertyValueAcceptor", typeof(IPropertyValueAcceptor), typeof(EditorBinding), new PropertyMetadata());
	}

	public interface IPropertyValueAcceptor
	{
		object AcceptEditedValue(object value);
	}
}
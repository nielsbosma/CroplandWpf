using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Components
{
	public class MessageBoxPresenter : Control
	{
		public object Content
		{
			get { return (object)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
		public static readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(object), typeof(MessageBoxPresenter), new PropertyMetadata());

		static MessageBoxPresenter()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxPresenter), new FrameworkPropertyMetadata(typeof(MessageBoxPresenter)));
		}

		public MessageBoxPresenter()
		{

		}


	}
}

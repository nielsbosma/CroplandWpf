using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CroplandWpf.Components
{
	public class MessageBoxWindow : Window
	{
		public object IconKey
		{
			get { return (object)GetValue(IconKeyProperty); }
			set { SetValue(IconKeyProperty, value); }
		}
		public static readonly DependencyProperty IconKeyProperty =
			DependencyProperty.Register("IconKey", typeof(object), typeof(MessageBoxWindow), new PropertyMetadata());

		public Brush IconBrush
		{
			get { return (Brush)GetValue(IconBrushProperty); }
			set { SetValue(IconBrushProperty, value); }
		}
		public static readonly DependencyProperty IconBrushProperty =
			DependencyProperty.Register("IconBrush", typeof(Brush), typeof(MessageBoxWindow), new PropertyMetadata());

		public object Footer
		{
			get { return (object)GetValue(FooterProperty); }
			set { SetValue(FooterProperty, value); }
		}
		public static readonly DependencyProperty FooterProperty =
			DependencyProperty.Register("Footer", typeof(object), typeof(MessageBoxWindow), new PropertyMetadata());

		public DataTemplate FooterTemplate
		{
			get { return (DataTemplate)GetValue(FooterTemplateProperty); }
			set { SetValue(FooterTemplateProperty, value); }
		}
		public static readonly DependencyProperty FooterTemplateProperty =
			DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(MessageBoxWindow), new PropertyMetadata());

		public MessageBoxButton Result
		{
			get { return (MessageBoxButton)GetValue(ResultProperty); }
			private set { SetValue(ResultProperty, value); }
		}
		public static readonly DependencyProperty ResultProperty =
			DependencyProperty.Register("Result", typeof(MessageBoxButton), typeof(MessageBoxWindow), new PropertyMetadata());

		public MessageBoxInfo Info
		{
			get { return (MessageBoxInfo)GetValue(InfoProperty); }
			set { SetValue(InfoProperty, value); }
		}
		public static readonly DependencyProperty InfoProperty =
			DependencyProperty.Register("Info", typeof(MessageBoxInfo), typeof(MessageBoxWindow), new PropertyMetadata());

		public object AdditionalContent
		{
			get { return (object)GetValue(AdditionalContentProperty); }
			set { SetValue(AdditionalContentProperty, value); }
		}
		public static readonly DependencyProperty AdditionalContentProperty =
			DependencyProperty.Register("AdditionalContent", typeof(object), typeof(MessageBoxWindow), new PropertyMetadata());

		public DataTemplate AdditionalContentTemplate
		{
			get { return (DataTemplate)GetValue(AdditionalContentTemplateProperty); }
			set { SetValue(AdditionalContentTemplateProperty, value); }
		}
		public static readonly DependencyProperty AdditionalContentTemplateProperty =
			DependencyProperty.Register("AdditionalContentTemplate", typeof(DataTemplate), typeof(MessageBoxWindow), new PropertyMetadata());

		#region Control buttons properties
		public bool AdditionalContentAvailable
		{
			get { return (bool)GetValue(AdditionalContentAvailableProperty); }
			private set { SetValue(AdditionalContentAvailableProperty, value); }
		}
		public static readonly DependencyProperty AdditionalContentAvailableProperty =
			DependencyProperty.Register("AdditionalContentAvailable", typeof(bool), typeof(MessageBoxWindow), new PropertyMetadata());

		public bool AdditionalContentVisible
		{
			get { return (bool)GetValue(AdditionalContentVisibleProperty); }
			set { SetValue(AdditionalContentVisibleProperty, value); }
		}
		public static readonly DependencyProperty AdditionalContentVisibleProperty =
			DependencyProperty.Register("AdditionalContentVisible", typeof(bool), typeof(MessageBoxWindow), new PropertyMetadata());

		public DelegateCommand ControlButtonCommand
		{
			get { return (DelegateCommand)GetValue(ControlButtonCommandProperty); }
			private set { SetValue(ControlButtonCommandProperty, value); }
		}
		public static readonly DependencyProperty ControlButtonCommandProperty =
			DependencyProperty.Register("ControlButtonCommand", typeof(DelegateCommand), typeof(MessageBoxWindow), new PropertyMetadata());

		public List<MessageBoxButton> Buttons
		{
			get { return (List<MessageBoxButton>)GetValue(ButtonsProperty); }
			private set { SetValue(ButtonsProperty, value); }
		}
		public static readonly DependencyProperty ButtonsProperty =
			DependencyProperty.Register("Buttons", typeof(List<MessageBoxButton>), typeof(MessageBoxWindow), new PropertyMetadata());

		public DelegateCommand ControlButtonLoadedCommand
		{
			get { return (DelegateCommand)GetValue(ControlButtonLoadedCommandProperty); }
			private set { SetValue(ControlButtonLoadedCommandProperty, value); }
		}
		public static readonly DependencyProperty ControlButtonLoadedCommandProperty =
			DependencyProperty.Register("ControlButtonLoadedCommand", typeof(DelegateCommand), typeof(MessageBoxWindow), new PropertyMetadata());
		#endregion

		static MessageBoxWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxWindow), new FrameworkPropertyMetadata(typeof(MessageBoxWindow)));
		}

		public MessageBoxWindow()
		{
			ControlButtonCommand = new DelegateCommand(ControlButtonCommand_Execute, ControlButtonCommand_CanExecute);
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
			ControlButtonLoadedCommand = new DelegateCommand(ControlButtonLoadedCommand_Execute);
		}

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
					Title = Info.Header;
					Content = Info.Content;
					Footer = Info.Footer;
					if (Info.IconBrushKey != null)
						SetResourceReference(IconBrushProperty, Info.IconBrushKey);
					if (Info.ContentTemplateKey != null)
						SetResourceReference(ContentTemplateProperty, Info.ContentTemplateKey);
					if (Info.FooterTemplateKey != null)
						SetResourceReference(FooterTemplateProperty, Info.FooterTemplateKey);
					if (Info.AdditionalContentTemplateKey != null)
						SetResourceReference(AdditionalContentTemplateProperty, Info.AdditionalContentTemplateKey);
					Buttons = Info.Buttons.GetFinalButtonsList();
				}
				if (e.OldValue != null)
					Resources.Clear();
			}
			if (e.Property == AdditionalContentTemplateProperty)
				AdditionalContentAvailable = e.NewValue != null;
		}

		public void Show(MessageBoxInfo info)
		{
			Info = info;
			ShowDialog();
		}

		private bool ControlButtonCommand_CanExecute(object arg)
		{
			//TODO
			return true;
		}

		private void ControlButtonCommand_Execute(object obj)
		{
			if (obj is MessageBoxButton mbb)
				Result = mbb;
			Close();
		}

		private void ControlButtonLoadedCommand_Execute(object obj)
		{
			System.Windows.Controls.Button loadedButton = obj as System.Windows.Controls.Button;
			if(loadedButton.DataContext == Buttons.First())
			{
				loadedButton.Focus();
			}
		}
	}
}
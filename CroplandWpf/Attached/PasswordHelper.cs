using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CroplandWpf.Attached
{
	public class PasswordHelper : FrameworkElement
	{
		public static PasswordController GetAttachedController(DependencyObject obj)
		{
			return (PasswordController)obj.GetValue(AttachedControllerProperty);
		}
		public static void SetAttachedController(DependencyObject obj, PasswordController value)
		{
			obj.SetValue(AttachedControllerProperty, value);
		}
		public static readonly DependencyProperty AttachedControllerProperty =
			DependencyProperty.RegisterAttached("AttachedController", typeof(PasswordController), typeof(PasswordHelper), new PropertyMetadata());

		public PasswordBox Target
		{
			get { return (PasswordBox)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(PasswordBox), typeof(PasswordHelper), new PropertyMetadata());

		public PasswordController Controller
		{
			get { return (PasswordController)GetValue(ControllerProperty); }
			set { SetValue(ControllerProperty, value); }
		}
		public static readonly DependencyProperty ControllerProperty =
			DependencyProperty.Register("Controller", typeof(PasswordController), typeof(PasswordHelper), new PropertyMetadata());

		public PasswordHelper()
		{
			
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == ControllerProperty)
			{
				if (e.OldValue != null)
					(e.OldValue as PasswordController).SetHelper(null);
				if (e.NewValue != null)
					(e.NewValue as PasswordController).SetHelper(this);
			}
		}
	}

	public class PasswordController
	{
		private PasswordHelper _helper;

		public bool HasHelper
		{
			get { return _helper != null; }
		}

		public bool IsPasswordAvailable
		{
			get { return helperInitialized && !String.IsNullOrWhiteSpace(_helper.Target.Password); }
		}

		public void SetHelper(PasswordHelper helper)
		{
			_helper = helper;
		}

		private bool helperInitialized
		{
			get { return _helper != null && _helper.Target != null; }
		}

		public void SetPassword(string password)
		{
			Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
			{
				if (helperInitialized)
					_helper.Target.Password = password;
			}), DispatcherPriority.Background);
		}

		public SecureString GetSecurePassword()
		{
			if (!helperInitialized)
				return null;
			else
				return _helper.Target.SecurePassword;
		}

		public string GetPassword()
		{
			if (!helperInitialized)
				throw new NullReferenceException();
			else
				return _helper.Target.Password;
		}
	}
}

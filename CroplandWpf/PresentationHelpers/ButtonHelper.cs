using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace CroplandWpf.PresentationHelpers
{
	public class ButtonHelper : DependencyObject
	{
		public static bool GetConvertToUpper(DependencyObject obj)
		{
			return (bool)obj.GetValue(ConvertToUpperProperty);
		}
		public static void SetConvertToUpper(DependencyObject obj, bool value)
		{
			obj.SetValue(ConvertToUpperProperty, value);
		}
		public static readonly DependencyProperty ConvertToUpperProperty =
			DependencyProperty.RegisterAttached("ConvertToUpper", typeof(bool), typeof(ButtonHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, (o, e) =>
			{
				if (o is ContentPresenter cp)
					if (cp.TemplatedParent as Button != null && cp.TemplatedParent.GetType().Equals(typeof(Button)))
						if (GetAttachedHelper(cp.TemplatedParent) == null)
							SetAttachedHelper(cp.TemplatedParent, new ButtonHelper(cp.TemplatedParent as Button, cp));
			}));

		public static bool GetIgnoreConvertToUpper(DependencyObject obj)
		{
			return (bool)obj.GetValue(IgnoreConvertToUpperProperty);
		}
		public static void SetIgnoreConvertToUpper(DependencyObject obj, bool value)
		{
			obj.SetValue(IgnoreConvertToUpperProperty, value);
		}
		public static readonly DependencyProperty IgnoreConvertToUpperProperty =
			DependencyProperty.RegisterAttached("IgnoreConvertToUpper", typeof(bool), typeof(ButtonHelper), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits, (o, e) =>
			{
				ButtonHelper helper = TryGetHelper(o);
				if (helper != null)
					SetIgnoreConvertToUpper(helper, (bool)e.NewValue);
			}));

		private static ButtonHelper TryGetHelper(DependencyObject target)
		{
			if (target is ContentPresenter cp)
				return GetAttachedHelper(cp.TemplatedParent);
			if (target.GetType().Equals(typeof(Button)))
				return GetAttachedHelper(target);
			return null;
		}

		public static ButtonHelper GetAttachedHelper(DependencyObject obj)
		{
			return (ButtonHelper)obj.GetValue(AttachedHelperProperty);
		}
		private static void SetAttachedHelper(DependencyObject obj, ButtonHelper value)
		{
			obj.SetValue(AttachedHelperProperty, value);
		}
		public static readonly DependencyProperty AttachedHelperProperty =
			DependencyProperty.RegisterAttached("AttachedHelper", typeof(ButtonHelper), typeof(ButtonHelper), new PropertyMetadata());

		public string UpperCaseStringContent
		{
			get { return (string)GetValue(UpperCaseStringContentProperty); }
			set { SetValue(UpperCaseStringContentProperty, value); }
		}
		public static readonly DependencyProperty UpperCaseStringContentProperty =
			DependencyProperty.Register("UpperCaseStringContent", typeof(string), typeof(ButtonHelper), new PropertyMetadata());

		public object OriginalContent
		{
			get { return (object)GetValue(OriginalContentProperty); }
			set { SetValue(OriginalContentProperty, value); }
		}
		public static readonly DependencyProperty OriginalContentProperty =
			DependencyProperty.Register("OriginalContent", typeof(object), typeof(ButtonHelper), new PropertyMetadata());

		private ContentPresenter buttonContentPresenter { get; set; }

		public Button Target
		{
			get { return (Button)GetValue(TargetProperty); }
			private set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(Button), typeof(ButtonHelper), new PropertyMetadata());

		public ButtonHelper(Button target, ContentPresenter contentPresenter)
		{
			Target = target;
			buttonContentPresenter = contentPresenter;
			BindingOperations.SetBinding(this, OriginalContentProperty, new Binding { Source = Target, Path = new PropertyPath(ButtonBase.ContentProperty), Mode = BindingMode.OneWay });
			BindingOperations.SetBinding(this, ConvertToUpperProperty, new Binding { Source = Target, Path = new PropertyPath(ConvertToUpperProperty), Mode = BindingMode.OneWay });
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == TargetProperty)
			{
				if (e.NewValue == null)
					BindingOperations.ClearBinding(this, OriginalContentProperty);
			}
			if (e.Property == OriginalContentProperty && (e.NewValue as string != null || e.NewValue == null))
			{
				if (e.NewValue != null)
					UpperCaseStringContent = e.NewValue.ToString().ToUpper();
				else
					UpperCaseStringContent = String.Empty;
			}
			if (e.Property == ConvertToUpperProperty)
				RefreshContentBinding((bool)e.NewValue);
			if (e.Property == IgnoreConvertToUpperProperty)
				RefreshContentBinding(GetIgnoreConvertToUpper(Target) ? false : GetConvertToUpper(Target));
		}

		private void RefreshContentBinding(bool convertToUpper)
		{
			if (buttonContentPresenter == null || Target == null)
				return;
			if (convertToUpper && GetIgnoreConvertToUpper(Target))
				return;
			if (convertToUpper)
				BindingOperations.SetBinding(buttonContentPresenter, ContentPresenter.ContentProperty, new Binding { Source = this, Path = new PropertyPath(UpperCaseStringContentProperty), Mode = BindingMode.OneWay });
			else
				BindingOperations.ClearBinding(buttonContentPresenter, ContentPresenter.ContentProperty);
		}
	}
}
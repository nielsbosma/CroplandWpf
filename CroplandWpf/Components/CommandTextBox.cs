using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public class CommandTextBox : TextBox
	{
		public bool IsButtonVisible
		{
			get { return (bool)GetValue(IsButtonVisibleProperty); }
			set { SetValue(IsButtonVisibleProperty, value); }
		}
		public static readonly DependencyProperty IsButtonVisibleProperty =
			DependencyProperty.Register("IsButtonVisible", typeof(bool), typeof(CommandTextBox), new PropertyMetadata(true));

		public Style ButtonStyle
		{
			get { return (Style)GetValue(ButtonStyleProperty); }
			set { SetValue(ButtonStyleProperty, value); }
		}
		public static readonly DependencyProperty ButtonStyleProperty =
			DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(CommandTextBox), new PropertyMetadata());

		public object ButtonContent
		{
			get { return (object)GetValue(ButtonContentProperty); }
			set { SetValue(ButtonContentProperty, value); }
		}
		public static readonly DependencyProperty ButtonContentProperty =
			DependencyProperty.Register("ButtonContent", typeof(object), typeof(CommandTextBox), new PropertyMetadata());

		public DataTemplate ButtonContentTemplate
		{
			get { return (DataTemplate)GetValue(ButtonContentTemplateProperty); }
			set { SetValue(ButtonContentTemplateProperty, value); }
		}
		public static readonly DependencyProperty ButtonContentTemplateProperty =
			DependencyProperty.Register("ButtonContentTemplate", typeof(DataTemplate), typeof(CommandTextBox), new PropertyMetadata());

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandTextBox), new PropertyMetadata());

		static CommandTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandTextBox), new FrameworkPropertyMetadata(typeof(CommandTextBox)));
		}

		public CommandTextBox()
		{

		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Button button = Template.FindName("PART_Button", this) as Button;
			if(button != null)
			{
				button.SetBinding(Button.StyleProperty, new Binding { Source = this, Path = new PropertyPath(ButtonStyleProperty), Mode = BindingMode.OneWay});
				button.SetBinding(Button.CommandProperty, new Binding { Source = this, Path = new PropertyPath(CommandProperty), Mode = BindingMode.OneWay });
				button.SetBinding(Button.CommandParameterProperty, new Binding { Source = this, Path = new PropertyPath(TextProperty), Mode = BindingMode.OneWay });
				button.SetBinding(Button.ContentProperty, new Binding { Source = this, Path = new PropertyPath(ButtonContentProperty), Mode = BindingMode.OneWay });
				button.SetBinding(Button.ContentTemplateProperty, new Binding { Source = this, Path = new PropertyPath(ButtonContentTemplateProperty), Mode = BindingMode.OneWay });
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Enter && Command != null && Command.CanExecute(Text))
				Command.Execute(Text);
		}
	}
}

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
	public class ModifiedInputTextBox : TextBox
	{
		public TextInputModifierBase InputModifier
		{
			get { return (TextInputModifierBase)GetValue(InputModifierProperty); }
			set { SetValue(InputModifierProperty, value); }
		}
		public static readonly DependencyProperty InputModifierProperty =
			DependencyProperty.Register("InputModifier", typeof(TextInputModifierBase), typeof(ModifiedInputTextBox), new PropertyMetadata());

		public Type InputModifierType
		{
			get { return (Type)GetValue(InputModifierTypeProperty); }
			set { SetValue(InputModifierTypeProperty, value); }
		}
		public static readonly DependencyProperty InputModifierTypeProperty =
			DependencyProperty.Register("InputModifierType", typeof(Type), typeof(ModifiedInputTextBox), new PropertyMetadata());

		private string textBackup;

		static ModifiedInputTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ModifiedInputTextBox), new FrameworkPropertyMetadata(typeof(ModifiedInputTextBox)));
		}

		public ModifiedInputTextBox()
		{
			Loaded += ModifiedInputTextBox_Loaded;
			Unloaded += ModifiedInputTextBox_Unloaded;
		}

		private void ModifiedInputTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			DataObject.AddPastingHandler(this, OnPreviewPaste);
		}

		private void OnPreviewPaste(object sender, DataObjectPastingEventArgs e)
		{
			e.CancelCommand();
		}

		private void ModifiedInputTextBox_Unloaded(object sender, RoutedEventArgs e)
		{
			DataObject.RemovePastingHandler(this, OnPreviewPaste);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == InputModifierTypeProperty)
			{
				if (e.NewValue != null)
					InputModifier = (TextInputModifierBase)Activator.CreateInstance(InputModifierType);
			}
		}

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			if (InputModifier != null)
			{
				TextCompositionEventArgs modifiedEventArgs = InputModifier.AcceptText(e);
				if (modifiedEventArgs != null)
					base.OnPreviewTextInput(modifiedEventArgs);
			}
			else
				base.OnPreviewTextInput(e);
		}

		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			textBackup = Text;
			SelectAllAfterFocus();
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonUp(e);
			SelectAllAfterFocus();
		}

		private void SelectAllAfterFocus()
		{
			Dispatcher.Invoke(() => SelectAll(), System.Windows.Threading.DispatcherPriority.Background);
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Text = textBackup;
				Keyboard.ClearFocus();
			}
			else if (e.Key == Key.Enter || e.Key == Key.Tab)
			{
				if (e.Key == Key.Enter)
					Keyboard.ClearFocus();
				if (String.IsNullOrWhiteSpace(Text))
					Text = "0";
				BindingExpression textBindingExpression = GetBindingExpression(TextProperty);
				if (textBindingExpression != null)
					textBindingExpression.UpdateSource();
			}
			else if (InputModifier != null)
			{
				KeyEventArgs modifiedEventArgs = InputModifier.AcceptKeyDown(e);
				if (modifiedEventArgs != null)
					base.OnPreviewKeyDown(modifiedEventArgs);
			}
			base.OnPreviewKeyDown(e);
		}
	}
}
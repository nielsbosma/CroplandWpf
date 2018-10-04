using CroplandWpf.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public bool ImmediateTargetRefresh
		{
			get { return (bool)GetValue(ImmediateTargetRefreshProperty); }
			set { SetValue(ImmediateTargetRefreshProperty, value); }
		}
		public static readonly DependencyProperty ImmediateTargetRefreshProperty =
			DependencyProperty.Register("ImmediateTargetRefresh", typeof(bool), typeof(ModifiedInputTextBox), new PropertyMetadata());

		public ICommand ValueUpdatedCommand
		{
			get { return (ICommand)GetValue(ValueUpdatedCommandProperty); }
			set { SetValue(ValueUpdatedCommandProperty, value); }
		}
		public static readonly DependencyProperty ValueUpdatedCommandProperty =
			DependencyProperty.Register("ValueUpdatedCommand", typeof(ICommand), typeof(ModifiedInputTextBox), new PropertyMetadata());

		private string textBackup;
		private Action<object, RoutedEventArgs> windowMouseDownAction;

		static ModifiedInputTextBox()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ModifiedInputTextBox), new FrameworkPropertyMetadata(typeof(ModifiedInputTextBox)));
		}

		public ModifiedInputTextBox()
		{
			Loaded += ModifiedInputTextBox_Loaded;
			Unloaded += ModifiedInputTextBox_Unloaded;
		}

		//protected override void OnLostFocus(RoutedEventArgs e)
		//{
		//	base.OnLostFocus(e);
		//	UpdateSource();
		//}

		private void ModifiedInputTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			DataObject.AddPastingHandler(this, OnPreviewPaste);
		}

		private void OnPreviewPaste(object sender, DataObjectPastingEventArgs e)
		{
			if (InputModifier != null)
				InputModifier.AcceptPaste(e);
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
			if (ImmediateTargetRefresh)
				TextChanged += ModifiedInputTextBox_TextChanged;
		}

		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnGotKeyboardFocus(e);
			textBackup = Text;
			SelectAllAfterFocus();
			if (windowMouseDownAction == null)
			{
				windowMouseDownAction = new Action<object, RoutedEventArgs>(Window_PreviewMouseDownHandler);
				WindowHelper.RegisterHandler(this, PreviewMouseDownEvent, windowMouseDownAction);
				//Trace.WriteLine("Added handler");
			}
		}

		private void Window_PreviewMouseDownHandler(object sender, RoutedEventArgs e)
		{
			UpdateSource();
			Keyboard.ClearFocus();
		}

		protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnLostKeyboardFocus(e);
			if (windowMouseDownAction != null)
			{
				WindowHelper.UnregisterHandler(this, PreviewMouseDownEvent);
				windowMouseDownAction = null;
				//Trace.WriteLine("Removed handler");
			}
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonUp(e);
			if (SelectedText.Length == 0)
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
				UpdateSource();
				Keyboard.ClearFocus();
			}
			else if (e.Key == Key.Enter || e.Key == Key.Tab)
			{
				if (e.Key == Key.Enter)
					Keyboard.ClearFocus();
				if (String.IsNullOrWhiteSpace(Text))
					Text = "0";
				UpdateSource();
			}
			else if (InputModifier != null)
			{
				KeyEventArgs modifiedEventArgs = InputModifier.AcceptKeyDown(e);
				if (modifiedEventArgs != null)
					base.OnPreviewKeyDown(modifiedEventArgs);
			}
			base.OnPreviewKeyDown(e);
			if ((e.Key == Key.Back || e.Key == Key.Delete) && ImmediateTargetRefresh)
			{
				TextChanged += ModifiedInputTextBox_TextChanged;
			}
		}

		private void ModifiedInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextChanged -= ModifiedInputTextBox_TextChanged;
			UpdateSource();
		}

		private void UpdateSource()
		{
			BindingExpression textBindingExpression = GetBindingExpression(TextProperty);
			if (textBindingExpression != null)
				textBindingExpression.UpdateSource();
			if (ValueUpdatedCommand != null)
				ValueUpdatedCommand.Execute(textBindingExpression.TargetProperty);
		}
	}
}
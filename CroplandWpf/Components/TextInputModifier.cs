using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CroplandWpf.Components
{
	public class TextInputModifierBase
	{
		public virtual TextCompositionEventArgs AcceptText(TextCompositionEventArgs args)
		{
			return args;
		}

		public virtual KeyEventArgs AcceptKeyDown(KeyEventArgs args)
		{
			return args;
		}

		public virtual void AcceptPaste(DataObjectPastingEventArgs args)
		{

		}

		protected virtual T NullArgs<T>(T args) where T : RoutedEventArgs
		{
			args.Handled = true;
			return null;
		}

		protected virtual bool IsAllowedChar(char c)
		{
			return true;
		}
	}

	public class DoubleTextModifier : TextInputModifierBase
	{
		protected string decimalSeparator
		{
			get
			{
				return CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
			}
		}

		protected string negativeSign
		{
			get
			{
				return CultureInfo.CurrentUICulture.NumberFormat.NegativeSign;
			}
		}

		public override TextCompositionEventArgs AcceptText(TextCompositionEventArgs args)
		{
			TextBox target = args.OriginalSource as TextBox;
			int caretIndex = target.CaretIndex;
			if (args.Text != null && args.Text.Length > 0)
			{
				string inputText = args.Text;
				char inputChar = inputText[0];
				if (!IsAllowedChar(inputChar))
					return NullArgs(args);
				if (IsSeparator(args.Text))
				{
					if (target.Text.Contains(decimalSeparator))
					{
						int separatorIndex = target.Text.IndexOf(decimalSeparator);
						target.Text = target.Text.Remove(separatorIndex, 1);
						if (caretIndex <= separatorIndex)
							target.CaretIndex = caretIndex;
						else
							target.CaretIndex = caretIndex - 1;
					}
					return args;
				}

				if (args.Text.Equals(negativeSign))
				{
					if (caretIndex == 0)
					{
						if (!target.Text.Contains(negativeSign))
							return args;
						else
							return NullArgs(args);
					}
					else
						return NullArgs(args);
				}
			}
			return base.AcceptText(args);
		}

		public override KeyEventArgs AcceptKeyDown(KeyEventArgs args)
		{
			if (args.Key == Key.Space)
				return NullArgs(args);
			return base.AcceptKeyDown(args);
		}

		protected override bool IsAllowedChar(char c)
		{
			return char.IsDigit(c) || c.ToString() == negativeSign || IsSeparator(c);
		}

		protected bool IsSeparator(string s)
		{
			return s == decimalSeparator;
		}

		protected bool IsSeparator(char c)
		{
			return c == Convert.ToChar(decimalSeparator);
		}

		public override void AcceptPaste(DataObjectPastingEventArgs args)
		{
			args.CancelCommand();
		}
	}

	public class SizeValueTextModifier : DoubleTextModifier
	{
		public override TextCompositionEventArgs AcceptText(TextCompositionEventArgs args)
		{
			TextBox target = args.OriginalSource as TextBox;
			int caretIndex = target.CaretIndex;
			if (args.Text != null && args.Text.Length > 0)
			{
				string inputText = args.Text;
				char inputChar = inputText[0];
				if (!IsAllowedChar(inputChar))
					return NullArgs(args);

				if (args.Text.Equals(negativeSign))
				{
					if (caretIndex == 0)
					{
						if (!target.Text.Contains(negativeSign))
							return args;
						else
							return NullArgs(args);
					}
					else
						return NullArgs(args);
				}
			}
			return base.AcceptText(args);
		}

		protected override bool IsAllowedChar(char c)
		{
			return char.IsDigit(c);
		}
	}

	public class IntegerSizeValueInputModifier : TextInputModifierBase
	{
		public IntegerSizeValueInputModifier()
		{
		}

		public override TextCompositionEventArgs AcceptText(TextCompositionEventArgs args)
		{
			TextBox target = args.OriginalSource as TextBox;
			int caretIndex = target.CaretIndex;
			if (args.Text != null && args.Text.Length > 0)
			{
				string inputText = args.Text;
				char inputChar = inputText[0];
				if (!IsAllowedChar(inputChar))
					return NullArgs(args);
			}
			return base.AcceptText(args);
		}

		public override void AcceptPaste(DataObjectPastingEventArgs args)
		{
			args.CancelCommand();
		}

		protected override bool IsAllowedChar(char c)
		{
			return Char.IsDigit(c);
		}
	}
}
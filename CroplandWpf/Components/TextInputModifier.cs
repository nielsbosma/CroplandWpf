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

		protected virtual T NullArgs<T>(T args) where T : RoutedEventArgs
		{
			args.Handled = true;
			return null;
		}
	}

	public class DoubleTextModifier : TextInputModifierBase
	{
		private string decimalSeparator
		{
			get
			{
				return CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
			}
		}

		private string negativeSign
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
			//return base.AcceptText(args)
			//string newText = args.Text;
			//if (newText.Length > 0)
			//{
			//	int caretIndex = target.CaretIndex;
			//	char c = newText[0];
			//	bool isValidChar = IsAllowedChar(c);
			//	if (!isValidChar)
			//		return NullArgs(args);
			//	if (target.Text.Contains('.') && c == '.')
			//		return NullArgs(args);
			//	if (c == '-')
			//	{
			//		if (target.Text.Contains('-') || target.CaretIndex != 0)
			//			return NullArgs(args);
			//	}
			//}
			//return base.AcceptText(target, args);
		}

		public override KeyEventArgs AcceptKeyDown(KeyEventArgs args)
		{
			if (args.Key == Key.Space)
				return NullArgs(args);
			return base.AcceptKeyDown(args);
		}

		private bool IsAllowedChar(char c)
		{
			return char.IsDigit(c) || c == '-' || IsSeparator(c);
		}

		protected bool IsSeparator(string s)
		{
			return s == decimalSeparator;
		}

		protected bool IsSeparator(char c)
		{
			return c == Convert.ToChar(decimalSeparator);
		}
	}
}
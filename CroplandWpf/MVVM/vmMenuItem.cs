using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CroplandWpf.MVVM
{
	public class CommandGesturePair
	{
		public vmMenuItem MenuItem;
		public ICommand Command;
		public KeyGesture Gesture;
	}

	public class MenuItemsCollection : ObservableCollection<vmMenuItemBase>
	{
		public bool AutoRegisterKeyBindings { get; set; }

		public MenuItemsCollection()
		{
			AutoRegisterKeyBindings = false;
		}

		public static List<CommandGesturePair> GetCommandGesturePairs(MenuItemsCollection collection)
		{
			List<CommandGesturePair> result = new List<CommandGesturePair>();
			result.AddRange(from mi in collection.OfType<vmMenuItem>()
							where mi.Command != null && mi.Gesture != null
							select new CommandGesturePair { MenuItem = mi, Command = mi.Command, Gesture = mi.Gesture });
			foreach (vmMenuItemsContainer mic in collection.OfType<vmMenuItemsContainer>())
				result.AddRange(GetCommandGesturePairs(mic.Items));
			return result;
		}
	}

	public class vmMenuItemBase : DependencyObject
	{
		public static string SeparatorHeader
		{
			get
			{
				return _separatorHeader;
			}
			set
			{
				_separatorHeader = value;
			}
		}
		private static string _separatorHeader = "_";

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register("Header", typeof(string), typeof(vmMenuItemBase), new PropertyMetadata());

		public bool IsSeparator
		{
			get { return (bool)GetValue(IsSeparatorProperty); }
			set { SetValue(IsSeparatorProperty, value); }
		}
		public static readonly DependencyProperty IsSeparatorProperty =
			DependencyProperty.Register("IsSeparator", typeof(bool), typeof(vmMenuItemBase), new PropertyMetadata(false));

		public vmMenuItemBase()
		{
			Header = "[no header]";
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == IsSeparatorProperty && (bool)e.NewValue)
				Header = SeparatorHeader;
		}
	}

	public class vmMenuItemsContainer : vmMenuItemBase
	{
		public MenuItemsCollection Items
		{
			get { return (MenuItemsCollection)GetValue(ItemsProperty); }
			set { SetValue(ItemsProperty, value); }
		}
		public static readonly DependencyProperty ItemsProperty =
			DependencyProperty.Register("Items", typeof(MenuItemsCollection), typeof(vmMenuItemsContainer), new PropertyMetadata());

		public vmMenuItemsContainer()
		{
			Items = new MenuItemsCollection();
		}
	}

	public class vmMenuItem : vmMenuItemBase
	{
		private static readonly Dictionary<string, string> KeyModifiersFriendlyNames = new Dictionary<string, string>()
		{
			{"Control", "Ctrl" },
			{ "Alt", "Alt" },
			{ "Windows", "Win" },
			{ "Shift", "Shift" }
		};

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(vmMenuItem), new PropertyMetadata());

		public object CommandParameter
		{
			get { return (object)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}
		public static readonly DependencyProperty CommandParameterProperty =
			DependencyProperty.Register("CommandParameter", typeof(object), typeof(vmMenuItem), new PropertyMetadata());

		public KeyGesture Gesture
		{
			get { return (KeyGesture)GetValue(GestureProperty); }
			set { SetValue(GestureProperty, value); }
		}
		public static readonly DependencyProperty GestureProperty =
			DependencyProperty.Register("Gesture", typeof(KeyGesture), typeof(vmMenuItem), new PropertyMetadata());

		public string GestureString
		{
			get { return (string)GetValue(GestureStringProperty); }
			private set { SetValue(GestureStringProperty, value); }
		}
		public static readonly DependencyProperty GestureStringProperty =
			DependencyProperty.Register("GestureString", typeof(string), typeof(vmMenuItem), new PropertyMetadata());

		public vmMenuItem()
		{

		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == GestureProperty && e.NewValue != null)
			{
				KeyGesture kg = e.NewValue as KeyGesture;
				if (kg.Modifiers == ModifierKeys.None)
					GestureString = String.Format("{0}", kg.Key);
				else
				{
					string[] modifiers = kg.Modifiers.ToString().Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
					StringBuilder sb = new StringBuilder();
					for (int count = 0; count < modifiers.Count(); count++)
					{
						sb.Append(KeyModifiersFriendlyNames[modifiers[count]]);
						sb.Append('+');
					}
					GestureString = sb.Append(kg.Key).ToString();
				}

			}
		}
	}

	public class vmMenuItemSeparator : vmMenuItemBase
	{
		public vmMenuItemSeparator()
		{
			IsSeparator = true;
		}
	}
}
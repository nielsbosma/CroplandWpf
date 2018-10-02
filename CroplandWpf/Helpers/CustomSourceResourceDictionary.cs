using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace CroplandWpf.Helpers
{
	/// <summary>Represents a resource dictionary with optional source string of resource dictionary with replacement values</summary>
	public class ReplacableValuesResourceDictionary : ResourceDictionary
	{
		#region Properties
		/// <summary>Gets or sets the source of replacement resource dictionary</summary>
		public string ReplacementSource
		{
			get
			{
				return _replacementSource;
			}
			set
			{
				if (DesignerProperties.GetIsInDesignMode(new FrameworkElement()))
					return;
				if (_replacementSource != value && value != null)
				{
					_replacementSource = value;
					TryLoadResources();
				}
			}
		}
		private string _replacementSource;

		/// <summary>Gets the full path to the replacement resource dictionary in the app folder</summary>
		private string replacementSourceFullPath
		{
			get { return AppDomain.CurrentDomain.BaseDirectory + _replacementSource; }
		}
		#endregion

		#region Ctor
		/// <summary>Initializes the new ReplacableValuesResourceDictionary class instance</summary>
		public ReplacableValuesResourceDictionary()
		{
			
		}
		#endregion

		#region Private methods
		/// <summary>Makes an attempt to load the replacement resources. If failed - re-creates the replacement source</summary>
		private void TryLoadResources()
		{
			ResourceDictionary replacementDictionary = new ResourceDictionary();
			try
			{
				replacementDictionary.BeginInit();
				replacementDictionary.Source = new Uri(replacementSourceFullPath, UriKind.Absolute);
				replacementDictionary.EndInit();

				Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { ReplaceAvailableResources(replacementDictionary); }), DispatcherPriority.Send);
			}
			catch (WebException)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => { RestoreReplacementDictionary(); }), DispatcherPriority.Send);
			}
		}

		/// <summary>Replaces resources values with values from replacement dictionary</summary>
		/// <param name="replacementDictionary">Resource dictionary with replacement values</param>
		private void ReplaceAvailableResources(ResourceDictionary replacementDictionary)
		{
			foreach (object key in replacementDictionary.Keys)
			{
				if (this.Keys.OfType<object>().Contains(key))
					this[key] = replacementDictionary[key];
			}
		}

		/// <summary>Restores the replacement source if it was failed to load it from the given replacement source string</summary>
		private void RestoreReplacementDictionary()
		{
			if (!File.Exists(replacementSourceFullPath))
			{
				using (FileStream fs = new FileStream(replacementSourceFullPath, FileMode.Create, FileAccess.ReadWrite))
				{
					using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings() { Indent = true }))
					{
						ResourceDictionary replacementDictionary = new ResourceDictionary();
						foreach (object key in Keys)
							replacementDictionary.Add(key, this[key]);
						XamlDesignerSerializationManager manager = new XamlDesignerSerializationManager(writer);
						XamlWriter.Save(replacementDictionary, manager);
					}
				}
			}
		}
		#endregion
	}
}
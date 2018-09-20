using System;
using System.ComponentModel;
using System.Windows;

namespace CroplandWpf.Helpers
{
	public class DesignTimeResourceDictionary : ResourceDictionary
	{
		private static DependencyObject designTimeCheckObject = new DependencyObject();

		public Uri DesignTimeSourceUri
		{
			get { return Source; }
			set
			{
				if (DesignerProperties.GetIsInDesignMode(designTimeCheckObject))
					return;
				Source = value;
			}
		}
	}
}

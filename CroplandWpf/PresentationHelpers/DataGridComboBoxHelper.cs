using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CroplandWpf.PresentationHelpers
{
	public class DataGridComboBoxHelper : FrameworkElement
	{
		public ComboBox Target
		{
			get { return (ComboBox)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(ComboBox), typeof(DataGridComboBoxHelper), new PropertyMetadata());

		public DataGridCell AssociatedCell
		{
			get { return (DataGridCell)GetValue(AssociatedCellProperty); }
			set { SetValue(AssociatedCellProperty, value); }
		}
		public static readonly DependencyProperty AssociatedCellProperty =
			DependencyProperty.Register("AssociatedCell", typeof(DataGridCell), typeof(DataGridComboBoxHelper), new PropertyMetadata());

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == AssociatedCellProperty || e.Property == TargetProperty)
			{
				if(AssociatedCell != null && Target != null)
				{
					DataGridComboBoxColumn column = AssociatedCell.Column as DataGridComboBoxColumn;
					if (column != null)
						Target.SetBinding(ComboBox.ItemsSourceProperty, new Binding { Source = column, Path = new PropertyPath(DataGridComboBoxColumn.ItemsSourceProperty) });
				}
			}
		}
	}
}

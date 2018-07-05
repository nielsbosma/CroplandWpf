using CroplandWpf.Attached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CroplandWpf.Components
{
	public class HeaderFooterContentControl : ContentControl
	{

		public DependencyObject HeaderFooterPropertiesSource
		{
			get { return (DependencyObject)GetValue(HeaderFooterPropertiesSourceProperty); }
			set { SetValue(HeaderFooterPropertiesSourceProperty, value); }
		}
		public static readonly DependencyProperty HeaderFooterPropertiesSourceProperty =
			DependencyProperty.Register("HeaderFooterPropertiesSource", typeof(DependencyObject), typeof(HeaderFooterContentControl), new PropertyMetadata());

		static HeaderFooterContentControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HeaderFooterContentControl), new FrameworkPropertyMetadata(typeof(HeaderFooterContentControl)));
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == HeaderFooterPropertiesSourceProperty && e.NewValue != null)
			{
				SetBinding(VisualHelper.HeaderProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.HeaderProperty) });
				SetBinding(VisualHelper.FooterProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.FooterProperty) });
				SetBinding(VisualHelper.HeaderAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.HeaderAlignmentProperty) });
				SetBinding(VisualHelper.FooterAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.FooterAlignmentProperty) });
				SetBinding(VisualHelper.HeaderHorizontalAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.HeaderHorizontalAlignmentProperty) });
				SetBinding(VisualHelper.FooterHorizontalAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.FooterHorizontalAlignmentProperty) });
				SetBinding(VisualHelper.HeaderVerticalAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.HeaderVerticalAlignmentProperty) });
				SetBinding(VisualHelper.HeaderVerticalAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.HeaderVerticalAlignmentProperty) });
				SetBinding(VisualHelper.FooterVerticalAlignmentProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.FooterVerticalAlignmentProperty) });
				SetBinding(VisualHelper.HeaderSharedGroupNameProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.HeaderSharedGroupNameProperty) });
				SetBinding(VisualHelper.FooterSharedGroupNameProperty, new Binding { Source = e.NewValue, Path = new PropertyPath(VisualHelper.FooterSharedGroupNameProperty) });
			}
		}
	}
}
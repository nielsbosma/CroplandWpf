using CroplandWpf.Attached;
using CroplandWpf.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CroplandWpf.Components
{
	public enum CombinedAlignment
	{
		LeftTop,
		CenterTop,
		RightTop,
		LeftCenter,
		CenterCenter,
		RightCenter,
		LeftBottom,
		CenterBottom,
		RightBottom
	}

	public class AlignmentEditor : Control
	{
		public static CombinedAlignment GetEditorRole(DependencyObject obj)
		{
			return (CombinedAlignment)obj.GetValue(EditorRoleProperty);
		}
		public static void SetEditorRole(DependencyObject obj, CombinedAlignment value)
		{
			obj.SetValue(EditorRoleProperty, value);
		}
		public static readonly DependencyProperty EditorRoleProperty =
			DependencyProperty.RegisterAttached("EditorRole", typeof(CombinedAlignment), typeof(AlignmentEditor), new PropertyMetadata());

		private class AlignmentAssociationsCollection : List<AlignmentAssociation>
		{
			public AlignmentAssociation this[CombinedAlignment key]
			{
				get { return this.FirstOrDefault(aa => aa.Key == key); }
			}

			public AlignmentAssociation this[HorizontalAlignment h, VerticalAlignment v]
			{
				get
				{
					return this.FirstOrDefault(aa => aa.HAlignment == h && aa.VAlignment == v);
				}
			}
		}

		private class AlignmentAssociation
		{
			public CombinedAlignment Key;
			public HorizontalAlignment HAlignment;
			public VerticalAlignment VAlignment;
		}

		private static readonly AlignmentAssociationsCollection alignmentAssociations = new AlignmentAssociationsCollection
		{
			new AlignmentAssociation{ Key = CombinedAlignment.LeftTop, HAlignment = HorizontalAlignment.Left, VAlignment = VerticalAlignment.Top },
			new AlignmentAssociation{ Key = CombinedAlignment.CenterTop, HAlignment = HorizontalAlignment.Center, VAlignment = VerticalAlignment.Top },
			new AlignmentAssociation{ Key = CombinedAlignment.RightTop, HAlignment = HorizontalAlignment.Right, VAlignment = VerticalAlignment.Top },
			new AlignmentAssociation{ Key = CombinedAlignment.LeftCenter, HAlignment = HorizontalAlignment.Left, VAlignment = VerticalAlignment.Center},
			new AlignmentAssociation{ Key = CombinedAlignment.CenterCenter, HAlignment = HorizontalAlignment.Center, VAlignment = VerticalAlignment.Center },
			new AlignmentAssociation{ Key = CombinedAlignment.RightCenter, HAlignment = HorizontalAlignment.Right, VAlignment = VerticalAlignment.Center },
			new AlignmentAssociation{ Key = CombinedAlignment.LeftBottom, HAlignment = HorizontalAlignment.Left, VAlignment = VerticalAlignment.Bottom},
			new AlignmentAssociation{ Key = CombinedAlignment.CenterBottom, HAlignment = HorizontalAlignment.Center, VAlignment = VerticalAlignment.Bottom},
			new AlignmentAssociation{ Key = CombinedAlignment.RightBottom, HAlignment = HorizontalAlignment.Right, VAlignment = VerticalAlignment.Bottom }
		};

		public VerticalAlignment VerticalAlignmentValue
		{
			get { return (VerticalAlignment)GetValue(VerticalAlignmentValueProperty); }
			set { SetValue(VerticalAlignmentValueProperty, value); }
		}
		public static readonly DependencyProperty VerticalAlignmentValueProperty =
			DependencyProperty.Register("VerticalAlignmentValue", typeof(VerticalAlignment), typeof(AlignmentEditor), new PropertyMetadata());

		public HorizontalAlignment HorizontalAlignmentValue
		{
			get { return (HorizontalAlignment)GetValue(HorizontalAlignmentValueProperty); }
			set { SetValue(HorizontalAlignmentValueProperty, value); }
		}
		public static readonly DependencyProperty HorizontalAlignmentValueProperty =
			DependencyProperty.Register("HorizontalAlignmentValue", typeof(HorizontalAlignment), typeof(AlignmentEditor), new PropertyMetadata());

		public string LocalGroupName
		{
			get { return (string)GetValue(LocalGroupNameProperty); }
			private set { SetValue(LocalGroupNameProperty, value); }
		}
		public static readonly DependencyProperty LocalGroupNameProperty =
			DependencyProperty.Register("LocalGroupName", typeof(string), typeof(AlignmentEditor), new PropertyMetadata());

		public CombinedAlignment CombinedAlignment
		{
			get { return (CombinedAlignment)GetValue(CombinedAlignmentProperty); }
			set { SetValue(CombinedAlignmentProperty, value); }
		}
		public static readonly DependencyProperty CombinedAlignmentProperty =
			DependencyProperty.Register("CombinedAlignment", typeof(CombinedAlignment), typeof(AlignmentEditor), new PropertyMetadata());

		public Thickness MarginValue
		{
			get { return (Thickness)GetValue(MarginValueProperty); }
			set { SetValue(MarginValueProperty, value); }
		}
		public static readonly DependencyProperty MarginValueProperty =
			DependencyProperty.Register("MarginValue", typeof(Thickness), typeof(AlignmentEditor), new PropertyMetadata());

		public double ThicknessLeft
		{
			get { return (double)GetValue(ThicknessLeftProperty); }
			set { SetValue(ThicknessLeftProperty, value); }
		}
		public static readonly DependencyProperty ThicknessLeftProperty =
			DependencyProperty.Register("ThicknessLeft", typeof(double), typeof(AlignmentEditor), new PropertyMetadata());

		public double ThicknessTop
		{
			get { return (double)GetValue(ThicknessTopProperty); }
			set { SetValue(ThicknessTopProperty, value); }
		}
		public static readonly DependencyProperty ThicknessTopProperty =
			DependencyProperty.Register("ThicknessTop", typeof(double), typeof(AlignmentEditor), new PropertyMetadata());

		public double ThicknessRight
		{
			get { return (double)GetValue(ThicknessRightProperty); }
			set { SetValue(ThicknessRightProperty, value); }
		}
		public static readonly DependencyProperty ThicknessRightProperty =
			DependencyProperty.Register("ThicknessRight", typeof(double), typeof(AlignmentEditor), new PropertyMetadata());

		public double ThicknessBottom
		{
			get { return (double)GetValue(ThicknessBottomProperty); }
			set { SetValue(ThicknessBottomProperty, value); }
		}
		public static readonly DependencyProperty ThicknessBottomProperty =
			DependencyProperty.Register("ThicknessBottom", typeof(double), typeof(AlignmentEditor), new PropertyMetadata());

		public bool ShowMarginEditors
		{
			get { return (bool)GetValue(ShowMarginEditorsProperty); }
			set { SetValue(ShowMarginEditorsProperty, value); }
		}
		public static readonly DependencyProperty ShowMarginEditorsProperty =
			DependencyProperty.Register("ShowMarginEditors", typeof(bool), typeof(AlignmentEditor), new PropertyMetadata());

		#region Commands
		public DelegateCommand SwitchValueCommand
		{
			get { return (DelegateCommand)GetValue(SwitchValueCommandProperty); }
			private set { SetValue(SwitchValueCommandProperty, value); }
		}
		public static readonly DependencyProperty SwitchValueCommandProperty =
			DependencyProperty.Register("SwitchValueCommand", typeof(DelegateCommand), typeof(AlignmentEditor), new PropertyMetadata());
		#endregion

		private bool blockActiveRadioButtonRefresh = false;
		private bool blockMarginValuesRefresh = false;

		static AlignmentEditor()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(AlignmentEditor), new FrameworkPropertyMetadata(typeof(AlignmentEditor)));
		}

		public AlignmentEditor()
		{
			LocalGroupName = Guid.NewGuid().ToString();
			SwitchValueCommand = new DelegateCommand(SwitchValueCommand_Execute);
			Loaded += AlignmentEditor_Loaded;
		}

		private void AlignmentEditor_Loaded(object sender, RoutedEventArgs e)
		{
			if (AlignmentControlHelper.GetLocalTargets(this) != null)
				CheckTargetForCurrentAlignmentValues();
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (!blockActiveRadioButtonRefresh && (e.Property == HorizontalAlignmentValueProperty || e.Property == VerticalAlignmentValueProperty))
				CheckTargetForCurrentAlignmentValues();
			if(e.Property == MarginValueProperty)
			{
				blockMarginValuesRefresh = true;
				ThicknessLeft = MarginValue.Left;
				ThicknessTop = MarginValue.Top;
				ThicknessRight = MarginValue.Right;
				ThicknessBottom = MarginValue.Bottom;
				blockMarginValuesRefresh = false;
			}
			if((e.Property == ThicknessLeftProperty || e.Property == ThicknessTopProperty || e.Property == ThicknessRightProperty || e.Property == ThicknessBottomProperty) && !blockMarginValuesRefresh )
			{
				MarginValue = new Thickness(ThicknessLeft, ThicknessTop, ThicknessRight, ThicknessBottom);
			}
		}

		public void CheckTargetForCurrentAlignmentValues()
		{
			RadioButton actualTarget = GetActualRadioButton(HorizontalAlignmentValue, VerticalAlignmentValue);
			RadioButton checkedRB = null;
			if (AlignmentControlHelper.GetLocalTargets(this) != null)
				checkedRB = AlignmentControlHelper.GetLocalTargets(this).FirstOrDefault(rb => rb.IsChecked.HasValue && rb.IsChecked.Value);
			if (checkedRB != null && actualTarget != checkedRB)
			{
				checkedRB.IsChecked = false;
			}
			if (actualTarget != null)
				actualTarget.IsChecked = true;
		}

		private RadioButton GetActualRadioButton(HorizontalAlignment hAlignment, VerticalAlignment vAlignment)
		{
			List<RadioButton> localTargets = AlignmentControlHelper.GetLocalTargets(this);
			if (localTargets != null)
				return localTargets.FirstOrDefault(lt => GetEditorRole(lt) == alignmentAssociations[hAlignment, vAlignment].Key);
			else
				return null;
		}

		private void SwitchValueCommand_Execute(object obj)
		{
			if (obj is CombinedAlignment role)
			{
				blockActiveRadioButtonRefresh = true;
				AlignmentAssociation aa = alignmentAssociations[role];
				HorizontalAlignmentValue = aa.HAlignment;
				VerticalAlignmentValue = aa.VAlignment;
				blockActiveRadioButtonRefresh = false;
			}
		}
	}
}
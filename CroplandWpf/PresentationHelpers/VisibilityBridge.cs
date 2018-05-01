using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CroplandWpf.PresentationHelpers
{
	public enum VisibilityBridgeRole
	{
		Source,
		Target
	}

	public class VisibilityBridge : FrameworkElement
	{
		private static List<VisibilityBridge> targets = new List<VisibilityBridge>();

		public bool IsSourceVisible
		{
			get { return (bool)GetValue(IsSourceVisibleProperty); }
			set { SetValue(IsSourceVisibleProperty, value); }
		}
		public static readonly DependencyProperty IsSourceVisibleProperty =
			DependencyProperty.Register("IsSourceVisible", typeof(bool), typeof(VisibilityBridge), new PropertyMetadata());

		public VisibilityBridgeRole Role
		{
			get { return (VisibilityBridgeRole)GetValue(RoleProperty); }
			set { SetValue(RoleProperty, value); }
		}
		public static readonly DependencyProperty RoleProperty =
			DependencyProperty.Register("Role", typeof(VisibilityBridgeRole), typeof(VisibilityBridge), new PropertyMetadata(VisibilityBridgeRole.Source));

		public FrameworkElement CommonParent
		{
			get { return (FrameworkElement)GetValue(CommonParentProperty); }
			set { SetValue(CommonParentProperty, value); }
		}
		public static readonly DependencyProperty CommonParentProperty =
			DependencyProperty.Register("CommonParent", typeof(FrameworkElement), typeof(VisibilityBridge), new PropertyMetadata());

		public VisibilityBridge()
		{
			IsVisibleChanged += VisibilityBridge_IsVisibleChanged;
			Loaded += VisibilityBridge_Loaded;
			Unloaded += VisibilityBridge_Unloaded;
			IsHitTestVisible = false;
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == CommonParentProperty && e.NewValue != null)
				VisibilityBridge_IsVisibleChanged(this, new DependencyPropertyChangedEventArgs());
		}

		private void VisibilityBridge_Loaded(object sender, RoutedEventArgs e)
		{
			if (Role == VisibilityBridgeRole.Target)
				RegisterTarget(this);
		}

		private void VisibilityBridge_Unloaded(object sender, RoutedEventArgs e)
		{
			if (Role == VisibilityBridgeRole.Target)
				UnregisterTarget(this);
		}

		private static void RegisterTarget(VisibilityBridge bridge)
		{
			if (!targets.Contains(bridge))
				targets.Add(bridge);
		}

		private static void UnregisterTarget(VisibilityBridge bridge)
		{
			if (targets.Contains(bridge))
				targets.Add(bridge);
		}

		private static VisibilityBridge GetTarget(FrameworkElement commonParent)
		{
			VisibilityBridge target = targets.FirstOrDefault(vb => vb.CommonParent == commonParent);
			return target;
		}

		private void VisibilityBridge_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (Role == VisibilityBridgeRole.Source)
			{
				if (IsVisible)
				{
					VisibilityBridge target = GetTarget(CommonParent);
					if (target != null)
						target.IsSourceVisible = true;
				}
				else
				{
					VisibilityBridge target = GetTarget(CommonParent);
					if (target != null)
						target.IsSourceVisible = false;
				}
			}
		}
	}
}
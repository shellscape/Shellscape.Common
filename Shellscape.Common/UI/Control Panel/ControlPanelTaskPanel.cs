using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Shellscape.UI.ControlPanel {
	public class ControlPanelTaskPanel : FlowLayoutPanel {

		private VisualStyleRenderer _renderer = VisualStyles.ControlPanel.GetRenderer(VisualStyles.ControlPanel.ControlPanelPart.TaskLink, (int)VisualStyles.ControlPanel.TaskLinkState.Normal, true);
		
		public ControlPanelTaskPanel() {
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.UpdateStyles();
		}

		protected override void OnControlAdded(ControlEventArgs e) {

			if (e.Control is ControlPanelTaskLink) {
				(e.Control as ControlPanelTaskLink).NormalColor = _renderer.GetColor(ColorProperty.TextColor);
			}
			
			base.OnControlAdded(e);
		}

	}
}

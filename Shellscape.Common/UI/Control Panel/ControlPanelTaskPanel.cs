using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.ControlPanel {
	public class ControlPanelTaskPanel : FlowLayoutPanel {

		public ControlPanelTaskPanel() {
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.UpdateStyles();
		}

	}
}

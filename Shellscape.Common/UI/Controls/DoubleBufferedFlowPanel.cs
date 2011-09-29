using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls {

	public class DoubleBufferedFlowPanel : FlowLayoutPanel {

		public DoubleBufferedFlowPanel() {
			// Set the value of the double-buffering style bits to true.
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.UpdateStyles();
		}

	}
}

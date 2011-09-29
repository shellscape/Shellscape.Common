using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Shellscape.UI;
using Shellscape.UI.Controls;

namespace Shellscape.UI.ControlPanel {
	public class ControlPanelTaskLink : ResponsiveLinkLabel {

		public ControlPanelTaskLink()	: base() {
			this.Padding = new Padding(0, 3, 0, 5);
			this.AutoSize = true;
			this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		}

		protected override void OnClick(EventArgs e) {

			if (this.AssociatedPanel != null && !this.AssociatedPanel.Visible) {
				(this.FindForm() as ControlPanelForm).HidePanels();

				this.AssociatedPanel.Show();
			}
			else {
				base.OnClick(e);
			}

			this.LinkVisited = false;
		}

		public DoubleBufferedPanel AssociatedPanel { get; set; }

	}
}

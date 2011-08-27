using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Shellscape.UI.Controls;

namespace Shellscape.UI.ControlPanel {
	public partial class ControlPanelForm : Form {

		public ControlPanelForm() {
			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();

			Padding = new System.Windows.Forms.Padding(0);

			InitializeComponent();
		}

		public void HidePanels() {
			foreach (var panel in this.Controls.OfType<DoubleBufferedPanel>()) {
				panel.Hide();
			}
		}
	}
}

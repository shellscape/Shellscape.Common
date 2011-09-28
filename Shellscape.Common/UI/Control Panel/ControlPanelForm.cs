using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Shellscape.UI.Controls;

namespace Shellscape.UI.ControlPanel {
	public partial class ControlPanelForm : Form {

		protected class ColorInfo {

			public Color ContentLinkNormal { get; set; }
			public Color ContentLinkHot { get; set; }
		}

		public ControlPanelForm() {
			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();

			Padding = new System.Windows.Forms.Padding(0);

			InitFonts();

			InitializeComponent();

			foreach (var panel in _Panels.Controls.OfType<DoubleBufferedPanel>()) {
				panel.Size = new Size(_Panels.Width - (_Panels.Padding.Left + _Panels.Padding.Right), _Panels.Height - (_Panels.Padding.Top + _Panels.Padding.Bottom));
				panel.Anchor = AnchorStyles.None;
			}
		}

		protected ColorInfo Colors { get; set; }

		private void InitFonts() {

			Colors = new ColorInfo();

			if (VisualStyleRenderer.IsSupported) {
				VisualStyleRenderer contentLink = VisualStyles.ControlPanel.GetRenderer(VisualStyles.ControlPanel.ControlPanelPart.ContentLink, (int)VisualStyles.ControlPanel.ContentLinkState.Normal, true);

				using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					Colors.ContentLinkNormal = contentLink.GetColor(ColorProperty.TextColor);

					contentLink = VisualStyles.ControlPanel.GetRenderer(VisualStyles.ControlPanel.ControlPanelPart.ContentLink, (int)VisualStyles.ControlPanel.ContentLinkState.Hot, true);

					Colors.ContentLinkHot = contentLink.GetColor(ColorProperty.TextColor);
				}
			}

		}

		public void HidePanels() {
			foreach (var panel in _Panels.Controls.OfType<DoubleBufferedPanel>()) {
				panel.Hide();
			}
		}

		public Control.ControlCollection Tasks { get { return _Tasks._tasks.Controls; } }

		public Control.ControlCollection OtherTasks { get { return _Tasks._otherTasks.Controls; } }

		public String OtherTasksText {
			get { return _Tasks.OtherTasksText; }
			set { _Tasks.OtherTasksText = value; }
		}
	}
}

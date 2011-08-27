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
	public class ControlPanelTaskLink : LinkLabel {

		private Color _normalColor;
		private Color _hoverColor;

		public ControlPanelTaskLink()	: base() {
			this.Padding = new Padding(0, 3, 0, 3);
			this.LinkVisited = false;
			this.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.AutoSize = true;
			this.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

			if (VisualStyleRenderer.IsSupported) {
				VisualStyles.ControlPanel.ControlPanelPart part = VisualStyles.ControlPanel.ControlPanelPart.TaskLink;
				VisualStyleRenderer renderer = VisualStyles.ControlPanel.GetRenderer(part, (int)VisualStyles.ControlPanel.TaskLinkState.Normal, true);

				using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					this.Font = renderer.GetFont(g, FontProperty.GlyphFont);
					this.LinkColor = this.VisitedLinkColor = _normalColor = renderer.GetColor(ColorProperty.TextColor);

					renderer = VisualStyles.ControlPanel.GetRenderer(part, (int)VisualStyles.ControlPanel.TaskLinkState.Pressed);
					this.ActiveLinkColor = _hoverColor = renderer.GetColor(ColorProperty.TextColor);

					renderer = VisualStyles.ControlPanel.GetRenderer(part, (int)VisualStyles.ControlPanel.TaskLinkState.Disabled);
					this.DisabledLinkColor = renderer.GetColor(ColorProperty.TextColor);
				}
			}
		}

		protected override void OnClick(EventArgs e) {

			if (this.AssociatedPanel != null) {
				(this.FindForm() as ControlPanelForm).HidePanels();

				this.AssociatedPanel.Show();
			}
			else {
				base.OnClick(e);
			}

			Debug.WriteLine("click");

			this.LinkVisited = false;
		}

		// reset the color, as we're no longer hovering. this is a sanity check.
		protected override void OnMouseLeave(EventArgs e) {

			if (base.Enabled) {
				this.LinkColor = _normalColor;
			}

			base.OnMouseLeave(e);
		}

		// this allows for a hover color
		protected override void OnMouseMove(MouseEventArgs e) {

			if (base.Enabled) {
				Link link = this.PointInLink(e.X, e.Y);

				if (this.ForeColor != _hoverColor && link != null) { // we're over the link
					this.LinkColor = _hoverColor;
				}
				else if (link == null && this.LinkColor != _normalColor) { // we're out of the link
					this.LinkColor = _normalColor;
				}
			}

			base.OnMouseMove(e);
		}

		public DoubleBufferedPanel AssociatedPanel { get; set; }

		[DllImport("user32.dll")]
		public static extern int LoadCursor(int hInstance, int lpCursorName);

		[DllImport("user32.dll")]
		public static extern int SetCursor(int hCursor);

		// fixes the shitty hand icon and uses the system default pretty hand
		protected override void WndProc(ref Message m) {
			//WM_SETCURSOR == 32
			if (m.Msg == 32) {
				//IDC_HAND == 32649
				SetCursor(LoadCursor(0, 32649));

				//the message has been handled
				m.Result = IntPtr.Zero;
				return;
			}

			base.WndProc(ref m);
		}
	}
}

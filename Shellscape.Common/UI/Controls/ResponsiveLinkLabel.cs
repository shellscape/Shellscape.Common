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

namespace Shellscape.UI.Controls {
	public class ResponsiveLinkLabel : LinkLabel {

		protected int _properHand;

		public ResponsiveLinkLabel() : base() {

			this.LinkVisited = false;
			this.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.AutoSize = true;

			this._properHand = LoadCursor(0, 32649);

			if (VisualStyleRenderer.IsSupported) {
				VisualStyles.ControlPanel.ControlPanelPart part = VisualStyles.ControlPanel.ControlPanelPart.TaskLink;
				VisualStyleRenderer renderer = VisualStyles.ControlPanel.GetRenderer(part, (int)VisualStyles.ControlPanel.TaskLinkState.Normal, true);

				using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					this.Font = renderer.GetFont(g, FontProperty.GlyphFont);
					this.LinkColor = this.VisitedLinkColor = NormalColor = renderer.GetColor(ColorProperty.TextColor);

					renderer = VisualStyles.ControlPanel.GetRenderer(part, (int)VisualStyles.ControlPanel.TaskLinkState.Pressed);
					this.ActiveLinkColor = HoverColor = renderer.GetColor(ColorProperty.TextColor);

					renderer = VisualStyles.ControlPanel.GetRenderer(part, (int)VisualStyles.ControlPanel.TaskLinkState.Disabled);
					this.DisabledLinkColor = renderer.GetColor(ColorProperty.TextColor);
				}
			}
		}

		public Color HoverColor { get; set; }
		public Color NormalColor { get; set; }
		
		// reset the color, as we're no longer hovering. this is a sanity check.
		protected override void OnMouseLeave(EventArgs e) {

			if (base.Enabled) {
				this.LinkColor = NormalColor;
			}

			base.OnMouseLeave(e);
		}

		// this allows for a hover color
		protected override void OnMouseMove(MouseEventArgs e) {

			if (base.Enabled) {
				Link link = this.PointInLink(e.X, e.Y);

				if (this.ForeColor != HoverColor && link != null) { // we're over the link
					this.LinkColor = HoverColor;
					SetCursor(_properHand);
				}
				else if (link == null && this.LinkColor != NormalColor) { // we're out of the link
					this.LinkColor = NormalColor;
					this.Cursor = Cursors.Default;
				}
			}

			base.OnMouseMove(e);
		}

		[DllImport("user32.dll")]
		public static extern int LoadCursor(int hInstance, int lpCursorName);

		[DllImport("user32.dll")]
		public static extern int SetCursor(int hCursor);

		// fixes the shitty hand icon and uses the system default pretty hand
		//protected override void WndProc(ref Message m) {
		//  //WM_SETCURSOR == 32
		//  if (m.Msg == 32) {
		//    //IDC_HAND == 32649
		//    SetCursor(_properHand);

		//    //the message has been handled
		//    m.Result = IntPtr.Zero;
		//    return;
		//  }

		//  base.WndProc(ref m);
		//}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Shellscape.UI.Controls {

	[DesignTimeVisible(true), Browsable(true)]
	public class ThemeLabel : Label {

		private VisualStyleRenderer _renderer;
		private VisualStyleElement _themeElement;

		public ThemeLabel() : base() {

			this.SetStyle(
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.SupportsTransparentBackColor,
			true);
			this.UpdateStyles();

			this.BackColor = Color.Transparent;
			this.AutoSize = true;
			this.MinimumSize = new Size(100, 20);

			this.Text = "Theme Label";
		}

		public VisualStyleElement ThemeElement {
			get { return _themeElement; }
			set {
				_themeElement = value;

				UpdateTheme();
			}
		}

		//public new String Text {
		//  get {
		//    return base.Text;
		//  }
		//  set {
		//    base.Text = value;

		//    Invalidate();
		//  }
		//}

		protected override void OnPaint(PaintEventArgs e) {

			//base.OnPaint(e);

			Graphics g = e.Graphics; // less typing, im lazy.

			if (_themeElement == null || !VisualStyleRenderer.IsSupported) {
				using (Brush brush = new SolidBrush(this.ForeColor)) {
					g.DrawString(this.Text, this.Font, brush, new PointF(0, 0));
				}

				return;
			}

			Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);

			_renderer.DrawText(g, bounds, this.Text, !this.Enabled, TextFormatFlags.Default);

		}

		private void UpdateTheme() {

			if (_themeElement == null) {
				return;
			}

			using (Graphics g = this.CreateGraphics()) {
				_renderer = new VisualStyleRenderer(_themeElement);

				Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
				Rectangle textExtent = _renderer.GetTextExtent(g, bounds, this.Text, TextFormatFlags.Default);

				this.Size = this.MinimumSize = new Size(textExtent.Width, textExtent.Height);
			}
		}
	}
}

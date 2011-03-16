using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Shellscape.UI.Controls.Preferences {

	public class PreferencesButtonGroup : FlowLayoutPanel, INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		private Bitmap _background = null;
		private Bitmap _backgroundOverlay = null;

		public PreferencesButtonGroup() {
			this.Dock = DockStyle.Left;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Width = 200;
			this.Padding = new Padding(6, 10, 6, 10);

			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();

			Font = SystemFonts.MessageBoxFont;
		}

		protected void OnPropertyChanged(string Property) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(Property));
			}
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);

			if (_background != null) {
				_background.Dispose();
			}

			_background = new Bitmap(this.Width, this.Height);
			DrawBackground();
		}

		protected override void OnPaint(PaintEventArgs e) {
			e.Graphics.DrawImage(_background, 0, 0, _background.Width, _background.Height);
			
			base.OnPaint(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			//base.OnPaintBackground(e);
		}

		private void DrawBackground() {

			if (_backgroundOverlay == null) {
				_backgroundOverlay = Utilities.ResourceHelper.GetResourcePNG("shell32.dll", "632");
			}

			VisualStyleElement cpGradient = VisualStyleElement.CreateElement("CONTROLPANEL", 1, 0);
			VisualStyleRenderer renderer = new VisualStyleRenderer(cpGradient);
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

			using (Graphics g = Graphics.FromImage(_background)) {
				renderer.DrawBackground(g, rect);
				g.DrawImage(_backgroundOverlay, 0, 0, _backgroundOverlay.Width, _backgroundOverlay.Height);
			}

		}

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI {
	public partial class About : Form {

		private Bitmap _buffer;

		public About() : base() {
			InitializeComponent();

			this.Text = "Shellscape.UI.About";
			this.TopMost = true;
			this.Size = new Size(532, 304);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
		}

		protected override void OnPaint(PaintEventArgs e) {

			if (_buffer == null) {

				_buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

				using (Graphics g = Graphics.FromImage(_buffer)) {
					
					g.FillRectangle(SystemBrushes.Window, new Rectangle(0, 0, _buffer.Width, _buffer.Height));
					g.DrawLine(SystemPens.ActiveBorder, new Point(0, 144), new Point(_buffer.Width, 144));

					g.DrawLine(SystemPens.Window, new Point(0, 145), new Point(_buffer.Width, 145));

					g.FillRectangle(SystemBrushes.Control, new Rectangle(0, 146, _buffer.Width, _buffer.Height - 146));

					using(Font titleFont = new Font(this.Font.FontFamily, 24, GraphicsUnit.Pixel)){
						e.Graphics.DrawString(Utilities.AssemblyMeta.Title, titleFont, SystemBrushes.ControlText, new Point(14, 24));
					}
				}

			}

			e.Graphics.DrawImage(_buffer, 0, 0, _buffer.Width, _buffer.Height);
			
			base.OnPaint(e);
		}
	}
}

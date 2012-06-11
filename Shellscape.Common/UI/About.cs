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
			
			this.Text = "Shellscape.UI.About";
			this.TopMost = true;
			this.Size = new Size(532, 304);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			InitializeComponent();

			this.SuspendLayout();
			
			this._TextVersion.Text = Utilities.AssemblyMeta.Version;
			this._TextVersion.Font = SystemFonts.MessageBoxFont;

			this._ButtonDonate.Click += delegate(object sender, EventArgs e) {
				Shellscape.Utilities.ApplicationHelper.Donate(this.DonationDescription);
			};

			this.ResumeLayout(true);
		}

		protected virtual void OnPaintIcon(Graphics g) {

		}

		protected override void OnPaint(PaintEventArgs e) {

			if (_buffer == null) {

			_buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

			  using (Graphics g = Graphics.FromImage(_buffer)) {

					g.FillRectangle(SystemBrushes.Window, new Rectangle(0, 0, _buffer.Width, _buffer.Height));
					g.DrawLine(SystemPens.ActiveBorder, new Point(0, 144), new Point(_buffer.Width, 144));

					OnPaintIcon(g);

					g.DrawLine(SystemPens.Window, new Point(0, 145), new Point(_buffer.Width, 145));

					g.FillRectangle(SystemBrushes.Control, new Rectangle(0, 146, _buffer.Width, _buffer.Height - 146));

					String appTitle = Utilities.AssemblyMeta.Title;
					SizeF titleSize;

					appTitle = String.IsNullOrEmpty(appTitle) ? "Shellscape Application" : appTitle;

					using(Font titleFont = new Font(this.Font.FontFamily, 30, GraphicsUnit.Pixel)){
						g.DrawString(appTitle, titleFont, SystemBrushes.ControlText, 4, 24);
						titleSize = g.MeasureString(appTitle, titleFont);
					}

					_TextVersion.Location = new Point(14, (int)titleSize.Height + 24);
			  }
			}


			e.Graphics.DrawImage(_buffer, 0, 0, _buffer.Width, _buffer.Height);
			
			base.OnPaint(e);
		}

		protected virtual string DonationDescription {
			get { return "Shellscape%20Software%20Donation"; }
		}

	}
}

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

namespace Shellscape.UI.ControlPanel {

	public class ControlPanelNavigation : Controls.DoubleBufferedPanel {

		private Bitmap _background = null;
		private Bitmap _backgroundOverlay = null;

		internal Label _otherLabel;

		internal ControlPanelTaskPanel _tasks;
		internal ControlPanelTaskPanel _otherTasks;

		public ControlPanelNavigation() : base() {
			
			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.ResizeRedraw, true);
			UpdateStyles();

			Dock = DockStyle.Left;
			BackColor = System.Drawing.Color.Transparent;
			Width = 200;
			Padding = new Padding(22, 10, 12, 15);
			Font = SystemFonts.MessageBoxFont;

			_tasks = new ControlPanelTaskPanel() { Dock = DockStyle.Fill };

			_otherTasks = new ControlPanelTaskPanel() { 
				AutoSize = true,
				AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
				Dock = DockStyle.Bottom
			};

			_otherLabel = new Label() {
				Text = "Other Tasks",
				Visible = false,
				Padding = new Padding(0, 0, 0, 5)
			};

			if (VisualStyleRenderer.IsSupported) {
				VisualStyleRenderer renderer = VisualStyles.ControlPanel.GetRenderer(VisualStyles.ControlPanel.ControlPanelPart.TaskLink, (int)VisualStyles.ControlPanel.TaskLinkState.Disabled, true);

				using (Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					_otherTasks.Font = renderer.GetFont(g, FontProperty.GlyphFont);
					_otherTasks.ForeColor = renderer.GetColor(ColorProperty.TextColor);
				}
			}

			_otherTasks.Controls.Add(_otherLabel);

			_otherTasks.ControlAdded += delegate(object Sender, ControlEventArgs e) {
				_otherLabel.Visible = true;
			};

			_otherTasks.ControlRemoved += delegate(object Sender, ControlEventArgs e) {
				if (_otherTasks.Controls.Count == 0) {
					_otherLabel.Visible = false;
				}
			};
		
			this.Controls.Add(_otherTasks);
			this.Controls.Add(_tasks);
		}

		public String OtherTasksText {
			get { return _otherLabel.Text; }
			set { _otherLabel.Text = value; }
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);

			Form form = this.FindForm();

			if (form != null) {
				if (!Visible || form.WindowState == FormWindowState.Minimized) {
					return;
				}
			}

			if (_background != null) {
				_background.Dispose();
			}

			_background = new Bitmap(this.Width, this.Height);

			DrawBackground();
		}

		protected override void OnPaint(PaintEventArgs e) {
			if (_background != null) {
				e.Graphics.DrawImage(_background, 0, 0, _background.Width, _background.Height);
			}
			
			base.OnPaint(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			//base.OnPaintBackground(e);
		}

		private void DrawBackground() {

			if (_backgroundOverlay == null) {
				_backgroundOverlay = Utilities.ResourceHelper.GetResourcePNG("shell32.dll", "632");
			}
		
			using (Graphics g = Graphics.FromImage(_background)) {

				if (VisualStyleRenderer.IsSupported) {
					VisualStyleElement cpGradient = VisualStyleElement.CreateElement("CONTROLPANEL", 1, 0);
					VisualStyleRenderer renderer = new VisualStyleRenderer(cpGradient);
					Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

					renderer.DrawBackground(g, rect);
				}
				else {
					g.FillRectangle(SystemBrushes.Control, 0, 0, _backgroundOverlay.Width, _backgroundOverlay.Height);
				}
				
				g.DrawImage(_backgroundOverlay, 0, 0, _backgroundOverlay.Width, _backgroundOverlay.Height);
			}

		}

	}
}

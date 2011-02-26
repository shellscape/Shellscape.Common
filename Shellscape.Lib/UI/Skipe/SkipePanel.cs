using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Shellscape.UI.Skipe {

	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
	[DesignTimeVisible(true), Browsable(true)]
	public partial class SkipePanel : SkipeBaseControl {

		private Color _controlBackColor = Color.White;
		private Color _headerForeColor = Color.FromArgb(100, 100, 100);
		private Color _borderColor = ColorTranslator.FromHtml("#777");
		private Color _shadowColor = SystemColors.ActiveBorder;
		private Color _seperatorColor = Color.FromArgb(200, 200, 200);
		private Color _headerColorFrom = Color.FromArgb(254, 254, 254);
		private Color _headerColorTo = Color.FromArgb(228, 228, 228);

		private int _headerHeight = 39;
		private Image _headerImage;
		private String _headerText;
		private String _headerTextPrefix;
		private Padding _headerPadding = new Padding(10, 5, 10, 5);
		private bool _drawHeader = true;

		public SkipePanel() {
			InitializeComponent();
			SetStyle(ControlStyles.ContainerControl, true);
			UpdateStyles();

			this.Size = new Size(420, 400);
			Margin = new Padding(12, 0, 0, 0);
			Padding = new Padding(10, 45, 10, 10);
			BackColor = Color.Transparent;
			HeaderText = Name;
			Dock = DockStyle.Fill;
		}

		#region .    Hidden Properties

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control AssociatedControl { get; set; }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoScroll {
			get {
				return base.AutoScroll;
			}
			set {
				base.AutoScroll = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Point AutoScrollOffset {
			get {
				return base.AutoScrollOffset;
			}
			set {
				base.AutoScrollOffset = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMinSize {
			get { return base.AutoScrollMinSize; }
			set { base.AutoScrollMinSize = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMargin {
			get { return base.AutoScrollMargin; }
			set { base.AutoScrollMargin = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoSize {
			get {
				return base.AutoSize;
			}
			set {
				base.AutoSize = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new AutoSizeMode AutoSizeMode {
			get {
				return base.AutoSizeMode;
			}
			set {
				base.AutoSizeMode = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage {
			get {
				return base.BackgroundImage;
			}
			set {
				base.BackgroundImage = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageLayout BackgroundImageLayout {
			get {
				return base.BackgroundImageLayout;
			}
			set {
				base.BackgroundImageLayout = value;
			}
		}

		#endregion

		#region .    Color Properties

		[Category("Style")]
		public new Color BackColor {
			get { return base.BackColor; }
			set { base.BackColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color ControlBackColor {
			get { return _controlBackColor; }
			set { _controlBackColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color HeaderForeColor {
			get { return _headerForeColor; }
			set { _headerForeColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color BorderColor {
			get { return _borderColor; }
			set { _borderColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color SeperatorColor {
			get { return _seperatorColor; }
			set { _seperatorColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color HeaderColorFrom {
			get { return _headerColorFrom; }
			set { _headerColorFrom = value; Invalidate(); }
		}

		[Category("Style")]
		public Color HeaderColorTo {
			get { return _headerColorTo; }
			set { _headerColorTo = value; Invalidate(); }
		}

		#endregion

		#region .    Header Properties

		public int HeaderHeight {
			get { return _headerHeight; }
			set { _headerHeight = value; OnResize(EventArgs.Empty); }
		}

		public Image HeaderImage {
			get { return _headerImage; }
			set { _headerImage = value; Invalidate(); }
		}

		internal string HeaderTextPrefix {
			get { return _headerTextPrefix; }
			set {
				_headerTextPrefix = value;
				Invalidate();
			}
		}

		public string HeaderText {
			get { return _headerText; }
			set { _headerText = value; Invalidate(); }
		}

		public Padding HeaderPadding {
			get { return _headerPadding; }
			set { _headerPadding = value; Invalidate(); }
		}

		public bool DrawHeader {
			get { return _drawHeader; }
			set { _drawHeader = value; Invalidate(); }
		}

		#endregion

		public new Boolean Visible {
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e) {
			base.OnPaint(e);

			if (Width == 0) { // designer is throwing fits about this all of a sudden.
				return;
			}

			Rectangle headerRect = new Rectangle(0, 0, Width, _headerHeight - 1);

			using (GraphicsPath path = DrawingHelper.CreateRoundedRectangle(1, 1, Width - 3, Height - 3, 3, DrawingHelper.RectangleCorners.TopLeft | DrawingHelper.RectangleCorners.TopRight))
			using (GraphicsPath borderPath = DrawingHelper.CreateRoundedRectangle(0, 0, Width - 1, Height - 1, 5, DrawingHelper.RectangleCorners.TopLeft | DrawingHelper.RectangleCorners.TopRight))
			using (LinearGradientBrush gradBrush = new LinearGradientBrush(headerRect, _headerColorFrom, _headerColorTo, 90f))
			using (Brush brush = new SolidBrush(ControlBackColor))
			using (Pen borderPathPen = new Pen(Color.FromArgb(75, _shadowColor)))
			using (Pen borderPen = new Pen(_borderColor))
			using(Pen separatorPen = new Pen(_seperatorColor)){

				e.Graphics.SetClip(headerRect);
				e.Graphics.FillPath(gradBrush, path);

				e.Graphics.ResetClip();

				e.Graphics.FillRectangle(brush, 1, _headerHeight, Width - 3, Height - 3 - (_headerHeight - 1));

				e.Graphics.DrawPath(borderPathPen, borderPath);
				e.Graphics.DrawPath(borderPen, path);
				e.Graphics.DrawLine(separatorPen, new Point(2, _headerHeight - 1), new Point(this.Width - 3, _headerHeight - 1));

			}

			PaintContent(e);
		}

		private void PaintContent(PaintEventArgs e) {
			int iconSpace = 5;
			int iconY = 0;
			int iconHeight = 0;
			int textX = 0;

			if (_headerImage != null) {
				if (_headerImage.Height < _headerHeight - (_headerPadding.Top + _headerPadding.Bottom)) {
					iconY = (int)((double)(_headerHeight - _headerImage.Height) / (double)2);
					iconHeight = _headerImage.Height;
				}
				else {
					iconY = _headerPadding.Top;
					iconHeight = _headerHeight - _headerPadding.Top - _headerPadding.Bottom;
				}

				textX = _headerPadding.Left + iconHeight + iconSpace;

				e.Graphics.DrawImage(_headerImage, new Rectangle(_headerPadding.Left, iconY, iconHeight, iconHeight));
			}
			else {
				textX = _headerPadding.Left;
			}

			String headerText = _headerTextPrefix;

			if (!String.IsNullOrEmpty(headerText) && !String.IsNullOrEmpty(_headerText)) {
				headerText += " : ";
			}

			if (!String.IsNullOrEmpty(_headerText)) {
				headerText += _headerText;
			}

			Size textSize = TextRenderer.MeasureText(headerText, Font);
			TextRenderer.DrawText(e.Graphics, headerText, Font, new Rectangle(textX, _headerPadding.Top, Width - textX - _headerPadding.Right, _headerHeight - _headerPadding.Top - _headerPadding.Bottom), _headerForeColor, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
		}

		protected override void OnParentBackgroundImageChanged(EventArgs e) { }

		protected override void OnBackgroundImageLayoutChanged(EventArgs e) { }
	}
}

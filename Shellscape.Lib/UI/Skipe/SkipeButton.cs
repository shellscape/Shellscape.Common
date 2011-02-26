using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Text;
using System.Runtime;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Drawing2D;

namespace Shellscape.UI.Skipe {

	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
	[DefaultEvent("Click"), DefaultProperty("ButtonText"), DesignTimeVisible(true), Browsable(true)]
	public partial class SkipeButton : SkipeButtonBase {

		private int _expandedSize = 0;
		private int _expandSize = 0;
		private int _buttonHeight = 40;

		private DelayedCall _dcExpand;
		private DelayedCall _dcCollapse;

		private bool _mouseHovered = false;
		private bool _mouseDown = false;
		private bool _isExpandable = true;
		private bool _isExpanded = false;

		private Color _buttonBackColor = Color.Transparent;
		protected Color _buttonForeColor = Color.FromArgb(100, 100, 100);
		private Color _borderColor = ColorTranslator.FromHtml("#777");
		private Color _shadowColor = SystemColors.ActiveBorder;
		private Color _normalColorFrom = Color.FromArgb(254, 254, 254);
		private Color _normalColorTo = Color.FromArgb(228, 228, 228);
		private Color _hoverColorFrom = Color.FromArgb(254, 254, 254);
		private Color _hoverColorTo = Color.FromArgb(251, 251, 251);
		private Color _downColorFrom = Color.FromArgb(224, 224, 224);
		private Color _downColorTo = Color.FromArgb(200, 200, 200);

		private SkipeButtonItemCollection _buttonItems = new SkipeButtonItemCollection();

		public event EventHandler Expanding;
		public event EventHandler Collapsing;
		public event EventHandler Activated;
		public event EventHandler<SkipeButtonItemEventArgs> ItemActivated;

		public SkipeButton() {
			InitializeComponent();
			BackColor = Color.Transparent;
			ButtonText = Name;
			Margin = new Padding(0, 0, 0, 6);
			Size = new System.Drawing.Size(175, 40);

			_buttonItems.ItemAdded += buttonItems_ItemAdded;
			_buttonItems.BeforeItemRemoved += buttonItems_BeforeItemRemoved;
			_buttonItems.ItemRemoved += buttonItems_ItemRemoved;
			_buttonItems.ItemActivated += buttonItems_SelectionChanged;
		}

		public void Activate() {
			if (Activated != null) {
				Activated(this, EventArgs.Empty);
			}
		}

		#region .    Events

		private void OnExpanding() {
			if (Expanding != null) {
				Expanding(this, EventArgs.Empty);
			}
		}

		private void OnCollapsing() {
			if (Collapsing != null) {
				Collapsing(this, EventArgs.Empty);
			}
		}

		#endregion

		#region .    Invisible Properties

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage {
			get { return base.BackgroundImage; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImageLayout BackgroundImageLayout {
			get { return base.BackgroundImageLayout; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new BorderStyle BorderStyle {
			get { return base.BorderStyle; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new RightToLeft RightToLeft {
			get { return base.RightToLeft; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AllowDrop {
			get { return base.AllowDrop; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoScroll {
			get { return base.AutoScroll; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMinSize {
			get { return base.AutoScrollMinSize; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size AutoScrollMargin {
			get { return base.AutoScrollMargin; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AutoSize {
			get { return base.AutoSize; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new AutoSizeMode AutoSizeMode {
			get { return base.AutoSizeMode; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size MaximumSize {
			get { return base.MaximumSize; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size MinimumSize {
			get { return base.MinimumSize; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding {
			get { return base.Padding; }
			set { base.Padding = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ImeMode ImeMode {
			get { return base.ImeMode; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ContextMenuStrip ContextMenuStrip {
			get { return base.ContextMenuStrip; }
			set { throw new NotSupportedException(); }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ForeColor {
			get { return base.ForeColor; }
			set { throw new NotSupportedException(); }
		}

		#endregion

		#region .    Color Properties

		[Category("Style")]
		public Color ShadowColor {
			get { return _shadowColor; }
			set {
				_shadowColor = value;
				Invalidate();
			}
		}

		[Category("Style")]
		public new Color BackColor {
			get { return base.BackColor; }
			set { base.BackColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color ButtonBackColor {
			get { return _buttonBackColor; }
			set { _buttonBackColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color ButtonForeColor {
			get { return _buttonForeColor; }
			set { _buttonForeColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color BorderColor {
			get { return _borderColor; }
			set { _borderColor = value; Invalidate(); }
		}

		[Category("Style")]
		public Color NormalColorFrom {
			get { return _normalColorFrom; }
			set { _normalColorFrom = value; Invalidate(); }
		}

		[Category("Style")]
		public Color NormalColorTo {
			get { return _normalColorTo; }
			set { _normalColorTo = value; Invalidate(); }
		}

		[Category("Style")]
		public Color HoverColorFrom {
			get { return _hoverColorFrom; }
			set { _hoverColorFrom = value; Invalidate(); }
		}

		[Category("Style")]
		public Color HoverColorTo {
			get { return _hoverColorTo; }
			set { _hoverColorTo = value; Invalidate(); }
		}

		[Category("Style")]
		public Color DownColorFrom {
			get { return _downColorFrom; }
			set { _downColorFrom = value; Invalidate(); }
		}

		[Category("Style")]
		public Color DownColorTo {
			get { return _downColorTo; }
			set { _downColorTo = value; Invalidate(); }
		}

		#endregion

		public int ButtonHeight {
			get { return _buttonHeight; }
			set { 
				_buttonHeight = value; 
				OnResize(EventArgs.Empty); 
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SkipeButtonItemCollection ButtonItems {
			get { return _buttonItems; }
		}

		#region .    Expand Properties

		public bool Expandable {
			get { return _isExpandable; }
			set { _isExpandable = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsExpanded {
			get {
				if (_isExpandable)
					return _isExpanded;
				else
					return false;
			}
		}

		#endregion

		#region .    Item Events

		private void buttonItems_SelectionChanged(object sender, SkipeButtonItemEventArgs e) {
			if (ItemActivated != null) {
				ItemActivated(this, e);
			}
		}

		private void buttonItems_BeforeItemRemoved(object sender, SkipeButtonItemEventArgs e) {
			if (_isExpanded) {
				this.Height -= e.Item.Height;
			}
		}

		private void buttonItems_ItemRemoved(object sender, EventArgs e) {
			UpdateItems(_isExpanded);
		}

		private void buttonItems_ItemAdded(object sender, SkipeButtonItemEventArgs e) {
			UpdateItems(_isExpanded);
		}

		#endregion

		#region .    Mouse Events

		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			_mouseHovered = true;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			_mouseHovered = false;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			_mouseDown = true;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			_mouseDown = false;
			Invalidate();
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			//TriggerExpandCollapse(); // we don't want this executing automatically.

			if (Activated != null) {
				Activated(this, EventArgs.Empty);
			}

		}

		#endregion

		#region .    Expand / Collapse Delayed Methods

		public void TriggerExpandCollapse() {

			if (_buttonItems.Count == 0) {
				return;
			}

			_expandSize = 2; // corresponds to the padding

			foreach (SkipeButtonItem val in _buttonItems) {
				_expandSize += val.Height;
			}

			if (_isExpandable) {
				if (_isExpanded) {
					if (_dcExpand != null) {
						_dcExpand.Cancel();
						_dcExpand.Dispose();
						_dcExpand = null;
					}

					OnCollapsing();
					_dcCollapse = DelayedCall.Create(CollapseButton, 1);
					_dcCollapse.Start();
				}
				else {
					if (_dcCollapse != null) {
						_dcCollapse.Cancel();
						_dcCollapse.Dispose();
						_dcCollapse = null;
					}

					OnExpanding();
					UpdateItems(true);
					_isExpanded = true;
					_dcExpand = DelayedCall.Create(ExpandButton, 1);
					_dcExpand.Start();
				}
			}
		}

		private void ExpandButton() {

			if (_buttonItems.Count == 0) {
				return;
			}

			if (_expandedSize < _expandSize) {
				if (_expandedSize > _expandSize - 40)
					_expandedSize += 5;
				else if (_expandedSize > _expandSize - 15)
					_expandedSize += 1;
				else
					_expandedSize += 10;

				Height = _buttonHeight + _expandedSize;
				_dcExpand.Reset();
			}
			else {
				Height = _buttonHeight + _expandSize;
				_isExpanded = true;
				OnResize(EventArgs.Empty);
			}
		}

		private void CollapseButton() {
			if (_expandedSize > 0) {
				if (_expandedSize > _expandSize - 40)
					_expandedSize -= 5;
				else if (_expandedSize > _expandSize - 15)
					_expandedSize -= 1;
				else
					_expandedSize -= 10;

				Height = _buttonHeight + _expandedSize;
				_dcCollapse.Reset();
			}
			else {
				Height = _buttonHeight;
				_isExpanded = false;
				UpdateItems(false);
				OnResize(EventArgs.Empty);
			}
		}

		#endregion

		protected override void OnResize(EventArgs e) {
			base.OnResize(e);

			if (!_isExpanded)
				Height = _buttonHeight;

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e) {
			//e.Graphics.Clear(Color.Transparent);
			PaintButton(e);
			PaintContent(e);
		}

		//protected override void OnPaintBackground(PaintEventArgs e) {
		//  //base.OnPaintBackground(e);
		//}

		//protected override void OnParentBackgroundImageChanged(EventArgs e) {
		//  //base.OnParentBackgroundImageChanged(e);
		//}

		//protected override void OnBackgroundImageLayoutChanged(EventArgs e) {
		//  //base.OnBackgroundImageLayoutChanged(e);
		//}

		private void UpdateItems(bool showItems) {
			SuspendLayout();
			Controls.Clear();

			if (showItems) {
				Padding = new Padding(2, _buttonHeight, 2, 0);

				foreach (SkipeButtonItem val in _buttonItems) {
					Controls.Add(val);
					val.Dock = DockStyle.Top;
					val.ActiveButton = false;
					val.BringToFront();
				}

			}

			ResumeLayout();

			Invalidate();
		}

		private void PaintContent(PaintEventArgs e) {
			int iconSpace = 5;
			int iconY = 0;
			int iconHeight = 0;
			int textX = 0;

			if (_buttonImage != null) {
				if (_buttonImage.Height < _buttonHeight - (_buttonPadding.Top + _buttonPadding.Bottom)) {
					iconY = (int)((double)(_buttonHeight - _buttonImage.Height) / (double)2);
					iconHeight = _buttonImage.Height;
				}
				else {
					iconY = _buttonPadding.Top;
					iconHeight = _buttonHeight - _buttonPadding.Top - _buttonPadding.Bottom;
				}

				textX = _buttonPadding.Left + iconHeight + iconSpace;

				e.Graphics.DrawImage(_buttonImage, new Rectangle(_buttonPadding.Left, iconY, iconHeight, iconHeight));
			}
			else {
				textX = _buttonPadding.Left;
			}

			Size textSize = TextRenderer.MeasureText(_buttonText, Font);
			TextRenderer.DrawText(e.Graphics, _buttonText, Font, new Rectangle(textX, _buttonPadding.Top, Width - textX - _buttonPadding.Right, _buttonHeight - _buttonPadding.Top - _buttonPadding.Bottom), _buttonForeColor, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
		}

		private void PaintButton(PaintEventArgs e) {
			GraphicsPath gp;
			GraphicsPath gpShadow;

			if (_isExpanded) {
				gp = DrawingHelper.CreateRoundedRectangle(1, 1, this.Width - 3, _buttonHeight - 3, 3, DrawingHelper.RectangleCorners.TopLeft | DrawingHelper.RectangleCorners.TopRight);
				gpShadow = DrawingHelper.CreateRoundedRectangle(0, 0, this.Width - 1, _buttonHeight - 1, 5, DrawingHelper.RectangleCorners.TopLeft | DrawingHelper.RectangleCorners.TopRight);
			}
			else {
				gp = DrawingHelper.CreateRoundedRectangle(1, 1, this.Width - 3, _buttonHeight - 3, 3);
				gpShadow = DrawingHelper.CreateRoundedRectangle(0, 0, this.Width - 1, _buttonHeight - 1, 5);
			}

			if (_isExpanded) {
				e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, _buttonHeight / 2, this.Width - 3, this.Height - (_buttonHeight / 2) - 2));

				LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, _buttonHeight), _normalColorFrom, _normalColorTo, 90f);
				e.Graphics.FillPath(lgb, gp);
				lgb.Dispose();
			}
			else {
				if (_mouseDown) {
					LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, _buttonHeight), _downColorFrom, _downColorTo, 90f);
					e.Graphics.FillPath(lgb, gp);
					lgb.Dispose();
				}
				else {
					if (_mouseHovered) {
						LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, _buttonHeight), _hoverColorFrom, _hoverColorTo, 90f);
						e.Graphics.FillPath(lgb, gp);
						lgb.Dispose();
					}
					else {
						LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, _buttonHeight), _normalColorFrom, _normalColorTo, 90f);
						e.Graphics.FillPath(lgb, gp);
						lgb.Dispose();
					}
				}
			}

			using (Pen pen = new Pen(_borderColor))
			using (Pen borderPen = new Pen(Color.FromArgb(75, _borderColor)))
			using (Pen shadowPen = new Pen(Color.FromArgb(75, _shadowColor))) {

				e.Graphics.DrawPath(shadowPen, gpShadow);
				e.Graphics.DrawPath(pen, gp);

				if (_isExpanded) {
					e.Graphics.DrawLine(pen, new Point(1, _buttonHeight - 1), new Point(1, Height - 2));
					e.Graphics.DrawLine(shadowPen, new Point(0, _buttonHeight), new Point(0, Height - 1));

					e.Graphics.DrawLine(pen, new Point(this.Width - 2, _buttonHeight - 1), new Point(this.Width - 2, Height - 2));
					e.Graphics.DrawLine(shadowPen, new Point(this.Width - 1, _buttonHeight), new Point(this.Width - 1, Height - 1));

					e.Graphics.DrawLine(pen, new Point(2, Height - 2), new Point(this.Width - 3, Height - 2));
					e.Graphics.DrawLine(shadowPen, new Point(1, Height - 1), new Point(this.Width - 2, Height - 1));

					e.Graphics.DrawLine(pen, new Point(2, _buttonHeight - 1), new Point(this.Width - 3, _buttonHeight - 1));
					e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 200, 200)), new Point(2, _buttonHeight - 2), new Point(this.Width - 3, _buttonHeight - 2));
				}
			}

			gp.Dispose();
			gpShadow.Dispose();
		}

	}
}

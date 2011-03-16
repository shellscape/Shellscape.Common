using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls.Preferences {

	[DesignTimeVisible(false), Browsable(false), Serializable]
	public partial class PreferencesButtonItem : PreferencesButtonBase {

		private Color _normalColor = Color.White;
		private Color _hoverColor = Color.White;
		private Color _activeColor = Color.FromArgb(130, 190, 250);
		private Color _buttonForeColor = Color.FromArgb(100, 100, 100);

		private bool _activeButton = false;
		private bool _mouseHovered = false;
		private bool _mouseDown = false;

		public event EventHandler Activated;

		public PreferencesButtonItem() {
			InitializeComponent();
			BackColor = Color.White;
			ButtonText = Name;
		}

		public void Activate() {
			OnActivated();
		}

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

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int TabIndex {
			get { return base.TabIndex; }
			set { base.TabIndex = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool TabStop {
			get { return base.TabStop; }
			set { base.TabStop = value; }
		}

		#endregion

		#region .    Color Properties
				
		[Category("Appearance")]
		public Color NormalColor {
			get { return _normalColor; }
			set { _normalColor = value; Invalidate(); }
		}

		[Category("Appearance")]
		public Color HoverColor {
			get { return _hoverColor; }
			set { _hoverColor = value; Invalidate(); }
		}

		private Color downColor = Color.White;
		[Category("Appearance")]
		public Color DownColor {
			get { return downColor; }
			set { downColor = value; Invalidate(); }
		}
				
		[Category("Appearance")]
		public Color ActiveColor {
			get { return _activeColor; }
			set { _activeColor = value; Invalidate(); }
		}

		[Category("Appearance")]
		public Color ButtonForeColor {
			get { return _buttonForeColor; }
			set { _buttonForeColor = value; Invalidate(); }
		}

		#endregion

		[Category("Appearance")]
		public bool ActiveButton {
			get { return _activeButton; }
			set { 
				_activeButton = value; 
				Invalidate(); 
			}
		}

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
			_activeButton = true;
			OnActivated();
			Invalidate();
		}

		private void OnActivated() {
			if (Activated != null) {
				Activated(this, EventArgs.Empty);
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			
			if (_activeButton) {
				e.Graphics.Clear(_activeColor);
			}
			else {
				if (_mouseDown) {
					e.Graphics.Clear(downColor);
				}
				else {
					if (_mouseHovered) {
						e.Graphics.Clear(_hoverColor);
					}
					else {
						e.Graphics.Clear(BackColor);
					}
				}
			}

			int iconSpace = 5;
			int iconY = 0;
			int iconHeight = 0;
			int textX = 0;

			if (_buttonImage != null) {
				if (_buttonImage.Height < Height - (_buttonPadding.Top + _buttonPadding.Bottom)) {
					iconY = (int)((double)(Height - _buttonImage.Height) / (double)2);
					iconHeight = _buttonImage.Height;
				}
				else {
					iconY = _buttonPadding.Top;
					iconHeight = Height - _buttonPadding.Top - _buttonPadding.Bottom;
				}

				textX = _buttonPadding.Left + iconHeight + iconSpace;

				e.Graphics.DrawImage(_buttonImage, new Rectangle(_buttonPadding.Left, iconY, iconHeight, iconHeight));
			}
			else {
				textX = _buttonPadding.Left;
			}

			Size textSize = TextRenderer.MeasureText(_buttonText, Font);
			TextRenderer.DrawText(e.Graphics, _buttonText, Font, new Rectangle(textX, _buttonPadding.Top, Width - textX - _buttonPadding.Right, Height - _buttonPadding.Top - _buttonPadding.Bottom), _buttonForeColor, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			//base.OnPaintBackground(e);
		}

		protected override void OnParentBackgroundImageChanged(EventArgs e) {
			//base.OnParentBackgroundImageChanged(e);
		}

		protected override void OnBackgroundImageLayoutChanged(EventArgs e) {
			//base.OnBackgroundImageLayoutChanged(e);
		}

	}
}

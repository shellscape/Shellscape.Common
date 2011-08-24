using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Shellscape.UI.Controls {

	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(TextBox))]
	public class NumericTextBox : TextBox {

		private const int _maxPrecision = 10;  // max dot length
		private const int _maxLength = 27; // decimal can be 28 bits.

		private int _precision = 0;

		private Boolean _allowNegativeValues = true;

		private String _valueFormat = string.Empty;
		private char _decimalChar = '.';
		private char _negativeChar = '-';

		#region .    Win32

		private const int WM_CHAR = 0x0102;
		private const int WM_CUT = 0x0300;
		private const int WM_COPY = 0x0301;
		private const int WM_PASTE = 0x0302;
		private const int WM_CLEAR = 0x0303;

		#endregion

		public NumericTextBox() {

			base.Text = "0";

			System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
			_decimalChar = ci.NumberFormat.CurrencyDecimalSeparator[0];
			_negativeChar = ci.NumberFormat.NegativeSign[0];

			this.SetValueFormatStr();
		}

		[DefaultValue(0)]
		public int Precision {
			get {
				return _precision;
			}
			set {
				if (_precision != value) {
					if (value < 0 || value > _maxPrecision) {
						_precision = 0;
					}
					else {
						_precision = value;
					}
					this.SetValueFormatStr();
					base.Text = this.DecimalValue.ToString(_valueFormat);
				}
			}
		}

		public decimal DecimalValue {
			get {
				decimal val;
				if (decimal.TryParse(base.Text, out val)) {
					return val;
				}
				return 0;
			}
		}

		public int Value {
			get {
				decimal val = this.DecimalValue;
				return (int)val;
			}
		}

		[DefaultValue(false)]
		public bool AllowNegativeValues {
			get { return _allowNegativeValues; }
			set {
				if (_allowNegativeValues != value) {
					_allowNegativeValues = value;
				}
			}
		}

		[DefaultValue("0")]
		public override string Text {
			get {
				return base.Text;
			}
			set {
				decimal val;
				if (decimal.TryParse(value, out val)) {
					base.Text = val.ToString(_valueFormat);
				}
				else {
					base.Text = 0.ToString(_valueFormat);
				}
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override int MaxLength {
			get {
				return base.MaxLength;
			}
		}

		protected override void WndProc(ref Message m) {
			if (m.Msg == WM_PASTE) { // mouse paste
				this.ClearSelection();
				SendKeys.Send(Clipboard.GetText());
				base.OnTextChanged(EventArgs.Empty);
			}
			else if (m.Msg == WM_COPY) { // mouse copy
				Clipboard.SetText(this.SelectedText);
			}
			else if (m.Msg == WM_CUT) { // mouse cut or ctrl+x shortcut
				Clipboard.SetText(this.SelectedText);
				this.ClearSelection();
				base.OnTextChanged(EventArgs.Empty);
			}
			else if (m.Msg == WM_CLEAR) {
				this.ClearSelection();
				base.OnTextChanged(EventArgs.Empty);
			}
			else {
				base.WndProc(ref m);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys)Shortcut.CtrlV) {
				this.ClearSelection();

				string text = Clipboard.GetText();
				//                SendKeys.Send(text);

				for (int k = 0; k < text.Length; k++) { // can not use SendKeys.Send
					SendCharKey(text[k]);
				}
				return true;
			}
			else if (keyData == (Keys)Shortcut.CtrlC) {
				Clipboard.SetText(this.SelectedText);
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);

			if (!this.ReadOnly) {
				if (e.KeyData == Keys.Delete || e.KeyData == Keys.Back) {
					if (this.SelectionLength > 0) {
						this.ClearSelection();
					}
					else {
						this.DeleteText(e.KeyData);
					}
					e.SuppressKeyPress = true;  // does not transform event to KeyPress, but to KeyUp
				}
			}

		}

		/// <summary>
		/// repostion SelectionStart, recalculate SelectedLength
		/// </summary>
		protected override void OnKeyPress(KeyPressEventArgs e) {
			base.OnKeyPress(e);

			if (this.ReadOnly) {
				return;
			}

			if (e.KeyChar == (char)13 || e.KeyChar == (char)3 || e.KeyChar == (char)22 || e.KeyChar == (char)24) {
				return;
			}

			if (_precision == 0 && e.KeyChar == _decimalChar) {
				e.Handled = true;
				return;
			}

			if (!_allowNegativeValues && e.KeyChar == _negativeChar && base.Text.IndexOf(_negativeChar) < 0) {
				e.Handled = true;
				return;
			}

			if (!char.IsDigit(e.KeyChar) && e.KeyChar != _negativeChar && e.KeyChar != _decimalChar) {
				e.Handled = true;
				return;
			}

			if (base.Text.Length >= _maxLength && e.KeyChar != _negativeChar) {
				e.Handled = true;
				return;
			}

			if (e.KeyChar == _decimalChar || e.KeyChar == _negativeChar) { // will position after dot(.) or first
				this.SelectionLength = 0;
			}
			else {
				this.ClearSelection();
			}

			bool isNegative = (base.Text[0] == _negativeChar) ? true : false;

			if (isNegative && this.SelectionStart == 0) {
				this.SelectionStart = 1;
			}

			if (e.KeyChar == _negativeChar) {
				int selStart = this.SelectionStart;

				if (!isNegative) {
					base.Text = _negativeChar + base.Text;
					this.SelectionStart = selStart + 1;
				}
				else {
					base.Text = base.Text.Substring(1, base.Text.Length - 1);
					if (selStart >= 1) {
						this.SelectionStart = selStart - 1;
					}
					else {
						this.SelectionStart = 0;
					}
				}
				e.Handled = true;  // minus(-) has been handled
				return;
			}

			int dotPos = base.Text.IndexOf(_decimalChar) + 1;

			if (e.KeyChar == _decimalChar) {
				if (dotPos > 0) {
					this.SelectionStart = dotPos;
				}
				e.Handled = true;  // dot has been handled 
				return;
			}

			if (base.Text == "0") {
				this.SelectionStart = 0;
				this.SelectionLength = 1;  // replace thre first char, ie. 0
			}
			else if (base.Text == _negativeChar + "0") {
				this.SelectionStart = 1;
				this.SelectionLength = 1;  // replace thre first char, ie. 0
			}
			else if (_precision > 0) {
				if (base.Text[0] == '0' && dotPos == 2 && this.SelectionStart <= 1) {
					this.SelectionStart = 0;
					this.SelectionLength = 1;  // replace thre first char, ie. 0
				}
				else if (base.Text.Substring(0, 2) == _negativeChar + "0" && dotPos == 3 && this.SelectionStart <= 2) {
					this.SelectionStart = 1;
					this.SelectionLength = 1;  // replace thre first char, ie. 0
				}
				else if (this.SelectionStart == dotPos + _precision) {
					e.Handled = true;  // last position after text
				}
				else if (this.SelectionStart >= dotPos) {
					this.SelectionLength = 1;
				}
				else if (this.SelectionStart < dotPos - 1) {
					this.SelectionLength = 0;
				}
			}
		}

		protected override void OnLeave(EventArgs e) {
			if (string.IsNullOrEmpty(base.Text)) {
				base.Text = 0.ToString(_valueFormat);
			}
			else {
				base.Text = this.DecimalValue.ToString(_valueFormat);
			}
			base.OnLeave(e);
		}

		private void SetValueFormatStr() {
			_valueFormat = "F" + _precision.ToString();
		}

		private void SendCharKey(char c) {
			Message msg = new Message();

			msg.HWnd = this.Handle;
			msg.Msg = WM_CHAR;
			msg.WParam = (IntPtr)c;
			msg.LParam = IntPtr.Zero;

			base.WndProc(ref msg);
		}

		private void DeleteText(Keys key) {
			int selStart = this.SelectionStart;  // base.Text will be delete at selStart - 1

			if (key == Keys.Delete)  // Delete key change to BackSpace key, adjust selStart value
            {
				selStart += 1;  // adjust position for BackSpace
				if (selStart > base.Text.Length)  // text end
                {
					return;
				}

				if (this.IsSeparator(selStart - 1))  // next if delete dot(.) or thousands(;)
                {
					selStart++;
				}
			}
			else  // BackSpace key
            {
				if (selStart == 0)  // first position
                {
					return;
				}

				if (this.IsSeparator(selStart - 1)) // char which will be delete is separator
                {
					selStart--;
				}
			}

			if (selStart == 0 || selStart > base.Text.Length)  // selStart - 1 no digig
            {
				return;
			}

			int dotPos = base.Text.IndexOf(_decimalChar);
			bool isNegative = (base.Text.IndexOf(_negativeChar) >= 0) ? true : false;

			if (selStart > dotPos && dotPos >= 0)  // delete digit after dot(.)
            {
				base.Text = base.Text.Substring(0, selStart - 1) + base.Text.Substring(selStart, base.Text.Length - selStart) + "0";
				base.SelectionStart = selStart - 1;  // SelectionStart is unchanged
			}
			else // delete digit before dot(.)
            {
				if (selStart == 1 && isNegative)  // delete 1st digit and Text is negative,ie. delete minus(-)
                {
					if (base.Text.Length == 1)  // ie. base.Text is '-'
                    {
						base.Text = "0";
					}
					else if (dotPos == 1)  // -.* format
                    {
						base.Text = "0" + base.Text.Substring(1, base.Text.Length - 1);
					}
					else {
						base.Text = base.Text.Substring(1, base.Text.Length - 1);
					}
					base.SelectionStart = 0;
				}
				else if (selStart == 1 && (dotPos == 1 || base.Text.Length == 1))  // delete 1st digit before dot(.) or Text.Length = 1
                {
					base.Text = "0" + base.Text.Substring(1, base.Text.Length - 1);
					base.SelectionStart = 1;
				}
				else if (isNegative && selStart == 2 && base.Text.Length == 2)  // -* format
                {
					base.Text = _negativeChar + "0";
					base.SelectionStart = 1;
				}
				else if (isNegative && selStart == 2 && dotPos == 2)  // -*.* format
                {
					base.Text = _negativeChar + "0" + base.Text.Substring(2, base.Text.Length - 2);
					base.SelectionStart = 1;
				}
				else  // selStart > 0
                {
					base.Text = base.Text.Substring(0, selStart - 1) + base.Text.Substring(selStart, base.Text.Length - selStart);
					base.SelectionStart = selStart - 1;
				}
			}
		}

		private void ClearSelection() {
			if (this.SelectionLength == 0) {
				return;
			}

			if (this.SelectedText.Length == base.Text.Length) {
				base.Text = 0.ToString(_valueFormat);
				return;
			}

			int selLength = this.SelectedText.Length;
			if (this.SelectedText.IndexOf(_decimalChar) >= 0) {
				selLength--; // selected text contains dot(.), selected length minus 1
			}

			this.SelectionStart += this.SelectedText.Length;  // after selected text
			this.SelectionLength = 0;

			for (int k = 1; k <= selLength; k++) {
				this.DeleteText(Keys.Back);
			}
		}

		private bool IsSeparator(int index) {
			return this.IsSeparator(base.Text[index]);
		}

		private bool IsSeparator(char c) {
			if (c == _decimalChar) {
				return true;
			}
			return false;
		}

		#region  .    Hidden Properties

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new string[] Lines { get { return base.Lines; } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode { get { return base.ImeMode; } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new char PasswordChar { get { return base.PasswordChar; } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseSystemPasswordChar { get { return base.UseSystemPasswordChar; } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool Multiline { get { return base.Multiline; } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new AutoCompleteStringCollection AutoCompleteCustomSource { get { return base.AutoCompleteCustomSource; } }

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new AutoCompleteMode AutoCompleteMode {
			get { return base.AutoCompleteMode; }
			set { base.AutoCompleteMode = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new AutoCompleteSource AutoCompleteSource { get { return base.AutoCompleteSource; } }

		#endregion
	}
}


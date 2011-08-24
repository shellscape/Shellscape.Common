using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls {

	/// <summary>
	/// This TextBox will only notify of TextChanged when it's perceived that the user has stopped entering data.
	/// </summary>
	public class InputBufferedTextBox : TextBox {

		private CallBuffer _buffer;

		public InputBufferedTextBox() : base() {
			_buffer = new CallBuffer(NotifyChanged, 1000);
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			_buffer.Buffer();
			base.OnKeyDown(e);
		}

		protected override void OnTextChanged(EventArgs e) {
			if(_buffer.Active){
				return;
			}
			base.OnTextChanged(e);
		}

		private void NotifyChanged() {
			if (this.InvokeRequired) {
				this.Invoke(new Action(delegate() {
					base.OnTextChanged(EventArgs.Empty);
				}));
				return;
			}
			base.OnTextChanged(EventArgs.Empty);
		}

	}
}

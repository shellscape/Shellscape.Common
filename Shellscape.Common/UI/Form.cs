using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI {

	/// <summary>
	/// A double-buffered (properly) utility form.
	/// </summary>
	public class Form : System.Windows.Forms.Form {

		public Form() {

			this.SetStyle(
				ControlStyles.ResizeRedraw |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint,
			true);

			this.UpdateStyles();

			this.Text = "Shellscape.UI.Form";

		}

	}
}

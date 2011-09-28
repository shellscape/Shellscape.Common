using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls {

	[DesignTimeVisible(false), Browsable(false)]
	public class DoubleBufferedControl : UserControl {

		public DoubleBufferedControl() {
			// Set the value of the double-buffering style bits to true.
			this.SetStyle(
				ControlStyles.DoubleBuffer | 
				ControlStyles.UserPaint | 
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.SupportsTransparentBackColor, 
			true);
			this.UpdateStyles();
		}

	}
}

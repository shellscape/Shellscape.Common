using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.ComponentModel;

namespace GmailNotifierPlus {
	public static class Extensions {

		public static IEnumerable<Control> All(this Control.ControlCollection controls) {
			foreach (Control control in controls) {
				foreach (Control grandChild in control.Controls.All())
					yield return grandChild;

				yield return control;
			}
		}

	}
}

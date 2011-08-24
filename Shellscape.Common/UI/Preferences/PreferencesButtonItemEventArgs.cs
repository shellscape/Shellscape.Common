using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.UI.Controls.Preferences {

	public class PreferencesButtonItemEventArgs : EventArgs {

		public PreferencesButtonItem Item { get; set; }
		public int Index { get; set; }

		public PreferencesButtonItemEventArgs(PreferencesButtonItem obj, int index) {
			Item = obj;
			Index = index;
		}
	}

}

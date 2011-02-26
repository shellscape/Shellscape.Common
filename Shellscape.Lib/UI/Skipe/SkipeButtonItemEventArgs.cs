using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.UI.Skipe {

	public class SkipeButtonItemEventArgs : EventArgs {

		public SkipeButtonItem Item { get; set; }
		public int Index { get; set; }

		public SkipeButtonItemEventArgs(SkipeButtonItem obj, int index) {
			Item = obj;
			Index = index;
		}
	}

}

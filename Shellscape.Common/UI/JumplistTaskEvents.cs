using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.UI {
	internal static class JumplistTaskEvents {

		static JumplistTaskEvents() {
			JumplistTaskEvents.ClickEvents = new Dictionary<String, JumplistClickHandler>();
		}

		public static Dictionary<String, JumplistClickHandler> ClickEvents { get; private set; }

	}
}

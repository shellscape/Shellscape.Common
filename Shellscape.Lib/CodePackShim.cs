using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Shellscape {
	public static class CodePackShim {

		// Resolves a bug with Windows API Code Pack v1.0.1
		// ThumbnailToolbarButton Click event isn’t fired when process is running elevated (i.e. as Administrator)
		// Since I run visual studio with elevated privs for debugging IIS attached asp.net apps, this needs to be here.
		// http://blogs.microsoft.co.il/blogs/arik/archive/2010/03.aspx

		// Updated to v1.1 of the code pack. Bug still present.

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr ChangeWindowMessageFilter(uint message, uint dwFlag);

		[DllImport("user32.dll")]
		public static extern uint RegisterWindowMessage(string message);

		private const uint MSGFLT_ADD = 1;
		private const uint WM_COMMAND = 0x0111;
		private const uint WM_SYSCOMMAND = 0x112;
		private const uint WM_ACTIVATE = 0x0006;

		/// <summary>
		/// Specifies that the taskbar-related windows messages should pass through the Windows UIPI mechanism even if the process is
		/// running elevated. Calling this method is not required unless the process is running elevated.
		/// </summary>
		public static void AllowTaskbarWindowMessagesThroughUIPI() {
			uint WM_TaskbarButtonCreated = RegisterWindowMessage("TaskbarButtonCreated");

			ChangeWindowMessageFilter(WM_TaskbarButtonCreated, MSGFLT_ADD);
			ChangeWindowMessageFilter(WM_COMMAND, MSGFLT_ADD);
			ChangeWindowMessageFilter(WM_SYSCOMMAND, MSGFLT_ADD);
			ChangeWindowMessageFilter(WM_ACTIVATE, MSGFLT_ADD);
		}

	}
}

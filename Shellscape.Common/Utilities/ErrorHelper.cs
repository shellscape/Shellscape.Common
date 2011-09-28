using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

using Microsoft.Win32;
using Microsoft.WindowsAPI.Dialogs;
using Microsoft.WindowsAPI.Shell;

namespace Shellscape.Utilities {
	public static class ErrorHelper {

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		const uint SWP_NOSIZE = 0x0001;
		const uint SWP_NOZORDER = 0x0004;
		const uint SWP_NOMOVE = 0x0002;

		private static IntPtr HWND_TOPMOST = new IntPtr(-1);

		private static String _LogPath = null;

		static ErrorHelper() {
			String appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			_LogPath = Path.Combine(appData, Utilities.AssemblyMeta.Title, "Error Logs");

			if (!Directory.Exists(_LogPath)) {
				Directory.CreateDirectory(_LogPath);
			}

		}

		/// <summary>
		/// Shows the user a dialog explaining the error, and creates a log file for the error.
		/// </summary>
		/// <param name="e"></param>
		public static void Report(Exception e) {
			Guid guid = Guid.NewGuid();
			Show(e, guid);
			Log(e, guid);
		}

		public static void Show(Exception e, Guid guid) {

			String message = String.Concat("Error ID: ", guid.ToString(), "\n\n", e.Message);

			TaskDialog dialog = new TaskDialog() {
				HyperlinksEnabled = true,
				DetailsExpanded = false,
				Cancelable = false,
				Icon = TaskDialogStandardIcon.Error,
				DetailsExpandedText = e.StackTrace,
				Caption = "Gmail Notifier Plus Error",
				InstructionText = "An unhandled exception occurred:",
				Text = message,
				FooterIcon = TaskDialogStandardIcon.Information,
				//Please <a href=\"#stub\">Click Here</a> to automagically report the issue.\
				FooterText = "Press CTL + C to copy this error to the clipboard."
			};

			dialog.HyperlinkClick += delegate(object sender, TaskDialogHyperlinkClickedEventArgs evt) {
				Send(guid);
			};

			SetWindowPos(dialog.OwnerWindowHandle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);

			dialog.Show();
		}

		public static void Log(Exception e, Guid guid) {

			var info = new Microsoft.VisualBasic.Devices.ComputerInfo();

			String path = Path.Combine(_LogPath, guid.ToString());

			String os = String.Join(" ", "CPU:", info.OSFullName, info.OSVersion, info.OSPlatform);
			String memory = String.Concat("Memory: ", info.TotalPhysicalMemory);
			String date = String.Concat("Date: ", DateTime.Now.ToString());
			String processor = "Processor: Unknown";

			try {
				using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0", false)) {
					processor = String.Join(" ", "Processor:",
						key.GetValue("VendorIdentifier"),
						key.GetValue("Identifier"),
						key.GetValue("ProcessorNameString"),
						key.GetValue("~Mhz"), "~Mhz"
					);
					key.Close();
				}
			}
			catch (System.Security.SecurityException) {
			}

			String system = String.Join("\n", date, os, processor, memory);

			String data = String.Join("\n\n", system, PrintException(e));

			using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write)) {
				using (StreamWriter sw = new StreamWriter(fs)) {
					sw.Write(data);
				}
			}

		}

		public static String PrintException(Exception e) {
			
			String result = String.Empty;

			if (e != null) {
				String desc = String.Concat("Exception: ", e.ToString());
				String message = String.Concat("Message: ", e.Message);
				String stack = String.Concat("Stack Trace:\n", e.StackTrace);
				String inner = String.Concat("Inner Exception: ", PrintException(e.InnerException));

				result = String.Join("\n\n", desc, message, stack, inner);
			}

			return result;
		}

		public static void Send(Guid guid) {
			//String path = Path.Combine(_LogPath, guid.ToString());
			//FileInfo file = new FileInfo(path);
			//String data = null;

			//using (StreamReader sr = file.OpenText()) {
			//  data = sr.ReadToEnd();
			//}

			// TODO: send this to a hop server, which will then send to github.
		}

	}
}

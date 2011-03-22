using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32;

namespace Shellscape.Utilities {
	public class BrowserHelper {

		public static Browser DefaultBrowser {
			get {

				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes\http\"))
				using (RegistryKey iconKey = key.OpenSubKey("DefaultIcon"))
				using (RegistryKey pathKey = key.OpenSubKey(@"shell\open\command")) {

					String iconPath = iconKey.GetValue(null) as String;
					String value = pathKey.GetValue(null) as String;

					value = value.Replace("\"%1\"", String.Empty);

					Browser browser = new Browser() {
						Name = "Default Browser",
						Path = value,
						IconPath = iconPath
					};

					return browser;
				}
			}
		}

		/// <summary>
		/// Returns a list of Browser objects, representing the browsers installed on the system.
		/// </summary>
		/// <returns></returns>
		public static List<Browser> Enumerate() {

			List<Browser> results = new List<Browser>();

			RegistryKey browserKeys = null;
			
			// 64bit
			try {
				browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");

				if (browserKeys == null) { // 32bit
					browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
				}

				foreach (String browserName in browserKeys.GetSubKeyNames()) {

					Browser browser = new Browser();
					RegistryKey browserKey = browserKeys.OpenSubKey(browserName);

					browser.Name = (string)browserKey.GetValue(null);

					RegistryKey browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
					browser.Path = (string)browserKeyPath.GetValue(null);

					RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
					browser.IconPath = (string)browserIconPath.GetValue(null);

					results.Add(browser);
				}

				results.Sort();

				return results;

			}
			finally {
				browserKeys.Close();
				browserKeys.Dispose();
			}
		}
		
	}
}

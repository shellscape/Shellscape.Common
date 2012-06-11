using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.Utilities {
	public class ApplicationHelper {

		public static void Donate(String description) {

			String business = "andrew@shellscape.org";

			String url = String.Format("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business={0}&lc=US&item_name={1}&currency_code=USD&bn=", business, description);

			System.Diagnostics.Process.Start(url);

		}

	}
}

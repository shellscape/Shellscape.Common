using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shellscape.UI;

namespace Shellscape.Remoting {
	internal class RemotingSingleton : MarshalByRefObject {

		public void Run(String[] arguments) {

			if (arguments.Length == 0) {
				return;
			}

			String taskArgument = arguments[0];
			String[] eventArguments = new String[] { String.Empty };
			char[] trimChars = new char[] { '-', '/' };

			if (arguments.Length > 1) {
				eventArguments = arguments.Skip(1).ToArray();
			}

			taskArgument = taskArgument.TrimStart(trimChars);

			foreach (var kvp in RemotingTaskMethods.Methods.Where(k => k.Key.TrimStart(trimChars) == taskArgument)) {
				String argument = kvp.Key.TrimStart(trimChars);
				RemoteTaskMethod handler = kvp.Value;

				if (Program.Form != null && Program.Form.InvokeRequired) {
					Program.Form.Invoke(handler, new object[] { eventArguments });
				}
				else {
					handler(eventArguments);
				}
			}

		}

	}
}

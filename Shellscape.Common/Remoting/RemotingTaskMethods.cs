using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.Remoting {
	public static class RemotingTaskMethods {

		static RemotingTaskMethods() {
			RemotingTaskMethods.Methods = new Dictionary<String, RemoteTaskMethod>();
		}

		public static Dictionary<String, RemoteTaskMethod> Methods { get; private set; }

	}
}

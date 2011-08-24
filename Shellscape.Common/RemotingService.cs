using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shellscape {

	public class RemoteServiceMethodAttribute : Attribute {

		public RemoteServiceMethodAttribute(String argument) {
			Argument = argument;
		}

		public String Argument { get; set; }

	}

	public class RemotingService<T> where T : ArgumentCollection, new() {

		private T _arguments;

		public RemotingService() {
			_arguments = new T();
		}

		public void Execute(String[] arguments) {

			if (arguments.Length == 0) {
				return;
			}

			String argument = arguments[0];

			if (!_arguments.Contains(argument)) {
				return;
			}

			Type thisType = this.GetType();
			MethodInfo[] methods = thisType.GetMethods();

			foreach (MethodInfo method in methods) {
				object[] attributes = method.GetCustomAttributes(typeof(RemoteServiceMethodAttribute), true);

				if (attributes.Length > 0 && (attributes[0]  as RemoteServiceMethodAttribute).Argument == argument) {
					method.Invoke(this, null);
					break;
				}
			}

		}

	}
}

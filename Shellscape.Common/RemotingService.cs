using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shellscape {

	public interface IRemotingService {
		void Execute(String[] arguments);
	}

	public class RemoteServiceMethodAttribute : Attribute {

		public RemoteServiceMethodAttribute(String methodName) {
			MethodName = methodName;
		}

		public String MethodName { get; set; }
	}

	public class RemotingService<T> : MarshalByRefObject, IRemotingService where T : ArgumentCollection, new() {

		private T _arguments;

		public String[] Arguments { get; set; }

		public RemotingService() {
			_arguments = new T();
		}

		public void Execute(String[] arguments) {

			if (arguments.Length == 0) {
				return;
			}

			String argument = arguments[0];

			if (arguments.Length > 1) {
				this.Arguments = arguments.Skip(1).ToArray();
			}
			else {
				this.Arguments = new String[] { String.Empty };
			}

			argument = argument.TrimStart(new char[] { '-', '/' });

			if (!_arguments.Contains(argument)) {
				return;
			}

			Type thisType = this.GetType();
			MethodInfo[] methods = thisType.GetMethods();

			foreach (MethodInfo method in methods) {
				object[] attributes = method.GetCustomAttributes(typeof(RemoteServiceMethodAttribute), true);

				if (attributes.Length > 0 && (attributes[0]  as RemoteServiceMethodAttribute).MethodName == argument) {
					method.Invoke(this, null);
					break;
				}
			}

		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shellscape {
	
	public class ArgumentCollection {

		private List<String> _arguments = new List<String>();

		public ArgumentCollection() {

			Type thisType = this.GetType();
			FieldInfo[] fields = thisType.GetFields(BindingFlags.Public | BindingFlags.Static);

			foreach (FieldInfo info in fields) {
				String value = info.GetValue(this) as String;
				_arguments.Add(value);
			}
		}

		public Boolean Contains(String argument) {

			if (argument.StartsWith("-") || argument.StartsWith("/")) {
				argument = argument.Remove(0, 1);
			}
			
			return _arguments.Contains(argument);
		}

	}
}

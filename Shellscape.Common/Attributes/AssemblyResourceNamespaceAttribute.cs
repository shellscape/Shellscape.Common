using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Reflection {

	/// <summary>
	/// Defines the root Resources namespace of the application.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public class AssemblyResourceNamespaceAttribute : Attribute {

		private String _namespace = String.Empty;

		public AssemblyResourceNamespaceAttribute(String @namespace) {
			this.Namespace = @namespace;
		}

		public String Namespace {
			get { return _namespace; }
			set {
				_namespace = value;

				if (!_namespace.EndsWith(".Resources")) {
					_namespace += "Resources";
				}
			}
		}

	}

}

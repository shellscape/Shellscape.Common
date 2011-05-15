using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape {

	[AttributeUsage(AttributeTargets.Assembly)]
	public class AssemblyResourceNamespaceAttribute : Attribute {

		public AssemblyResourceNamespaceAttribute(String @namespace) {
			this.Namespace = @namespace;
		}

		public String Namespace { get; private set; }

	}
}

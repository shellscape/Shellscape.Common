using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Shellscape {

	[DataContract(Name = "browser")]
	public class Browser : IComparable<Browser> {

		[DataMember(Name = "name")]
		public String Name { get; set; }

		[DataMember(Name = "path")]
		public String Path { get; set; }

		[DataMember(Name = "iconpath")]
		public String IconPath { get; set; }

		public int CompareTo(Browser o) {
			return this.Name.CompareTo(o.Name);
		}

	}
}

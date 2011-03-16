using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;

namespace Shellscape.Utilities {
	public static class AssemblyMeta {

		private static Assembly _assembly;
		private static object[] _attributes;

		private static void Init() {
			if (_assembly == null) {
				_assembly = Assembly.GetExecutingAssembly();
			}

			if (_attributes == null) {
				_attributes = _assembly.GetCustomAttributes(false);
			}
		}

		private static String GetAttribute<T>(String property) where T : Attribute {
			Init();

			T attribute = (T)_attributes.Where(o => o is T).FirstOrDefault();
			
			if (attribute != null){
				PropertyInfo propinfo = typeof(T).GetProperty(property);

				if (propinfo != null) {
					String value = propinfo.GetValue(attribute, null) as String;

					if (!String.IsNullOrEmpty(value)) {
						return value;
					}
				}
			}

			return String.Empty;
		}

		public static String Title { 
			get { 
				String result = GetAttribute<AssemblyTitleAttribute>("Title");

				if (String.IsNullOrEmpty(result)) {
					return AssemblyName;
				}

				return result;
			} 
		}

		public static String AssemblyName { 
			get {
				Init();

				if (_assembly == null) {
					return String.Empty;
				}

				return System.IO.Path.GetFileNameWithoutExtension(_assembly.CodeBase); 
			} 
		}

		public static String Guid { get { return GetAttribute<GuidAttribute>("Value"); } }
		public static String Copyright { get { return GetAttribute<AssemblyCopyrightAttribute>("Copyright"); } }

		public static String Version {
			get {
				Init();
				return _assembly.GetName().Version.ToString();
			}
		}

	}
}

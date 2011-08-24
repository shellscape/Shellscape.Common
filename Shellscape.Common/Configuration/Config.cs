using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Shellscape.Configuration {

	[DataContract(Name = "config")]
	public abstract class Config {

		private static String _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static String _path;
		private static String _fileName = "app.config";

		public event ConfigSavedEventHandler Saved;

		public Config() {

			_path = Path.Combine(_appData, this.ApplicationName);

			SetDefaults();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context) {
			SetDefaults();
		}

		protected abstract String ApplicationName { get; }
		protected abstract void SetDefaults();

		public static void Init<T>() where T : Config, new() {

			if (!Directory.Exists(_path)) {
				Directory.CreateDirectory(_path);
			}

			Config config = new T();
			String xml = null;
			FileInfo file = new FileInfo(Path.Combine(_path, _fileName));

			if (file.Exists) {
				using (StreamReader sr = file.OpenText()) {
					xml = sr.ReadToEnd();
				}

				if (!String.IsNullOrEmpty(xml)) {
					config = Utilities.Serializer.DeserializeContract<Config>(xml);
				}
			}
			else {
				config.Save();
			}

		}

		public void Save() {

			String serialized = Utilities.Serializer.SerializeContract<Config>(this);

			using (FileStream fs = new FileStream(Path.Combine(_path, _fileName), FileMode.Create, FileAccess.ReadWrite)) {
				using (StreamWriter sw = new StreamWriter(fs)) {
					sw.Write(serialized);
				}
			}

			OnSaved();
		}

		protected virtual void OnSaved() {
			if (this.Saved != null) {
				this.Saved(this);
			}
		}

	}
}

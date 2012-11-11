using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Shellscape.Configuration {

	[DataContract(Name = "config")]
	public abstract class Config<T> where T : Config<T>, new() {

		private static String _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static String _path;
		private static String _fileName = "app.config";
		private static T _current = null;

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

		public static T Current { get { return _current as T; } }

		public static void Init() {

			if (!Directory.Exists(_path)) {
				Directory.CreateDirectory(_path);
			}

			Config<T> config = new T();
			String xml = null;
			FileInfo file = new FileInfo(Path.Combine(_path, _fileName));

			if (file.Exists) {
				using (StreamReader sr = file.OpenText()) {
					xml = sr.ReadToEnd();
				}

				if (!String.IsNullOrEmpty(xml)) {
					config = Utilities.Serializer.DeserializeContract<Config<T>>(xml);
				}
			}
			else {
				config.Save();
			}

			_current = config as T;

			_current.OnInit();
		}

		public void Save() {

			String serialized = Utilities.Serializer.SerializeContract<Config<T>>(this);

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

		protected virtual void OnInit() {

		}

	}
}

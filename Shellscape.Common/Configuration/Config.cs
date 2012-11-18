using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Shellscape.Configuration {

	/// <summary>
	/// Derrived classes must also add the [DataContract(Name = "config")] attribute to the class definition.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DataContract(Name = "config")]
	public abstract class Config<T> where T : Config<T>, new() {

		private String _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private String _path;
		private static T _current = null;

		public event ConfigSavedEventHandler Saved;

		public Config() {
			this.FileName = "app.config";
			_path = Path.Combine(_appData, this.ApplicationName);
			SetDefaultsInternal();
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context) {
			SetDefaultsInternal();
		}

		public virtual String AppDataPath {
			get { return _appData; }
			protected set { _appData = value; }
		}

		public virtual String StorePath {
			get { return _path; }
			set { _path = value; }
		}

		public  virtual String FileName { get; protected set; }

		protected abstract String ApplicationName { get; }
		protected abstract void SetDefaults();

		private void SetDefaultsInternal() {
			
			SetDefaults();
		}

		public static T Current { get { return _current as T; } }

		public static void Init() {

			Config<T> config = new T();
			String xml = null;
			
			if (!Directory.Exists(config.StorePath)) {
				Directory.CreateDirectory(config.StorePath);
			}

			FileInfo file = new FileInfo(Path.Combine(config.StorePath, config.FileName));

			if (file.Exists) {
				using (StreamReader sr = file.OpenText()) {
					xml = sr.ReadToEnd();
				}

				if (!String.IsNullOrEmpty(xml)) {
					config = Utilities.Serializer.DeserializeContract<T>(xml);
				}
			}
			else {
				config.Save();
			}

			_current = config as T;

			_current.OnInit();
		}

		public void Save() {

			String serialized = Utilities.Serializer.SerializeContract<T>(this as T);

			using (FileStream fs = new FileStream(Path.Combine(_path, this.FileName), FileMode.Create, FileAccess.ReadWrite)) {
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

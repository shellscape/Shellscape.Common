using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;

using Ionic.Zip;

namespace Shellscape {

	/// <summary>
	/// Class for checking for updated versions using github.
	/// </summary>
	public class UpdateManager : AsyncOperation {

		public enum UpdateStatus {
			Checking,
			UpToDate,
			NewVersion,
			Problem
		}

		public class LatestVersion {

			public LatestVersion(String fileUrl, String fileName, String version) {
				this.FileName = fileName;
				this.FileUrl = fileUrl;
				this.Version = version;
			}

			public String FileUrl { get; private set; }
			public String FileName { get; private set; }
			public String Version { get; private set; }
		}

		internal class GitHubDownload {

			public String description { get; set; }
			public String created_at { get; set; }
			public String html_url { get; set; }
			public String name { get; set; }
			public String url { get; set; }
			public int size { get; set; }
			public int download_count { get; set; }
			public int id { get; set; }
		}

		private  String _api = "https://api.github.com/repos/{0}/{1}/downloads";
		private Timer _timer;
		private static UpdateManager _current = null;

		public delegate void UpdateManagerEventHandler(UpdateManager sender);

		private UpdateManager() {
			this._api = "https://api.github.com/repos/{0}/{1}/downloads";
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			this.CurrentVersion = (entryAssembly != null) ? entryAssembly.GetName().Version.ToString() : "1.0.0.0";
			_current = this;
			this.UpdateInterval = 3600000; // 1 hour
		}

		public UpdateManager(string user, string repository, string appName) : this() {
			this.User = user;
			this.Repository = repository;
			this.AppName = appName + "-";
		}

		public event UnhandledExceptionEventHandler Error;
		public event UpdateManagerEventHandler UpdateAvailable;

		public string AppName { get; private set; }

		public static UpdateManager Current {
			get {
				if(_current == null) {
					new UpdateManager();
				}

				return _current;
			}
		}

		public string CurrentVersion { get; private set; }
		public LatestVersion Latest { get; private set; }
		public string NewVersion { get; private set; }
		public string Repository { get; private set; }
		public UpdateStatus Status { get; private set; }
		public int UpdateInterval { get; set; }
		public string User { get; private set; }
		
		private void Check(string data) {
			if(string.IsNullOrEmpty(data)) {
				this.Status = UpdateStatus.Problem;
			}
			else {
				List<GitHubDownload> list = new List<GitHubDownload>();
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				try {
					list = serializer.Deserialize<List<GitHubDownload>>(data);
				}
				catch(Exception exception) {
					this.Status = UpdateStatus.Problem;
					if(this.Error != null) {
						this.Error(this, new UnhandledExceptionEventArgs(exception, false));
					}
				}
				if(list.Count == 0) {
					this.Status = UpdateStatus.Problem;
				}
				else {
					GitHubDownload download = (from d in list
																		 where !d.name.Contains("debug")
																		 select d).First<GitHubDownload>();
					FileInfo info = new FileInfo(download.name);
					string remoteVersion = info.Name.Replace(this.AppName, string.Empty).Replace(info.Extension, string.Empty);
					Version current = new Version(this.CurrentVersion);
					Version remote = new Version(remoteVersion);
					
					this.Latest = new LatestVersion(download.html_url, download.name, remoteVersion);
					
					if(current < remote) {
						this.Status = UpdateStatus.NewVersion;
						this.NewVersion = remote.ToString();
						if(this.UpdateAvailable != null) {
							this.UpdateAvailable(this);
						}
					}
					else {
						this.Status = UpdateStatus.UpToDate;
					}
				}
			}
		}

		public void CheckOnce() {
			if(base.IsDone) {
				base.Start();
			}
		}

		public void Download(string dataPath, AsyncCompletedEventHandler callback) {
			try {
				using(WebClient client = new WebClient()) {
					FileInfo dest = new FileInfo(Path.Combine(dataPath, "Updates", this.Latest.FileName));
					if(!dest.Directory.Exists) {
						dest.Directory.Create();
					}
					client.DownloadFileCompleted += delegate(object sender, AsyncCompletedEventArgs e) {
						if(!e.Cancelled && (e.Error == null)) {
							this.Unzip(dest.FullName);
							callback(sender, e);
						}
					};
					client.DownloadFileAsync(new Uri(this.Latest.FileUrl), dest.FullName);
				}
			}
			catch(WebException exception) {
				if(this.Error != null) {
					this.Error(this, new UnhandledExceptionEventArgs(exception, false));
				}
				this.Status = UpdateStatus.Problem;
			}
		}

		protected override void DoWork() {
			this.Request();
			if(base.CancelRequested) {
				base.AcknowledgeCancel();
			}
		}

		public void Replace(string dataPath, string appPath) {
			DirectoryInfo source = new DirectoryInfo(Path.Combine(dataPath, "Updates", this.NewVersion));
			this.ReplaceDirectory(source, new DirectoryInfo(appPath));
		}

		private void ReplaceDirectory(DirectoryInfo source, DirectoryInfo dest) {
			
			if(!dest.Exists) {
				dest.Create();
			}
			
			FileInfo[] files = source.GetFiles();
			
			foreach(FileInfo file in files) {
				string path = Path.Combine(dest.FullName, file.Name);
				if(File.Exists(path)) {
					file.Replace(path, path + "." + this.CurrentVersion, true);
				}
				else {
					file.CopyTo(path);
				}
			}
			
			DirectoryInfo[] directories = source.GetDirectories();
			
			foreach(DirectoryInfo dir in directories) {
				this.ReplaceDirectory(dir, new DirectoryInfo(Path.Combine(dest.FullName, dir.Name)));
			}
		}

		private void Request() {
			this.Status = UpdateStatus.Checking;
			string data = string.Empty;
			try {
				using(WebClient client = new WebClient()) {
					data = client.DownloadString(string.Format(this._api, this.User, this.Repository));
				}
			}
			catch(Exception exception) {
				if(this.Error != null) {
					this.Error(this, new UnhandledExceptionEventArgs(exception, false));
				}
				this.Status = UpdateStatus.Problem;
			}
			Thread.Sleep(0x1388);
			this.Check(data);
		}

		public new void Start() {
			this._timer = new Timer(delegate(object state) {
				this.CheckOnce();
			}, this, 0x2710, this.UpdateInterval);
		}

		public void Stop() {
			this._timer.Dispose();
			base.Cancel();
		}

		private void Unzip(string zipFile) {
			FileInfo zip = new FileInfo(zipFile);
			DirectoryInfo dir = zip.Directory.CreateSubdirectory(this.NewVersion);
			using(ZipFile file = ZipFile.Read(zip.FullName)) {
				foreach(ZipEntry entry in file) {
					entry.Extract(dir.FullName, ExtractExistingFileAction.OverwriteSilently);
				}
			}
			zip.Delete();
		}
	}
}

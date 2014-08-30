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

using Microsoft.Win32;

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

		internal class GitHubRelease {
            public String url { get; set; }
            public String html_url { get; set; }
            public int id { get; set; }
            public String tag_name { get; set; }
			public String name { get; set; }
            public bool draft { get; set; }
            public bool prerelease { get; set; }
            public DateTime created_at { get; set; }
            public DateTime published_at { get; set; }
            public List<GitHubReleaseAsset> assets { get; set; }
            public String body { get; set; }
		}

        internal class GitHubReleaseAsset {
            public String url { get; set; }
            public int id { get; set; }
            public String name { get; set; }
            public String content_type { get; set; }
            public String state { get; set; }
            public int size { get; set; }
            public int download_count { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public String browser_download_url { get; set; }
        }

		private String _api = "https://api.github.com/repos/{0}/{1}/releases";
		private const String _userAgent = "Shellscape-Updater";
		private Timer _timer;
		private static UpdateManager _current = null;

		public delegate void UpdateManagerEventHandler(UpdateManager sender);

		private UpdateManager() {
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			this.CurrentVersion = (entryAssembly != null) ? entryAssembly.GetName().Version.ToString() : "1.0.0.0";
			_current = this;
			this.UpdateInterval = 3600000; // 1 hour

			SystemEvents.PowerModeChanged += OnPowerChange;
		}

		public UpdateManager(string user, string repository, string appName, string userAgent = _userAgent) : this() {
			this.User = user;
			this.Repository = repository;
			this.AppName = appName + "-";
			this.UserAgent = userAgent;
		}

		public event UnhandledExceptionEventHandler Error;
		public event UpdateManagerEventHandler UpdateAvailable;

		public string AppName { get; private set; }
		public string UserAgent { get; set; }

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
				List<GitHubRelease> list = new List<GitHubRelease>();
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				try {
					list = serializer.Deserialize<List<GitHubRelease>>(data);
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
                    // First, check if there's some stable release
					GitHubRelease release = list.FirstOrDefault(p => !p.draft && !p.prerelease);
                    if (release == null) {
                        this.Status = UpdateStatus.Problem;
                        return;
                    }

                    // Second, see if it contains an asset suitable for UpdateManager needs
                    GitHubReleaseAsset asset = release.assets.FirstOrDefault(p => !p.name.Contains("debug") && p.name.ToLower().EndsWith(".zip"));
                    if (asset == null) {
                        this.Status = UpdateStatus.Problem;
                        return;
                    }

                    // The release tag is expected to be in format "X.X.X.X" or "vX.X.X.X"
                    string remoteVersion = release.tag_name.TrimStart('v');
					Version current = new Version(this.CurrentVersion);
					Version remote = new Version(remoteVersion);

					this.Latest = new LatestVersion(asset.browser_download_url, asset.name, remoteVersion);

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
					client.Headers.Add("User-Agent", this.UserAgent);
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

		private void OnPowerChange(Object sender, PowerModeChangedEventArgs e) {
			switch(e.Mode) {
				case PowerModes.Resume:
					this.Start();
					break;
				case PowerModes.Suspend:
					this.Stop();
					break;
			}
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Web.Script.Serialization;

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
				this.FileName = FileName;
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

		private readonly int _updateInterval = 300000; // 5 minutes
		private const String _api = "https://api.github.com/repos/{0}/{1}/downloads";

		//private WebRequest _webRequest;
		private Timer _timer;

		public UpdateManager(String user, String repository, String appName) {
			this.User = user;
			this.Repository = repository;
			this.AppName = String.Concat(appName, "-"); // this is the file name format i use; [name]-[version].zip
			this.CurrentVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

			//_webRequest = WebRequest.Create(String.Format(_api, this.User, this.Repository));
			
			UpdateManager.Current = this;
		}

		public event UnhandledExceptionEventHandler Error;

		public static UpdateManager Current { get; private set; }

		public UpdateStatus Status { get; private set; }
		public String AppName { get; private set; }
		public String CurrentVersion { get; private set; }
		public String User { get; private set; }
		public String Repository { get; private set; }

		public LatestVersion Latest { get; private set; }

		public void StartTimer() {
			_timer = new Timer(delegate(object state){

				Debug.WriteLine("Trigger");
				
				SafeStart();
				
			}, this, 0, _updateInterval);
		}

		public void Stop() {
			_timer.Dispose();
			this.CancelAndWait();
		}

		public void SafeStart() {
			if (this.IsDone) {
				this.Start();
			}
		}

		protected override void DoWork() {

			//while (!CancelRequested) {
			Request();
				//System.Threading.Thread.Sleep(this._updateInterval);
			//}
			
			if (CancelRequested) {
				AcknowledgeCancel();
			}
		}

		private void Request() {

			this.Status = UpdateStatus.Checking;

			String data = String.Empty;

			try {
				using (WebClient webClient = new WebClient()) {
					data = webClient.DownloadString(String.Format(_api, this.User, this.Repository));
				}
			}
			catch (WebException e) {
				if (Error != null) {
					Error.Invoke(this, new UnhandledExceptionEventArgs(e, false));
				}
				this.Status = UpdateStatus.Problem;
			}

			System.Threading.Thread.Sleep(5000);

			Check(data);

			//try {
			//  HttpWebResponse response = _webRequest.GetResponse() as HttpWebResponse;

			//  if (response.StatusCode == HttpStatusCode.OK) {

			//    String data = String.Empty;

			//    using (Stream stream = response.GetResponseStream()) {

			//      if (stream == null || !stream.CanRead) {
			//        this.Status = UpdateStatus.Problem;
			//        Debug.WriteLine("Stream Problem");
			//        return;
			//      }

			//      using (StreamReader reader = new StreamReader(stream)) {
			//        data = reader.ReadToEnd();
			//      }
			//    }

			//    Check(data);
			//  }
			//}
			//catch (Exception e) {
			//  if (Error != null) {
			//    Error.Invoke(this, new UnhandledExceptionEventArgs(e, false));
			//  }
			//  this.Status = UpdateStatus.Problem;
			//}

		}

		private void Check(String data) {

			if (String.IsNullOrEmpty(data)) {
				this.Status = UpdateStatus.Problem;
				return;
			}
		
			List<GitHubDownload> downloads = new List<GitHubDownload>();
			JavaScriptSerializer serializer = new JavaScriptSerializer();

			try {
				downloads = serializer.Deserialize<List<GitHubDownload>>(data);
			}
			catch (Exception e) {
				
				this.Status = UpdateStatus.Problem;

				if (Error != null) {
					Error.Invoke(this, new UnhandledExceptionEventArgs(e, false));
				}
			}

			if (downloads.Count == 0) {
				this.Status = UpdateStatus.Problem;
				return;
			}

			GitHubDownload latest = downloads.Where(d => !d.name.Contains("debug")).First();
			FileInfo file = new FileInfo(latest.name);
			String version = file.Name.Replace(this.AppName, String.Empty).Replace(file.Extension, String.Empty);

			Version current = new Version(this.CurrentVersion);
			Version remote = new Version(version);

			this.Latest = new LatestVersion(latest.html_url, latest.name, version);

			if(current < remote){
				this.Status = UpdateStatus.NewVersion;
			}
			else{
				this.Status = UpdateStatus.UpToDate;
			}

			Debug.WriteLine(version);
			Debug.WriteLine(current < remote ? "newer" : "current");
		}
	}
}

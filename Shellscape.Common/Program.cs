using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Shellscape {

	public static class Program {

		public static event Action MainInstanceStarted;
		public static event Action<object, RemoteCallEventArgs> RemoteCall;

		//private static String _channelName;

		static Program() {
			SingleInstance = true;
			MutexName = String.Concat("Local\\", Utilities.AssemblyMeta.Title, "-", Utilities.AssemblyMeta.Guid);
			ChannelName = String.Concat(WindowsIdentity.GetCurrent().Name, "@", Utilities.AssemblyMeta.Title.Replace(" ", String.Empty));
			ObjectUri = "service.rem";
		}

		/// <summary>
		/// Determines whether or not this application is a single-instance-only application.
		/// </summary>
		public static Boolean SingleInstance { get; set; }

		public static String MutexName { get; set; }
		public static String ChannelName { get; set; }
		public static String ObjectUri { get; set; }

		public static Type RemotingServiceType { get; set; }

		public static Form Form { get; private set; }

		public static void Run<TForm>(String[] arguments) where TForm : Form, new() {

			bool createdNew;

			using (new Mutex(true, MutexName, out createdNew)) {
				if (!createdNew) {
					CallRunningInstance(arguments);
					return;
				}

				TForm form = null;

				InitRemoting();

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.ThreadException += Application_ThreadException;

				try {
					OnMainInstanceStarted();

					Program.Form = form = new TForm();
				}
				catch (Exception e) {
					Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
				}

				Application.Run(form);
			}

		}

		private static void InitRemoting() {

			if (RemotingServiceType == null) {
				return;
			}

			ChannelServices.RegisterChannel(new IpcChannel(ChannelName), false);
			RemotingConfiguration.RegisterWellKnownServiceType(RemotingServiceType, ObjectUri, WellKnownObjectMode.Singleton);
		}

		private static void CallRunningInstance(string[] arguments) {

			if (RemotingServiceType == null || arguments.Length == 0) {
				return;
			}

			object service = RemotingServices.Connect(RemotingServiceType, "ipc://" + ChannelName + "/" + ObjectUri);

			OnRemoteCall(service, new RemoteCallEventArgs() { Arguments = arguments });
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			Utilities.ErrorHelper.Report(e.Exception);
		}

		private static void OnMainInstanceStarted() {
			if (MainInstanceStarted != null) {
				MainInstanceStarted();
			}
		}

		private static void OnRemoteCall(object service, RemoteCallEventArgs e) {

			try {
				(service as IRemotingService).Execute(e.Arguments);
			}
			catch (Exception ex) {
				Utilities.ErrorHelper.Report(ex);
			}		
			
			if (RemoteCall != null) {
				RemoteCall(service, e);
			}
		}

	}
}

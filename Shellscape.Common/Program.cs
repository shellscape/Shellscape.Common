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

using Shellscape.Remoting;

namespace Shellscape {

	public static class Program {

		public static event Action MainInstanceStarted;
		public static event Action<String[]> RemoteCallMade;

		//private static String _channelName;

		static Program() {
			SingleInstance = true;
			MutexName = String.Concat("Local\\", Utilities.AssemblyMeta.Title, "-", Utilities.AssemblyMeta.Guid);
			JumplistChannelName = String.Concat(WindowsIdentity.GetCurrent().Name, "@", Utilities.AssemblyMeta.Title.Replace(" ", String.Empty));
			JumplistObjectName = "jumplist.rem";
		}

		/// <summary>
		/// Determines whether or not this application is a single-instance-only application.
		/// </summary>
		public static Boolean SingleInstance { get; set; }

		public static String MutexName { get; set; }
		public static String JumplistChannelName { get; set; }
		public static String JumplistObjectName { get; set; }

		//public static Type RemotingServiceType { get; set; }

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

					if (arguments.Length > 0) {
						(new RemotingSingleton()).Run(arguments);
					}

					Program.Form = form = new TForm();
				}
				catch (Exception e) {
					Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
				}

				Application.Run(form);
			}

		}

		public static Form FindForm(Type formType) {
			foreach (Form form in Application.OpenForms) {
				if (form.GetType() == formType) {
					return form;
				}
			}

			return null;
		}

		private static void InitRemoting() {

			ChannelServices.RegisterChannel(new IpcChannel(JumplistChannelName), false);
			RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingSingleton), JumplistObjectName, WellKnownObjectMode.Singleton);
		}

		private static void CallRunningInstance(string[] arguments) {

			if (arguments.Length == 0) {
				return;
			}

			object proxy = RemotingServices.Connect(typeof(RemotingSingleton), "ipc://" + JumplistChannelName + "/" + JumplistObjectName);
			RemotingSingleton service = proxy as RemotingSingleton;

			try {
				service.Run(arguments);

				if (RemoteCallMade != null) {
					RemoteCallMade(arguments);
				}
			}
			catch (Exception ex) {
				Utilities.ErrorHelper.Report(ex);
			}		
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			Utilities.ErrorHelper.Report(e.Exception);
		}

		private static void OnMainInstanceStarted() {
			if (MainInstanceStarted != null) {
				MainInstanceStarted();
			}
		}
	}
}

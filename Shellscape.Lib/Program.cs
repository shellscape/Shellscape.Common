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

	public class Program {

		public event EventHandler MainInstanceStarted;
		public event EventHandler RemoteCall;

		private String _channelName;

		public Program() {
			SingleInstance = true;
			MutexName = String.Concat(@"Local\", Utilities.AssemblyMeta.Title, "\\", Utilities.AssemblyMeta.Guid);
		}

		public void Run<TForm>(String[] arguments) where TForm : Form, new() {

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

					if (MainInstanceStarted != null) {
						MainInstanceStarted(this, EventArgs.Empty);
					}

					form = new TForm();
				}
				catch (Exception e) {
					Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
				}

				Application.Run(form);
			}

		}

		/// <summary>
		/// Determines whether or not this application is a single-instance-only application.
		/// </summary>
		public Boolean SingleInstance { get; set; }

		public String MutexName { get; set; }

		public Type RemotingServiceType { get; set; }

		public void InitRemoting() {

			if (RemotingServiceType == null) {
				return;
			}

			_channelName = String.Concat(WindowsIdentity.GetCurrent().Name, "@", Utilities.AssemblyMeta.Title.Replace(" ", String.Empty));

			ChannelServices.RegisterChannel(new IpcChannel(_channelName), false);
			RemotingConfiguration.RegisterWellKnownServiceType(RemotingServiceType, "service.rem", WellKnownObjectMode.Singleton);
		}

		private void CallRunningInstance(string[] arguments) {

			if (RemotingServiceType == null || arguments.Length == 0) {
				return;
			}

			object service = RemotingServices.Connect(RemotingServiceType, "ipc://" + _channelName + "/service.rem");

			if (RemoteCall != null) {
				RemoteCall(service, EventArgs.Empty);
			}
		}

		public void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			Utilities.ErrorHelper.Report(e.Exception);
		}

	}
}

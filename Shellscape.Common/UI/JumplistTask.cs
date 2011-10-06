using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shellscape.UI {

	public delegate void JumplistClickHandler(String[] arguments);

	public class JumplistTask : System.Windows.Shell.JumpTask {

		public JumplistTask(String argument, String title, String iconFileName) {

			this.ApplicationPath = System.Windows.Forms.Application.ExecutablePath;
			this.IconResourceIndex = 0;

			String iconPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.ApplicationPath), "Resources\\Icons");
			
			this.Arguments = argument;
			this.Title = title;
			this.IconResourcePath = System.IO.Path.Combine(iconPath, iconFileName);
		}

		public event Remoting.RemoteTaskMethod Click {
			add {
				Remoting.RemotingTaskMethods.Methods.Add(this.Arguments, value);
			}
			remove {
				Remoting.RemotingTaskMethods.Methods.Remove(this.Arguments);
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls.Preferences {

	[DesignTimeVisible(false), Browsable(false)]
	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
	public partial class PreferencesBaseControl : UserControl, INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		public PreferencesBaseControl() {
			InitializeComponent();

			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			UpdateStyles();

			Font = SystemFonts.MessageBoxFont;
		}

		protected void OnPropertyChanged(string Property) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(Property));
			}
		}


	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Skipe {

	[DesignTimeVisible(true), Browsable(false)]
	public class SkipeButtonGroup : FlowLayoutPanel, INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		public SkipeButtonGroup() {
			this.Dock = DockStyle.Left;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Width = 182;

			SetStyle(ControlStyles.ContainerControl, false);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			UpdateStyles();

			Font = SystemFonts.MessageBoxFont;
		}

		protected void OnPropertyChanged(string Property) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(Property));
			}
		}

		//protected override void OnControlAdded(ControlEventArgs e) {
		//  base.OnControlAdded(e);

		//  if (e.Control is SkipeButton) {
		//    (e.Control as SkipeButton).ButtonItems.ItemAdded += buttonItems_ItemAdded;
		//  }
		//}

		//private void buttonItems_ItemAdded(object sender, SkipeButtonItemEventArgs e) {
		//  this.PerformLayout();
		//}

	}
}

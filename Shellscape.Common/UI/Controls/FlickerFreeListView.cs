using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Shellscape.UI.Controls {
	public class FlickerFreeListView : ListView {

		[StructLayout(LayoutKind.Sequential)]
		struct NMHDR {
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public int code;
		}

		protected Boolean isInWmPaint;

		public FlickerFreeListView() : base() {
			this.DoubleBuffered = true;
		}

		protected override void OnDrawItem(DrawListViewItemEventArgs e) {
			if (this.isInWmPaint) {
				base.OnDrawItem(e);
			}
		}

		protected override void WndProc(ref Message m) {
			switch (m.Msg) {
				case 0x0F: // WM_PAINT
					this.isInWmPaint = true;
					base.WndProc(ref m);
					this.isInWmPaint = false;
					break;
				case 0x204E: // WM_REFLECT_NOTIFY
					NMHDR nmhdr = (NMHDR)m.GetLParam(typeof(NMHDR));
					if (nmhdr.code == -12) { // NM_CUSTOMDRAW
						if (this.isInWmPaint)
							base.WndProc(ref m);
					}
					else
						base.WndProc(ref m);
					break;
				default:
					base.WndProc(ref m);
					break;
			}
		}

		
	}
}

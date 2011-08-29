using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Shellscape {

	public class AnimatedCursor {

		#region .     win32

		private const int IMAGE_BITMAP = 0x0;
		private const int IMAGE_ICON = 0x1;
		private const int IMAGE_CURSOR = 0x2;

		private const int LR_DEFAULTCOLOR = 0x0000;
		private const int LR_MONOCHROME = 0x0001;
		private const int LR_COLOR = 0x0002;
		private const int LR_COPYRETURNORG = 0x0004;
		private const int LR_COPYDELETEORG = 0x0008;
		private const int LR_LOADFROMFILE = 0x10;
		private const int LR_LOADTRANSPARENT = 0x20;
		private const int LR_DEFAULTSIZE = 0x0040;
		private const int LR_LOADMAP3DCOLORS = 0x1000;
		private const int LR_CREATEDIBSECTION = 0x2000;
		private const int LR_COPYFROMRESOURCE = 0x4000;

		private const int DI_MASK = 0x1;
		private const int DI_IMAGE = 0x2;
		private const int DI_NORMAL = 0x3;
		private const int DI_COMPAT = 0x4;
		private const int DI_DEFAULTSIZE = 0x8;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stepNumber">This must not be a negative value!</param>
		[DllImport("user32")]
		private extern static int DrawIconEx(IntPtr handle, int left, int top, IntPtr hIcon, int cxWidth, int cyWidth, int stepNumber, IntPtr hbrFlickerFreeDraw, int diFlags);

		[DllImport("user32", CharSet = CharSet.Auto)]
		private extern static IntPtr LoadImage(IntPtr hInst, string lpsz, int uType, int cx, int cy, int uFlags);

		[DllImport("user32", CharSet = CharSet.Auto)]
		private extern static IntPtr LoadImage(IntPtr hInst, int lpsz, int uType, int cx, int cy, int uFlags);

		[DllImport("user32")]
		private extern static IntPtr CopyImage(IntPtr handle, int uType, int cxDesired, int cyDesired, int uFlags);

		[DllImport("user32")]
		private extern static int DestroyCursor(IntPtr hCursor);

		#endregion

		private Cursor _cursor = null;
		private int _frames = 0;
		private int _frame = 0;
		private IntPtr _handle = IntPtr.Zero;

		public AnimatedCursor() { }

		public AnimatedCursor(Cursor cursor) {
			this._cursor = cursor;
			this._handle = cursor.Handle;
			
			CountFrames();
		}

		public int Frames { get { return _frames; } }

		public int Frame {
			get { return this._frame; }
			set {
				if (value < 0 || value > this._frames) {
					value = 0;
				}

				this._frame = value;

			}
		}

		public Cursor Cursor { get { return this._cursor; } }

		public void DrawStep(Graphics g, int x, int y) {
			DrawStep(g, x, y, 0, 0);
		}

		public void DrawStep(Graphics g, int x, int y, int width, int height) {

			if (!Draw(g, x, y, width, height)) {
				this._frame = 0;
				DrawStep(g, x, y, width, height);
			}

			this._frame++;
		}

		public Boolean Draw(Graphics g, int x, int y, int width, int height) {
			IntPtr hdc = g.GetHdc();
			int result;

			result = DrawIconEx(hdc, x, y, this._handle, width, height, this._frame, IntPtr.Zero, DI_NORMAL);
			g.ReleaseHdc(hdc);

			return (result != 0);
		}

		private void CountFrames() {

			this._frames = 1;

			using (Bitmap bitmap = new Bitmap(128, 128)) {
				using (Graphics g = Graphics.FromImage(bitmap)) {
					
					IntPtr hdc = g.GetHdc();
					int result = 1;

					while (result != 0) {
						result = DrawIconEx(hdc, 0, 0, _handle, 0, 0, this._frames, IntPtr.Zero, DI_NORMAL);
						
						if (result != 0) {
							this._frames++;
						}
					}

					g.ReleaseHdc(hdc);
				}
			}
		}

	}
}

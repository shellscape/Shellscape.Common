using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Shellscape.Utilities {
	/// <summary>
	/// Provides utility methods for colourising bitmaps
	/// and working with Windows DIB Section objects.
	/// Uses unsafe native code to perform the colourisation
	/// until (if) another way can be determined.
	/// </summary>
	internal class ImageUtility {
		#region Unmanged Code
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct BITMAP {
			public Int32 bmType;
			public Int32 bmWidth;
			public Int32 bmHeight;
			public Int32 bmWidthBytes;
			public Int16 bmPlanes;
			public Int16 bmBitsPixel;
			public IntPtr bmBits;
		}
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct BITMAPINFOHEADER {
			public int biSize;
			public int biWidth;
			public int biHeight;
			public Int16 biPlanes;
			public Int16 biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int bitClrImportant;
		}
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct DIBSECTION {
			public BITMAP dsBm;
			public BITMAPINFOHEADER dsBmih;
			public int dsBitField1;
			public int dsBitField2;
			public int dsBitField3;
			public IntPtr dshSection;
			public int dsOffset;
		}
		[DllImport("gdi32", CharSet = CharSet.Auto, EntryPoint = "GetObject")]
		private static extern int GetObjectBitmap(IntPtr hObject, int nCount, ref       BITMAP lpObject);
		[DllImport("gdi32", CharSet = CharSet.Auto, EntryPoint = "GetObject")]
		private static extern int GetObjectDIBSection(IntPtr hObject, int nCount,
		 ref DIBSECTION lpObject);
		[DllImport("gdi32")]
		private static extern int BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);
		private const int SRCCOPY = 0xCC0020;
		[DllImport("gdi32")]
		private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
		[DllImport("gdi32")]
		public static extern int DeleteObject(IntPtr hObject);
		[DllImport("gdi32")]
		private static extern IntPtr CreateCompatibleDC(IntPtr hDC);
		[DllImport("gdi32")]
		private static extern int DeleteDC(IntPtr hdc);
		[DllImport("user32")]
		private static extern IntPtr GetDesktopWindow();
		[DllImport("user32")]
		private static extern IntPtr GetDC(IntPtr hWnd);
		[DllImport("user32")]
		private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
		[DllImport("kernel32", EntryPoint = "RtlMoveMemory")]
		private static extern int CopyMemory(IntPtr lpvDest, IntPtr lpvSrc, int cbCopy);
		#endregion


		/// <summary>
		/// Prevent instances of this class from being created
		/// </summary>
		private ImageUtility() {
		}

		/// <summary>
		/// Converts a Windows DIBSection object to a Bitmap.
		/// </summary>
		/// <param name="hDib">Handle to a Windows DIBSection.</param>
		/// <returns>Bitmap containing the DIBSection picture.</returns>
		public static Bitmap DibToBitmap(IntPtr hDib) {

			BITMAP tBM = new BITMAP();
			GetObjectBitmap(hDib, Marshal.SizeOf(tBM), ref tBM);
			Bitmap bm = new Bitmap(tBM.bmWidth, tBM.bmHeight);

			// set the bitmap's data to the data from
			// the DIB:
			if (tBM.bmBitsPixel == 32) {
				// Bizarre but true: you *must* clone the newly created
				// bitmap to get one with the correct pixel format, even
				// if you attempted to create the original one with the 
				// correct format...
				bm = bm.Clone(new Rectangle(0, 0, tBM.bmWidth, tBM.bmHeight), PixelFormat.Format32bppArgb);

				// Lock the bitmap bits
				BitmapData destData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				int destWidth = destData.Stride;
				IntPtr destScan0 = destData.Scan0;

				unsafe {
					byte* pDest = (byte*)(void*)destScan0;
					// The DIB is upside down compared to a GDI+ bitmap
					pDest += ((bm.Width * 4) * (bm.Height - 1));
					byte* pSrc = (byte*)(void*)tBM.bmBits;

					for (int y = 0; y < bm.Height; ++y) {
						for (int x = 0; x < bm.Width; ++x) {
							pDest[0] = pSrc[0]; // blue
							pDest[1] = pSrc[1]; // green
							pDest[2] = pSrc[2]; // red
							pDest[3] = pSrc[3]; // alpha

							// Move to next BGRA
							pDest += 4;
							pSrc += 4;
						}
						pDest -= (bm.Width * 8);
					}
				}

				bm.UnlockBits(destData);
			}
			else {
				// Easier to just copy src -> dst using GDI.

				// Put the DIB into a DC:
				IntPtr hWndDesktop = GetDesktopWindow();
				IntPtr hDCComp = GetDC(hWndDesktop);
				IntPtr hDCSrc = CreateCompatibleDC(hDCComp);

				ReleaseDC(hWndDesktop, hDCComp);
				
				IntPtr hBmpOld = SelectObject(hDCSrc, hDib);

				Graphics gfx = Graphics.FromImage(bm);
				IntPtr hDCDest = gfx.GetHdc();
				
				BitBlt(hDCDest, 0, 0, tBM.bmWidth, tBM.bmHeight, hDCSrc, 0, 0, SRCCOPY);
				gfx.ReleaseHdc(hDCDest);

				SelectObject(hDCSrc, hBmpOld);
				DeleteDC(hDCSrc);
			}


			return bm;

		}

	}
}

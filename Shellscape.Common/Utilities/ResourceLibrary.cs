using System;
using System.Runtime.InteropServices;

namespace Shellscape.Utilities {
	/// <summary>
	/// Class to manage access to External Win32 Resources
	/// </summary>
	internal class ResourceLibrary : IDisposable {
		private IntPtr hLib = IntPtr.Zero;
		private string filename = "";

		#region Unmanged Code
		[DllImport("kernel32", CharSet = CharSet.Auto)]
		private extern static IntPtr LoadLibraryEx(string lpLibFileName, IntPtr hFile, int dwFlags);
		private const int DONT_RESOLVE_DLL_REFERENCES = 0x1;
		private const int LOAD_LIBRARY_AS_DATAFILE = 0x2;
		private const int LOAD_WITH_ALTERED_SEARCH_PATH = 0x8;

		[DllImport("kernel32", CharSet = CharSet.Auto)]
		private extern static int FreeLibrary(IntPtr hLibModule);

		[DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "LoadImage")]
		private extern static IntPtr LoadImageLong(IntPtr hInst, int lpsz, int uType, int cX, int cY, int uFlags);

		[DllImport("user32", CharSet = CharSet.Auto, EntryPoint = "LoadImage")]
		private extern static IntPtr LoadImageString(IntPtr hInst, string lpsz, int uType, int cX, int cY, int uFlags);

		public enum ImageType {
			IMAGE_BITMAP = 0,
			IMAGE_ICON = 1,
			IMAGE_CURSOR = 2
		}

		[Flags]
		public enum ImageLoadOptions {
			LR_DEFAULTCOLOR = 0,
			LR_MONOCHROME = 0x1,
			LR_COLOR = 0x2,
			LR_COPYRETURNORG = 0x4,
			LR_COPYDELETEORG = 0x8,
			LR_LOADFROMFILE = 0x10,
			LR_LOADTRANSPARENT = 0x20,
			LR_DEFAULTSIZE = 0x40,
			LR_VGACOLOR = 0x80,
			LR_LOADMAP3DCOLORS = 0x1000,
			LR_CREATEDIBSECTION = 0x2000,
			LR_COPYFROMRESOURCE = 0x4000,
			LR_SHARED = 0x8000
		}

		#endregion

		/// <summary>
		/// Construct a new instance of the class
		/// </summary>
		public ResourceLibrary() {
		}

		/// <summary>
		/// Dispose and clear up any resources
		/// </summary>
		public void Dispose() {
			ClearUp();
		}

		/// <summary>
		/// Set the filename of the external library
		/// </summary>
		public string Filename {
			get {
				return filename;
			}
			set {
				filename = value;
				Load();
			}
		}

		/// <summary>
		/// Gets the handle to the library
		/// </summary>
		public IntPtr Handle {
			get {
				return hLib;
			}
		}

		/// <summary>
		/// Gets an image resource by id
		/// </summary>
		/// <param name="id">Id to get</param>
		/// <param name="imageType">Image type to get</param>
		/// <param name="options">Options to use</param>
		/// <returns>Handle to image resource if successful, otherwise <c>IntPtr.Zero</c></returns>
		public IntPtr GetResource(int id, ImageType imageType, ImageLoadOptions options) {
			IntPtr handle = IntPtr.Zero;
			if (!hLib.Equals(IntPtr.Zero)) {
				handle = LoadImageLong(hLib, id, (int)imageType, 0, 0, (int)
				 options);
			}
			return handle;
		}

		/// <summary>
		/// Gets an image resource by name
		/// </summary>
		/// <param name="name">Name to get</param>
		/// <param name="imageType">Image type to get</param>
		/// <param name="options">Options to use</param>
		/// <returns>Handle to image resource if successful, otherwise <c>IntPtr.Zero</c></returns>
		public IntPtr GetResource(string name, ImageType imageType, ImageLoadOptions options) {
			IntPtr handle = IntPtr.Zero;
			if (!hLib.Equals(IntPtr.Zero)) {
				handle = LoadImageString(hLib, name, (int)imageType, 0, 0, (int)
				 options);
			}
			return handle;
		}

		/// <summary>
		/// Loads the library.
		/// </summary>
		private void Load() {
			ClearUp();
			hLib = LoadLibraryEx(filename, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
		}

		/// <summary>
		/// Clears up any resources associated with the class.
		/// </summary>
		private void ClearUp() {
			if (!hLib.Equals(IntPtr.Zero)) {
				FreeLibrary(hLib);
				hLib = IntPtr.Zero;
			}
		}

	}
}
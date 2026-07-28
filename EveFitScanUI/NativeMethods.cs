using System.Runtime.InteropServices;

namespace EveFitScanUI;

internal static class NativeMethods
{
	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool AddClipboardFormatListener(nint hwnd);

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal static extern bool RemoveClipboardFormatListener(nint hwnd);
}

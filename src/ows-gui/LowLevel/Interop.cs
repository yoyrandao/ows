using System;
using System.Runtime.InteropServices;

namespace Ows.LowLevel;

internal static class Interop
{
	[DllImport("ows-window-api.dll", SetLastError = true)]
	public static extern bool IsTrueWindow(IntPtr hWnd);
	
	[DllImport("gdi32.dll", SetLastError = true)]
	public static extern bool DeleteObject(IntPtr hObject);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool SetForegroundWindow(IntPtr hWnd);
}
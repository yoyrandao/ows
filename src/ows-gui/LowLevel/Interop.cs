using System;
using System.Runtime.InteropServices;

namespace Ows.LowLevel;

internal static class Interop
{
	[DllImport("ows-window-api.dll", SetLastError = true)]
	public static extern bool IsTrueWindow(IntPtr hWnd);
	
	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr SetFocus(IntPtr hWnd);
}
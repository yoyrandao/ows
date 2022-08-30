using System;
using System.Runtime.InteropServices;

namespace Ows.LowLevel;

internal static class LowLevelMethods
{
	[DllImport("ows-window-api.dll")]
	public static extern bool IsTrueWindow(IntPtr hWnd);
}
using System;

namespace Ows.Dto;

internal record ActiveWindow
{
	public IntPtr HWnd { get; init; }
	
	public string ProcessName { get; init; }
	
	public string WindowTitle { get; init; }
}
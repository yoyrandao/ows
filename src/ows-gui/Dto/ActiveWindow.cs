using System;
using System.Windows.Media;

namespace Ows.Dto;

internal record ActiveWindow
{
	public IntPtr HWnd { get; init; }
	
	public string ProcessName { get; init; }
	
	public string WindowTitle { get; init; }

	public ImageSource? ImageSource { get; set; } = null;
}
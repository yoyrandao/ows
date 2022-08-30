using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ows.Dto;
using Ows.LowLevel;

namespace Ows.Providers;

internal class ActiveWindowsProvider
{
	public IEnumerable<ActiveWindow> GetActiveWindows()
	{
		const int systemProcess = 4;
		
		var activeProcesses = Process.GetProcesses()
			.Where(p => p.Id > systemProcess && Interop.IsTrueWindow(p.MainWindowHandle));

		return activeProcesses.Select(process => new ActiveWindow
		{
			hWnd = process.MainWindowHandle,
			ProcessName = process.ProcessName,
			WindowTitle = process.MainWindowTitle
		});
	}
}
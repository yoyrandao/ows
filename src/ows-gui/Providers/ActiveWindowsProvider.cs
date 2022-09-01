using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using Ows.Dto;
using Ows.LowLevel;

namespace Ows.Providers;

internal class ActiveWindowsProvider
{
	public static IEnumerable<ActiveWindow> GetActiveWindows()
	{
		const int systemProcess = 4;
		
		var activeProcesses = Process.GetProcesses()
			.Where(p => p.Id > systemProcess && Interop.IsTrueWindow(p.MainWindowHandle));
		foreach (var activeWindow in activeProcesses)
        {
			var executablePath = activeWindow.MainModule?.FileName;
			if (executablePath == null)
            {
				Trace.WriteLine(string.Format("Unable to locate executable for proc with Pid [{0}] and Tittle [{1}]",
					activeWindow.Id,
					activeWindow.MainWindowTitle));
            }

			ImageSource source;
			try
            {
				source = Icon.ExtractAssociatedIcon(activeWindow.MainModule?.FileName).ToImageSource();
			}
            catch
            {
				Trace.WriteLine(string.Format("Unable to extract icon for file [{0}] and Tittle [{1}]", executablePath));
				continue;
            }

			if (source != null && source.CanFreeze)
            {
				source.Freeze();
            }

			yield return new ActiveWindow
			{
				HWnd = activeWindow.MainWindowHandle,
				ProcessName = activeWindow.ProcessName,
				WindowTitle = activeWindow.MainWindowTitle,
				ImageSource = source
            };
		}
	}
}
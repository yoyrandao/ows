using System.Windows.Forms;

namespace Ows.Dto;

internal record NotifyRequestRecord
{
	public string Title { get; set; } = "";

	public string Text { get; set; } = "";

	public int Duration { get; set; } = 1000;

	public ToolTipIcon Icon { get; set; } = ToolTipIcon.Info;
}
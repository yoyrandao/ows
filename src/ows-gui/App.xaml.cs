using Ows.ViewModels;
using System.Windows;

namespace Ows;

public partial class App : Application
{
	public App()
	{
		var viewModel = new MainWindowViewModel();
		var window = new MainWindow();
		window.DataContext = viewModel;
		window.Show();
	}
}
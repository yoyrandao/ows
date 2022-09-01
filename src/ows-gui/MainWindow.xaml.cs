using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ows;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void MainWindow_OnStateChanged(object? sender, EventArgs e)
	{
		if (sender is not Window window) return;

		if (window.WindowState == WindowState.Normal)
		{
			window.Activate();
			search.Focus();
		}
	}

	private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (sender is not Window window) return;
		if (e.Key == Key.Escape)
		{
			window.WindowState = WindowState.Minimized;
		}
	}

	private void Window_Deactivated(object sender, EventArgs e)
	{
		if (sender is not Window window) return;

		window.WindowState = WindowState.Minimized;
	}
}
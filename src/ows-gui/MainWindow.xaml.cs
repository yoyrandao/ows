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

	private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
	{
	}

	private void MainWindow_OnStateChanged(object? sender, EventArgs e)
	{
		if (sender is not Window window) return;

		if (window.WindowState == WindowState.Normal)
		{
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
}
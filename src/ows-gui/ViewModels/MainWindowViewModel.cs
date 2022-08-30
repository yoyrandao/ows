using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NonInvasiveKeyboardHookLibrary;
using Ows.Dto;
using Ows.LowLevel;
using ModifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys;

namespace Ows.ViewModels;

internal class MainWindowViewModel : ObservableRecipient
{
	public MainWindowViewModel()
	{
		LoadedCommand = new RelayCommand(Loaded);
		ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);
		NotifyCommand = new RelayCommand(() => Notify("Hello world!"));
		NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
		NotifyIconExitCommand = new RelayCommand(() => { Application.Current.Shutdown(); });

		_hookManager = new KeyboardHookManager();

		_hookManager.Start();
		_hookManager.RegisterHotkey(new[] { ModifierKeys.Control, ModifierKeys.Alt }, 0x57,
			() => { WindowState = WindowState.Normal; });

		var names = new List<string>();
		var procs = Process.GetProcesses().Where(p => p.Id > 4 && LowLevelMethods.IsTrueWindow(p.MainWindowHandle)).ToArray();
		
		foreach (var proc in procs)
		{
			names.Add(proc.MainWindowTitle);
		}
		
		Console.Out.WriteLine(names.Count);
	}

	public ICommand LoadedCommand { get; }
	public ICommand ClosingCommand { get; }
	public ICommand NotifyCommand { get; }
	public ICommand NotifyIconOpenCommand { get; }
	public ICommand NotifyIconExitCommand { get; }

	public WindowState WindowState
	{
		get => _windowState;
		set
		{
			ShowInTaskbar = true;
			SetProperty(ref _windowState, value);
			ShowInTaskbar = value != WindowState.Minimized;
		}
	}

	public bool ShowInTaskbar
	{
		get => _showInTaskbar;
		set => SetProperty(ref _showInTaskbar, value);
	}

	public NotifyRequestRecord NotifyRequest
	{
		get => _notifyRequest!;
		set => SetProperty(ref _notifyRequest, value);
	}

	private void Notify(string message)
	{
		NotifyRequest = new NotifyRequestRecord
		{
			Title = "Notify",
			Text = message,
			Duration = 1000
		};
	}

	private void Loaded()
	{
		WindowState = WindowState.Minimized;
	}

	private void Closing(CancelEventArgs? e)
	{
		if (e == null) return;

		e.Cancel = true;
		WindowState = WindowState.Minimized;

		_hookManager.UnregisterAll();
		_hookManager.Stop();
	}

	private NotifyRequestRecord? _notifyRequest;

	private bool _showInTaskbar;
	private WindowState _windowState;

	private readonly KeyboardHookManager _hookManager;
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NonInvasiveKeyboardHookLibrary;
using Ows.Dto;
using Ows.LowLevel;
using Ows.Providers;
using ModifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys;

namespace Ows.ViewModels;

internal class MainWindowViewModel : ObservableRecipient
{
	public MainWindowViewModel()
	{
		LoadedCommand = new RelayCommand(Loaded);
		ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);
		NotifyCommand = new RelayCommand(() => Notify("Hello world!"));
		ActivatedCommand = new RelayCommand(OnWindowActivated);
		MakeActiveCommand = new RelayCommand<ActiveWindow>(MakeActive);
		NotifyIconOpenCommand = new RelayCommand(() => { WindowState = WindowState.Normal; });
		NotifyIconExitCommand = new RelayCommand(() => { Application.Current.Shutdown(); });

		_hookManager = new KeyboardHookManager();

		_hookManager.Start();
		_hookManager.RegisterHotkey(new[] { ModifierKeys.Control, ModifierKeys.Alt }, 0x57,
			() => ShowWindow());

		ActiveWindows = new List<ActiveWindow>();
	}

	public ICommand LoadedCommand { get; }
	public ICommand ClosingCommand { get; }
	public ICommand ActivatedCommand { get; }
	public ICommand NotifyCommand { get; }
	public ICommand MakeActiveCommand { get; }
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

	public List<ActiveWindow> ActiveWindows
	{
		get => _activeWindows;
		set => SetProperty(ref _activeWindows, value);
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

	private void OnWindowActivated()
	{
		ActiveWindows = new(ActiveWindowsProvider.GetActiveWindows());
	}

	private void MakeActive(ActiveWindow window)
	{
		Interop.SetForegroundWindow(window.HWnd);
		WindowState = WindowState.Minimized;
	}

	private void ShowWindow()
	{
		WindowState = WindowState.Normal;
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
	private List<ActiveWindow> _activeWindows;
	private readonly KeyboardHookManager _hookManager;
}
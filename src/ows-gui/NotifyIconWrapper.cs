using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Ows.Dto;
using Application = System.Windows.Application;

namespace Ows;

internal class NotifyIconWrapper : FrameworkElement, IDisposable
{
	public NotifyIconWrapper()
	{
		if (DesignerProperties.GetIsInDesignMode(this)) return;

		_notifyIcon = new NotifyIcon
		{
			Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
			Visible = true,
			ContextMenuStrip = CreateContextMenu()
		};
		_notifyIcon.DoubleClick += OpenItemOnClick;
		Application.Current.Exit += (obj, args) => { _notifyIcon.Dispose(); };
	}

	public void Dispose()
	{
		_notifyIcon?.Dispose();
	}

	public static readonly DependencyProperty TextProperty =
		DependencyProperty.Register("Text", typeof(string), typeof(NotifyIconWrapper), 
			new PropertyMetadata((d, e) =>
		{
			var notifyIcon = ((NotifyIconWrapper)d)._notifyIcon;
			if (notifyIcon == null) return;

			notifyIcon.Text = (string)e.NewValue;
		}));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public NotifyRequestRecord NotifyRequest
	{
		get => (NotifyRequestRecord)GetValue(NotifyRequestRecordProperty);
		set => SetValue(NotifyRequestRecordProperty, value);
	}

	public event RoutedEventHandler OpenSelected
	{
		add => AddHandler(OpenSelectedEvent, value);
		remove => RemoveHandler(OpenSelectedEvent, value);
	}

	public event RoutedEventHandler ExitSelected
	{
		add => AddHandler(ExitSelectedEvent, value);
		remove => RemoveHandler(ExitSelectedEvent, value);
	}

	private static readonly DependencyProperty NotifyRequestRecordProperty =
		DependencyProperty.Register("NotifyRequest", typeof(NotifyRequestRecord), typeof(NotifyIconWrapper),
			new PropertyMetadata(
				(d, e) =>
				{
					var record = (NotifyRequestRecord)e.NewValue;
					((NotifyIconWrapper)d)._notifyIcon?.ShowBalloonTip(record.Duration, record.Title, record.Text,
						record.Icon);
				}));

	private static readonly RoutedEvent OpenSelectedEvent = EventManager.RegisterRoutedEvent("OpenSelected",
		RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NotifyIconWrapper));

	private static readonly RoutedEvent ExitSelectedEvent = EventManager.RegisterRoutedEvent("ExitSelected",
		RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(NotifyIconWrapper));

	private ContextMenuStrip CreateContextMenu()
	{
		var openItem = new ToolStripMenuItem("Open");
		openItem.Click += OpenItemOnClick;

		var exitItem = new ToolStripMenuItem("Exit");
		exitItem.Click += ExitItemOnClick;

		return new ContextMenuStrip { Items = { openItem, exitItem } };
	}

	private void OpenItemOnClick(object? sender, EventArgs eventArgs)
	{
		RaiseEvent(new RoutedEventArgs(OpenSelectedEvent));
	}

	private void ExitItemOnClick(object? sender, EventArgs eventArgs)
	{
		RaiseEvent(new RoutedEventArgs(ExitSelectedEvent));
	}

	private readonly NotifyIcon? _notifyIcon;
}
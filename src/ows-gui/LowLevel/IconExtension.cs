using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ows.LowLevel;

internal static class IconExtensions
{
	public static ImageSource? ToImageSource(this Icon? icon)
	{
		if (icon == null) return null;
		Bitmap bitmap = icon.ToBitmap();
		IntPtr hBitmap = bitmap.GetHbitmap();

		ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
			hBitmap,
			IntPtr.Zero,
			Int32Rect.Empty,
			BitmapSizeOptions.FromEmptyOptions());

		if (!Interop.DeleteObject(hBitmap))
		{
			throw new Win32Exception();
		}

		return wpfBitmap;
	}
}
#include "twindows.h"

/**
 * Checks for "truthness" of window.
 *
 * "Truthness" is a condition when process has an active window which is not an overlay.
 * Also process should have an active window title which is not equals empty string.
 *
 * @param hWnd Window handle.
 * @return is the window with provided handle need to show in alt-tab interface.
 */
bool IsTrueWindow(HWND hWnd)
{
    if (!IsWindowVisible(hWnd))
        return false;

    WINDOWINFO wInfo;
    GetWindowInfo(hWnd, &wInfo);

    if ((wInfo.dwExStyle & WS_EX_TOOLWINDOW) != 0)
        return false;

    u_int uiCloaked;
    DwmGetWindowAttribute(hWnd, DWMWA_CLOAKED, &uiCloaked, sizeof(u_int));

    return uiCloaked == 0;
}
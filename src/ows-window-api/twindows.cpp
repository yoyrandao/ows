#include "twindows.h"


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
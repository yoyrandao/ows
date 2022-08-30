#include <windows.h>
#include <dwmapi.h>

#pragma comment(lib, "Dwmapi.lib")

#ifdef TWINDOWSLIBRARY_EXPORTS
    #define TWINDOWS_API __declspec(dllexport)
#else
    #define TWINDOWS_API __declspec(dllimport)
#endif

/**
 * Checks for "truthness" of window.
 *
 * "Truthness" is a condition when process has an active window which is not an overlay.
 * Also process should have an active window title which is not equals empty string.
 *
 * @param hWnd Window handle.
 * @return is the window with provided handle need to show in alt-tab interface.
 */
extern "C" TWINDOWS_API bool IsTrueWindow(HWND hWnd);
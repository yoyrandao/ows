#include <windows.h>
#include <dwmapi.h>

#pragma comment(lib, "Dwmapi.lib")

#ifdef TWINDOWSLIBRARY_EXPORTS
    #define TWINDOWS_API __declspec(dllexport)
#else
    #define TWINDOWS_API __declspec(dllimport)
#endif

// Checking for "true" window.
// "True" window is a window that showing in alt-tab interface
extern "C" TWINDOWS_API bool IsTrueWindow(HWND hWnd);
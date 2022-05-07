using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

/// <summary>
/// This is actually not currently used by our program but I am leaving it here as an optional feature that a future developer could turn
/// on if they wanted to go further with this project. Basically this calls external windows libraries to modify the window style
/// of an already running application and add or remove style elements. I was considering using this to remove the border and
/// title bar to make our window feel less like wpf window application and more game-like. If you'd like to see it in action uncomment
/// the corresponding code App.xaml.cs.
/// </summary>
namespace WordGame.Utilities
{
    /*  The following flag values and code snippets were adapted from MSDN Documentation
        and pinvoke.net which is a website dedicated to documenting invokation of external 
        assemblies in C# and Visual Basic
    
        You can read more on these topics from MSDN here:
        https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlonga
        https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga
        https://docs.microsoft.com/en-us/windows/win32/winmsg/window-styles

        More can be read about invoking windows native methods and other commonly referenced 
        assemblies from other APIs in general here:
        https://www.pinvoke.net/default.aspx/user32.SetWindowLongPtr    */

    // Message codes for GetWindowLong
    public struct GWL
    {
        public const int GWL_WNDPROC = (-4);
        public const int GWL_HINSTANCE = (-6);
        public const int GWL_HWNDPARENT = (-8);
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);
        public const int GWL_USERDATA = (-21);
        public const int GWL_ID = (-12);
    }

    // Message codes for WindowStyles which arevisual properties regarding how microsoft natively decorates a window.
    // For our purposes we will only really be making use of WS_POPUP which instructs Win32 to remove the default
    // window menu bar and buttons (e.g. a blank slate which is perfect for a game).
    public struct WS_STYLE
    {
        public const UInt32 WS_BORDER = 0x800000;
        public const UInt32 WS_CAPTION = 0xc00000;
        public const UInt32 WS_CHILD = 0x40000000;
        public const UInt32 WS_CLIPCHILDREN = 0x2000000;
        public const UInt32 WS_CLIPSIBLINGS = 0x4000000;
        public const UInt32 WS_DISABLED = 0x8000000;
        public const UInt32 WS_DLGFRAME = 0x400000;
        public const UInt32 WS_GROUP = 0x20000;
        public const UInt32 WS_HSCROLL = 0x100000;
        public const UInt32 WS_MAXIMIZE = 0x1000000;
        public const UInt32 WS_MAXIMIZEBOX = 0x10000;
        public const UInt32 WS_MINIMIZE = 0x20000000;
        public const UInt32 WS_MINIMIZEBOX = 0x20000;
        public const UInt32 WS_OVERLAPPED = 0x0;
        public const UInt32 WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
        public const UInt32 WS_POPUP = 0x80000000u;
        public const UInt32 WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU;
        public const UInt32 WS_SIZEFRAME = 0x40000;
        public const UInt32 WS_SYSMENU = 0x80000;
        public const UInt32 WS_TABSTOP = 0x10000;
        public const UInt32 WS_VISIBLE = 0x10000000;
        public const UInt32 WS_VSCROLL = 0x200000;
    };

    // This is a static class containing helper functions loaded from Windows standard assembly.
    // They assist in changing hardcoded properties of already created Windows.
    public static class NativeMethodsHelper
    { 

        // The following code utilizes the keyword extern, which tells the compiler (I promise this exists and is defined in another assembly).
        // Attributes appear above classes, methods, or members and gives a kind of special directive to the compiler (meta programming) that adds meta data to the program.
        // They are marked with square brackets and have the format: [ATTRIBUTE(ATTRIBUTE ARGUMENTS)]
        // In C# we can use the DllImport attribute to import assemblies which are already built (particularly useful for accessing assemblies built using other languages like user32.dll


        #region 32-Bit Imported Functions
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        internal static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        #endregion // 32-Bit Imported Functions

        #region 64-Bit Imported Functions
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLongPtr")]
        internal static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        #endregion // 64-Bit Imported Functions

        #region Static Helpers
        // Since C# has no native support for these functions (because they are imported from a system assembly written and compiled in C++)
        // We need to manually check to make sure we are running 64-bit architecure, and if we are not we need to call the legacy 32 bit version of SetWindowLong and GetWindowLong.
        // Most new computers are, but it is still unsafe to call these types of imported functions if you don't know for sure that they are defined for the current architecure the program is running on.
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4) // IntPtr has a size of 4 on 32 bit systems.
            {
                Trace.WriteLine("32 Bit");
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            else if (IntPtr.Size == 8) // If it's size 8 then we are on a 64 bit system.
            {
                Trace.WriteLine("64 Bit");
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            else
            {
                throw new ArgumentException("IntPtr Size is not 4 or 8, what is going on?");
            }
        }

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4) // IntPtr has a size of 4 on 32 bit systems.
            {
                Trace.WriteLine("32 Bit");
                return GetWindowLongPtr32(hWnd, nIndex);
            }
            else if (IntPtr.Size == 8) // If it's size 8 then we are on a 64 bit system.
            {
                Trace.WriteLine("64 Bit");
                return GetWindowLongPtr64(hWnd, nIndex);
            }
            else
            {
                throw new ArgumentException("IntPtr Size is not 4 or 8, what is going on?");
            }
        }
        #endregion // End Static Helpers
    }
}
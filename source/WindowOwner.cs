using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace BIMPlugins.Windows
{
    /// <summary>
    /// Keeps a window above Revit without keeping it above every other program, as Topmost would.
    /// </summary>
    internal static class WindowOwner
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        /// <summary>
        /// Makes the active window of Revit's UI thread the owner: an add-in window that is open (so a message comes above it),
        /// or else Revit's main window. With neither, as at Revit's start, the window stays Topmost so that it is not lost.
        /// </summary>
        public static void SetToRevit(Window window)
        {
            var owner = GetActiveWindow();
            if (owner == IntPtr.Zero)
            {
                using var process = Process.GetCurrentProcess();
                owner = process.MainWindowHandle;
            }

            if (owner == IntPtr.Zero)
            {
                window.Topmost = true;
                return;
            }

            new WindowInteropHelper(window).Owner = owner;
        }
    }
}

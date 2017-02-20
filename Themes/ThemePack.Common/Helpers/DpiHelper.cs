using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using ThemePack.Common.Base;
using ThemePack.Common.Interop;

namespace ThemePack.Common.Helpers
{
    public static class DpiHelper
    {
        private static bool _initialized = false;
        private static DPI _dpi;

        public static DPI DPI
        {
            get
            {
                if (!_initialized)
                {
                    throw new InvalidOperationException("not initialized");
                }

                return _dpi;
            }
        }
        
        /// <summary>
        /// Get <see cref="DPI"/> for <paramref name="hwnd"/>, where <paramref name="hwnd"/> is pointer to window.
        /// </summary>
        /// <param name="hwnd"><see cref="IntPtr"/> to get <see cref="DPI"/> for monitor when it's shown.</param>
        /// <returns><see cref="DPI"/> from monitor.</returns>
        public static DPI GetDpiForHwnd(IntPtr hwnd)
        {
            var monitor = user32.MonitorFromWindow(hwnd, user32.MONITOR_DEFAULTTONEAREST);
            return GetDpiForMonitor(monitor);
        }

        /// <summary>
        /// Get system <see cref="DPI"/> (not monitor aware).
        /// </summary>
        /// <returns>system <see cref="DPI"/>.</returns>
        public static DPI GetSystemDpi()
        {
            var hDc = user32.GetDC(IntPtr.Zero);
            var newDpiX = gdi32.GetDeviceCaps(hDc, gdi32.LOGPIXELSX);
            var newDpiY = gdi32.GetDeviceCaps(hDc, gdi32.LOGPIXELSY);
            user32.ReleaseDC(IntPtr.Zero, hDc);

            return new DPI(newDpiX, newDpiY);
        }

        /// <summary>
        /// Get <see cref="DPI"/> for <paramref name="monitor"/>, where <paramref name="monitor"/> is pointer to monitor.
        /// </summary>
        /// <param name="monitor"><see cref="IntPtr"/> to monitor to get <see cref="DPI"/> for.</param>
        /// <returns><see cref="DPI"/> for <paramref name="monitor"/>.</returns>
        private static DPI GetDpiForMonitor(IntPtr monitor)
        {
            int tempX;
            int tempY;

            var result = shcore.GetDpiForMonitor(monitor, shcore.MonitorDpiType.Effective, out tempX, out tempY).ToInt32();
            if (result != shcore.S_OK)
            {
                return new DPI(96.0, 96.0);
            }

            return new DPI(tempX, tempY);
        }
    }
}

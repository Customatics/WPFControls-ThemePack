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
        private static readonly object _lock = new object();
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

        public static void Init(Visual visual)
        {
            lock (_lock)
            {
                if (_initialized)
                {
                    throw new InvalidOperationException("already initialized");
                }

                var _dpiX = 96.0;
                var _dpiY = 96.0;

                var source = PresentationSource.FromVisual(visual);
                if (source?.CompositionTarget != null)
                {
                    _dpiX *= source.CompositionTarget.TransformToDevice.M11;
                    _dpiY *= source.CompositionTarget.TransformToDevice.M22;
                }

                _dpi = new DPI(_dpiX, _dpiY);

                _initialized = true;
            }
        }

        /// <summary>
        /// Transforms device independent units (1/96 of an inch) to pixels.
        /// </summary>
        /// <param name="visual">a visual object.</param>
        /// <param name="unitX">a device independent unit value X.</param>
        /// <param name="unitY">a device independent unit value Y.</param>
        /// <param name="pixelX">returns the X value in pixels.</param>
        /// <param name="pixelY">returns the Y value in pixels.</param>
        public static void TransformToPixels(Visual visual, double unitX, double unitY, out double pixelX, out double pixelY)
        {
            var source = visual == null
                ? null
                : PresentationSource.FromVisual(visual);
            var compositionTarget = source?.CompositionTarget;

            Matrix? matrix = null;
            if (compositionTarget != null)
            {
                matrix = compositionTarget.TransformToDevice;
            }
            else
            {
                using (var src = new HwndSource(new HwndSourceParameters()))
                {
                    if (src.CompositionTarget != null)
                    {
                        matrix = src.CompositionTarget.TransformToDevice;
                    }
                }
            }

            if (matrix != null)
            {
                pixelX = unitX * matrix.Value.M11;
                pixelY = unitY * matrix.Value.M22;
            }
            else
            {
                pixelX = unitX;
                pixelY = unitY;
            }
        }

        public static Point TransformToPixels(Point point)
        {
            double x;
            double y;
            TransformToPixels(null, point.X, point.Y, out x, out y);
            return new Point(x, y);
        }

        /// <summary>
        /// Get <see cref="DPI"/> for monitor by <paramref name="monitor"/>.
        /// </summary>
        /// <param name="monitor">pointor to monitor.</param>
        /// <returns></returns>
        public static DPI GetMonitorDpi(IntPtr monitor)
        {
            return DesktopHelper.IsWindows8OrOlder()
                ? GetSystemDpi()
                : GetDpiForMonitor(monitor);
        }

        /// <summary>
        /// Get <see cref="DPI"/> for monitor by <paramref name="deviceName"/>.
        /// </summary>
        /// <param name="deviceName">monitor name.</param>
        /// <returns>monitor <see cref="DPI"/>.</returns>
        public static DPI GetMonitorDpi(string deviceName)
        {
            return DesktopHelper.IsWindows8OrOlder()
                ? GetSystemDpi()
                : GetDpiForDevice(deviceName);
        }

        /// <summary>
        /// Get <see cref="DPI"/> for monitor with index <paramref name="monitor"/>.
        /// </summary>
        /// <param name="monitor">monitor index.</param>
        public static DPI GetMonitorDpi(int monitor)
        {
            return DesktopHelper.IsWindows8OrOlder()
                ? GetSystemDpi()
                : GetDpiForMonitor(monitor);
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

        /// <summary>
        /// Get <see cref="DPI"/> for monitor with <paramref name="deviceName"/>.
        /// </summary>
        /// <param name="deviceName">monitor name.</param>
        /// <returns><see cref="DPI"/> for <paramref name="deviceName"/>.</returns>
        private static DPI GetDpiForDevice(string deviceName)
        {
            DPI dpi = null;
            user32.EnumDisplayMonitors(user32.NullHandleRef, null, (monitor, hdc, lprcMonitor, lParam) =>
            {
                var info = user32.MONITORINFOEX.New();
                if (user32.GetMonitorInfo(monitor, ref info) == false)
                {
                    return false;
                }

                if (info.DeviceName == deviceName)
                {
                    dpi = GetDpiForMonitor(monitor);
                }
                return true;
            },
                IntPtr.Zero);

            return dpi ?? GetSystemDpi();
        }

        /// <summary>
        /// Get <see cref="DPI"/> for monitor with index <paramref name="display"/>.
        /// Results can be different based on <see cref="shcore.ProcessDpiAwareness"/>.
        /// </summary>
        /// <param name="display">monitor index to get <see cref="DPI"/> for.</param>
        /// <returns>monitor <see cref="DPI"/>.</returns>
        private static DPI GetDpiForMonitor(int display)
        {
            shcore.ProcessDpiAwareness awarness;
            shcore.GetProcessDpiAwareness(Process.GetCurrentProcess().Handle, out awarness);

            var dpis = new List<DPI>();
            user32.EnumDisplayMonitors(user32.NullHandleRef, null, (monitor, hdc, lprcMonitor, lParam) =>
            {
                int tempX;
                int tempY;

                var result = shcore.GetDpiForMonitor(monitor, shcore.MonitorDpiType.Effective, out tempX, out tempY).ToInt32();
                if (result != shcore.S_OK)
                {
                    return false;
                }

                dpis.Add(new DPI(tempX, tempY));
                return true;
            }, IntPtr.Zero);

            return dpis.Count < display
                ? GetSystemDpi()
                : dpis.ElementAt(display);
        }
    }
}

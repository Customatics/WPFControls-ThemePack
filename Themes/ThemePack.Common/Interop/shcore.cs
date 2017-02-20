using System;
using System.Runtime.InteropServices;

namespace ThemePack.Common.Interop
{
    public class shcore
    {
        public enum MonitorDpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2
        }

        public const int S_OK = 0;

        [DllImport("shcore.dll", SetLastError = true)]
        public static extern IntPtr GetDpiForMonitor(IntPtr hMonitor, MonitorDpiType dpiType, out int dpiX, out int dpiY);
    }
}
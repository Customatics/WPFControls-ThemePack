using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ThemePack.Common.Interop
{
    public class shcore
    {
        public const int S_OK = 0;
        public const int E_INVALIDARG = -2147024809;

        [DllImport("shcore.dll", SetLastError = true)]
        public static extern IntPtr GetDpiForMonitor(IntPtr hMonitor, MonitorDpiType dpiType, out int dpiX, out int dpiY);

        [DllImport("shcore.dll", SetLastError = true)]
        public static extern IntPtr SetProcessDpiAwareness(ProcessDpiAwareness awareness);

        [DllImport("shcore.dll", SetLastError = true)]
        public static extern void GetProcessDpiAwareness(IntPtr hprocess, out ProcessDpiAwareness awareness);

        public enum MonitorDpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2
        }

        public enum ProcessDpiAwareness
        {
            Unaware = 0,
            SystemDpiAware = 1,
            PerMonitorDpiAware = 2
        }
    }
}

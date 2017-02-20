using System;
using System.Runtime.InteropServices;

namespace ThemePack.Common.Interop
{
    public static class gdi32
    {
        private const string DllName = "gdi32.dll";

        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
}
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace ThemePack.Common.Interop
{
    public static class user32
    {
        public delegate bool EnumDesktopsDelegate(string desktop, IntPtr lParam);

        public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        public delegate IntPtr HookHandlerDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

        public delegate void WinEventDelegate(
            IntPtr hWinEventHook,
            uint eventType,
            IntPtr hwnd,
            int idObject,
            int idChild,
            uint dwEventThread,
            uint dwmsEventTime);

        public enum WinMessages
        {
            WM_DPICHANGED = 0x02E0,
            WM_GETMINMAXINFO = 0x0024,
            WM_SIZE = 0x0005,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047
        }

        private const int CCHDEVICENAME = 32;
        public const int MONITOR_DEFAULTTONEAREST = 2;
        private const string DllName = "user32.dll";
        public const int WsExTransparent = 0x00000020;
        public const int GwlExstyle = -20;
        public const int WmHotKey = 0x0312;

        public const int SW_SHOWNORMAL = 1;
        public const int SW_MINIMIZE = 6;
        public const int SW_MAXIMIZE = 3;
        public const int SW_RESTORE = 9;

        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;

        public const int CURSOR_SHOWING = 0x00000001;

        public const uint DESKTOP_CREATEMENU = 0x0004;
        public const uint DESKTOP_CREATEWINDOW = 0x0002;
        public const uint DESKTOP_ENUMERATE = 0x0040;
        public const uint DESKTOP_HOOKCONTROL = 0x0008;
        public const uint DESKTOP_WRITEOBJECTS = 0x0080;
        public const uint DESKTOP_READOBJECTS = 0x0001;
        public const uint DESKTOP_SWITCHDESKTOP = 0x0100;

        // winnt.h
        public const long GENERIC_WRITE = 0x40000000L;
        public const long GENERIC_READ = 0x80000000;
        public const long GENERIC_ALL = 0x10000000L;

        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int ENUM_REGISTRY_SETTINGS = -2;
        public const uint WM_CLOSE = 0x10;
        public const uint WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        public const int SC_RESTORE = 0xF120;
        public const int SC_MINIMIZE = 0xF020;

        public const int GWL_STYLE = -16;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_MAXIMIZE = 0x1000000;

        public const uint WPF_SETMINPOSITION = 0x0001;
        public const uint WPF_RESTORETOMAXIMIZED = 0x0002;
        public const uint WPF_ASYNCWINDOWPLACEMENT = 0x0004;

        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;
        public const uint WM_MOUSEMOVE = 0x0200;
        public const uint WM_MOUSEWHEEL = 0x020A;
        public const uint WM_MOUSEHWHEEL = 0x020E;
        public const uint WM_RBUTTONDOWN = 0x0204;
        public const uint WM_RBUTTONUP = 0x0205;
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy,
            int wFlags);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport(DllName, CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        [StructLayout(LayoutKind.Sequential)]
        public class COMRECT
        {
            public int bottom;
            public int left;
            public int right;
            public int top;

            public COMRECT()
            {
            }

            public COMRECT(Rectangle r)
            {
                left = r.X;
                top = r.Y;
                right = r.Right;
                bottom = r.Bottom;
            }

            public COMRECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public static COMRECT FromXYWH(int x, int y, int width, int height)
            {
                return new COMRECT(x, y, x + width, y + height);
            }

            public override string ToString()
            {
                return "Left = " + left + " Top " + (string) (object) top + " Right = " + (string) (object) right +
                       " Bottom = " + (string) (object) bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Size Size
            {
                get { return new Size(Right - Left, Bottom - Top); }
            }

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MONITORINFOEX
        {
            public int Size;
            public RECT Monitor;
            public RECT WorkArea;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)] public string DeviceName;

            public bool IsPrimary
            {
                get { return (Flags & 1) > 0; }
            }

            public static MONITORINFOEX New()
            {
                var item = new MONITORINFOEX();
                item.Size = 40 + 2 * CCHDEVICENAME;
                item.Flags = 0;
                item.DeviceName = string.Empty;
                return item;
            }
        }

        public sealed class SafeDesktopHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeDesktopHandle()
                : base(true)
            {
            }

            public SafeDesktopHandle(IntPtr handle, bool ownHandle)
                : base(ownHandle)
            {
                SetHandle(handle);
            }

            protected override bool ReleaseHandle()
            {
                CloseDesktop(handle);
                return true;
            }
        }
    }
}
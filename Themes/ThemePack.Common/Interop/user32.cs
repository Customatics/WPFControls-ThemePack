using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace ThemePack.Common.Interop
{
    public static class user32
    {
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
        private const int CCHDEVICENAME = 32;
        public const int MONITOR_DEFAULTTONEAREST = 2;
        private const string DllName = "user32.dll";
        public const int WsExTransparent = 0x00000020;
        public const int GwlExstyle = (-20);
        public const int WmHotKey = 0x0312;

        public const int SW_SHOWNORMAL = 1;
        public const int SW_MINIMIZE = 6;
        public const int SW_MAXIMIZE = 3;
        public const int SW_RESTORE = 9;

        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOACTIVATE = 0x0010;

        public delegate IntPtr HookHandlerDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookHandlerDelegate callbackPtr, IntPtr hInstance, uint dwThreadId);

        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        public static extern short VkKeyScan(char ch);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        public static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

        [DllImport(DllName)]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [DllImport(DllName, SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumDesktops(IntPtr hwinsta, EnumDesktopsDelegate lpEnumFunc, IntPtr lParam);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool LockWorkStation();

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr GetShellWindow();
        [DllImport(DllName, SetLastError = true)]
        public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport(DllName, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);
        [DllImport(DllName, SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport(DllName, SetLastError = true)]
        public static extern bool IsWindowVisible(int hWnd);
        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(int hWnd, StringBuilder title, int size);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(DllName, SetLastError = true)]
        public static extern SafeDesktopHandle OpenInputDesktop(uint dwFlags, bool fInherit, uint dwDesiredAccess);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport(DllName)]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr SetWinEventHook(
            SetWinEventHookEvent eventMin,
            SetWinEventHookEvent eventMax,
            IntPtr hmodWinEventProc,
            WinEventDelegate lpfnWinEventProc,
            uint idProcess,
            uint idThread,
            SetWinEventHookFlags dwFlags);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetUserObjectInformation(IntPtr hObj, GetUserObjectInformationIndex nIndex, IntPtr pvInfo, uint nLength, ref uint lpnLengthNeeded);

        [DllImport(DllName, SetLastError = true)]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        [DllImport(DllName, SetLastError = true)]
        public static extern uint MapVirtualKeyEx(uint uCode, MapType uMapType, IntPtr dwhkl);

        [DllImport(DllName, SetLastError = true)]
        public static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [DllImport(DllName, SetLastError = true)]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport(DllName, SetLastError = true)]
        public static extern short GetKeyState(int vKey);

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport(DllName, SetLastError = true)]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        [DllImport(DllName, SetLastError = true)]
        public static extern IntPtr OpenWindowStation(string lpszWinSta, bool fInherit, uint dwDesiredAccess);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CloseWindowStation(IntPtr hWinsta);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern bool IsProcessDpiAware();

        [DllImport(DllName, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetProcessWindowStation(IntPtr hWindowStation);

        [DllImport(DllName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern DISP_CHANGE ChangeDisplaySettings(ref DEVMODE devMode, ChangeDisplaySettingsFlags flags);

        [DllImport(DllName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE mode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport(DllName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [DllImport(DllName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport(DllName)]
        public static extern bool EnumDisplayMonitors(HandleRef hdc, COMRECT rcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);

        public delegate bool MonitorEnumProc(IntPtr monitor, IntPtr hdc, IntPtr lprcMonitor, IntPtr lParam);

        [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        [DllImport(DllName)]
        public static extern IntPtr MonitorFromPoint(Point pt, int dwFlags);

        [DllImport(DllName, CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetCursorInfo(ref CURSORINFO pci);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();


        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool DrawIconEx(
            IntPtr hdc, int xLeft, int yTop, IntPtr hIcon,
            int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw,
            int diFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CURSORINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hCursor;
            public POINTL ptScreenPos;
        }

        public struct GUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public RECT rcCaret;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct ICONINFO
        {
            /// <summary>
            /// Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies an icon; FALSE specifies a cursor.
            /// </summary>
            public bool fIcon;

            /// <summary>
            /// Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot
            /// spot is always in the center of the icon, and this member is ignored.
            /// </summary>
            public Int32 xHotspot;

            /// <summary>
            /// Specifies the y-coordinate of a cursor's hot spot. If this structure defines an icon, the hot
            /// spot is always in the center of the icon, and this member is ignored.
            /// </summary>              
            public Int32 yHotspot;

            /// <summary>
            /// (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon,
            /// this bitmask is formatted so that the upper half is the icon AND bitmask and the lower half is
            /// the icon XOR bitmask. Under this condition, the height should be an even multiple of two. If
            /// this structure defines a color icon, this mask only defines the AND bitmask of the icon.
            /// </summary>
            public IntPtr hbmMask;

            /// <summary>
            /// (HBITMAP) Handle to the icon color bitmap. This member can be optional if this
            /// structure defines a black and white icon. The AND bitmask of hbmMask is applied with the SRCAND
            /// flag to the destination; subsequently, the color bitmap is applied (using XOR) to the
            /// destination by using the SRCINVERT flag.
            /// </summary>
            public IntPtr hbmColor;
        }

        public const int CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class COMRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public COMRECT()
            {
            }

            public COMRECT(Rectangle r)
            {
                this.left = r.X;
                this.top = r.Y;
                this.right = r.Right;
                this.bottom = r.Bottom;
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
                return "Left = " + (object)this.left + " Top " + (string)(object)this.top + " Right = " + (string)(object)this.right + " Bottom = " + (string)(object)this.bottom;
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
                get
                {
                    return new Size(this.Right - this.Left, this.Bottom - this.Top);
                }
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
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            public string DeviceName;
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

        [Flags]
        public enum ChangeDisplaySettingsFlags : uint
        {
            CDS_NONE = 0,
            CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            CDS_FULLSCREEN = 0x00000004,
            CDS_GLOBAL = 0x00000008,
            CDS_SET_PRIMARY = 0x00000010,
            CDS_VIDEOPARAMETERS = 0x00000020,
            CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            CDS_RESET = 0x40000000,
            CDS_RESET_EX = 0x20000000,
            CDS_NORESET = 0x10000000
        }

        [Flags]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        //public sealed class SafeWindowStationHandle : SafeHandleZeroOrMinusOneIsInvalid
        //{
        //    public SafeWindowStationHandle()
        //        : base(true)
        //    {
        //    }

        //    protected override bool ReleaseHandle()
        //    {
        //        return SafeNativeMethods.CloseWindowStation(handle);
        //    }
        //}

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        public struct DEVMODE
        {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            [FieldOffset(0)]
            public string dmDeviceName;
            [FieldOffset(32)]
            public short dmSpecVersion;
            [FieldOffset(34)]
            public short dmDriverVersion;
            [FieldOffset(36)]
            public short dmSize;
            [FieldOffset(38)]
            public short dmDriverExtra;
            [FieldOffset(40)]
            public DM dmFields;

            [FieldOffset(44)]
            short dmOrientation;
            [FieldOffset(46)]
            short dmPaperSize;
            [FieldOffset(48)]
            short dmPaperLength;
            [FieldOffset(50)]
            short dmPaperWidth;
            [FieldOffset(52)]
            short dmScale;
            [FieldOffset(54)]
            short dmCopies;
            [FieldOffset(56)]
            short dmDefaultSource;
            [FieldOffset(58)]
            short dmPrintQuality;

            [FieldOffset(44)]
            public POINTL dmPosition;
            [FieldOffset(52)]
            public Int32 dmDisplayOrientation;
            [FieldOffset(56)]
            public Int32 dmDisplayFixedOutput;

            [FieldOffset(60)]
            public short dmColor; // See note below!
            [FieldOffset(62)]
            public short dmDuplex; // See note below!
            [FieldOffset(64)]
            public short dmYResolution;
            [FieldOffset(66)]
            public short dmTTOption;
            [FieldOffset(68)]
            public short dmCollate; // See note below!
            [FieldOffset(72)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            [FieldOffset(102)]
            public short dmLogPixels;
            [FieldOffset(104)]
            public Int32 dmBitsPerPel;
            [FieldOffset(108)]
            public Int32 dmPelsWidth;
            [FieldOffset(112)]
            public Int32 dmPelsHeight;
            [FieldOffset(116)]
            public Int32 dmDisplayFlags;
            [FieldOffset(116)]
            public Int32 dmNup;
            [FieldOffset(120)]
            public Int32 dmDisplayFrequency;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINTL
        {
            public Int32 x;
            public Int32 y;
        }

        public enum DISP_CHANGE : int
        {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

        [Flags]
        public enum DM : int
        {
            Orientation = 0x1,
            PaperSize = 0x2,
            PaperLength = 0x4,
            PaperWidth = 0x8,
            Scale = 0x10,
            Position = 0x20,
            NUP = 0x40,
            DisplayOrientation = 0x80,
            Copies = 0x100,
            DefaultSource = 0x200,
            PrintQuality = 0x400,
            Color = 0x800,
            Duplex = 0x1000,
            YResolution = 0x2000,
            TTOption = 0x4000,
            Collate = 0x8000,
            FormName = 0x10000,
            LogPixels = 0x20000,
            BitsPerPixel = 0x40000,
            PelsWidth = 0x80000,
            PelsHeight = 0x100000,
            DisplayFlags = 0x200000,
            DisplayFrequency = 0x400000,
            ICMMethod = 0x800000,
            ICMIntent = 0x1000000,
            MediaType = 0x2000000,
            DitherType = 0x4000000,
            PanningWidth = 0x8000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }


        /// <summary>
        /// The INPUT structure is used by SendInput to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks. (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
        /// Declared in Winuser.h, include Windows.h
        /// </summary>
        /// <remarks>
        /// This structure contains information identical to that used in the parameter list of the keybd_event or mouse_event function.
        /// Windows 2000/XP: INPUT_KEYBOARD supports nonkeyboard input methods, such as handwriting recognition or voice recognition, as if it were text input by using the KEYEVENTF_UNICODE flag. For more information, see the remarks section of KEYBDINPUT.
        /// </remarks>
        public struct INPUT
        {
            /// <summary>
            /// Specifies the type of the input event. This member can be one of the following values. 
            /// <see cref="InputType.Mouse"/> - The event is a mouse event. Use the mi structure of the union.
            /// <see cref="InputType.Keyboard"/> - The event is a keyboard event. Use the ki structure of the union.
            /// <see cref="InputType.Hardware"/> - Windows 95/98/Me: The event is from input hardware other than a keyboard or mouse. Use the hi structure of the union.
            /// </summary>
            public InputType Type;

            /// <summary>
            /// The data structure that contains information about the simulated Mouse, Keyboard or Hardware event.
            /// </summary>
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        /// <summary>
        /// The combined/overlayed structure that includes Mouse, Keyboard and Hardware Input message data (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct MOUSEKEYBDHARDWAREINPUT
        {
            /// <summary>
            /// The <see cref="MOUSEINPUT"/> definition.
            /// </summary>
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;

            /// <summary>
            /// The <see cref="KEYBDINPUT"/> definition.
            /// </summary>
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;

            /// <summary>
            /// The <see cref="HARDWAREINPUT"/> definition.
            /// </summary>
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MSLLHOOKSTRUCT
        {
            public POINTL pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        /// <summary>
        /// The HARDWAREINPUT structure contains information about a simulated message generated by an input device other than a keyboard or mouse.  (see: http://msdn.microsoft.com/en-us/library/ms646269(VS.85).aspx)
        /// Declared in Winuser.h, include Windows.h
        /// </summary>
        public struct HARDWAREINPUT
        {
            /// <summary>
            /// Value specifying the message generated by the input hardware. 
            /// </summary>
            public uint Msg;

            /// <summary>
            /// Specifies the low-order word of the lParam parameter for uMsg. 
            /// </summary>
            public ushort ParamL;

            /// <summary>
            /// Specifies the high-order word of the lParam parameter for uMsg. 
            /// </summary>
            public ushort ParamH;
        }

        /// <summary>
        /// The KEYBDINPUT structure contains information about a simulated keyboard event.  (see: http://msdn.microsoft.com/en-us/library/ms646271(VS.85).aspx)
        /// Declared in Winuser.h, include Windows.h
        /// </summary>
        /// <remarks>
        /// Windows 2000/XP: INPUT_KEYBOARD supports nonkeyboard-input methodssuch as handwriting recognition or voice recognitionas if it were text input by using the KEYEVENTF_UNICODE flag. If KEYEVENTF_UNICODE is specified, SendInput sends a WM_KEYDOWN or WM_KEYUP message to the foreground thread's message queue with wParam equal to VK_PACKET. Once GetMessage or PeekMessage obtains this message, passing the message to TranslateMessage posts a WM_CHAR message with the Unicode character originally specified by wScan. This Unicode character will automatically be converted to the appropriate ANSI value if it is posted to an ANSI window.
        /// Windows 2000/XP: Set the KEYEVENTF_SCANCODE flag to define keyboard input in terms of the scan code. This is useful to simulate a physical keystroke regardless of which keyboard is currently being used. The virtual key value of a key may alter depending on the current keyboard layout or what other keys were pressed, but the scan code will always be the same.
        /// </remarks>
        public struct KEYBDINPUT
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. The Winuser.h header file provides macro definitions (VK_*) for each value. If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0. 
            /// </summary>
            public ushort KeyCode;

            /// <summary>
            /// Specifies a hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application. 
            /// </summary>
            public ushort Scan;

            /// <summary>
            /// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
            /// KEYEVENTF_EXTENDEDKEY - If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
            /// KEYEVENTF_KEYUP - If specified, the key is being released. If not specified, the key is being pressed.
            /// KEYEVENTF_SCANCODE - If specified, wScan identifies the key and wVk is ignored. 
            /// KEYEVENTF_UNICODE - Windows 2000/XP: If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section. 
            /// </summary>
            public KeyboardFlag Flags;

            /// <summary>
            /// Time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp. 
            /// </summary>
            public uint Time;

            /// <summary>
            /// Specifies an additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information. 
            /// </summary>
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// The MOUSEINPUT structure contains information about a simulated mouse event. (see: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
        /// Declared in Winuser.h, include Windows.h
        /// </summary>
        /// <remarks>
        /// If the mouse has moved, indicated by MOUSEEVENTF_MOVE, dx and dy specify information about that movement. The information is specified as absolute or relative integer values. 
        /// If MOUSEEVENTF_ABSOLUTE value is specified, dx and dy contain normalized absolute coordinates between 0 and 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) maps onto the upper-left corner of the display surface; coordinate (65535,65535) maps onto the lower-right corner. In a multimonitor system, the coordinates map to the primary monitor. 
        /// Windows 2000/XP: If MOUSEEVENTF_VIRTUALDESK is specified, the coordinates map to the entire virtual desktop.
        /// If the MOUSEEVENTF_ABSOLUTE value is not specified, dx and dy specify movement relative to the previous mouse event (the last reported position). Positive values mean the mouse moved right (or down); negative values mean the mouse moved left (or up). 
        /// Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. A user sets these three values with the Pointer Speed slider of the Control Panel's Mouse Properties sheet. You can obtain and set these values using the SystemParametersInfo function. 
        /// The system applies two tests to the specified relative mouse movement. If the specified distance along either the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system doubles the distance. If the specified distance along either the x or y axis is greater than the second mouse threshold value, and the mouse speed is equal to two, the system doubles the distance that resulted from applying the first threshold test. It is thus possible for the system to multiply specified relative mouse movement along the x or y axis by up to four times.
        /// </remarks>
        public struct MOUSEINPUT
        {
            /// <summary>
            /// Specifies the absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member. Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels moved. 
            /// </summary>
            public int X;

            /// <summary>
            /// Specifies the absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member. Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels moved. 
            /// </summary>
            public int Y;

            /// <summary>
            /// If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120. 
            /// Windows Vista: If dwFlags contains MOUSEEVENTF_HWHEEL, then dwData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left. One wheel click is defined as WHEEL_DELTA, which is 120.
            /// Windows 2000/XP: IfdwFlags does not contain MOUSEEVENTF_WHEEL, MOUSEEVENTF_XDOWN, or MOUSEEVENTF_XUP, then mouseData should be zero. 
            /// If dwFlags contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then mouseData specifies which X buttons were pressed or released. This value may be any combination of the following flags. 
            /// </summary>
            public uint MouseData;

            /// <summary>
            /// A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this member can be any reasonable combination of the following values. 
            /// The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions. For example, if the left mouse button is pressed and held down, MOUSEEVENTF_LEFTDOWN is set when the left button is first pressed, but not for subsequent motions. Similarly, MOUSEEVENTF_LEFTUP is set only when the button is first released. 
            /// You cannot specify both the MOUSEEVENTF_WHEEL flag and either MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP flags simultaneously in the dwFlags parameter, because they both require use of the mouseData field. 
            /// </summary>
            public uint Flags;

            /// <summary>
            /// Time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp. 
            /// </summary>
            public uint Time;

            /// <summary>
            /// Specifies an additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra information. 
            /// </summary>
            public IntPtr ExtraInfo;
        }

        public enum SetWinEventHookFlags : uint
        {
            WINEVENT_OUTOFCONTEXT = 0,
            WINEVENT_SKIPOWNTHREAD = 1,
            WINEVENT_SKIPOWNPROCESS = 2,
            WINEVENT_INCONTEXT = 4,
        }

        public enum SetWinEventHookEvent : uint
        {
            EVENT_MIN = 0x00000001,
            EVENT_SYSTEM_DESKTOPSWITCH = 0x0020,
            EVENT_MAX = 0x7FFFFFFF
        }

        /// <summary>
        /// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
        /// </summary>
        [Flags]
        public enum KeyboardFlag : uint // UInt32
        {
            /// <summary>
            /// KEYEVENTF_EXTENDEDKEY = 0x0001 (If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).)
            /// </summary>
            ExtendedKey = 0x0001,

            /// <summary>
            /// KEYEVENTF_KEYUP = 0x0002 (If specified, the key is being released. If not specified, the key is being pressed.)
            /// </summary>
            KeyUp = 0x0002,

            /// <summary>
            /// KEYEVENTF_UNICODE = 0x0004 (If specified, wScan identifies the key and wVk is ignored.)
            /// </summary>
            Unicode = 0x0004,

            /// <summary>
            /// KEYEVENTF_SCANCODE = 0x0008 (Windows 2000/XP: If specified, the system synthesizes a VK_PACKET keystroke. The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, see the Remarks section.)
            /// </summary>
            ScanCode = 0x0008,
        }

        /// <summary>
        /// Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
        ///  
        /// This Enum and declaration signature was written by Gabriel T. Sharp
        /// ai_productions@verizon.net or osirisgothra@hotmail.com
        /// Obtained on pinvoke.net, please contribute your code to support the wiki!
        /// </summary>
        public enum SystemMetric : int
        {
            /// <summary>
            /// The flags that specify how the system arranged minimized windows. For more information, see the Remarks section in this topic.
            /// </summary>
            SM_ARRANGE = 56,

            /// <summary>
            /// The value that specifies how the system is started:
            /// 0 Normal boot
            /// 1 Fail-safe boot
            /// 2 Fail-safe with network boot
            /// A fail-safe boot (also called SafeBoot, Safe Mode, or Clean Boot) bypasses the user startup files.
            /// </summary>
            SM_CLEANBOOT = 67,

            /// <summary>
            /// The number of display monitors on a desktop. For more information, see the Remarks section in this topic.
            /// </summary>
            SM_CMONITORS = 80,

            /// <summary>
            /// The number of buttons on a mouse, or zero if no mouse is installed.
            /// </summary>
            SM_CMOUSEBUTTONS = 43,

            /// <summary>
            /// The width of a window border, in pixels. This is equivalent to the SM_CXEDGE value for windows with the 3-D look.
            /// </summary>
            SM_CXBORDER = 5,

            /// <summary>
            /// The width of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CXCURSOR = 13,

            /// <summary>
            /// This value is the same as SM_CXFIXEDFRAME.
            /// </summary>
            SM_CXDLGFRAME = 7,

            /// <summary>
            /// The width of the rectangle around the location of a first click in a double-click sequence, in pixels. ,
            /// The second click must occur within the rectangle that is defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system
            /// to consider the two clicks a double-click. The two clicks must also occur within a specified time.
            /// To set the width of the double-click rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKWIDTH.
            /// </summary>
            SM_CXDOUBLECLK = 36,

            /// <summary>
            /// The number of pixels on either side of a mouse-down point that the mouse pointer can move before a drag operation begins.
            /// This allows the user to click and release the mouse button easily without unintentionally starting a drag operation.
            /// If this value is negative, it is subtracted from the left of the mouse-down point and added to the right of it.
            /// </summary>
            SM_CXDRAG = 68,

            /// <summary>
            /// The width of a 3-D border, in pixels. This metric is the 3-D counterpart of SM_CXBORDER.
            /// </summary>
            SM_CXEDGE = 45,

            /// <summary>
            /// The thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels.
            /// SM_CXFIXEDFRAME is the height of the horizontal border, and SM_CYFIXEDFRAME is the width of the vertical border.
            /// This value is the same as SM_CXDLGFRAME.
            /// </summary>
            SM_CXFIXEDFRAME = 7,

            /// <summary>
            /// The width of the left and right edges of the focus rectangle that the DrawFocusRectdraws.
            /// This value is in pixels.
            /// Windows 2000:  This value is not supported.
            /// </summary>
            SM_CXFOCUSBORDER = 83,

            /// <summary>
            /// This value is the same as SM_CXSIZEFRAME.
            /// </summary>
            SM_CXFRAME = 32,

            /// <summary>
            /// The width of the client area for a full-screen window on the primary display monitor, in pixels.
            /// To get the coordinates of the portion of the screen that is not obscured by the system taskbar or by application desktop toolbars,
            /// call the SystemParametersInfofunction with the SPI_GETWORKAREA value.
            /// </summary>
            SM_CXFULLSCREEN = 16,

            /// <summary>
            /// The width of the arrow bitmap on a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHSCROLL = 21,

            /// <summary>
            /// The width of the thumb box in a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHTHUMB = 10,

            /// <summary>
            /// The default width of an icon, in pixels. The LoadIcon function can load only icons with the dimensions
            /// that SM_CXICON and SM_CYICON specifies.
            /// </summary>
            SM_CXICON = 11,

            /// <summary>
            /// The width of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size
            /// SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CXICON.
            /// </summary>
            SM_CXICONSPACING = 38,

            /// <summary>
            /// The default width, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CXMAXIMIZED = 61,

            /// <summary>
            /// The default maximum width of a window that has a caption and sizing borders, in pixels.
            /// This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMAXTRACK = 59,

            /// <summary>
            /// The width of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CXMENUCHECK = 71,

            /// <summary>
            /// The width of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
            /// </summary>
            SM_CXMENUSIZE = 54,

            /// <summary>
            /// The minimum width of a window, in pixels.
            /// </summary>
            SM_CXMIN = 28,

            /// <summary>
            /// The width of a minimized window, in pixels.
            /// </summary>
            SM_CXMINIMIZED = 57,

            /// <summary>
            /// The width of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged.
            /// This value is always greater than or equal to SM_CXMINIMIZED.
            /// </summary>
            SM_CXMINSPACING = 47,

            /// <summary>
            /// The minimum tracking width of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMINTRACK = 34,

            /// <summary>
            /// The amount of border padding for captioned windows, in pixels. Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_CXPADDEDBORDER = 92,

            /// <summary>
            /// The width of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
            /// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
            /// </summary>
            SM_CXSCREEN = 0,

            /// <summary>
            /// The width of a button in a window caption or title bar, in pixels.
            /// </summary>
            SM_CXSIZE = 30,

            /// <summary>
            /// The thickness of the sizing border around the perimeter of a window that can be resized, in pixels.
            /// SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border.
            /// This value is the same as SM_CXFRAME.
            /// </summary>
            SM_CXSIZEFRAME = 32,

            /// <summary>
            /// The recommended width of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CXSMICON = 49,

            /// <summary>
            /// The width of small caption buttons, in pixels.
            /// </summary>
            SM_CXSMSIZE = 52,

            /// <summary>
            /// The width of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_XVIRTUALSCREEN metric is the coordinates for the left side of the virtual screen.
            /// </summary>
            SM_CXVIRTUALSCREEN = 78,

            /// <summary>
            /// The width of a vertical scroll bar, in pixels.
            /// </summary>
            SM_CXVSCROLL = 2,

            /// <summary>
            /// The height of a window border, in pixels. This is equivalent to the SM_CYEDGE value for windows with the 3-D look.
            /// </summary>
            SM_CYBORDER = 6,

            /// <summary>
            /// The height of a caption area, in pixels.
            /// </summary>
            SM_CYCAPTION = 4,

            /// <summary>
            /// The height of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CYCURSOR = 14,

            /// <summary>
            /// This value is the same as SM_CYFIXEDFRAME.
            /// </summary>
            SM_CYDLGFRAME = 8,

            /// <summary>
            /// The height of the rectangle around the location of a first click in a double-click sequence, in pixels.
            /// The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider
            /// the two clicks a double-click. The two clicks must also occur within a specified time. To set the height of the double-click
            /// rectangle, call SystemParametersInfo with SPI_SETDOUBLECLKHEIGHT.
            /// </summary>
            SM_CYDOUBLECLK = 37,

            /// <summary>
            /// The number of pixels above and below a mouse-down point that the mouse pointer can move before a drag operation begins.
            /// This allows the user to click and release the mouse button easily without unintentionally starting a drag operation.
            /// If this value is negative, it is subtracted from above the mouse-down point and added below it.
            /// </summary>
            SM_CYDRAG = 69,

            /// <summary>
            /// The height of a 3-D border, in pixels. This is the 3-D counterpart of SM_CYBORDER.
            /// </summary>
            SM_CYEDGE = 46,

            /// <summary>
            /// The thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels.
            /// SM_CXFIXEDFRAME is the height of the horizontal border, and SM_CYFIXEDFRAME is the width of the vertical border.
            /// This value is the same as SM_CYDLGFRAME.
            /// </summary>
            SM_CYFIXEDFRAME = 8,

            /// <summary>
            /// The height of the top and bottom edges of the focus rectangle drawn byDrawFocusRect.
            /// This value is in pixels.
            /// Windows 2000:  This value is not supported.
            /// </summary>
            SM_CYFOCUSBORDER = 84,

            /// <summary>
            /// This value is the same as SM_CYSIZEFRAME.
            /// </summary>
            SM_CYFRAME = 33,

            /// <summary>
            /// The height of the client area for a full-screen window on the primary display monitor, in pixels.
            /// To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars,
            /// call the SystemParametersInfo function with the SPI_GETWORKAREA value.
            /// </summary>
            SM_CYFULLSCREEN = 17,

            /// <summary>
            /// The height of a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CYHSCROLL = 3,

            /// <summary>
            /// The default height of an icon, in pixels. The LoadIcon function can load only icons with the dimensions SM_CXICON and SM_CYICON.
            /// </summary>
            SM_CYICON = 12,

            /// <summary>
            /// The height of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size
            /// SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CYICON.
            /// </summary>
            SM_CYICONSPACING = 39,

            /// <summary>
            /// For double byte character set versions of the system, this is the height of the Kanji window at the bottom of the screen, in pixels.
            /// </summary>
            SM_CYKANJIWINDOW = 18,

            /// <summary>
            /// The default height, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CYMAXIMIZED = 62,

            /// <summary>
            /// The default maximum height of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop.
            /// The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing
            /// the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMAXTRACK = 60,

            /// <summary>
            /// The height of a single-line menu bar, in pixels.
            /// </summary>
            SM_CYMENU = 15,

            /// <summary>
            /// The height of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CYMENUCHECK = 72,

            /// <summary>
            /// The height of menu bar buttons, such as the child window close button that is used in the multiple document interface, in pixels.
            /// </summary>
            SM_CYMENUSIZE = 55,

            /// <summary>
            /// The minimum height of a window, in pixels.
            /// </summary>
            SM_CYMIN = 29,

            /// <summary>
            /// The height of a minimized window, in pixels.
            /// </summary>
            SM_CYMINIMIZED = 58,

            /// <summary>
            /// The height of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged.
            /// This value is always greater than or equal to SM_CYMINIMIZED.
            /// </summary>
            SM_CYMINSPACING = 48,

            /// <summary>
            /// The minimum tracking height of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions.
            /// A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMINTRACK = 35,

            /// <summary>
            /// The height of the screen of the primary display monitor, in pixels. This is the same value obtained by calling 
            /// GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
            /// </summary>
            SM_CYSCREEN = 1,

            /// <summary>
            /// The height of a button in a window caption or title bar, in pixels.
            /// </summary>
            SM_CYSIZE = 31,

            /// <summary>
            /// The thickness of the sizing border around the perimeter of a window that can be resized, in pixels.
            /// SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border.
            /// This value is the same as SM_CYFRAME.
            /// </summary>
            SM_CYSIZEFRAME = 33,

            /// <summary>
            /// The height of a small caption, in pixels.
            /// </summary>
            SM_CYSMCAPTION = 51,

            /// <summary>
            /// The recommended height of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CYSMICON = 50,

            /// <summary>
            /// The height of small caption buttons, in pixels.
            /// </summary>
            SM_CYSMSIZE = 53,

            /// <summary>
            /// The height of the virtual screen, in pixels. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_YVIRTUALSCREEN metric is the coordinates for the top of the virtual screen.
            /// </summary>
            SM_CYVIRTUALSCREEN = 79,

            /// <summary>
            /// The height of the arrow bitmap on a vertical scroll bar, in pixels.
            /// </summary>
            SM_CYVSCROLL = 20,

            /// <summary>
            /// The height of the thumb box in a vertical scroll bar, in pixels.
            /// </summary>
            SM_CYVTHUMB = 9,

            /// <summary>
            /// Nonzero if User32.dll supports DBCS; otherwise, 0.
            /// </summary>
            SM_DBCSENABLED = 42,

            /// <summary>
            /// Nonzero if the debug version of User.exe is installed; otherwise, 0.
            /// </summary>
            SM_DEBUG = 22,

            /// <summary>
            /// Nonzero if the current operating system is Windows 7 or Windows Server 2008 R2 and the Tablet PC Input
            /// service is started; otherwise, 0. The return value is a bitmask that specifies the type of digitizer input supported by the device.
            /// For more information, see Remarks.
            /// Windows Server 2008, Windows Vista, and Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_DIGITIZER = 94,

            /// <summary>
            /// Nonzero if Input Method Manager/Input Method Editor features are enabled; otherwise, 0.
            /// SM_IMMENABLED indicates whether the system is ready to use a Unicode-based IME on a Unicode application.
            /// To ensure that a language-dependent IME works, check SM_DBCSENABLED and the system ANSI code page.
            /// Otherwise the ANSI-to-Unicode conversion may not be performed correctly, or some components like fonts
            /// or registry settings may not be present.
            /// </summary>
            SM_IMMENABLED = 82,

            /// <summary>
            /// Nonzero if there are digitizers in the system; otherwise, 0. SM_MAXIMUMTOUCHES returns the aggregate maximum of the
            /// maximum number of contacts supported by every digitizer in the system. If the system has only single-touch digitizers,
            /// the return value is 1. If the system has multi-touch digitizers, the return value is the number of simultaneous contacts
            /// the hardware can provide. Windows Server 2008, Windows Vista, and Windows XP/2000:  This value is not supported.
            /// </summary>
            SM_MAXIMUMTOUCHES = 95,

            /// <summary>
            /// Nonzero if the current operating system is the Windows XP, Media Center Edition, 0 if not.
            /// </summary>
            SM_MEDIACENTER = 87,

            /// <summary>
            /// Nonzero if drop-down menus are right-aligned with the corresponding menu-bar item; 0 if the menus are left-aligned.
            /// </summary>
            SM_MENUDROPALIGNMENT = 40,

            /// <summary>
            /// Nonzero if the system is enabled for Hebrew and Arabic languages, 0 if not.
            /// </summary>
            SM_MIDEASTENABLED = 74,

            /// <summary>
            /// Nonzero if a mouse is installed; otherwise, 0. This value is rarely zero, because of support for virtual mice and because
            /// some systems detect the presence of the port instead of the presence of a mouse.
            /// </summary>
            SM_MOUSEPRESENT = 19,

            /// <summary>
            /// Nonzero if a mouse with a horizontal scroll wheel is installed; otherwise 0.
            /// </summary>
            SM_MOUSEHORIZONTALWHEELPRESENT = 91,

            /// <summary>
            /// Nonzero if a mouse with a vertical scroll wheel is installed; otherwise 0.
            /// </summary>
            SM_MOUSEWHEELPRESENT = 75,

            /// <summary>
            /// The least significant bit is set if a network is present; otherwise, it is cleared. The other bits are reserved for future use.
            /// </summary>
            SM_NETWORK = 63,

            /// <summary>
            /// Nonzero if the Microsoft Windows for Pen computing extensions are installed; zero otherwise.
            /// </summary>
            SM_PENWINDOWS = 41,

            /// <summary>
            /// This system metric is used in a Terminal Services environment to determine if the current Terminal Server session is
            /// being remotely controlled. Its value is nonzero if the current session is remotely controlled; otherwise, 0.
            /// You can use terminal services management tools such as Terminal Services Manager (tsadmin.msc) and shadow.exe to
            /// control a remote session. When a session is being remotely controlled, another user can view the contents of that session
            /// and potentially interact with it.
            /// </summary>
            SM_REMOTECONTROL = 0x2001,

            /// <summary>
            /// This system metric is used in a Terminal Services environment. If the calling process is associated with a Terminal Services
            /// client session, the return value is nonzero. If the calling process is associated with the Terminal Services console session,
            /// the return value is 0.
            /// Windows Server 2003 and Windows XP:  The console session is not necessarily the physical console.
            /// For more information, seeWTSGetActiveConsoleSessionId.
            /// </summary>
            SM_REMOTESESSION = 0x1000,

            /// <summary>
            /// Nonzero if all the display monitors have the same color format, otherwise, 0. Two displays can have the same bit depth,
            /// but different color formats. For example, the red, green, and blue pixels can be encoded with different numbers of bits,
            /// or those bits can be located in different places in a pixel color value.
            /// </summary>
            SM_SAMEDISPLAYFORMAT = 81,

            /// <summary>
            /// This system metric should be ignored; it always returns 0.
            /// </summary>
            SM_SECURE = 44,

            /// <summary>
            /// The build number if the system is Windows Server 2003 R2; otherwise, 0.
            /// </summary>
            SM_SERVERR2 = 89,

            /// <summary>
            /// Nonzero if the user requires an application to present information visually in situations where it would otherwise present
            /// the information only in audible form; otherwise, 0.
            /// </summary>
            SM_SHOWSOUNDS = 70,

            /// <summary>
            /// Nonzero if the current session is shutting down; otherwise, 0. Windows 2000:  This value is not supported.
            /// </summary>
            SM_SHUTTINGDOWN = 0x2000,

            /// <summary>
            /// Nonzero if the computer has a low-end (slow) processor; otherwise, 0.
            /// </summary>
            SM_SLOWMACHINE = 73,

            /// <summary>
            /// Nonzero if the current operating system is Windows 7 Starter Edition, Windows Vista Starter, or Windows XP Starter Edition; otherwise, 0.
            /// </summary>
            SM_STARTER = 88,

            /// <summary>
            /// Nonzero if the meanings of the left and right mouse buttons are swapped; otherwise, 0.
            /// </summary>
            SM_SWAPBUTTON = 23,

            /// <summary>
            /// Nonzero if the current operating system is the Windows XP Tablet PC edition or if the current operating system is Windows Vista
            /// or Windows 7 and the Tablet PC Input service is started; otherwise, 0. The SM_DIGITIZER setting indicates the type of digitizer
            /// input supported by a device running Windows 7 or Windows Server 2008 R2. For more information, see Remarks.
            /// </summary>
            SM_TABLETPC = 86,

            /// <summary>
            /// The coordinates for the left side of the virtual screen. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_CXVIRTUALSCREEN metric is the width of the virtual screen.
            /// </summary>
            SM_XVIRTUALSCREEN = 76,

            /// <summary>
            /// The coordinates for the top of the virtual screen. The virtual screen is the bounding rectangle of all display monitors.
            /// The SM_CYVIRTUALSCREEN metric is the height of the virtual screen.
            /// </summary>
            SM_YVIRTUALSCREEN = 77,
        }

        public enum WinMessages : int
        {
            WM_DPICHANGED = 0x02E0,
            WM_GETMINMAXINFO = 0x0024,
            WM_SIZE = 0x0005,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
        }

        /// <summary>
        /// The set of MouseFlags for use in the Flags property of the <see cref="MOUSEINPUT"/> structure. (See: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
        /// </summary>
        [Flags]
        public enum MouseFlag : uint // UInt32
        {
            /// <summary>
            /// Specifies that movement occurred.
            /// </summary>
            Move = 0x0001,

            /// <summary>
            /// Specifies that the left button was pressed.
            /// </summary>
            LeftDown = 0x0002,

            /// <summary>
            /// Specifies that the left button was released.
            /// </summary>
            LeftUp = 0x0004,

            /// <summary>
            /// Specifies that the right button was pressed.
            /// </summary>
            RightDown = 0x0008,

            /// <summary>
            /// Specifies that the right button was released.
            /// </summary>
            RightUp = 0x0010,

            /// <summary>
            /// Specifies that the middle button was pressed.
            /// </summary>
            MiddleDown = 0x0020,

            /// <summary>
            /// Specifies that the middle button was released.
            /// </summary>
            MiddleUp = 0x0040,

            /// <summary>
            /// Windows 2000/XP: Specifies that an X button was pressed.
            /// </summary>
            XDown = 0x0080,

            /// <summary>
            /// Windows 2000/XP: Specifies that an X button was released.
            /// </summary>
            XUp = 0x0100,

            /// <summary>
            /// Windows NT/2000/XP: Specifies that the wheel was moved, if the mouse has a wheel. The amount of movement is specified in mouseData. 
            /// </summary>
            VerticalWheel = 0x0800,

            /// <summary>
            /// Specifies that the wheel was moved horizontally, if the mouse has a wheel. The amount of movement is specified in mouseData. Windows 2000/XP:  Not supported.
            /// </summary>
            HorizontalWheel = 0x1000,

            /// <summary>
            /// Windows 2000/XP: Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
            /// </summary>
            VirtualDesk = 0x4000,

            /// <summary>
            /// Specifies that the dx and dy members contain normalized absolute coordinates. If the flag is not set, dxand dy contain relative data (the change in position since the last reported position). This flag can be set, or not set, regardless of what kind of mouse or other pointing device, if any, is connected to the system. For further information about relative mouse motion, see the following Remarks section.
            /// </summary>
            Absolute = 0x8000,
        }

        /// <summary>
        /// Specifies the type of the input event. This member can be one of the following values. 
        /// </summary>
        public enum InputType : uint // UInt32
        {
            /// <summary>
            /// INPUT_MOUSE = 0x00 (The event is a mouse event. Use the mi structure of the union.)
            /// </summary>
            Mouse = 0,

            /// <summary>
            /// INPUT_KEYBOARD = 0x01 (The event is a keyboard event. Use the ki structure of the union.)
            /// </summary>
            Keyboard = 1,

            /// <summary>
            /// INPUT_HARDWARE = 0x02 (Windows 95/98/Me: The event is from input hardware other than a keyboard or mouse. Use the hi structure of the union.)
            /// </summary>
            Hardware = 2,
        }

        public enum GetUserObjectInformationIndex : int
        {
            UOI_NAME = 2
        }

        public enum VirtualKey : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
            //
            VK_XBUTTON1 = 0x05,
            VK_XBUTTON2 = 0x06,
            //
            VK_BACK = 0x08,
            VK_TAB = 0x09,
            //
            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,
            //
            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,
            //
            VK_KANA = 0x15,
            VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,
            //
            VK_ESCAPE = 0x1B,
            //
            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,
            //
            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,
            //
            VK_0 = 0x30,
            VK_1 = 0x31,
            VK_2 = 0x32,
            VK_3 = 0x33,
            VK_4 = 0x34,
            VK_5 = 0x35,
            VK_6 = 0x36,
            VK_7 = 0x37,
            VK_8 = 0x38,
            VK_9 = 0x39,
            //
            VK_A = 0x41,
            VK_B = 0x42,
            VK_C = 0x43,
            VK_D = 0x44,
            VK_E = 0x45,
            VK_F = 0x46,
            VK_G = 0x47,
            VK_H = 0x48,
            VK_I = 0x49,
            VK_J = 0x4A,
            VK_K = 0x4B,
            VK_L = 0x4C,
            VK_M = 0x4D,
            VK_N = 0x4E,
            VK_O = 0x4F,
            VK_P = 0x50,
            VK_Q = 0x51,
            VK_R = 0x52,
            VK_S = 0x53,
            VK_T = 0x54,
            VK_U = 0x55,
            VK_V = 0x56,
            VK_W = 0x57,
            VK_X = 0x58,
            VK_Y = 0x59,
            VK_Z = 0x5A,
            //
            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,
            //
            VK_SLEEP = 0x5F,
            //
            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,
            //
            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,
            //
            VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
                                       //
            VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
            VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
            VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
            VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
            VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
                                     //
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,
            //
            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,
            //
            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,
            //
            VK_OEM_1 = 0xBA,   // ';:' for US
            VK_OEM_PLUS = 0xBB,   // '+' any country
            VK_OEM_COMMA = 0xBC,   // ',' any country
            VK_OEM_MINUS = 0xBD,   // '-' any country
            VK_OEM_PERIOD = 0xBE,   // '.' any country
            VK_OEM_2 = 0xBF,   // '/?' for US
            VK_OEM_3 = 0xC0,   // '`~' for US
                               //
            VK_OEM_4 = 0xDB,  //  '[{' for US
            VK_OEM_5 = 0xDC,  //  '\|' for US
            VK_OEM_6 = 0xDD,  //  ']}' for US
            VK_OEM_7 = 0xDE,  //  ''"' for US
            VK_OEM_8 = 0xDF,
            //
            VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
            VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
            VK_ICO_HELP = 0xE3,  //  Help key on ICO
            VK_ICO_00 = 0xE4,  //  00 key on ICO
                               //
            VK_PROCESSKEY = 0xE5,
            //
            VK_ICO_CLEAR = 0xE6,
            //
            VK_PACKET = 0xE7,
            //
            VK_OEM_RESET = 0xE9,
            VK_OEM_JUMP = 0xEA,
            VK_OEM_PA1 = 0xEB,
            VK_OEM_PA2 = 0xEC,
            VK_OEM_PA3 = 0xED,
            VK_OEM_WSCTRL = 0xEE,
            VK_OEM_CUSEL = 0xEF,
            VK_OEM_ATTN = 0xF0,
            VK_OEM_FINISH = 0xF1,
            VK_OEM_COPY = 0xF2,
            VK_OEM_AUTO = 0xF3,
            VK_OEM_ENLW = 0xF4,
            VK_OEM_BACKTAB = 0xF5,
            //
            VK_ATTN = 0xF6,
            VK_CRSEL = 0xF7,
            VK_EXSEL = 0xF8,
            VK_EREOF = 0xF9,
            VK_PLAY = 0xFA,
            VK_ZOOM = 0xFB,
            VK_NONAME = 0xFC,
            VK_PA1 = 0xFD,
            VK_OEM_CLEAR = 0xFE
        }

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        public delegate void WinEventDelegate(
            IntPtr hWinEventHook,
            uint eventType,
            IntPtr hwnd,
            int idObject,
            int idChild,
            uint dwEventThread,
            uint dwmsEventTime);

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

        public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
        public const uint WM_CLOSE = 0x10;
        public const uint WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        public const int SC_RESTORE = 0xF120;
        public const int SC_MINIMIZE = 0xF020;

        public const int GWL_STYLE = -16;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_MAXIMIZE = 0x1000000;

        public delegate bool EnumDesktopsDelegate(string desktop, IntPtr lParam);

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

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowPlacement
        {
            public uint length;
            public uint flags;
            public ShowWindowCommands showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        public enum ShowWindowCommands : uint
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximize = 3,
            NoActivate = 4,
            Show = 5,
            Minimize = 6,
            MinNoActive = 7,
            Showna = 8,
            Restore = 9,
        }

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

        [DllImport(DllName)]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport(DllName, EntryPoint = "GetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPosition(ref Point lpPoint);

        [DllImport(DllName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vlc);

        [DllImport(DllName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}

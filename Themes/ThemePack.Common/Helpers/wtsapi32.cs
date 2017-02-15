using System;
using System.Runtime.InteropServices;

namespace ThemePack.Common.Helpers
{
    public static class wtsapi32
    {
        private const string DllName = "wtsapi32.dll";

        [DllImport(DllName, SetLastError = true)]
        public static extern uint WTSQueryUserToken(uint SessionId, ref IntPtr phToken);

        [DllImport(DllName, SetLastError = true)]
        public static extern int WTSEnumerateSessions(
            IntPtr hServer,
            uint Reserved,
            uint Version,
            ref IntPtr ppSessionInfo,
            ref uint pCount);

        [DllImport(DllName, SetLastError = false)]
        public static extern void WTSFreeMemory(IntPtr memory);

        [StructLayout(LayoutKind.Sequential)]
        public struct WTS_SESSION_INFO
        {
            public readonly uint SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public readonly string pWinStationName;

            public readonly WTS_CONNECTSTATE_CLASS State;
        }

        public static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }
    }
}

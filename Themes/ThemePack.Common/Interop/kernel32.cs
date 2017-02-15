using System;
using System.Runtime.InteropServices;

namespace ThemePack.Common.Interop
{
    public static class kernel32
    {
        private const string DllName = "kernel32.dll";

        [DllImport(DllName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int lstrlenA(IntPtr nullTerminatedString);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(DllName, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hSnapshot);

        [DllImport(DllName, SetLastError = true)]
        public static extern uint WTSGetActiveConsoleSessionId();

        [DllImport(DllName, SetLastError = true)]
        public static extern uint GetCurrentThreadId();

        [DllImport(DllName, SetLastError = true)]
        public static extern uint GetLastError();

        [DllImport(DllName, SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        /// <summary>
        /// Enables an application to inform the system that it is in use, thereby preventing the system from entering sleep or turning off the display while the application is running.
        /// </summary>
        /// <param name="state">The thread's execution requirements.</param>
        /// <returns>If the function succeeds, the return value is the previous thread execution state. If the function fails, the return value is null.</returns>
        [DllImport(DllName, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint SetThreadExecutionState(ThreadExecutionState state);

        public const uint INVALID_SESSION_ID = 0xFFFFFFFF;

        [Flags]
        public enum ThreadExecutionState : uint
        {
            /// <summary>
            /// Enables away mode. This value must be specified with <see cref="CONTINUOUS"/>.
            /// Away mode should be used only by media-recording and media-distribution applications
            /// that must perform critical background processing on desktop computers while the computer appears to be sleeping. 
            /// </summary>
            AWAYMODE_REQUIRED = 0x00000040,

            /// <summary>
            /// Informs the system that the state being set should remain in effect until the next call that uses <see cref="CONTINUOUS"/> and one of the other state flags is cleared.
            /// </summary>
            CONTINUOUS = 0x80000000,

            /// <summary>
            /// Forces the display to be on by resetting the display idle timer.
            /// </summary>
            DISPLAY_REQUIRED = 0x00000002,

            /// <summary>
            /// Forces the system to be in the working state by resetting the system idle timer.
            /// </summary>
            SYSTEM_REQUIRED = 0x00000001,

            /// <summary>
            /// This value is not supported. If <see cref="USER_PRESENT"/> is combined with other <see cref="ThreadExecutionState"/> values, the call will fail and none of the specified states will be set.
            /// </summary>
            USER_PRESENT = 0x00000004
        }
    }
}

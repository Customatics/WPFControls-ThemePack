using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using ThemePack.Common.Interop;
using ThemePack.Common.Models;
using ThemePack.Common.Models.Constants;
using ThemePack.Common.Win;

namespace ThemePack.Common.Helpers
{
    public static class DesktopHelper
    {
        private const string WinlogonDesktopName = "Winlogon";
        private const int ClosingMinimizingApplsTimeoutMs = 2000;

        private static readonly wtsapi32.WTS_CONNECTSTATE_CLASS[] _consoleSessionInvalidStates =
            new[] { wtsapi32.WTS_CONNECTSTATE_CLASS.WTSConnectQuery, wtsapi32.WTS_CONNECTSTATE_CLASS.WTSInit };

        public static SystemType SystemType { get; }
        public static bool VersionNTIs601 { get; }
        public static string MachineName { get { return Environment.MachineName; } }

        public static readonly Func<PixelRegion, PackedRegion> PixelEncoder;

        static DesktopHelper()
        {
            SystemType = GetSystemType();
            VersionNTIs601 = SystemType == SystemType.Windows7 || SystemType == SystemType.WindowsServer2008R2;

            PixelEncoder =
                region =>
                {
                    var compressedPixels = Utils.Compress(region.Pixels);

                    return new PackedRegion(region.Left, region.Top, region.Width, region.Height, compressedPixels);
                };
        }

        /// <summary>
        /// Allocates pointer and puts null terminated UTF-8 string bytes.
        /// </summary>
        /// <param name="value">Original managed string</param>
        /// <returns></returns>
        public static IntPtr NativeUtf8FromString(string value)
        {
            var length = Encoding.UTF8.GetByteCount(value);
            var buffer = new byte[length + 1];
            Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);
            var nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
            return nativeUtf8;
        }

        /// <summary>
        /// Reads all bytes as a null terminated UTF-8 string.
        /// </summary>
        /// <param name="nativeUtf8"></param>
        public static string StringFromNativeUtf8(IntPtr nativeUtf8)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
            var length = kernel32.lstrlenA(nativeUtf8);
            return StringFromNativeUtf8(nativeUtf8, length);
        }

        /// <summary>
        /// Reads all bytes as a null terminated UTF-8 string.
        /// </summary>
        /// <param name="nativeUtf8"></param>
        public static string StringFromNativeUtf8(IntPtr nativeUtf8, int length)
        {
            if (nativeUtf8 == IntPtr.Zero)
            {
                return null;
            }
            var buffer = new byte[length];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Check if current OS is Windows 8 or older.
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows8OrOlder()
        {
            var os = Environment.OSVersion;
            var version = os.Version;

            return (os.Platform == PlatformID.Win32NT) && ((version.Major < 6) || ((version.Major == 6) && (version.Minor <= 2)));
        }

        public static IntPtr GetFocusedInputThreadKeyboardLayout()
        {
            var threadInfo = new user32.GUITHREADINFO();
            threadInfo.cbSize = Marshal.SizeOf(threadInfo);

            if (user32.GetGUIThreadInfo(0, ref threadInfo) == false)
            {
                throw new Exception($"can't get gui thread info: " + Marshal.GetLastWin32Error());
            }

            uint doesnotmatter;

            var threadid = user32.GetWindowThreadProcessId(threadInfo.hwndActive, out doesnotmatter);
            if (threadid == 0)
            {
                throw new Exception($"can't get window thread id: " + Marshal.GetLastWin32Error());
            }

            var activeWindowLayout = user32.GetKeyboardLayout(threadid);
            if (activeWindowLayout == IntPtr.Zero)
            {
                throw new Exception($"can't get keyboard layout for thread: " + Marshal.GetLastWin32Error());
            }

            return activeWindowLayout;
        }

        public static string GetMachineCode()
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            var registryView =
                Environment.Is64BitOperatingSystem
                ? RegistryView.Registry64
                : RegistryView.Registry32;

            using (var localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                using (var rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                    {
                        throw new KeyNotFoundException($"Key Not Found: {location}");
                    }

                    var machineGuid = rk.GetValue(name);
                    if (machineGuid == null)
                    {
                        throw new IndexOutOfRangeException($"Index Not Found: {name}");
                    }

                    return machineGuid.ToString();
                }
            }
        }

        public static IDisposable Impersonate(string userNameWithDomainName, string password)
        {
            string[] temp = userNameWithDomainName.Split('\\');
            if (temp.Length != 2)
            {
                throw new ArgumentException(@"userName should include domainName, in the format <domainName>\<userName>", "userName");
            }

            return Impersonate(temp[0], temp[1], password);
        }

        public static IDisposable Impersonate(string domainName, string userName, string password)
        {
            advapi32.SafeTokenHandle token = null;
            advapi32.SafeTokenHandle tokenDuplicate = null;
            WindowsIdentity identity = null;

            try
            {
                if (advapi32.RevertToSelf() == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (advapi32.LogonUser(userName, domainName, password,
                        advapi32.LogonType.LOGON32_LOGON_INTERACTIVE,
                        advapi32.LogonProvider.LOGON32_PROVIDER_DEFAULT, out token) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                if (advapi32.DuplicateToken(token, 2, out tokenDuplicate) == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                identity = new WindowsIdentity(tokenDuplicate.DangerousGetHandle());

                var impersonationContext = identity.Impersonate();

                return new ImpersonationUndo(identity, impersonationContext);
            }
            catch (Exception ex)
            {
                if (identity != null)
                {
                    identity.Dispose();
                }

                throw new Exception($@"Could not obtain a WindowsIdentity for {domainName}\{userName} using the supplied credentials.", ex);
            }
            finally
            {
                if (tokenDuplicate != null)
                {
                    tokenDuplicate.Dispose();
                }

                if (token != null)
                {
                    token.Dispose();
                }
            }
        }

        public static bool SupportsResolutionChanges(string desktopName)
        {
            return desktopName != WinlogonDesktopName;
        }

        public static string GetDesktopName(IntPtr hDesktop)
        {
            // get the length of the name.
            uint needed = 0;
            var name = string.Empty;

            // return value here is always false
            user32.GetUserObjectInformation(hDesktop, user32.GetUserObjectInformationIndex.UOI_NAME, IntPtr.Zero, 0, ref needed);

            // get the name.
            var nativeMemory = IntPtr.Zero;

            try
            {
                nativeMemory = Marshal.AllocHGlobal((int)needed);
                var result = user32.GetUserObjectInformation(hDesktop, user32.GetUserObjectInformationIndex.UOI_NAME, nativeMemory, needed, ref needed);

                // something went wrong.
                if (result == false)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception($"can't get desktop name. error code: {errorCode}");
                }

                name = Marshal.PtrToStringUni(nativeMemory);
            }
            finally
            {
                if (nativeMemory != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(nativeMemory);
                }
            }

            return name;
        }

        public static bool CurrentSessionIsRemote()
        {
            return user32.GetSystemMetrics(user32.SystemMetric.SM_REMOTESESSION) != 0;
        }

        public static user32.SafeDesktopHandle OpenInputDesktop()
        {
            //var desiredAccess =
            //    user32.DESKTOP_CREATEMENU |
            //    user32.DESKTOP_CREATEWINDOW |
            //    user32.DESKTOP_ENUMERATE |
            //    user32.DESKTOP_HOOKCONTROL |
            //    user32.DESKTOP_WRITEOBJECTS |
            //    user32.DESKTOP_READOBJECTS |
            //    user32.DESKTOP_SWITCHDESKTOP |
            //    (uint)user32.GENERIC_WRITE;

            var desiredAccess = (uint)user32.GENERIC_ALL;

            var _desktop = user32.OpenInputDesktop(0, false, desiredAccess);
            if (_desktop.IsInvalid)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception($"Could not openInputDesktop. Error code {errorCode}");
            }

            return _desktop;
        }

        public static void SetThreadDesktop(IntPtr hDesktop)
        {
            if (user32.SetThreadDesktop(hDesktop) == false)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), $"Could not set thread desktop");
            }
        }

        public static string PrepareDesktop()
        {
            using (var safeDesktopHandle = OpenInputDesktop())
            {
                var desktopName = GetDesktopName(safeDesktopHandle.DangerousGetHandle());
                var nativeThreadId = kernel32.GetCurrentThreadId();

                Debug.WriteLine($"desktop name: {desktopName}; native thread id: {nativeThreadId}");

                SetThreadDesktop(safeDesktopHandle.DangerousGetHandle());

                return desktopName;
            }
        }

        public static void StopApplication()
        {
            Environment.Exit(0);
        }

        public static user32.DISPLAY_DEVICE[] GetAttachedToDesktopNonMirroringDevices()
        {
            var devices =
                GetDisplayDevices()
                    .Where(x => x.StateFlags.HasFlag(user32.DisplayDeviceStateFlags.AttachedToDesktop))
                    .Where(x => x.StateFlags.HasFlag(user32.DisplayDeviceStateFlags.MirroringDriver) == false)
                    .ToArray();

            return devices;
        }

        public static GetDisplayDeviceByNumberResult GetDisplayDeviceByNumber(int number)
        {
            var devices = GetAttachedToDesktopNonMirroringDevices();

            user32.DISPLAY_DEVICE device;
            bool found;

            if (number < devices.Length)
            {
                found = true;
                device = devices.ElementAt(number);
            }
            else
            {
                found = false;
                device = GetPrimaryDisplayDevice();
            }

            return new GetDisplayDeviceByNumberResult(device, found);
        }

        public static FindMonitorResult FindMonitor(int displayIndex)
        {
            var monitors = GetSystemMonitors();

            if (!monitors.Any())
            {
                throw new Exception("no monitors were found");
            }

            bool found;
            user32.MONITORINFOEX monitor;

            if (monitors.Count > displayIndex)
            {
                found = true;
                monitor = monitors[displayIndex];
            }
            else
            {
                found = false;
                monitor = monitors.Single(x => x.IsPrimary);
            }

            return new FindMonitorResult(monitor, found);
        }

        public static int GetDisplayIndex(user32.MONITORINFOEX monitor)
        {
            var monitors = GetSystemMonitors();

            if (!monitors.Any())
            {
                throw new Exception("no monitors were found");
            }

            for (int i = 0; i < monitors.Count; i++)
            {
                if (monitor.DeviceName.Equals(monitors[i].DeviceName, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            }

            return 0;
        }

        public static user32.MONITORINFOEX FindMonitor(IntPtr hwnd)
        {
            var monitor = user32.MonitorFromWindow(hwnd, user32.MONITOR_DEFAULTTONEAREST);
            var info = user32.MONITORINFOEX.New();

            if (user32.GetMonitorInfo(monitor, ref info) == false)
            {
                throw new Win32Exception($"can't get monitor info. error {Marshal.GetLastWin32Error()}");
            }

            return info;
        }

        private static List<user32.MONITORINFOEX> GetSystemMonitors()
        {
            var monitors = new List<user32.MONITORINFOEX>();
            var result =
                user32.EnumDisplayMonitors(user32.NullHandleRef, null, (monitor, hdc, lprcMonitor, lParam) =>
                {
                    var info = user32.MONITORINFOEX.New();
                    if (user32.GetMonitorInfo(monitor, ref info) == false)
                    {
                        return false;
                    }

                    monitors.Add(info);
                    return true;
                }, IntPtr.Zero);

            if (result == false)
            {
                throw new Win32Exception($"can't get system monitors. error {Marshal.GetLastWin32Error()}");
            }

            return monitors;
        }

        public static user32.DISPLAY_DEVICE GetPrimaryDisplayDevice()
        {
            var devices =
                GetDisplayDevices()
                    .Where(x => x.StateFlags.HasFlag(user32.DisplayDeviceStateFlags.PrimaryDevice))
                    .Where(x => x.StateFlags.HasFlag(user32.DisplayDeviceStateFlags.MirroringDriver) == false)
                    .ToList();

            if (devices.Any() == false)
            {
                throw new Exception("no primary display device");
            }

            return devices.Single();
        }

        public static user32.DISPLAY_DEVICE GetDisplayDeviceByDeviceString(string deviceString)
        {
            var devices = GetDisplayDevices().Where(x => x.DeviceString == deviceString).ToList();

            if (devices.Any() == false)
            {
                throw new GraphicsDeviceNotFoundException(null, deviceString);
            }

            return devices.Single();
        }

        public static user32.DISPLAY_DEVICE GetDisplayDeviceByDeviceName(string deviceName)
        {
            var devices = GetDisplayDevices().Where(x => x.DeviceName == deviceName).ToList();

            if (devices.Any() == false)
            {
                throw new GraphicsDeviceNotFoundException(deviceName, null);
            }

            return devices.Single();
        }

        public static IEnumerable<user32.DEVMODE> GetDisplaySettings(string deviceName)
        {
            var modeNum = 0;

            var currentDevMode = GetCurrentDisplayMode(deviceName);

            var devModes = new List<user32.DEVMODE>();

            while (true)
            {
                var devMode = new user32.DEVMODE();
                devMode.dmSize = (short)Marshal.SizeOf(typeof(user32.DEVMODE));

                if (user32.EnumDisplaySettings(deviceName, modeNum, ref devMode) == false)
                {
                    break;
                }

                //if (DevModeValid(devMode, currentDevMode))
                //{
                devModes.Add(devMode);
                //}

                modeNum += 1;
            }

            return devModes;
        }

        public static Process CreateUserProcess(string exePath, string[] parameters)
        {
            using (var currentProcess = Process.GetCurrentProcess())
            {
                var userProcess = CreateUserProcess(currentProcess, exePath, parameters);

                return userProcess;
            }
        }

        private static void ResetDisplayFrequency(string deviceName)
        {
            var currentMode = GetCurrentDisplayMode(deviceName);

            var success = false;

            var mode = new user32.DEVMODE();
            mode.dmSize = (short)Marshal.SizeOf(typeof(user32.DEVMODE));
            mode.dmDisplayFrequency = currentMode.dmDisplayFrequency;
            mode.dmFields = user32.DM.DisplayFrequency;

            var result = user32.ChangeDisplaySettingsEx(deviceName, ref mode, IntPtr.Zero, user32.ChangeDisplaySettingsFlags.CDS_TEST, IntPtr.Zero);

            if (result == user32.DISP_CHANGE.Successful)
            {
                result = user32.ChangeDisplaySettingsEx(deviceName, ref mode, IntPtr.Zero, user32.ChangeDisplaySettingsFlags.CDS_NONE, IntPtr.Zero);

                if (result == user32.DISP_CHANGE.Successful)
                {
                    success = true;
                }
            }

            if (success == false)
            {
                Debug.WriteLine($"could not reset display frequency: {result}");
            }
        }

        private static SystemType GetSystemType()
        {
            return GetSystemType(Environment.OSVersion);
        }

        public static bool SupportsDesktopDuplicationAPI()
        {
            return VersionNTIs601 == false;
        }

        public static bool SupportsMirroring()
        {
            return VersionNTIs601 && CurrentSessionIsRemote() == false;
        }

        public static Task HandleUIProcesses(ProcessesAction action, CancellationToken token)
        {
            //ProcessUIProcesses(action, token);
            return HandleUIProcessesNative(action, token);
            //HandleUIProcessesAutomation(action, token);
        }

        public static int GetParentPID(int Id)
        {
            var parentPid = 0;
            using (var mo = new ManagementObject($"win32_process.handle='{Id}'"))
            {
                mo.Get();
                parentPid = Convert.ToInt32(mo["ParentProcessId"]);
            }

            return parentPid;
        }

        public static void MinimizeWindow(IntPtr hWnd)
        {
            if (user32.ShowWindow(hWnd, user32.SW_MINIMIZE) == false)
            {
                throw new Win32Exception();
            }
        }

        private static async Task HandleUIProcessesNative(ProcessesAction action, CancellationToken token)
        {
            var status = Utils.lt_processes_handle_action(action);
            EnsureSuccess(status, nameof(Utils.lt_processes_handle_action));

            await Task.Delay(ClosingMinimizingApplsTimeoutMs, token);
        }

        [Obsolete("use native version")]
        private static async Task ProcessUIProcesses(ProcessesAction action, CancellationToken token)
        {
            if (action == ProcessesAction.NoAction)
            {
                return;
            }

            using (var localTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                Debug.WriteLine("closing processes");

                const string explorerProcessName = "explorer";
                const string leaptestProcessesWildcard = "leaptest";

                var explorer =
                    Process.GetProcesses()
                    .FirstOrDefault(_ => (_.MainWindowHandle != IntPtr.Zero) && (_.ProcessName == explorerProcessName));
                if (explorer != null)
                {
                    ProcessExplorerWindows(explorer, action);
                }

                Process[] processes = null;
                List<Process> toClose;

                var cancellationToken = localTokenSource.Token;
                do
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }

                    try
                    {
                        processes = Process.GetProcesses();
                        toClose =
                            processes.Where(
                                _ =>
                                    (_.ProcessName != explorerProcessName) &&
                                    (_.ProcessName.ToLower().Contains(leaptestProcessesWildcard) == false) &&
                                    (_.MainWindowHandle != IntPtr.Zero))
                              .ToList();
#if DEBUG
                        const string devenvProcessName = "devenv";
                        toClose = toClose.Where(_ => _.ProcessName.ToLower().Contains(devenvProcessName) == false).ToList();
#endif
                        if (action == ProcessesAction.Minimize)
                        {
                            toClose = toClose.Where(ShouldBeMinimized).ToList();
                        }

                        if (cancellationToken.IsCancellationRequested == false)
                        {
                            await ProcessProcesses(action, cancellationToken, toClose);
                        }
                    }
                    finally
                    {
                        if (processes != null)
                        {
                            foreach (var process in processes)
                            {
                                try
                                {
                                    process.Dispose();
                                }
                                catch (Exception exception)
                                {
                                    Debug.WriteLine(exception, "exception during process disposing");
                                }
                            }
                        }
                    }
                } while (toClose.Any());
            }
        }

        private static Process CreateUserProcess(Process currentProcess, string exePath, string[] parameters)
        {
            var procHandle = currentProcess.Handle;

            IntPtr token = IntPtr.Zero;
            IntPtr userToken = IntPtr.Zero;
            var procInfo = new advapi32.PROCESS_INFORMATION();

            Process userProcess;

            try
            {
                if (advapi32.OpenProcessToken(procHandle, advapi32.TOKEN_DUPLICATE, out token) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), $"Error in {nameof(advapi32.OpenProcessToken)}");
                }

                if (advapi32.DuplicateTokenEx(
                    token,
                    advapi32.MAXIMUM_ALLOWED,
                    IntPtr.Zero,
                    advapi32.SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation,
                    advapi32.TOKEN_TYPE.TokenPrimary,
                    ref userToken) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), $"Error in {nameof(advapi32.DuplicateTokenEx)}");
                }

                if (IsServiceSession(currentProcess.SessionId))
                {
                    var sessionId = GetConsoleSessionId();

                    Debug.WriteLine($"session id {sessionId}");

                    if (advapi32.SetTokenInformation(userToken,
                        advapi32.TOKEN_INFORMATION_CLASS.TokenSessionId, ref sessionId, sizeof(uint)) == false)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), $"Error in {nameof(advapi32.SetTokenInformation)}");
                    }

                    uint dwAccess = 1;

                    if (advapi32.SetTokenInformation(userToken, advapi32.TOKEN_INFORMATION_CLASS.TokenUIAccess, ref dwAccess, sizeof(uint)) == false)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(), $"Error2 in {nameof(advapi32.SetTokenInformation)}");
                    }
                }

                var startInfo = new advapi32.STARTUPINFO();
                startInfo.cb = Marshal.SizeOf(startInfo);

                var path = $"{exePath} {ConcatCommandLineArguments(parameters)}";

                Debug.WriteLine($"launching app in an user session: {path}");

                var processCreationFlags =
                    advapi32.CreateProcessFlags.CREATE_UNICODE_ENVIRONMENT
                    | advapi32.CreateProcessFlags.NORMAL_PRIORITY_CLASS
                    | advapi32.CreateProcessFlags.CREATE_NO_WINDOW;

                if (advapi32.CreateProcessAsUser(
                    userToken,
                    null,
                    path,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    true,
                    processCreationFlags,
                    IntPtr.Zero,
                    null,
                    ref startInfo,
                    out procInfo) == false)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), $"Error in CreateProcessAsUser");
                }

                userProcess = Process.GetProcessById((int)procInfo.dwProcessId);

                //Debug.WriteLine($"user app process id {userProcess.Id}. Exited {userProcess.HasExited}");

                userProcess.EnableRaisingEvents = true;
            }
            finally
            {
                if (procInfo.hThread != IntPtr.Zero)
                {
                    if (kernel32.CloseHandle(procInfo.hThread) == false)
                    {
                        Debug.WriteLine("closing process info hThread failed with code " + Marshal.GetLastWin32Error());
                    }
                }

                if (procInfo.hProcess != IntPtr.Zero)
                {
                    if (kernel32.CloseHandle(procInfo.hProcess) == false)
                    {
                        Debug.WriteLine("closing process info hProcess failed with code " + Marshal.GetLastWin32Error());
                    }
                }

                if (userToken != IntPtr.Zero)
                {
                    if (kernel32.CloseHandle(userToken) == false)
                    {
                        Debug.WriteLine("closing user token failed with code " + Marshal.GetLastWin32Error());
                    }
                }

                if (token != IntPtr.Zero)
                {
                    if (kernel32.CloseHandle(token) == false)
                    {
                        Debug.WriteLine("closing process token failed with code " + Marshal.GetLastWin32Error());
                    }
                }
            }

            return userProcess;
        }

        private static bool IsServiceSession(uint sessionId)
        {
            return sessionId == 0;
        }

        private static bool IsServiceSession(int sessionId)
        {
            return IsServiceSession((uint)sessionId);
        }

        private static List<user32.DISPLAY_DEVICE> GetDisplayDevices()
        {
            uint iDevNum = 0;
            var devices = new List<user32.DISPLAY_DEVICE>();

            while (true)
            {
                var device = new user32.DISPLAY_DEVICE();
                device.cb = Marshal.SizeOf(device);

                if (user32.EnumDisplayDevices(null, iDevNum, ref device, 0) == false)
                {
                    break;
                }

                Debug.WriteLine($"enumeration devices: name '{device.DeviceName}'; string {device.DeviceString}; flags: {device.StateFlags}");

                devices.Add(device);

                iDevNum += 1;
            }

            return devices;
        }

        private static async Task ProcessProcesses(ProcessesAction action, CancellationToken token, IList<Process> toProcess)
        {
            foreach (var process in toProcess)
            {
                try
                {
                    Debug.WriteLine($"processing '{process.ProcessName}' process with action '{action}'");
                    SendMessage(process.MainWindowHandle, action);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"can't process process '{process.ProcessName}' because of: {ex.Message}");
                }
            }

            if (action == ProcessesAction.Close)
            {
                await WaitForExit(token, toProcess);
            }
            else
            {
                await WaitForMinimize(token, toProcess);
            }
        }

        public static void SendMessage(IntPtr handle, ProcessesAction action)
        {
            switch (action)
            {
                case ProcessesAction.Close:
                    SendMessage(handle, user32.SC_CLOSE);
                    break;
                case ProcessesAction.Minimize:
                    SendMessage(handle, user32.SC_MINIMIZE);
                    break;
                case ProcessesAction.Restore:
                    SendMessage(handle, user32.SC_RESTORE);
                    break;
            }
        }

        private static void SendMessage(IntPtr pointer, int param)
        {
            user32.SendMessage(pointer, user32.WM_SYSCOMMAND, param, 0);
        }

        private static bool ShouldBeMinimized(Process process)
        {
            return ShouldBeMinimized(process.MainWindowHandle);
        }

        private static bool ShouldBeMinimized(IntPtr pointer)
        {
            var placement = new user32.WindowPlacement();
            user32.GetWindowPlacement(pointer, ref placement);

            var state = placement.showCmd;
            var position = placement.rcNormalPosition;

            return ((state == user32.ShowWindowCommands.Normal) || (state == user32.ShowWindowCommands.Maximize)) && (position.Width != 0) && (position.Height != 0);
        }

        private static async Task WaitForExit(CancellationToken token, IList<Process> toCheck, int timeToWait = 100, int waitTimeout = 3000)
        {
            var waited = 0;
            while (waited <= waitTimeout && toCheck.Any(_ => _.HasExited == false))
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                waited += timeToWait;
                await Task.Delay(timeToWait);
            }
        }

        private static async Task WaitForMinimize(CancellationToken token, IList<Process> toCheck, int timeToWait = 100, int waitTimeout = 1000)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var i = 0; i < toCheck.Count; i++)
            {
                if ((token.IsCancellationRequested) || (stopwatch.ElapsedMilliseconds > waitTimeout))
                {
                    return;
                }

                if (ShouldBeMinimized(toCheck[i]) == false)
                {
                    continue;
                }

                i--;
                await Task.Delay(timeToWait);
            }
        }

        private static void ProcessExplorerWindows(Process explorerProcess, ProcessesAction action)
        {
            var shell = user32.GetShellWindow();
            var desktop = user32.GetDesktopWindow();

            var windows7Button = user32.FindWindowEx(desktop, IntPtr.Zero, "Button", null);

            foreach (ProcessThread processThread in explorerProcess.Threads)
            {
                user32.EnumThreadWindows(processThread.Id,
                 (hWnd, lParam) =>
                 {
                     //Skip desktop
                     if (explorerProcess.MainWindowHandle == hWnd)
                     {
                         return true;
                     }

                     if (hWnd == desktop)
                     {
                         return true;
                     }

                     if (hWnd == shell)
                     {
                         return true;
                     }

                     if (VersionNTIs601 && windows7Button == hWnd)
                     {
                         return true;
                     }

                     //Check if Window is Visible or not.
                     if (!user32.IsWindowVisible((int)hWnd))
                     {
                         return true;
                     }

                     //Get the Window's Title.
                     var title = new StringBuilder(256);
                     user32.GetWindowText((int)hWnd, title, 256);

                     //Check if Window has Title.
                     if (title.Length == 0)
                     {
                         return true;
                     }

                     SendMessage(hWnd, action);
                     //user32.SendMessage(hWnd, user32.WM_SYSCOMMAND, user32.SC_CLOSE, 0);

                     return true;
                 }, IntPtr.Zero);
            }
        }

        private static uint GetConsoleSessionId()
        {
            var attempt = 0;
            var maxAttempts = 20;
            var timeToWait = 50;

            do
            {
                var pSessionInfo = IntPtr.Zero;
                var sessionCount = 0u;

                var consoleSessions = new List<wtsapi32.WTS_SESSION_INFO>();

                if (wtsapi32.WTSEnumerateSessions(wtsapi32.WTS_CURRENT_SERVER_HANDLE, 0, 1, ref pSessionInfo, ref sessionCount) != 0)
                {
                    try
                    {
                        var arrayElementSize = Marshal.SizeOf(typeof(wtsapi32.WTS_SESSION_INFO));
                        var current = pSessionInfo;

                        for (var index = 0; index < sessionCount; index++)
                        {
                            var session = (wtsapi32.WTS_SESSION_INFO)Marshal.PtrToStructure(current, typeof(wtsapi32.WTS_SESSION_INFO));
                            current += arrayElementSize;

                            if ((session.pWinStationName ?? "").ToLower() == "console")
                            {
                                consoleSessions.Add(session);
                            }
                        }
                    }
                    finally
                    {
                        wtsapi32.WTSFreeMemory(pSessionInfo);
                        pSessionInfo = IntPtr.Zero;
                    }
                }
                else
                {
                    Debug.WriteLine($"error ({Marshal.GetLastWin32Error()}) during enumerating wts sessions");
                }

                LogConsoleSessions(consoleSessions);

                var sessionId = kernel32.WTSGetActiveConsoleSessionId();
                var consoleSessionIsValid =
                    consoleSessions
                        .Where(x => x.SessionID == sessionId)
                        .Where(x => _consoleSessionInvalidStates.Contains(x.State) == false)
                        .Any();

                if (sessionId != kernel32.INVALID_SESSION_ID && consoleSessionIsValid)
                {
                    return sessionId;
                }

                attempt += 1;

                if (attempt == maxAttempts)
                {
                    throw new Win32Exception($"no active console session id");
                }

                Thread.Sleep(timeToWait);

            } while (true);
        }

        private static void LogConsoleSessions(IEnumerable<wtsapi32.WTS_SESSION_INFO> consoleSessions)
        {
            if (consoleSessions.Any())
            {
                foreach (var session in consoleSessions)
                {
                    Debug.WriteLine($"console session {session.SessionID}; state {session.State}");
                }
            }
            else
            {
                Debug.WriteLine($"no console sessions");
            }
        }

        private static string ConcatCommandLineArguments(string[] arguments)
        {
            var concat = string.Join(" ", arguments.Select(x => StringHelper.QuoteIfNecessary(x)));

            return concat;
        }

        private static SystemType GetSystemType(OperatingSystem os)
        {
            switch (os.Platform)
            {
                case PlatformID.Win32NT:
                    return Win32NT(os);
                default:
                    return NotSupported(os);
            }
        }

        private static SystemType Win32NT(OperatingSystem os)
        {
            switch (os.Version.Major)
            {
                case 6:
                    return Win32NT_6(os);
                case 10:
                    return Win32NT_10(os);
                default:
                    return NotSupported(os);
            }
        }

        private static SystemType Win32NT_10(OperatingSystem os)
        {
            var isServer = shlwapi.IsOS(shlwapi.OSType.AnyServer);

            switch (os.Version.Minor)
            {
                case 0:
                    return isServer ? SystemType.WindowsServer2016 : SystemType.Windows10;
                default:
                    return NotSupported(os);
            }
        }

        private static SystemType Win32NT_6(OperatingSystem os)
        {
            var isServer = shlwapi.IsOS(shlwapi.OSType.AnyServer);

            switch (os.Version.Minor)
            {
                case 1:
                    return isServer ? SystemType.WindowsServer2008R2 : SystemType.Windows7;
                case 2:
                    return isServer ? SystemType.WindowsServer2012 : SystemType.Windows8;
                case 3:
                    return isServer ? SystemType.WindowsServer2012R2 : SystemType.Windows8_1;
                default:
                    return NotSupported(os);
            }
        }

        private static SystemType NotSupported(OperatingSystem os)
        {
            return SystemType.NotSupported;
        }

        private static void PrepareWinStation()
        {
            var winStationName = "WinSta0";

            Debug.WriteLine($@"opening windows station ""{winStationName}""");

            var hWinStation = user32.OpenWindowStation(winStationName, false, advapi32.MAXIMUM_ALLOWED);
            if (hWinStation == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Exception($"Could not window station . Error code {errorCode}");
            }

            Debug.WriteLine("setting process station");

            if (!user32.SetProcessWindowStation(hWinStation))
            {
                user32.CloseWindowStation(hWinStation);
                var errorCode = Marshal.GetLastWin32Error();
                throw new Exception($"Could not set process window station. Error code {errorCode}");
            }

            user32.CloseWindowStation(hWinStation);
        }

        private static void Die()
        {
            Environment.Exit(0);
        }

        private static user32.DEVMODE GetCurrentDisplayMode(string deviceName)
        {
            var displayDevice = GetDisplayDeviceByDeviceName(deviceName);

            return GetCurrentDisplayMode(displayDevice);
        }

        private static user32.DEVMODE GetCurrentDisplayMode(user32.DISPLAY_DEVICE device)
        {
            var devMode = new user32.DEVMODE();
            devMode.dmSize = (short)Marshal.SizeOf(typeof(user32.DEVMODE));

            if (user32.EnumDisplaySettings(device.DeviceName, user32.ENUM_CURRENT_SETTINGS, ref devMode) == false)
            {
                throw new Exception("can't get current display settings");
            }

            return devMode;
        }

        public static void EnsureSuccess(Utils.LT_STATUS status, string message)
        {
            switch (GetErrorType(status))
            {
                case ErrorType.NoError: return;
                case ErrorType.Fatal:
                    Die();
                    return;
                default:
                    throw new NativeException(status, message);
            }
        }

        public static ErrorType GetErrorType(Utils.LT_STATUS error)
        {
            if (error == Utils.LT_STATUS.LT_OK)
            {
                return ErrorType.NoError;
            }

            if (error < 0)
            {
                return ErrorType.Fatal;
            }

            return ErrorType.Normal;
        }

        private static bool DevModeValid(user32.DEVMODE mode, user32.DEVMODE currentMode)
        {
            //TCHAR dmDeviceName[CCHDEVICENAME];
            //WORD dmSpecVersion;
            //WORD dmDriverVersion;
            //WORD dmSize;
            //WORD dmDriverExtra;
            //DWORD dmFields;
            //POINTL dmPosition;

            //DWORD dmPelsWidth;
            //DWORD dmPelsHeight;

            return
                mode.dmDisplayOrientation == currentMode.dmDisplayOrientation
                && mode.dmDisplayFixedOutput == currentMode.dmDisplayFixedOutput
                && mode.dmLogPixels == currentMode.dmLogPixels
                && mode.dmBitsPerPel == currentMode.dmBitsPerPel
                && mode.dmDisplayFlags == currentMode.dmDisplayFlags
                && mode.dmDisplayFrequency == currentMode.dmDisplayFrequency;
        }

        public sealed class GetDisplayDeviceByNumberResult
        {
            public bool Found { get; }
            public user32.DISPLAY_DEVICE Device { get; }

            public GetDisplayDeviceByNumberResult(user32.DISPLAY_DEVICE device, bool found)
            {
                Found = found;
                Device = device;
            }
        }

        public sealed class FindMonitorResult
        {
            public bool Found { get; }
            public user32.MONITORINFOEX Monitor { get; }

            public FindMonitorResult(user32.MONITORINFOEX monitor, bool found)
            {
                Found = found;
                Monitor = monitor;
            }
        }

        private sealed class ImpersonationUndo : IDisposable
        {
            private readonly WindowsImpersonationContext _context;
            private readonly WindowsIdentity _identity;

            public ImpersonationUndo(WindowsIdentity identity, WindowsImpersonationContext context)
            {
                _identity = identity;
                _context = context;
            }

            public void Dispose()
            {
                _context.Undo();
                _context.Dispose();
                _identity.Dispose();
            }
        }

        public enum ErrorType
        {
            NoError,
            Normal,
            Fatal
        }

        public static void SetForegroundWindow(IntPtr windowHandle)
        {
            user32.SetForegroundWindow(windowHandle);
        }

        public static void ShowdWindow(IntPtr windowHandle)
        {
            var info = new user32.WindowPlacement();
            user32.GetWindowPlacement(windowHandle, ref info);
            if (info.showCmd == user32.ShowWindowCommands.Minimized)
            {
                user32.ShowWindow(windowHandle, user32.SW_RESTORE);
            }
        }
    }
}

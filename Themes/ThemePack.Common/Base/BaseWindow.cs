using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using ThemePack.Common.Base.Abstractions;
using ThemePack.Common.BusinessModels;
using ThemePack.Common.Interop;

namespace ThemePack.Common.Base
{
    /// <summary>
    /// Base for ViewModels.
    /// </summary>
    /// <date>17:05 05/14/2015</date>
    /// <author>Anton Liakhovich</author>
    public class BaseWindow : DpiAwareWindow
    {
        /// <summary>
        /// <see cref="WindowSizePositionManager"/> of current <see cref="BaseWindow"/> instance.
        /// </summary>
        private WindowSizePositionManager sizePositionManager;

        /// <summary>
        /// Initializes a new instance of <see cref="BaseWindow"/>.
        /// </summary>
        public BaseWindow() : this(false)
        {
            AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnError));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseWindow"/>.
        /// </summary>
        public BaseWindow(bool isPerMonitorEnabled) : base(isPerMonitorEnabled)
        {
            DataContextChanged += OnDataContextChanged;
            Loaded += OnWindowLoaded;
            Unloaded += OnWindowUnloaded;
        }

        /// <summary>
        /// Current <see cref="BaseWindow"/>'s <see cref="FrameworkElement.DataContextChanged"/> event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var requestClose = e.OldValue as IRequestCloseViewModel;
            if (requestClose != null)
            {
                requestClose.RequestClose -= OnRequestClose;
            }
            var requestActivate = e.OldValue as IRequestActivateViewModel;
            if (requestActivate != null)
            {
                requestActivate.RequestActivate -= OnRequestActivate;
            }

            requestClose = e.NewValue as IRequestCloseViewModel;
            if (requestClose != null)
            {
                requestClose.RequestClose += OnRequestClose;
            }
            requestActivate = e.NewValue as IRequestActivateViewModel;
            if (requestActivate != null)
            {
                requestActivate.RequestActivate += OnRequestActivate;
            }
        }

        /// <summary>
        /// Current <see cref="BaseWindow"/>'s <see cref="FrameworkElement.Loaded"/> event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            sizePositionManager = new WindowSizePositionManager(this);
        }

        /// <summary>
        /// Current <see cref="BaseWindow"/>'s <see cref="FrameworkElement.Unloaded"/> event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        private void OnWindowUnloaded(object sender, RoutedEventArgs e)
        {
            RemoveHandler(Validation.ErrorEvent, new RoutedEventHandler(OnError));
        }

        /// <summary>
        /// <see cref="IRequestActivateViewModel"/>'s <see cref="IRequestActivateViewModel.RequestActivate"/> event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        private void OnRequestActivate(object sender, EventArgs e)
        {
            Invoke(() => Activate(), "window activating");
        }

        /// <summary>
        /// <see cref="IRequestCloseViewModel"/>'s <see cref="IRequestCloseViewModel.RequestClose"/> event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        private void OnRequestClose(object sender, DataEventArgs<bool?> e)
        {
            Invoke(() => CloseWindow(e.Data), "window closing");
        }

        /// <summary>
        /// Close current <see cref="BaseWindow"/>.
        /// </summary>
        /// <param name="dialogResult">dialog result value.</param>
        private void CloseWindow(bool? dialogResult)
        {
            if (Owner == null)
            {
                Close();
            }
            try
            {
                if (DialogResult == null)
                {
                    DialogResult = dialogResult;
                }
            }
            catch (Exception)
            {
                Close();
            }
        }

        /// <summary>
        /// Invoke <paramref name="toExecute"/> respectfully to Dispatcher.
        /// </summary>
        /// <param name="toExecute"><see cref="Action"/> to execute.</param>
        /// <param name="error">error message if execution failed.</param>
        private void Invoke(Action toExecute, string error)
        {
            if (Dispatcher.CheckAccess())
            {
                InvokeSafe(toExecute, error);
            }
            else
            {
                Dispatcher.BeginInvoke((Action)(() => InvokeSafe(toExecute, error)));
            }
        }

        /// <summary>
        /// Invoke <paramref name="toExecute"/> safely.
        /// </summary>
        /// <param name="toExecute"><see cref="Action"/> to execute.</param>
        /// <param name="error">error message if execution failed.</param>
        private static void InvokeSafe(Action toExecute, string error)
        {
            try
            {
                toExecute();
            }
            catch (Exception ex)
            {
                //Logger.Error(ex, $"Error occurred during {error}.");
            }
        }

        /// <summary>
        /// <see cref="Validation.ErrorEvent"/> handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="e">event arguments.</param>
        private void OnError(object sender, RoutedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        /// <summary>
        /// Stick current <see cref="BaseWindow"/> to cursor.
        /// </summary>
        public void StickToCursor()
        {
            sizePositionManager.StickToCursor();
        }

        protected virtual void OnTitleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        protected virtual void OnTitleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        #region Refactor!!!

        protected override IntPtr WindowProcedureHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var result = base.WindowProcedureHook(hwnd, msg, wParam, lParam, ref handled);
            if (handled)
            {
                return result;
            }


            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;

                case 0x0046:
                    {
                        WINDOWPOS pos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
                        if ((pos.flags & (int)SWP.NOMOVE) != 0)
                        {
                            return IntPtr.Zero;
                        }

                        Window wnd = (Window)HwndSource.FromHwnd(hwnd).RootVisual;
                        if (wnd == null)
                        {
                            return IntPtr.Zero;
                        }

                        bool changedPos = false;

                        // ***********************
                        // Here you check the values inside the pos structure
                        // if you want to override them just change the pos
                        // structure and set changedPos to true
                        // ***********************

                        // this is a simplified version that doesn't work in high-dpi settings
                        // pos.cx and pos.cy are in "device pixels" and MinWidth and MinHeight 
                        // are in "WPF pixels" (WPF pixels are always 1/96 of an inch - if your
                        // system is configured correctly).
                        if (pos.cx < wnd.MinWidth) { pos.cx = (int)wnd.MinWidth; changedPos = true; }
                        if (pos.cy < wnd.MinHeight) { pos.cy = (int)wnd.MinHeight; changedPos = true; }


                        // ***********************
                        // end of "logic"
                        // ***********************

                        if (!changedPos)
                        {
                            return IntPtr.Zero;
                        }

                        Marshal.StructureToPtr(pos, lParam, true);
                        handled = true;
                    }
                    break;
            }

            return IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        public static class SWP
        {
            public static readonly int NOMOVE = 0x0002;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            IntPtr lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
            var lPrimaryScreenInfo = new user32.MONITORINFOEX();
            if (user32.GetMonitorInfo(lPrimaryScreen, ref lPrimaryScreenInfo) == false)
            {
                return;
            }

            IntPtr lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);

            MINMAXINFO lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            if (lPrimaryScreen.Equals(lCurrentScreen) == true)
            {
                var work = lPrimaryScreenInfo.WorkArea;
                lMmi.ptMaxPosition.X = work.Left;
                lMmi.ptMaxPosition.Y = work.Top;
                lMmi.ptMaxSize.X = work.Right - work.Left;
                lMmi.ptMaxSize.Y = work.Bottom - work.Top;
            }
            else
            {
                var monitor = lPrimaryScreenInfo.Monitor;
                lMmi.ptMaxPosition.X = monitor.Left;
                lMmi.ptMaxPosition.Y = monitor.Top;
                lMmi.ptMaxSize.X = monitor.Right - monitor.Left;
                lMmi.ptMaxSize.Y = monitor.Bottom - monitor.Top;
            }

            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        #endregion
    }
}

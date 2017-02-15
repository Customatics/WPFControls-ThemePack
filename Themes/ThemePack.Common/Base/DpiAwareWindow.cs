using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using ThemePack.Common.Helpers;
using ThemePack.Common.Interop;

namespace ThemePack.Common.Base
{
    /// <summary>
    /// Base class for DPI aware <see cref="Window"/>.
    /// </summary>
    /// <date>10:39 09/07/2016</date>
    /// <author>Anton Liakhovich</author>
    public class DpiAwareWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DpiAwareWindow"/>.
        /// </summary>
        protected DpiAwareWindow(bool isPerMonitorEnabled)
        {
            IsPerMonitorEnabled = isPerMonitorEnabled;
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Flag if current <see cref="DpiAwareWindow"/> is configured to work in DPI per Monitor environemnt.
        /// </summary>
        protected bool IsPerMonitorEnabled { get; private set; }

        /// <summary>
        /// The DPI used by WPF.
        /// </summary>
        protected DPI WpfDpi { get; private set; }

        /// <summary>
        /// DPI of the monitor of the window.
        /// </summary>
        public virtual DPI CurrentDpi { get; set; }

        /// <summary>
        /// DPI of the monitor of the window.
        /// </summary>
        protected DPI SystemDpi { get; private set; }

        /// <summary>
        /// The scale factor used to modify window size, graphics and text.
        /// </summary>
        protected virtual DPI ScaleFactor { get; private set; }

        /// <summary>
        /// Actual <see cref="DPI"/>.
        /// </summary>
        protected DPI ActualDpi => IsPerMonitorEnabled ? CurrentDpi : SystemDpi;

        /// <summary>
        /// Initial <see cref="DpiAwareWindow"/> size.
        /// </summary>
        protected Size? InitialSize;

        /// <summary>
        /// Initial <see cref="DpiAwareWindow"/> location.
        /// </summary>
        protected Point? InitialLocation;

        /// <summary>
        /// <see cref="FrameworkElement.Loaded"/> event handler.
        /// </summary>
        /// <param name="sender">event sender.</param>
        /// <param name="args">event arguments.</param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs args)
        {
            // WPF has already scaled window size, graphics and text based on system DPI. In order to scale the window based on monitor DPI, update the 
            // window size, graphics and text based on monitor DPI. For example consider an application with size 600 x 400 in device independent pixels
            //		- Size in device independent pixels = 600 x 400 
            //		- Size calculated by WPF based on system/WPF DPI = 192 (scale factor = 2)
            //		- Expected size based on monitor DPI = 144 (scale factor = 1.5)

            // Similarly the graphics and text are updated updated by applying appropriate scale transform to the top level node of the WPF application

            // Important Note: This method overwrites the size of the window and the scale transform of the root node of the WPF Window. Hence, 
            // this sample may not work "as is" if 
            //	- The size of the window impacts other portions of the application like this WPF  Window being hosted inside another application. 
            //  - The WPF application that is extending this class is setting some other transform on the root visual; the sample may 
            //     overwrite some other transform that is being applied by the WPF application itself.

            SystemDpi = DpiHelper.GetSystemDpi();
            var source = (HwndSource)PresentationSource.FromVisual(this);
            source?.AddHook(WindowProcedureHook);

            // Calculate the DPI used by WPF.
            var transform = source?.CompositionTarget?.TransformToDevice;
            WpfDpi = new DPI(96 * (transform?.M11 ?? 1), 96 * (transform?.M22 ?? 1));

            if (IsPerMonitorEnabled && (source != null))
            {
                // Get the Current DPI of the monitor of the window.
                CurrentDpi = DpiHelper.GetDpiForHwnd(source.Handle);

                // Calculate the scale factor used to modify window size, graphics and text.
                ScaleFactor = new DPI(CurrentDpi.X / WpfDpi.X, CurrentDpi.Y / WpfDpi.Y);
            }

            UpdateWindowSize();
            UpdateLocation();

            // Update graphics and text based on the current DPI of the monitor.
            UpdateLayoutTransform(ScaleFactor);
            OnDpiChanged();
        }

        /// <summary>
        /// Update <see cref="DpiAwareWindow"/>'s size.
        /// </summary>
        protected void UpdateWindowSize()
        {
            if (InitialSize.HasValue)
            {
                Width = InitialSize.Value.Width / (WpfDpi.X / 96); //InitialSize.Value.Width * (InitialSize.Value.Width / Width) / (WpfDpi.X / 96); //* ScaleFactor.X; //InitialWidth.Value * (WpfDpi.X / 96) * ScaleFactor.X;
                Height = InitialSize.Value.Height / (WpfDpi.Y / 96); //InitialSize.Value.Height * (InitialSize.Value.Height / Height) / (WpfDpi.X / 96);// * ScaleFactor.Y;// InitialHeight.Value * (WpfDpi.Y / 96) * ScaleFactor.Y;
                return;
            }

            if (IsPerMonitorEnabled == false)
            {
                return;
            }
            Width = Width * ScaleFactor.X;
            Height = Height * ScaleFactor.Y;
        }

        /// <summary>
        /// Update <see cref="DpiAwareWindow"/>'s location.
        /// </summary>
        protected void UpdateLocation()
        {
            if (InitialLocation.HasValue)
            {
                Left = InitialLocation.Value.X / (WpfDpi.X / 96);  //* (InitialLocation.Value.X / Left) / (WpfDpi.X / 96); //ScaleFactor.X;
                Top = InitialLocation.Value.Y / (WpfDpi.X / 96);   //* (InitialLocation.Value.Y / Top) / (WpfDpi.X / 96);//ScaleFactor.Y;
                return;
            }

            if (IsPerMonitorEnabled == false)
            {
                return;
            }
            Left = Left * ScaleFactor.X;
            Top = Top * ScaleFactor.Y;
        }

        /// <summary>
        /// Message handler of the Per_Monitor_DPI_Aware window. The handles the WM_DPICHANGED message and adjusts window size, graphics and text
	    /// based on the DPI of the monitor. The window message provides the new window size (lparam) and new DPI (wparam)
        /// </summary>
        protected virtual IntPtr WindowProcedureHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((user32.WinMessages)msg)
            {
                case user32.WinMessages.WM_DPICHANGED:

                    if (IsPerMonitorEnabled)
                    {
                        user32.RECT newRect = (user32.RECT)Marshal.PtrToStructure(lParam, typeof(user32.RECT));
                        user32.SetWindowPos(hwnd, 0, newRect.Left, newRect.Top, newRect.Right - newRect.Left,
                            newRect.Bottom - newRect.Top,
                            user32.SWP_NOZORDER | user32.SWP_NOOWNERZORDER | user32.SWP_NOACTIVATE);

                        // Set the Window's position & size.
                        //Vector ul = source.CompositionTarget.TransformFromDevice.Transform(new Vector(newRect.Left, newRect.Top));
                        //Vector hw = source.CompositionTarget.TransformFromDevice.Transform(new Vector(newRect.Right = newRect.Left, newRect.Bottom - newRect.Top));
                        //Left = ul.X;
                        //Top = ul.Y;
                        //Width = hw.X;
                        //Height = hw.Y;

                        // Remember the current DPI settings.
                        var oldDpi = CurrentDpi;

                        // Get the new DPI settings from wParam
                        CurrentDpi = new DPI(wParam.ToInt32() >> 16, wParam.ToInt32() & 0x0000FFFF);
                        if ((oldDpi.X != CurrentDpi.X) || (oldDpi.Y != CurrentDpi.Y))
                        {
                            OnDpiChangedInteranl();
                        }

                        handled = true;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Process changing DPI value.
        /// </summary>
        protected virtual void OnDpiChanged()
        {
        }

        private void OnDpiChangedInteranl()
        {
            ScaleFactor = new DPI(CurrentDpi.X / WpfDpi.X, CurrentDpi.Y / WpfDpi.Y);
            UpdateLayoutTransform(ScaleFactor);

            OnDpiChanged();
        }

        /// <summary>
        /// Uodate <see cref="Window.LayoutTransform"/> of the current <see cref="DpiAwareWindow"/>.
        /// </summary>
        /// <param name="scaleFactor"><see cref="DPI"/> representing scale factor.</param>
        private void UpdateLayoutTransform(DPI scaleFactor)
        {
            if (IsPerMonitorEnabled == false)
            {
                return;
            }

            var child = GetVisualChild(0);
            if ((scaleFactor.X != 1.0) || (scaleFactor.Y != 1.0))
            {
                child?.SetValue(LayoutTransformProperty, new ScaleTransform(scaleFactor.X, scaleFactor.Y));
            }
            else
            {
                child?.SetValue(LayoutTransformProperty, null);
            }
        }
    }
}

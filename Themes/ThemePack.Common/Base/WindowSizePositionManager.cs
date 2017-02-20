using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using ThemePack.Common.Helpers;

namespace ThemePack.Common.Base
{
	public class WindowSizePositionManager
	{
		private Window _window = null;

		private bool resizeRight = false;
		private bool resizeLeft = false;
		private bool resizeUp = false;
		private bool resizeDown = false;

		private Dictionary<UIElement, short> leftElements = new Dictionary<UIElement, short>();
		private Dictionary<UIElement, short> rightElements = new Dictionary<UIElement, short>();
		private Dictionary<UIElement, short> upElements = new Dictionary<UIElement, short>();
		private Dictionary<UIElement, short> downElements = new Dictionary<UIElement, short>();

		private PointAPI resizePoint = new PointAPI();
		private Size resizeSize = new Size();
		private Point resizeWindowPoint = new Point();

		private delegate void RefreshDelegate();

		public WindowSizePositionManager(Window window)
		{
			this._window = window;


            this.addResizerRight((Rectangle)window.Template.FindName("rightSizeGrip", window));
            this.addResizerLeft((Rectangle)window.Template.FindName("leftSizeGrip", window));
            this.addResizerUp((Rectangle)window.Template.FindName("topSizeGrip", window));
            this.addResizerDown((Rectangle)window.Template.FindName("bottomSizeGrip", window));
            this.addResizerLeftUp((Rectangle)window.Template.FindName("topLeftSizeGrip", window));
            this.addResizerRightUp((Rectangle)window.Template.FindName("topRightSizeGrip", window));
            this.addResizerLeftDown((Rectangle)window.Template.FindName("bottomLeftSizeGrip", window));
            this.addResizerRightDown((Rectangle)window.Template.FindName("bottomRightSizeGrip", window));

            if (window == null)
			{
				throw new Exception("Invalid Window handle");
			}
		}

		#region add resize components
		private void connectMouseHandlers(UIElement element)
		{
			if ( element == null )
				return;

			element.MouseLeftButtonDown += new MouseButtonEventHandler(element_MouseLeftButtonDown);
			element.MouseLeftButtonUp += new MouseButtonEventHandler(element_MouseLeftButtonUp);
			element.MouseEnter += new MouseEventHandler(element_MouseEnter);
			element.MouseLeave += new MouseEventHandler(setArrowCursor);
		}

		public void addResizerRight(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			rightElements.Add(element, 0);
		}

		public void addResizerLeft(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			leftElements.Add(element, 0);
		}

		public void addResizerUp(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			upElements.Add(element, 0);
		}

		public void addResizerDown(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			downElements.Add(element, 0);
		}

		public void addResizerRightDown(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			rightElements.Add(element, 0);
			downElements.Add(element, 0);
		}

		public void addResizerLeftDown(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			leftElements.Add(element, 0);
			downElements.Add(element, 0);
		}

		public void addResizerRightUp(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			rightElements.Add(element, 0);
			upElements.Add(element, 0);
		}

		public void addResizerLeftUp(UIElement element)
		{
			if ( element == null )
				return;

			connectMouseHandlers(element);
			leftElements.Add(element, 0);
			upElements.Add(element, 0);
		}
		#endregion

		#region resize handlers
		private void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			(sender as UIElement).CaptureMouse();

			GetCursorPos(out resizePoint);
			resizeSize = new Size(_window.Width, _window.Height);
			resizeWindowPoint = new Point(_window.Left, _window.Top);

			#region updateResizeDirection
			UIElement sourceSender = (UIElement)sender;
			if (leftElements.ContainsKey(sourceSender))
			{
				resizeLeft = true;
			}
			if (rightElements.ContainsKey(sourceSender))
			{
				resizeRight = true;
			}
			if (upElements.ContainsKey(sourceSender))
			{
				resizeUp = true;
			}
			if (downElements.ContainsKey(sourceSender))
			{
				resizeDown = true;
			}
			#endregion

			Thread t = new Thread(new ThreadStart(updateSizeLoop));
			t.Name = "Mouse Position Poll Thread";
			t.Start();
		}

		void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			(sender as UIElement).ReleaseMouseCapture();
		}

		private void updateSizeLoop()
		{
			try
			{
				while (resizeDown || resizeLeft || resizeRight || resizeUp)
				{
					_window.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new RefreshDelegate(updateSize));
					_window.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new RefreshDelegate(updateMouseDown));
				    DelayHelper.DelayMilliseconds(10);
				}

				_window.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new RefreshDelegate(setArrowCursor));
			}
			catch (Exception)
			{
			}
		}

		#region updates
		private void updateSize()
		{
			PointAPI p = new PointAPI();
			GetCursorPos(out p);
            _window.SizeToContent = SizeToContent.Manual;
            var deltaX = resizePoint.X - p.X;
            var deltaY = resizePoint.Y - p.Y;

            if (resizeRight)
			{
				_window.Width = Math.Max(0, this.resizeSize.Width - deltaX);
			}

			if (resizeDown)
			{
				_window.Height = Math.Max(0, resizeSize.Height - deltaY);
			}

			if (resizeLeft)
			{
                var dw = Math.Max(_window.MinWidth, resizeSize.Width + deltaX);
                var shiftX = dw - resizeSize.Width;
                //_window.Left = Math.Max(0, resizeWindowPoint.X  - shiftX);
                //_window.Width = dw;
                //_window.Left = resizeWindowPoint.X - shiftX;
                var hwndSource = PresentationSource.FromVisual(_window) as HwndSource;
                var source = PresentationSource.FromVisual(_window);
                Matrix transformToDevice = source.CompositionTarget.TransformToDevice;
                Point[] point = new Point[] { new Point(resizeWindowPoint.X - shiftX, _window.Top), new Point(dw, _window.Height) };
                transformToDevice.Transform(point);
                SetWindowPos(hwndSource.Handle, IntPtr.Zero, Convert.ToInt32(point[0].X), Convert.ToInt32(point[0].Y), Convert.ToInt32(point[1].X), Convert.ToInt32(point[1].Y), (uint)SWP.SHOWWINDOW);
            }

			if (resizeUp)
			{
                var dh = Math.Max(_window.MinHeight, resizeSize.Height + deltaY);
                var shiftY = dh - resizeSize.Height;
                //_window.Top = Math.Max(0, resizeWindowPoint.Y - shiftY);
                _window.Height = dh;
                _window.Top = resizeWindowPoint.Y - shiftY;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        /// <summary>
        /// SetWindowPos Flags
        /// </summary>
        public static class SWP
        {
            public static readonly int
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            DRAWFRAME = 0x0020,
            FRAMECHANGED = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000;
        }
        private void updateMouseDown()
		{
			if (Mouse.LeftButton == MouseButtonState.Released)
			{
				resizeRight = false;
				resizeLeft = false;
				resizeUp = false;
				resizeDown = false;
			}
		}

		#endregion
		#endregion

		#region cursor updates
		private void element_MouseEnter(object sender, MouseEventArgs e)
		{
			bool resizeRight = false;
			bool resizeLeft = false;
			bool resizeUp = false;
			bool resizeDown = false;

			var window = (Window)((FrameworkElement)sender).TemplatedParent;
			if ( window != null && window.WindowState == WindowState.Maximized)
			{
				return;
			}

			UIElement sourceSender = (UIElement)sender;

			if (leftElements.ContainsKey(sourceSender))
			{
				resizeLeft = true;
			}
			if (rightElements.ContainsKey(sourceSender))
			{
				resizeRight = true;
			}
			if (upElements.ContainsKey(sourceSender))
			{
				resizeUp = true;
			}
			if (downElements.ContainsKey(sourceSender))
			{
				resizeDown = true;
			}

			if ((resizeLeft && resizeDown) || (resizeRight && resizeUp))
			{
				setNESWCursor(sender, e);
			}
			else if ((resizeRight && resizeDown) || (resizeLeft && resizeUp))
			{
				setNWSECursor(sender, e);
			}
			else if (resizeLeft || resizeRight)
			{
				setWECursor(sender, e);
			}
			else if (resizeUp || resizeDown)
			{
				setNSCursor(sender, e);
			}
		}

		private void setWECursor(object sender, MouseEventArgs e)
		{
			_window.Cursor = Cursors.SizeWE;
		}

		private void setNSCursor(object sender, MouseEventArgs e)
		{
			_window.Cursor = Cursors.SizeNS;
		}

		private void setNESWCursor(object sender, MouseEventArgs e)
		{
			_window.Cursor = Cursors.SizeNESW;
		}

		private void setNWSECursor(object sender, MouseEventArgs e)
		{
			_window.Cursor = Cursors.SizeNWSE;
		}

		private void setArrowCursor(object sender, MouseEventArgs e)
		{
			if (!resizeDown && !resizeLeft && !resizeRight && !resizeUp)
			{
				_window.Cursor = Cursors.Arrow;
			}
		}

		private void setArrowCursor()
		{
			_window.Cursor = Cursors.Arrow;
		}
		#endregion

		#region external call
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetCursorPos(out PointAPI lpPoint);

        struct PointAPI
        {
            public int X;
            public int Y;
        }
        #endregion

        public void StickToCursor()
	    {
            var pos = new PointAPI();
            GetCursorPos(out pos);
            _window.Left = pos.X - _window.Width / 2;
            _window.Top = pos.Y - 5;
        }
	}
}

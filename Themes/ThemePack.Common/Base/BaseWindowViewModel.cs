using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using ThemePack.Common.Base.Command;
using ThemePack.Common.Messages;
using ThemePack.Common.Services;

namespace ThemePack.Common.Base
{
    public class BaseWindowViewModel : BaseViewModel
    {
        #region Property

        public virtual string WindowTitle { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Field for <see cref="RequestCloseCommand"/>.
        /// </summary>
        private RelayCommand requestCloseCommand;

        /// <summary>
        /// Field for <see cref="MaximizeCommand"/>.
        /// </summary>
        private RelayCommand<Window> maximizeCommand;

        /// <summary>
        /// Field for <see cref="MinimizeCommand"/>.
        /// </summary>
        private RelayCommand<Window> minimizeCommand;

        /// <summary>
        /// Field for <see cref="TitleMouseLeftButtonDownCommand"/>.
        /// </summary>
        private RelayCommand<MouseButtonEventArgs> titleMouseLeftButtonDownCommand;

        /// <summary>
        /// Field for <see cref="TitleMouseLeftButtonUpCommand"/>.
        /// </summary>
        private RelayCommand<MouseButtonEventArgs> titleMouseLeftButtonUpCommand;

        /// <summary>
        /// Field for <see cref="WindowLoadedCommand"/>.
        /// </summary>
        private RelayCommand loadedCommand;

        /// <summary>
        /// Field for <see cref="WindowUnloadedCommand"/>.
        /// </summary>
        private RelayCommand unloadedCommand;

        /// <summary>
        /// <see cref="ICommand"/> for close window.
        /// </summary>
        public virtual ICommand RequestCloseCommand => RelayCommand.CreateCommand(ref requestCloseCommand, OnRequestCloseCommandExecute, RequestCloseCommandCanExecute);

        /// <summary>
        /// <see cref="ICommand"/> for maximize window.
        /// </summary>
        public ICommand MaximizeCommand => RelayCommand.CreateCommand(ref maximizeCommand, OnMaximize);

        /// <summary>
        /// <see cref="ICommand"/> for minimize window.
        /// </summary>
        public ICommand MinimizeCommand => RelayCommand.CreateCommand(ref minimizeCommand, OnMinimize);

        /// <summary>
        /// <see cref="ICommand"/> for minimize window.
        /// </summary>
        public ICommand TitleMouseLeftButtonDownCommand => RelayCommand.CreateCommand(ref titleMouseLeftButtonDownCommand, OnTitleMouseLeftButtonDown);

        /// <summary>
        /// <see cref="ICommand"/> for minimize window.
        /// </summary>
        public ICommand TitleMouseLeftButtonUpCommand => RelayCommand.CreateCommand(ref titleMouseLeftButtonUpCommand, OnTitleMouseLeftButtonUp);

        /// <summary>
        /// <see cref="ICommand"/> for minimize window.
        /// </summary>
        public ICommand WindowLoadedCommand => RelayCommand.CreateCommand(ref loadedCommand, OnWindowLoaded);

        /// <summary>
        /// <see cref="ICommand"/> for window unloaded event handler.
        /// </summary>
        public ICommand WindowUnloadedCommand => RelayCommand.CreateCommand(ref unloadedCommand, OnWindowUnloaded);

        protected void OnRequestCloseCommandExecute()
        {
            if (PreventRequestClose() == false)
            {
                OnRequestCloseInternal(false);
            }
        }

        protected virtual bool RequestCloseCommandCanExecute()
        {
            return true;
        }

        protected virtual void OnRequestCloseInternal(bool? dialogResult = true)
        {
            OnRequestClose(dialogResult);
            Messenger.Default.Unregister<CloseModalWindowsMessage>(this);
        }

        protected virtual bool PreventRequestClose()
        {
            return false;
        }

        private void OnMaximize(Window window)
        {
            window.SizeToContent = SizeToContent.Manual;
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnMinimize(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        private void OnTitleMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var window = (BaseWindow)((FrameworkElement)e.Source).TemplatedParent;
            // Check if the control have been double clicked.
            if (e.ClickCount == 2 && window.ResizeMode != ResizeMode.NoResize)
            {
                // If double clicked then maximize the window.
                OnMaximize(window);
            }
            else
            {
                // If not double clicked then just drag the window around.
                if (window.WindowState == WindowState.Maximized)
                {
                    OnMaximize(window);
                    window.StickToCursor();
                }
                window.DragMove();
            }
        }

        private void OnTitleMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            CursorService.SetNormalCursor();
        }

        /// <summary>
        /// Called when a window gets loaded.
        /// We initialize resizers and update constraints.
        /// </summary>
        protected virtual void OnWindowLoaded()
        {
            Messenger.Default.Register<CloseModalWindowsMessage>(this, _ => OnRequestCloseInternal(false));
        }

        /// <summary>
        /// Called when a window gets unloaded.
        /// </summary>
        protected virtual void OnWindowUnloaded()
        {
            Dispose();
        }

        #endregion

        #region Overrides of BaseViewModel

        /// <summary>
        /// Dispose current <see cref="BaseWindowViewModel"/> instance.
        /// </summary>
        /// <param name="disposing">flag if <see cref="BaseWindowViewModel"/> is disposing from destructor or <see cref="BaseViewModel.Dispose"/> method.</param>
        /// <returns>true - if disposing is started and should clean up resurces; false otherwise (if disposing process is already happening/happened).</returns>
        protected override bool Dispose(bool disposing)
        {
            var dispose = base.Dispose(disposing);
            if (dispose == false)
            {
                return false;
            }

            Messenger.Default.Unregister<CloseModalWindowsMessage>(this);
            return true;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ThemeWindow.ViewModel;
using ThemeWindow.Windows;

namespace ThemeWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Application Overriding

        /// <summary>
        /// Raises the <see cref="Application.Startup"/> event.
        /// </summary>
        /// <param name="e">a <see cref="StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            RunApplication();
        }

        /// <summary>
        /// Raises the <see cref="Application.Exit"/> event.
        /// </summary>
        /// <param name="e">an <see cref="ExitEventArgs"/> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        #endregion

        private void RunApplication()
        {
            var mainWindowVm = new MainWindowViewModel();
            var mainWindow = new MainWindow { DataContext = mainWindowVm };

            Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
    }
}

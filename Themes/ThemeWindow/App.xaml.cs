using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using ThemePack.Common.Abstractions;
using ThemePack.Common.ThemeManagement;
using ThemeWindow.ViewModel;
using ThemeWindow.Windows;

namespace ThemeWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;

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
            container = new UnityContainer();
            container.RegisterType<IThemesSeeker, DllThemeSeeker>();

            var mainWindowVM = container.Resolve<MainWindowViewModel>();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.DataContext = mainWindowVM;
            Current.MainWindow = mainWindow;

            mainWindow.Show();
        }
    }
}

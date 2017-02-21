using System;
using System.Windows;
using ThemePack.Common.Base;
using ThemeWindow.Properties;

namespace ThemeWindow.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += LoadSettings;
        }

        private void LoadSettings(object sender, RoutedEventArgs routedEventArgs)
        {
            this.WindowState = Settings.Default.WindowState;
            this.Height = Settings.Default.WindowHeight;
            this.Width = Settings.Default.WindowWidth;
            this.Top = Settings.Default.WindowTop;
            this.Left = Settings.Default.WindowLeft;
            this.tabControl.SelectedIndex = Settings.Default.SelectedTabIndex;
        }

        private void Window_Closing(object sender,
                     System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.WindowHeight = this.Height;
            Settings.Default.WindowWidth = this.Width;
            Settings.Default.WindowTop = this.Top;
            Settings.Default.WindowLeft = this.Left < 0 ? 0 : this.Left;
            Settings.Default.WindowState = this.WindowState;
            Settings.Default.SelectedTabIndex = tabControl.SelectedIndex;
            Settings.Default.Save();
        }
    }
}

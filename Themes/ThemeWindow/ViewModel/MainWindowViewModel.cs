using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThemePack.Common.Base;
using ThemePack.Common.Factories;
using ThemePack.Common.ThemeManagement;
using ThemePack.Models.Models;

namespace ThemeWindow.ViewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        #region private fields and constants

        private const string ThemeFolder = "Themes";


        private ThemeSeekerFactory themeSeekerFactory;


        /// <summary>
        /// field for <see cref="SelectedTheme"/>
        /// </summary>
        private ThemeM selectedTheme;

        /// <summary>
        /// field for <see cref="Themes"/>
        /// </summary>
        private ObservableCollection<ThemeM> themes;

        /// <summary>
        /// field for <see cref="SelecteColorScheme"/>
        /// </summary>
        private ColorSchemeM selecteColorScheme;

        /// <summary>
        /// filed for <see cref="ColorSchemes"/>
        /// </summary>
        private ObservableCollection<ColorSchemeM> colorSchemes;

        #endregion


        #region C'tors

        /// <summary>
        /// Create instance of <see cref="MainWindowViewModel"/>
        /// </summary>
        public MainWindowViewModel()
        {
            themeSeekerFactory = new ThemeSeekerFactory();
            Initialize();
        }

        #endregion


        #region Properties

        /// <summary>
        /// Selected theme from <see cref="Themes"/>
        /// </summary>
        public ThemeM SelectedTheme
        {
            get { return selectedTheme; }
            set { SetValue(ref selectedTheme, value); }
        }

        /// <summary>
        /// Available themes <see cref="ThemeM"/>
        /// </summary>
        public ObservableCollection<ThemeM> Themes
        {
            get { return themes; }
            set { SetValue(ref themes, value); }
        }

        /// <summary>
        /// Selected color scheme <see cref="ColorSchemes"/>
        /// </summary>
        public ColorSchemeM SelecteColorScheme
        {
            get { return selecteColorScheme; }
            set
            {
                if (value != selecteColorScheme)
                {
                    ThemeManager.ChangeApplicationTheme(Application.Current, SelectedTheme.ControlStyleModels, new List<ColorSchemeM>() { value });
                }
                SetValue(ref selecteColorScheme, value);
            }
        }

        /// <summary>
        /// Color schemes in selected theme
        /// </summary>
        public ObservableCollection<ColorSchemeM> ColorSchemes
        {
            get { return colorSchemes; }
            set
            {
                SetValue(ref colorSchemes, value);
            }
        }

        #endregion

        #region private methods

        private void Initialize()
        {
            var themeSeeker = themeSeekerFactory.GetThemeSeeker();
            Themes = new ObservableCollection<ThemeM>(themeSeeker.GetThemes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ThemeFolder)));
            if (Themes.Any())
            {
                SelectedTheme = Themes.First();
                ColorSchemes = new ObservableCollection<ColorSchemeM>(Themes.First().ColorSchemeModels);
                SelecteColorScheme = ColorSchemes.FirstOrDefault();
                ThemeManager.ChangeApplicationTheme(Application.Current, SelectedTheme.ControlStyleModels, new List<ColorSchemeM>() { SelecteColorScheme });
            }
        }

        #endregion
    }
}

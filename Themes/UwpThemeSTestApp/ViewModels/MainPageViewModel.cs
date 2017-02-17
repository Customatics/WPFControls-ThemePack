using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Uwp.ThemePack.Common.Base;
using Uwp.ThemePack.Common.Factories;
using Uwp.ThemePack.Common.ThemeManagement;
using Uwp.ThemePack.Models.Models;
using Uwp.ThemePack.Models.Models.Enums;

namespace UwpThemeSTestApp.ViewModels
{
    class MainPageViewModel : BaseViewModel
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
        /// Create instance of <see cref="BaseViewModel"/>
        /// </summary>
        public MainPageViewModel()
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
            set
            {
                if (value != selectedTheme)
                {
                    SetValue(ref selectedTheme, value);
                    ChangeSelectedTheme();
                }
            }
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
                    SetValue(ref selecteColorScheme, value);
                    ChangeSelectedTheme();
                }
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
            Themes = new ObservableCollection<ThemeM>(themeSeeker.GetThemes(Path.Combine(AppContext.BaseDirectory, ThemeFolder)));
            if (Themes.Any())
            {
                selectedTheme = Themes.First();
                colorSchemes = new ObservableCollection<ColorSchemeM>(Themes.First().ColorSchemeModels);
                selecteColorScheme = ColorSchemes.FirstOrDefault();
                ChangeSelectedTheme();
            }
        }

        private void ChangeSelectedTheme()
        {
            ThemeManager.ChangeApplicationTheme(Application.Current, selectedTheme.ControlStyleModels, selecteColorScheme);
        }

        #endregion
    }
}

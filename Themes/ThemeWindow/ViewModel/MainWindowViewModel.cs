using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using ThemePack.Common.Abstractions;
using ThemePack.Common.Base;
using ThemePack.Common.ThemeManagement;
using ThemePack.Models.Models;

namespace ThemeWindow.ViewModel
{
    class MainWindowViewModel : BaseWindowViewModel
    {
        #region private fields and constants

        private const string ThemeFolder = "Themes";

        private IThemesSeeker themesSeeker;


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
        public MainWindowViewModel(IUnityContainer container)
        {
            themesSeeker = container.Resolve<IThemesSeeker>();
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
            try
            {
                Themes = new ObservableCollection<ThemeM>(themesSeeker.GetThemes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ThemeFolder)));
                if (Themes.Any())
                {
                    SelectedTheme = Themes.First();
                    ColorSchemes = new ObservableCollection<ColorSchemeM>(Themes.First().ColorSchemeModels);
                    SelecteColorScheme = ColorSchemes.FirstOrDefault();
                    ThemeManager.ChangeApplicationTheme(Application.Current, SelectedTheme.ControlStyleModels, SelectedTheme.NumericValuesModels, SelecteColorScheme);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeSelectedTheme()
        {
            ThemeManager.ChangeApplicationTheme(Application.Current, selectedTheme.ControlStyleModels, SelectedTheme.NumericValuesModels, selecteColorScheme);
        }

        #endregion
    }
}

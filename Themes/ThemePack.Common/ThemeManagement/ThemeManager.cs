using System.Collections;
using System.Collections.Generic;
using System.Windows;
using ThemePack.Models.Models;

namespace ThemePack.Common.ThemeManagement
{
    /// <summary>
    /// A class that allows to change theme and color scheme of current application
    /// </summary>
    public static class ThemeManager
    {
        //private IList<Theme> themes;
        //private IList<ColorScheme> colorSchemes;

        //public ThemeManager(IList<Theme> themes, IList<ColorScheme> colorSchemes)
        //{
        //    this.themes = themes;
        //    this.colorSchemes = colorSchemes;
        //}

        /// <summary>
        /// Change theme and color scheme and theme for the application
        /// </summary>
        /// <param name="app"><see cref="Application"/></param>
        /// <param name="themes">new <see cref="ColorScheme"/></param>
        /// <param name="colorSchemes">new <see cref="ControlStyleM"/></param>
        public static  void ChangeApplicationTheme(Application app, IList<ControlStyleM> themes, IList<ColorSchemeM> colorSchemes)
        {
            app.Resources.Clear();
            app.Resources.BeginInit();

            foreach (var theme in themes)
            {
                foreach (DictionaryEntry themeResource in theme.Resources)
                {
                    app.Resources.Add(themeResource.Key, themeResource.Value);
                }
            }
            foreach (var theme in colorSchemes)
            {
                foreach (DictionaryEntry themeResource in theme.Resources)
                {
                    app.Resources.Add(themeResource.Key, themeResource.Value);
                }
            }

            app.Resources.EndInit();
        }
    }
}

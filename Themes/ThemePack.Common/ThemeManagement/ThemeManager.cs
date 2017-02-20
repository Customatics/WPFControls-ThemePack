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
        /// <summary>
        /// Change theme and color scheme and theme for the application
        /// </summary>
        /// <param name="app"><see cref="Application"/></param>
        /// <param name="styles">new <see cref="ControlStyleM"/></param>
        /// <param name="colorScheme">new <see cref="ColorSchemeM"/></param>
        public static void ChangeApplicationTheme(Application app, IList<ControlStyleM> styles, ColorSchemeM colorScheme)
        {
            if (app == null) { return; }
            if (styles == null) { return; }
            if (colorScheme == null) { return; }

            app.Resources.MergedDictionaries.Clear();

            foreach (var style in styles)
            {
                app.Resources.MergedDictionaries.Add(style.Resources);
            }
            app.Resources.MergedDictionaries.Add(colorScheme.Resources);
        }
    }
}

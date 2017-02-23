using System.Collections;
using System.Collections.Generic;
using System.Windows;
using ThemePack.Models.Models;

namespace ThemePack.Common.ThemeManagement
{
    /// <summary>
    /// A class that allows to change theme and color scheme
    /// </summary>
    public static class ThemeManager
    {
        /// <summary>
        /// Change theme and color scheme and theme for the application
        /// </summary>
        /// <param name="app"><see cref="Application"/></param>
        /// <param name="styles">new <see cref="ControlStyleM"/></param>
        /// <param name="numericValues"><see cref="NumericValuesM"/></param>
        /// <param name="colorScheme">new <see cref="ColorSchemeM"/></param>
        public static void ChangeApplicationTheme(Application app, IList<ControlStyleM> styles, IList<NumericValuesM> numericValues, ColorSchemeM colorScheme)
        {
            if (app == null) { return; }
            if (styles == null) { return; }
            if (colorScheme == null) { return; }

            app.Resources.BeginInit();

            app.Resources.MergedDictionaries.Clear();


            foreach (var numericValue in numericValues)
            {
                app.Resources.MergedDictionaries.Add(numericValue.Resources);//Numerics first
            }

            app.Resources.MergedDictionaries.Add(colorScheme.Resources);

            foreach (var style in styles)
            {
                app.Resources.MergedDictionaries.Add(style.Resources);
            }

            app.Resources.EndInit();
        }
    }
}

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
        /// <param name="styles">new <see cref="ControlStyleM"/></param>
        /// <param name="colorScheme">new <see cref="ColorSchemeM"/></param>
        public static void ChangeApplicationTheme(Application app, IList<ControlStyleM> styles, ColorSchemeM colorScheme)
        {
            if (app == null) { return; }
            if (styles == null) { return; }
            if (colorScheme == null) { return; }

            app.Resources.MergedDictionaries.Clear();
            //app.Resources.BeginInit();

            foreach (var style in styles)
            {
                app.Resources.MergedDictionaries.Add(style.Resources);
            }
            app.Resources.MergedDictionaries.Add(colorScheme.Resources);

            //app.Resources.BeginInit();

            //foreach (var controlStyleM in styles)
            //{
            //    foreach (DictionaryEntry resource in controlStyleM.Resources)
            //    {
            //        if (app.Resources.Contains(resource.Key))
            //        {
            //            app.Resources.Remove(resource.Key);
            //        }
            //        app.Resources.Add(resource.Key, resource.Value);
            //    }
            //}
            //foreach (var controlStyleM in colorSchemes)
            //{
            //    foreach (DictionaryEntry resource in controlStyleM.Resources)
            //    {
            //        if (app.Resources.Contains(resource.Key))
            //        {
            //            app.Resources.Remove(resource.Key);
            //        }
            //        app.Resources.Add(resource.Key, resource.Value);
            //    }
            //}

            //app.Resources.EndInit();

            //app.Resources.EndInit();
        }
    }
}

using System.Collections.Generic;
using Windows.UI.Xaml;
using Uwp.ThemePack.Models.Models;

namespace Uwp.ThemePack.Common.ThemeManagement
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
        /// <param name="colorSchemes">new <see cref="ColorSchemeM"/></param>
        public static void ChangeApplicationTheme(Application app, IList<ControlStyleM> styles, IList<ColorSchemeM> colorSchemes)
        {
            if (app == null) { return; }
            if (styles == null) { return; }
            if (colorSchemes == null) { return; }

            app.Resources.MergedDictionaries.Clear();
            //app.Resources.BeginInit();

            foreach (var style in styles)
            {
                app.Resources.MergedDictionaries.Add(style.Resources);
            }
            foreach (var scheme in colorSchemes)
            {
                app.Resources.MergedDictionaries.Add(scheme.Resources);
            }

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

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Change theme and color scheme and theme for the application. Every resource should contain color scheme inside it merged dictionary (because of StaticResource)
        /// There is no DynamicResource in UWP
        /// </summary>
        /// <param name="app"><see cref="Application"/></param>
        ///// <param name="styles">new <see cref="ControlStyleM"/></param>
        /// <param name="colorScheme">new <see cref="ColorSchemeM"/></param>
        public static void ChangeApplicationTheme(Application app, IList<ControlStyleM> styles, ColorSchemeM colorScheme)
        {
            if (app == null) { return; }
            if (styles == null) { return; }
            if (colorScheme == null) { return; }

            var colorSchemeResource = new ResourceDictionary() { Source = colorScheme.Uri };

            var colorSchemeResourceThemes = colorSchemeResource.ThemeDictionaries.ToList();
            colorSchemeResource.ThemeDictionaries.Clear();
            app.Resources.ThemeDictionaries.Clear();
            foreach (var theme in colorSchemeResourceThemes)
            {
                app.Resources.ThemeDictionaries.Add(theme.Key, theme.Value);
            }

            app.Resources.MergedDictionaries.Clear();
            foreach (var controlStyleM in styles)
            {
                var styleResource = new ResourceDictionary() { Source = controlStyleM.Uri };
                app.Resources.MergedDictionaries.Add(styleResource);
            }
        }
    }
}

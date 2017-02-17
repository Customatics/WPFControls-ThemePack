using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uwp.ThemePack.Common.Abstractions;
using Uwp.ThemePack.Models.Models;
using Uwp.ThemePack.Models.Models.Enums;
using UwpTheme_001;

namespace Uwp.ThemePack.Common.ThemeManagement
{
    public class DllThemeSeeker : IThemesSeeker
    {
        public IList<ThemeM> GetThemes(string folder)
        {
            var themes = new List<ThemeM>();

            //**add here others themes
            var currentDllResources = new Theme001AssemblyContainer().GetAssemblyResources();
            var assemblyName = "Theme_001";
            themes.Add(new ThemeM(assemblyName, currentDllResources.Where(_ => _.ResourceType == XamlResourceType.ColorScheme).Select(it => new ColorSchemeM(it.Name, it.Uri)).ToList(),
                                  currentDllResources.Where(_ => _.ResourceType == XamlResourceType.ControlStyle).Select(it => new ControlStyleM(it.Name, it.Uri)).ToList()));
            //**

            return themes;
        }


    }
}

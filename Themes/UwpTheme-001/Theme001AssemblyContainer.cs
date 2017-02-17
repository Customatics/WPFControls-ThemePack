using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Uwp.ThemePack.Models.Abstractions;
using Uwp.ThemePack.Models.Models;
using Uwp.ThemePack.Models.Models.Enums;

namespace UwpTheme_001
{
    /// <summary>
    /// Implement <see cref="IThemeAssemblyContainer"/>
    /// </summary>
    public class Theme001AssemblyContainer : IThemeAssemblyContainer
    {
        public List<DllResourceInfo> GetAssemblyResources()
        {
            var resources = new List<DllResourceInfo>();

            //color schemes (accents)
            resources.Add(new DllResourceInfo("Light", new Uri("ms-appx:///UwpTheme_001/Themes/ColorSchemes/Light.xaml"), XamlResourceType.ColorScheme));

            resources.Add(new DllResourceInfo("Dark", new Uri("ms-appx:///UwpTheme_001/Themes/ColorSchemes/Dark.xaml"), XamlResourceType.ColorScheme));

            //controls styles
            resources.Add(new DllResourceInfo("Button", new Uri("ms-appx:///UwpTheme_001/Themes/Styles/ButtonStyle.xaml"), XamlResourceType.ControlStyle));


            return resources;
        }
    }
}

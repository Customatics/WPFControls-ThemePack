using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Uwp.ThemePack.Common.Abstractions;
using Uwp.ThemePack.Common.Helpers;
using Uwp.ThemePack.Models.Models;
using Uwp.ThemePack.Models.Models.Enums;

namespace Uwp.ThemePack.Common.ThemeManagement
{
    public class DllThemeSeeker : IThemesSeeker
    {
        public IList<ThemeM> GetThemes(string folder)
        {
            var themes = new List<ThemeM>();

            if (Directory.Exists(folder))
            {
                var  dllFileNames = Directory.GetFiles(folder, "*.dll");
                //ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    var assemblyName = "thisidf";//AssemblyName.GetAssemblyName(dllFile).Name;
                    var currentDllResources = AssemblyResourceHelper.LoadXaml(dllFile);
                    themes.Add(new ThemeM(assemblyName, currentDllResources.Where(_ => _.ResourceType == XamlResourceType.ColorScheme).Select(it => new ColorSchemeM(it.Name,it.Resource, it.Uri)).ToList(),
                                          currentDllResources.Where(_ => _.ResourceType == XamlResourceType.ControlStyle).Select(it => new ControlStyleM(it.Name, it.Resource, it.Uri)).ToList()));
                }
            }

            return themes;
        }


    }
}

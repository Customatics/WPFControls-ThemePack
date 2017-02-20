using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using ThemePack.Common.Extentions;
using ThemePack.Models.Models;

namespace ThemePack.Common.Helpers
{
    /// <summary>
    /// Helper for seek .xaml resources in assembly
    /// </summary>
    public static class AssemblyResourceHelper
    {
        /// <summary>
        /// Load .xaml resources from assembly
        /// </summary>
        /// <param name="assemblypath">Full path to assembly</param>
        /// <returns>List of <see cref="DllResourceInfo"/></returns>
        public static List<DllResourceInfo> LoadXaml(string assemblypath)
        {
            var resources = new List<DllResourceInfo>();

            var assembly = Assembly.LoadFile(assemblypath);
            using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources"))
            {
                var resourceReader = new ResourceReader(stream);

                foreach (DictionaryEntry resource in resourceReader)
                {
                    if (new FileInfo(resource.Key.ToString()).Extension.Equals(".baml"))
                    {
                        var name = Path.GetFileName(resource.Key.ToString()).Replace(".baml", "");
                        var parentDir = Path.GetDirectoryName(resource.Key.ToString());
                        //var name = resource.Key.ToString().Replace(".baml", "");
                        Uri uri = new Uri("/" + assembly.GetName().Name + ";component/" + resource.Key.ToString().Replace(".baml", ".xaml"), UriKind.Relative);
                        //ResourceDictionary skin = Application.LoadComponent(uri) as ResourceDictionary;
                        resources.Add(new DllResourceInfo(name, uri, parentDir.FolderNameToResourceType()));
                    }
                }
            }
            return resources;
        }
    }
}

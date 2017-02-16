using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using Windows.UI.Xaml;
using Uwp.ThemePack.Common.BusinessModels;
using Uwp.ThemePack.Common.Extensions;

namespace Uwp.ThemePack.Common.Helpers
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

            var asName = new AssemblyName();
            asName.Name = "UwpTheme-001";
            var assembly = Assembly.Load(asName);

            var resourceNames = assembly.GetManifestResourceNames();
            var loc = assembly.GetManifestResourceInfo("").ResourceLocation;//todo loading dll's
            var resourceManager = new ResourceManager(resourceNames.First(), assembly);
            var str = resourceManager.GetString("sdfds");
            //var assembly = Assembly.LoadFile(assemblypath);
            using (var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources"))
            {

                string result = string.Empty;

                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
                
                //var resourceReader = new ResourceReader(stream);

                //foreach (DictionaryEntry resource in resourceReader)
                //{
                //    if (new FileInfo(resource.Key.ToString()).Extension.Equals(".baml"))
                //    {
                //        var name = Path.GetFileName(resource.Key.ToString()).Replace(".baml", "");
                //        var parentDir = Path.GetDirectoryName(resource.Key.ToString());
                //        //var name = resource.Key.ToString().Replace(".baml", "");
                //        Uri uri = new Uri("/" + assembly.GetName().Name + ";component/" + resource.Key.ToString().Replace(".baml", ".xaml"), UriKind.Relative);
                //        ResourceDictionary skin = new ResourceDictionary() { Source = uri };

                //        resources.Add(new DllResourceInfo(name, uri, skin, parentDir.FolderNameToResourceType()));
                //    }
                //}
            }
            return resources;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using ThemePack.Common.Abstractions;
using ThemePack.Models.Models;

namespace ThemePack.Common.ThemeManagement
{
    public class DllThemeSeeker : IThemesSeeker
    {
        public IList<ThemeM> GetThemes()
        {
            var themes = new List<ThemeM>();

            List<string> resourceNames = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            var rm = new ResourceManager(assembly.GetName().Name + ".g", assembly);
            try
            {
                var list = rm.GetResourceSet(CultureInfo.CurrentCulture, true, true);
                foreach (DictionaryEntry item in list)
                    resourceNames.Add((string)item.Key);
            }
            finally
            {
                rm.ReleaseAllResources();
            }


            return themes;
        }
    }
}

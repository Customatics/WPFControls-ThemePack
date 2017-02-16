using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemePack.Common.Abstractions;
using ThemePack.Common.ThemeManagement;

namespace ThemePack.Common.Factories
{
    public class ThemeSeekerFactory
    {
        public IThemesSeeker GetThemeSeeker()
        {
            return new DllThemeSeeker();
        }
    }
}

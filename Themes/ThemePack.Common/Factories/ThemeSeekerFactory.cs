using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemePack.Common.Abstractions;
using ThemePack.Common.ThemeManagement;

namespace ThemePack.Common.Factories
{
    /// <summary>
    /// Factory for theme seeker <see cref="IThemesSeeker"/>
    /// </summary>
    public class ThemeSeekerFactory
    {
        public IThemesSeeker GetThemeSeeker()// possible others realizations
        {
            return new DllThemeSeeker();
        }
    }
}

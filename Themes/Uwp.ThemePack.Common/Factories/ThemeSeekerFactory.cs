using Uwp.ThemePack.Common.Abstractions;
using Uwp.ThemePack.Common.ThemeManagement;

namespace Uwp.ThemePack.Common.Factories
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

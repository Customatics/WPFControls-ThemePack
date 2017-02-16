using Uwp.ThemePack.Common.Abstractions;
using Uwp.ThemePack.Common.ThemeManagement;

namespace Uwp.ThemePack.Common.Factories
{
    public class ThemeSeekerFactory
    {
        public IThemesSeeker GetThemeSeeker()
        {
            return new DllThemeSeeker();
        }
    }
}

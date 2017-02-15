using System.Collections.Generic;
using ThemePack.Models.Models;

namespace ThemePack.Common.Abstractions
{
    public interface IThemesSeeker
    {
        IList<ThemeM> GetThemes(string folder);
    }
}
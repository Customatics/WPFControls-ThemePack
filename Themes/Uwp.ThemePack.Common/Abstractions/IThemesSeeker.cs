using System.Collections.Generic;
using Uwp.ThemePack.Models.Models;

namespace Uwp.ThemePack.Common.Abstractions
{
    /// <summary>
    /// Themes reader
    /// </summary>
    public interface IThemesSeeker
    {
        IList<ThemeM> GetThemes(string folder);
    }
}
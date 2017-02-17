using System.Collections.Generic;
using Windows.UI.Xaml;
using Uwp.ThemePack.Models.Models;

namespace Uwp.ThemePack.Models.Abstractions
{
    /// <summary>
    /// Help to work with <see cref="ResourceDictionary"/> in assembly
    /// </summary>
    public interface IThemeAssemblyContainer
    {
        List<DllResourceInfo> GetAssemblyResources();
    }
}
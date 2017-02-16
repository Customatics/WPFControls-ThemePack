using System;
using Windows.UI.Xaml;

namespace Uwp.ThemePack.Models.Models
{
    /// <summary>
    /// Represents color scheme of the app.
    /// </summary>
    public class ColorSchemeM : ControlStyleM
    {
        public ColorSchemeM(string name, ResourceDictionary resource, Uri uri) : base(name, resource, uri)
        {
        }
    }
}

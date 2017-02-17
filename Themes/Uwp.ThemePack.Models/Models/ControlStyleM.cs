using System;
using Windows.UI.Xaml;

namespace Uwp.ThemePack.Models.Models
{
    /// <summary>
    /// Represents resource for control style.
    /// </summary>
    public class ControlStyleM
    {
        public ControlStyleM(string name, Uri uri)
        {
            this.Name = name;
            this.Uri = uri;
        }

        /// <summary>
        /// The name of the application theme.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Resource <see cref="Uri"/>
        /// </summary>
        public Uri Uri { get; private set; }
    }
}
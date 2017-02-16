using System;
using Windows.UI.Xaml;

namespace Uwp.ThemePack.Models.Models
{
    /// <summary>
    /// Represents resource for control style.
    /// </summary>
    public class ControlStyleM
    {
        public ControlStyleM(string name, ResourceDictionary resource, Uri uri)
        {
            this.Name = name;
            this.Uri = uri;
            resource.Source = uri;
            this.Resources = resource;
        }
        /// <summary>
        /// The ResourceDictionary that represents this application theme.
        /// </summary>
        public ResourceDictionary Resources { get; private set; }

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
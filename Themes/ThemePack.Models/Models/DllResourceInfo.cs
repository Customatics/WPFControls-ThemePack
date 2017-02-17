using System;
using System.Windows;
using ThemePack.Models.Models.Enums;

namespace ThemePack.Models.Models
{
    /// <summary>
    /// Assembly <see cref="ResourceDictionary"/> info
    /// </summary>
    public class DllResourceInfo
    {
        /// <summary>
        /// Initialize new instance of <see cref="DllResourceInfo"/>
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="uri">Resource uri</param>
        /// <param name="resourceType"><see cref="ResourceType"/></param>
        public DllResourceInfo(string name, Uri uri, XamlResourceType resourceType)
        {
            Name = name;
            Resource = new ResourceDictionary() { Source = uri };
            ResourceType = resourceType;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Resource <see cref="ResourceDictionary"/>
        /// </summary>
        public ResourceDictionary Resource { get; set; }

        /// <summary>
        /// <see cref="ResourceType"/>
        /// </summary>
        public XamlResourceType ResourceType { get; set; }
    }
}

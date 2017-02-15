using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThemePack.Models.Models.Enums;

namespace ThemePack.Common.BusinessModels
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
        /// <param name="resource">.xaml resource <see cref="ResourceDictionary"/></param>
        /// <param name="resourceType"><see cref="ResourceType"/></param>
        public DllResourceInfo(string name, Uri uri, ResourceDictionary resource, XamlResourceType resourceType)
        {
            Name = name;
            Uri = uri;
            Resource = resource;
            ResourceType = resourceType;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Resource <see cref="Uri"/>
        /// </summary>
        public Uri Uri { get; set; }

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

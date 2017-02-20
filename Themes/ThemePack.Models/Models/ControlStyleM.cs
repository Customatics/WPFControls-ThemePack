using System;
using System.Windows;

namespace ThemePack.Models.Models
{
    /// <summary>
    /// Represents resource for control style.
    /// </summary>
    public class ControlStyleM
    {
        public ControlStyleM(string name, ResourceDictionary resource)
        {
            this.Name = name;
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
    }
}
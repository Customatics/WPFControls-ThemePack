using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace ThemePack.Common.Helpers
{
    /// <summary>
    /// Enumeration helper.
    /// </summary>
    /// <date>15:34 05/15/2015</date>
    /// <author>Anton Liakhovich</author>
    public static class EnumerationHelper
    {
        /// <summary>
        /// Get enumeration value marked by <paramref name="attributeName"/>.
        /// </summary>
        /// <param name="attributeName">attribute name to get value.</param>
        /// <param name="type">enumeration type.</param>
        /// <param name="useAttributeIfValueNotFound">use attribute name if field doesn't exist.</param>
        /// <returns>enumeration value.</returns>
        /// <exception cref="AmbiguousMatchException">more than one of the requested attributes was found.</exception>
        /// <exception cref="TypeLoadException">a custom attribute type cannot be loaded.</exception>
        public static object GetValueByAttribute(string attributeName, Type type, bool useAttributeIfValueNotFound = true)
        {
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(XmlEnumAttribute)) as XmlEnumAttribute;
                if (attribute == null)
                {
                    if (field.Name == attributeName)
                    {
                        return field.GetValue(null);
                    }
                }
                else
                {
                    if (attribute.Name == attributeName)
                    {
                        return field.GetValue(null);
                    }
                }
            }

            return useAttributeIfValueNotFound
                ? attributeName
                : null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uwp.ThemePack.Common.Constants;
using Uwp.ThemePack.Models.Models.Enums;

namespace Uwp.ThemePack.Common.Extensions
{
    public static class ResourceTypeExtenstions
    {
        public static XamlResourceType FolderNameToResourceType(this string folderName)
        {
            switch (folderName.ToLower())
            {
                case ThemeNameConstants.ThemeStyle:
                case ThemeNameConstants.Values:
                    return XamlResourceType.ControlStyle;
                case ThemeNameConstants.ColorScheme:
                    return XamlResourceType.ColorScheme;
            }
            throw new ArgumentException("Unexpected argument. Some .xaml resource placed in wrong folder.");
        }
    }
}

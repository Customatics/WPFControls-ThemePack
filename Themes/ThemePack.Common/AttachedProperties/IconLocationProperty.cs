using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThemePack.Common.AttachedProperties.Enums;

namespace ThemePack.Common.AttachedProperties
{
    public class IconLocation : FrameworkElement
    {
        public static IconAlignmentEnum GetIconAlignment(DependencyObject obj)
        {
            return (IconAlignmentEnum)obj.GetValue(IconAlignmentProperty);
        }

        public static void SetIconAlignment(DependencyObject obj, IconAlignmentEnum value)
        {
            obj.SetValue(IconAlignmentProperty, value);
        }

        public static readonly DependencyProperty IconAlignmentProperty =
            DependencyProperty.RegisterAttached("IconAlignment", typeof(IconAlignmentEnum), typeof(IconLocation), new FrameworkPropertyMetadata(null));
    }
}

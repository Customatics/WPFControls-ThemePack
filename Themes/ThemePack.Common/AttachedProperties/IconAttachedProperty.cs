using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThemePack.Common.AttachedProperties.Enums;

namespace ThemePack.Common.AttachedProperties
{
    public static class IconAttachedProperty
    {
        #region Image dependency property

        /// <summary>
        /// An attached dependency property which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static readonly DependencyProperty FontIconProperty = DependencyProperty.RegisterAttached("FontIcon",
            typeof(Style), typeof(IconAttachedProperty), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="FontIconProperty"/> for a given
        /// <see cref="DependencyObject"/>, which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static Style GetFontIcon(DependencyObject obj)
        {
            return (Style)obj.GetValue(FontIconProperty);
        }

        /// <summary>
        /// Sets the attached <see cref="FontIconProperty"/> for a given
        /// <see cref="DependencyObject"/>, which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static void SetFontIcon(DependencyObject obj, Style value)
        {
            obj.SetValue(FontIconProperty, value);
        }

        #endregion

        #region IconAlignment

        public static readonly DependencyProperty IconAlignmentProperty = DependencyProperty.RegisterAttached(
            "IconAlignment", typeof(IconAlignmentEnum), typeof(IconAttachedProperty), new PropertyMetadata(default(IconAlignmentEnum)));

        public static void SetIconAlignment(DependencyObject element, IconAlignmentEnum value)
        {
            element.SetValue(IconAlignmentProperty, value);
        }

        public static IconAlignmentEnum GetIconAlignment(DependencyObject element)
        {
            return (IconAlignmentEnum) element.GetValue(IconAlignmentProperty);
        }


        #endregion
    }
}

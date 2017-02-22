using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ThemePack.Common.AttachedProperties
{
    public class IconAttached
    {
        #region Image dependency property

        /// <summary>
        /// An attached dependency property which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static readonly DependencyProperty FontIconProperty = DependencyProperty.RegisterAttached("FontIcon",
            typeof(Style), typeof(IconAttached), new FrameworkPropertyMetadata(null));

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
    }
}

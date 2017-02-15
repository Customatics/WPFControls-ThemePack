using System.Windows;

namespace ThemePack.Common.AttachedProperties
{
    public class PathButton
    {
        #region Image dependency property

        /// <summary>
        /// An attached dependency property which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.RegisterAttached("PathStyle",
            typeof (Style), typeof (PathButton), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="PathStyleProperty"/> for a given
        /// <see cref="DependencyObject"/>, which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static Style GetPathStyle(DependencyObject obj)
        {
            return (Style) obj.GetValue(PathStyleProperty);
        }

        /// <summary>
        /// Sets the attached <see cref="PathStyleProperty"/> for a given
        /// <see cref="DependencyObject"/>, which provides an
        /// <see cref="Style" /> for arbitrary WPF elements.
        /// </summary>
        public static void SetPathStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(PathStyleProperty, value);
        }

        #endregion
    }
}

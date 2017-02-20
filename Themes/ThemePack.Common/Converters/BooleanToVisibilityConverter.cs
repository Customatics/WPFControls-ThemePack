using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ThemePack.Common.Converters
{
    public class BooleanToVisibilityConverter : ConverterMarkupExtension<BooleanToVisibilityConverter>
    {
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">the value produced by the binding source.</param>
        /// <param name="targetType">the type of the binding target property.</param>
        /// <param name="parameter">the converter parameter to use.</param>
        /// <param name="culture">the culture to use in the converter.</param>
        /// <returns>a converted value. If the method returns null, the valid null value is used.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            var isVisible = true;

            if (value is bool)
            {
                isVisible = (bool)value;
            }

            var stringParam = parameter?.ToString() ?? string.Empty;
            if (stringParam.IndexOf("inverse", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                isVisible = isVisible == false;
            }

            return isVisible
                ? Visibility.Visible
                : stringParam.IndexOf("hidden", StringComparison.OrdinalIgnoreCase) >= 0
                    ? Visibility.Hidden
                    : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">the value that is produced by the binding target.</param>
        /// <param name="targetType">the type to convert to.</param>
        /// <param name="parameter">the converter parameter to use.</param>
        /// <param name="culture">the culture to use in the converter.</param>
        /// <returns>a converted value. If the method returns null, the valid null value is used.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ThemePack.Common.Helpers;

namespace ThemePack.Common.Converters
{
    public class NullToVisibilityConverter : ConverterMarkupExtension<NullToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool reverse = false;
            if (parameter != null)
            {
                ConversionHelper.TryConvertValue(parameter, out reverse);
            }

            var isNotNull = (value != null) && (value != DependencyProperty.UnsetValue);
            return isNotNull ^ reverse
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

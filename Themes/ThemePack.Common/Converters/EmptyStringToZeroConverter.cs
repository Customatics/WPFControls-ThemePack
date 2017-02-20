using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemePack.Common.Converters
{
    public class EmptyStringToZeroConverter : ConverterMarkupExtension<EmptyStringToZeroConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            return string.IsNullOrEmpty(str) ? 0 : value;
        }
    }
}

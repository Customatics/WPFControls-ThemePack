using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ThemePack.Common.Converters
{
    /// <summary>
    /// Generic <see cref="MarkupExtension"/> to provide converters.
    /// </summary>
    /// <date>16:25 06/08/2015</date>
    /// <author>Anton Liakhovich</author>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class ConverterMarkupExtension<T> : MarkupExtension, IValueConverter
        where T : class, IValueConverter, new()
    {
        /// <summary>
        /// <see cref="T"/> converter instance.
        /// </summary>
        private static T converter;

        #region Overrides of MarkupExtension

        /// <summary>
        /// Returns <see cref="T"/> converter that is provided as the value of the target property for this markup extension. 
        /// </summary>
        /// <param name="serviceProvider">a service provider helper that can provide services for the markup extension.</param>
        /// <returns>the <see cref="T"/> converte value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new T());
        }

        #endregion

        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">the value produced by the binding source.</param>
        /// <param name="targetType">the type of the binding target property.</param>
        /// <param name="parameter">the converter parameter to use.</param>
        /// <param name="culture">the culture to use in the converter.</param>
        /// <returns>a converted value. If the method returns null, the valid null value is used.</returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">the value that is produced by the binding target.</param>
        /// <param name="targetType">the type to convert to.</param>
        /// <param name="parameter">the converter parameter to use.</param>
        /// <param name="culture">the culture to use in the converter.</param>
        /// <returns>a converted value. If the method returns null, the valid null value is used.</returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion

        #region Help Methods

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">the value produced by the binding source.</param>
        /// <returns>a converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value)
        {
            return Convert(value, null, null, null);
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <param name="value">the value that is produced by the binding target.</param>
        /// <returns>a converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value)
        {
            return ConvertBack(value, null, null, null);
        }

        #endregion
    }
}

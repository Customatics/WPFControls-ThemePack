using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace ThemePack.Common.Helpers
{
    /// <summary>
    /// Helper to convert data from one type to another.
    /// </summary>
    /// <date>11:02 06/09/2015</date>
    /// <author>Anton Liakhovich</author>
    public static class ConversionHelper
    {
        /// <summary>
        /// Try to convert the <paramref name="value"/> to to typified value.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">the type to convert <paramref name="value"/> to.</typeparam>
        /// <param name="value">>the value to convert.</param>
        /// <param name="result">when this method returns, contains the typified value, if the conversion succeeded, or default <see cref="T"/> value if the conversion failed.</param>
        /// <returns>true if value was converted successfully; otherwise, false.</returns>
        public static bool TryConvertValue<T>(object value, out T result)
        {
            result = default(T);

            if (value == null)
                return false;

            try
            {
                result = (T) ConvertValue(value, typeof (T));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Convert the <paramref name="value"/> to typified value.  
        /// </summary>
        /// <typeparam name="T">the type to convert <paramref name="value"/> to.</typeparam>
        /// <param name="value">the value to convert.</param>
        /// <returns>convertion result.</returns>
        /// <exception cref="AmbiguousMatchException">more than one of the requested attributes was found.</exception>
        /// <exception cref="TypeLoadException">a custom attribute type cannot be loaded.</exception>
        /// <exception cref="TargetException">The field is non-static.
        /// Note: In the .NET for Windows Store apps or the Portable Class Library, catch <see cref="Exception" /> instead.</exception>
        /// <exception cref="FieldAccessException">The caller does not have permission to access this field.
        /// Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="MemberAccessException" />, instead.</exception>
        /// <exception cref="InvalidCastException">This conversion is not supported. -or-<paramref name="value" /> is null and <see cref="T" /> is a value type.-
        /// or-<paramref name="value" /> does not implement the <see cref="IConvertible"/> interface.</exception>
        /// <exception cref="OverflowException"><paramref name="value" /> represents a number that is out of the range of <see cref="T" />.</exception>
        public static T ConvertValue<T>(object value)
        {
            return (T) ConvertValue(value, typeof (T));
        }

        /// <summary>
        /// Convert the <paramref name="value"/> to typified value. 
        /// </summary>
        /// <param name="value">the value to convert.</param>
        /// <param name="type">the <see cref="Type"/> to convert <paramref name="value"/> to.</param>
        /// <returns>convertion result.</returns>
        /// <exception cref="AmbiguousMatchException">more than one of the requested attributes was found.</exception>
        /// <exception cref="TypeLoadException">a custom attribute type cannot be loaded.</exception>
        /// <exception cref="OverflowException">The format of <paramref name="value" /> is invalid. </exception>
        public static object ConvertValue(object value, Type type)
        {
            object toReturn;
            if (value == null)
            {
                toReturn = null;
            }
            else if (type.IsInstanceOfType(value))
            {
                toReturn = value;
            }
            else if (type == typeof (Guid))
            {
                toReturn = new Guid(value.ToString());
            }
            else if (type == typeof (bool))
            {
                toReturn = value.ToString().Equals("1") || value.ToString().ToLower().Equals("true");
            }
            else if (type.IsEnum)
            {
                toReturn = EnumerationHelper.GetValueByAttribute(value.ToString(), type, false) ??
                           Enum.Parse(type, value.ToString());
            }
            else if (type == typeof (XElement))
            {
                toReturn = XElement.Parse(value.ToString());
            }
            else if ((type == typeof (DateTime)) || ((type == typeof (DateTime?))))
            {
                toReturn = (value is long)
                    ? new DateTime((long) value)
                    : DateTime.Parse(value.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal);
            }
            else if (type == typeof (string))
            {
                toReturn = value.ToString();
            }
            else
            {
                toReturn = Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }

            return toReturn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Uwp.ThemePack.Common.Base
{
    /// <summary>
    /// Base for ViewModels.
    /// </summary>
    /// <date>17:05 05/14/2015</date>
    /// <author>Anton Liakhovich</author>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for each property from <paramref name="properties"/>.
        /// </summary>
        /// <param name="properties">expressions for getting property name.</param>
        /// <exception cref="Exception"><see cref="PropertyChanged"/>'s callback throws an exception.</exception>
        protected void OnPropertiesChanged(params Expression<Func<object>>[] properties)
        {
            foreach (var item in properties)
            {
                OnPropertyChanged(item);
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for a property from expression.
        /// </summary>
        /// <typeparam name="T">property type.</typeparam>
        /// <param name="property">expression for getting property name.</param>
        /// <exception cref="Exception"><see cref="PropertyChanged"/>'s callback throws an exception.</exception>
        protected void OnPropertyChanged<T>(Expression<Func<T>> property)
        {
            OnPropertyChanged(GetPropertyName(property));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for a given property.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        /// <exception cref="Exception"><see cref="PropertyChanged"/>'s callback throws an exception.</exception>
        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for a given property.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        /// <exception cref="Exception"><see cref="PropertyChanged"/>'s callback throws an exception.</exception>
        protected void OnPropertyChanged(string propertyName = null)
        {
            if ((string.IsNullOrEmpty(propertyName) == false))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Setting Value

        /// <summary>
        /// Sets <paramref name="newValue"/> to property with specified <paramref name="propertyName"/>.
        /// </summary>
        /// <typeparam name="T">value type.</typeparam>
        /// <param name="local">ref value to set <paramref name="newValue"/> to.</param>
        /// <param name="newValue">new value to set.</param>
        /// <param name="propertyName">property name.</param>
        /// <returns>is setting successful.</returns>
        /// <exception cref="Exception"><see cref="PropertyChanged"/>'s callback throws an exception.</exception>
        protected bool SetValue<T>(ref T local, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(local, newValue))
            {
                return false;
            }

            local = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Sets <paramref name="newValue"/> to <paramref name="properties"/>.
        /// </summary>
        /// <typeparam name="T">value type.</typeparam>
        /// <param name="local">ref value to set <paramref name="newValue"/> to.</param>
        /// <param name="newValue">new value to set.</param>
        /// <param name="properties">property expressions.</param>
        /// <returns>is setting successful.</returns>
        /// <exception cref="Exception"><see cref="PropertyChanged"/>'s callback throws an exception.</exception>
        protected bool SetValue<T>(ref T local, T newValue, params Expression<Func<object>>[] properties)
        {
            if (Equals(newValue, local))
            {
                return false;
            }

            local = newValue;
            OnPropertiesChanged(properties);
            return true;
        }

        #endregion

        #region Getting Property Name

        /// <summary>
        /// Get <paramref name="property"/> name. 
        /// </summary>
        /// <typeparam name="T"><paramref name="property"/> type.</typeparam>
        /// <param name="property">property to get name from.</param>
        /// <returns><paramref name="property"/> name.</returns>
        private static string GetPropertyName<T>(Expression<Func<T>> property)
        {
            var lambda = (LambdaExpression)property;
            var lambdaBody = lambda.Body as UnaryExpression;

            var memberExpression = (lambdaBody != null)
                ? (MemberExpression)lambdaBody.Operand
                : (MemberExpression)lambda.Body;

            return memberExpression.Member.Name;
        }

        #endregion
    }
}

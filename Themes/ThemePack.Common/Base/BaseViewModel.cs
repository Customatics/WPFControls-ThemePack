using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ThemePack.Common.Base.Abstractions;
using ThemePack.Common.Models;

namespace ThemePack.Common.Base
{
    /// <summary>
    /// Base for ViewModels.
    /// </summary>
    /// <date>17:05 05/14/2015</date>
    /// <author>Anton Liakhovich</author>
    public abstract class BaseViewModel : INotifyPropertyChanged, IRequestCloseViewModel, IRequestWaitViewModel, IDisposable
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

        #region UI Update

        /// <summary>
        /// Begin update UI with invocation to <see cref="Application.Current"/>'s <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="action"><see cref="Action"/> to begin invocation.</param>
        /// <param name="checkAccess">check access before invoke if call in main thread.</param>
        /// <exception cref="Exception">a <paramref name="action"/>'s callback throws an exception.</exception>
        protected void BeginUpdatesUI(Action action, bool checkAccess = true)
        {
            if ((action == null) || (Application.Current == null))
            {
                return;
            }

            var dispatcher = Application.Current.Dispatcher;
            if (checkAccess && dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action);
            }
        }

        /// <summary>
        /// Update UI with invocation to <see cref="Application.Current"/>'s <see cref="Dispatcher"/>.
        /// </summary>
        /// <param name="action"><see cref="Action"/> to invocation.</param>
        /// <exception cref="Exception">a <paramref name="action"/>'s callback throws an exception.</exception>
        protected void UpdateUI(Action action)
        {
            if ((action == null) || (Application.Current == null))
            {
                return;
            }

            var dispatcher = Application.Current.Dispatcher;
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action);
            }
        }

        #endregion

        #region IRequestWaitViewModel

        public event EventHandler<EventArgs> RequestBlock;

        public event EventHandler<EventArgs> RequestUnblock;

        protected void OnRequestBlock()
        {
            RequestBlock?.Invoke(this, EventArgs.Empty);
        }

        protected void OnRequestUnblock()
        {
            RequestUnblock?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region IRequestCloseViewModel Implementation

        /// <summary>
        /// Is <see cref="RequestClose"/> event called.
        /// </summary>
        public bool IsCloseRequested { get; private set; }

        /// <summary>
        /// Close request event.
        /// </summary>
        public event EventHandler<DataEventArgs<bool?>> RequestClose;

        /// <summary>
        /// Raises <see cref="RequestClose"/> event.
        /// </summary>
        /// <exception cref="Exception"><see cref="RequestClose"/>'s callback throws an exception.</exception>
        public void OnRequestClose(bool? dialogResult = true)
        {
            IsCloseRequested = true;
            RequestClose?.Invoke(this, new DataEventArgs<bool?>(dialogResult));
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Flag if disposing process in progress.
        /// </summary>
        public bool IsDisposing { get; private set; }

        /// <summary>
        /// <see cref="BaseViewModel"/> destructor.
        /// </summary>
        ~BaseViewModel()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose current <see cref="BaseViewModel"/> instance.
        /// </summary>
        /// <param name="disposing">flag if <see cref="BaseViewModel"/> is disposing from destructor or <see cref="Dispose"/> method.</param>
        /// <returns>true - if disposing is started and should clean up resurces; false otherwise (if disposing process is already happening/happened).</returns>
        protected virtual bool Dispose(bool disposing)
        {
            if (disposing == false)
            {
                return false;
            }

            if (IsDisposing)
            {
                return false;
            }

            IsDisposing = true;
            return true;
        }

        #endregion
    }
}

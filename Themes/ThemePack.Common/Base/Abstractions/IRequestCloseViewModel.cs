using System;
using ThemePack.Common.Models;

namespace ThemePack.Common.Base.Abstractions
{
    /// <summary>
    /// Interface for ViewModel which can request view close.
    /// </summary>
    /// <date>18:09 05/14/2015</date>
    /// <author>Anton Liakhovich</author>
    public interface IRequestCloseViewModel
    {
        /// <summary>
        /// Close request event.
        /// </summary>
        event EventHandler<DataEventArgs<bool?>> RequestClose;
    }
}

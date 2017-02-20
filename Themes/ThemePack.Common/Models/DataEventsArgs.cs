using System;

namespace ThemePack.Common.Models
{
    /// <summary>
    /// Event arguments with data inside.
    /// </summary>
    /// <date>12:34 05/15/2015</date>
    /// <author>Anton Liakhovich</author>
    public class DataEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DataEventArgs{T}"/>
        /// </summary>
        public DataEventArgs(T data)
        {
            Data = data;
        }

        /// <summary>
        /// <see cref="DataEventArgs{T}"/>'s data.
        /// </summary>
        public T Data { get; private set; }
    }
}

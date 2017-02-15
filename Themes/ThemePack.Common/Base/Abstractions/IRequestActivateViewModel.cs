using System;

namespace ThemePack.Common.Base.Abstractions
{
    /// <summary>
    /// Interface for ViewModel which can request to activate window.
    /// </summary>
    public interface IRequestActivateViewModel
    {
        /// <summary>
        /// Activate window request event.
        /// </summary>
        event EventHandler<EventArgs> RequestActivate;
    }
}
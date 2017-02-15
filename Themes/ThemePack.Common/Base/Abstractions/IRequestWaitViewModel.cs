using System;

namespace ThemePack.Common.Base.Abstractions
{
    /// <summary>
    /// Interface for ViewModel which can request view close.
    /// </summary>
    /// <date>18:09 05/14/2015</date>
    /// <author>Anton Liakhovich</author>
    public interface IRequestWaitViewModel
    {
        event EventHandler<EventArgs> RequestBlock;
        event EventHandler<EventArgs> RequestUnblock;
    }
}

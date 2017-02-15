namespace ThemePack.Common.Base.Abstractions
{
    /// <summary>
    /// Interface for window can be blocked.
    /// </summary>
    /// <date>15:06 08/25/2015</date>
    /// <author>Anton Liakhovich</author>
    public interface IBlockableWindow
    {
        /// <summary>
        /// Is <see cref="IBlockableWindow"/> blocked.
        /// </summary>
        bool IsBlocked { get; set; }
    }
}

using System.Drawing;

namespace ThemePack.Common.Abstractions
{
    /// <summary>
    /// Interface for environment region.
    /// </summary>
    /// <date>15:35 11/24/2015</date>
    /// <author>Anton Liakhovich</author>
    public interface IEnvironmentRegion
    {
        Point From { get; }
        Point To { get; }
        Size Size { get; }
    }
}

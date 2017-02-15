namespace ThemePack.Common.Base.Abstractions
{
    /// <summary>
    /// Interface to represent environment rectangle data.
    /// </summary>
    /// <date>13:34 11/12/2015</date>
    /// <author>Anton Liakhovich</author>
    public interface IEnvironmentRectangle
    {
        /// <summary>
        /// Gets the width of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>A positive number that represents the width of <see cref="IEnvironmentRectangle"/>. The default is 0.</returns>
        int Width { get; }

        /// <summary>
        /// Gets the height of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>A positive number that represents the height of <see cref="IEnvironmentRectangle"/>. The default is 0.</returns>
        int Height { get; }

        /// <summary>
        /// Pixels array.
        /// </summary>
        byte[] Pixels { get; }

        /// <summary>
        /// Gets the x-axis value of the left side of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The x-axis value of the left side of <see cref="IEnvironmentRectangle"/>.</returns>
        int Left { get; }

        /// <summary>
        /// Gets the y-axis position of the top of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The y-axis position of the top of <see cref="IEnvironmentRectangle"/>.
        /// If <see cref="IEnvironmentRectangle"/> is <see cref="IEnvironmentRectangle.IsEmpty"/>, the value is less or equal than 0.</returns>
        int Top { get; }

        /// <summary>
        /// Gets the x-axis value of the right side of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The x-axis value of the right side of <see cref="IEnvironmentRectangle"/>.</returns>
        int Right { get; }

        /// <summary>
        /// Gets the y-axis value of the bottom of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The y-axis value of the bottom of <see cref="IEnvironmentRectangle"/>.
        /// If <see cref="IEnvironmentRectangle"/> is <see cref="IEnvironmentRectangle.IsEmpty"/>, the value is less or equal than 0.</returns>
        int Bottom { get; }

        /// <summary>
        /// Indicates whether the <paramref name="rect"/> intersects with the current <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <param name="rect"><see cref="IEnvironmentRectangle"/> to check.</param>
        /// <returns>true if the <paramref name="rect"/> intersects with the current <see cref="IEnvironmentRectangle"/>; otherwise, false.</returns>
        bool IntersectsWith(IEnvironmentRectangle rect);

        /// <summary>
        /// Finds the intersection of the current <see cref="IEnvironmentRectangle"/> and the <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect"><see cref="IEnvironmentRectangle"/> to intersect with the current <see cref="IEnvironmentRectangle"/>.</param>
        /// <returns>intersection of the current <see cref="IEnvironmentRectangle"/> and <paramref name="rect"/></returns>
        IEnvironmentRectangle Intersect(IEnvironmentRectangle rect);

        /// <summary>
        /// Gets a value that indicates whether the <see cref="IEnvironmentRectangle"/> is the <see cref="IEnvironmentRectangle"/> with non-positive <see cref="Width"/> or <see cref="Height"/>.
        /// </summary>
        /// <returns>true if the <see cref="IEnvironmentRectangle"/> is the empty <see cref="IEnvironmentRectangle"/>; otherwise, false.</returns>
        bool IsEmpty { get; }

        /// <summary>
        /// Get the area of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>A positive number that represents the area of <see cref="IEnvironmentRectangle"/>. If <see cref="IEnvironmentRectangle"/> is <see cref="IsEmpty"/> returns 0.</returns>
        int Area { get; }
    }
}

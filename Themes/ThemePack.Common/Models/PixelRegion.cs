using System;
using System.Drawing;
using ThemePack.Common.Abstractions;
using ThemePack.Common.Base.Abstractions;

namespace ThemePack.Common.Models
{
    public struct PixelRegion : IEnvironmentRectangle
    {
        /// <summary>
        /// Gets the x-axis value of the left side of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The x-axis value of the left side of <see cref="IEnvironmentRectangle"/>.</returns>
        public int Left { get; }

        /// <summary>
        /// Gets the y-axis position of the top of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The y-axis position of the top of <see cref="IEnvironmentRectangle"/>.
        /// If <see cref="IEnvironmentRectangle"/> is <see cref="IEnvironmentRectangle.IsEmpty"/>, the value is less or equal than 0.</returns>
        public int Top { get; }

        /// <summary>
        /// Gets the x-axis value of the right side of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The x-axis value of the right side of <see cref="IEnvironmentRectangle"/>.</returns>
        public int Right { get; }

        /// <summary>
        /// Gets the y-axis value of the bottom of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>The y-axis value of the bottom of <see cref="IEnvironmentRectangle"/>.
        /// If <see cref="IEnvironmentRectangle"/> is <see cref="IEnvironmentRectangle.IsEmpty"/>, the value is less or equal than 0.</returns>
        public int Bottom { get; }

        /// <summary>
        /// Gets or sets the width of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>A positive number that represents the width of <see cref="IEnvironmentRectangle"/>. The default is 0.</returns>
        public int Width { get; }

        /// <summary>
        /// Gets or sets the height of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>A positive number that represents the height of <see cref="IEnvironmentRectangle"/>. The default is 0.</returns>
        public int Height { get; }

        /// <summary>
        /// Pixels array.
        /// </summary>
        public byte[] Pixels { get; }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="IEnvironmentRectangle"/> is the <see cref="IEnvironmentRectangle"/> with non-positive <see cref="Width"/> or <see cref="Height"/>.
        /// </summary>
        /// <returns>true if the <see cref="IEnvironmentRectangle"/> is the empty <see cref="IEnvironmentRectangle"/>; otherwise, false.</returns>
        public bool IsEmpty => (Width <= 0) || (Height <= 0);

        public PixelRegion(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Bottom = top + height - 1;
            Right = left + width - 1;

            Pixels = new byte[width * height * ProtocolSettings.PixelSize];
        }

        public PixelRegion(int left, int top, int width, int height, byte[] pixels)
        {
            if (pixels == null)
            {
                throw new ArgumentNullException(nameof(pixels));
            }

            if (pixels.Length != width * height * ProtocolSettings.PixelSize)
            {
                throw new ArgumentException("pixels array does not suite width/height", nameof(pixels));
            }

            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Bottom = top + height - 1;
            Right = left + width - 1;

            Pixels = pixels;
        }

        public PixelRegion Crop(Rectangle bounds)
        {
            var currentBounds = new Rectangle(Left, Top, Width, Height);

            var inter = Rectangle.Intersect(currentBounds, bounds);

            if (inter == currentBounds)
            {
                return this;
            }
            else
            {
                var region = new PixelRegion(inter.Left, inter.Top, inter.Width, inter.Height);

                var srcPointer = (region.Top - Top) * Width + (region.Left - Left);
                var dstPointer = 0;

                var srcOffset = Width;
                var dstOffset = region.Width;

                for (var row = 0; row < inter.Height; row += 1)
                {
                    Array.Copy(Pixels, srcPointer, region.Pixels, dstPointer, region.Width);

                    srcPointer += srcOffset;
                    dstPointer += dstOffset;
                }

                return region;
            }

        }

        #region Implementation of IEnvironmentRectangle
        
        /// <summary>
        /// Indicates whether the <paramref name="rect"/> intersects with the current <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <param name="rect"><see cref="IEnvironmentRectangle"/> to check.</param>
        /// <returns>true if the <paramref name="rect"/> intersects with the current <see cref="IEnvironmentRectangle"/>; otherwise, false.</returns>
        public bool IntersectsWith(IEnvironmentRectangle rect)
        {
            return (IsEmpty == false) && (rect.IsEmpty == false) &&
                   (rect.Left <= Right) && (rect.Right >= Left) &&
                   (rect.Top <= Bottom) && (rect.Bottom >= Top);
        }

        /// <summary>
        /// Finds the intersection of the current <see cref="IEnvironmentRectangle"/> and <paramref name="rect"/>.
        /// </summary>
        /// <param name="rect"><see cref="IEnvironmentRectangle"/> to intersect with the current <see cref="IEnvironmentRectangle"/>.</param>
        /// <returns>intersection of the current <see cref="IEnvironmentRectangle"/> and <paramref name="rect"/></returns>
        public IEnvironmentRectangle Intersect(IEnvironmentRectangle rect)
        {
            if (IntersectsWith(rect) == false)
            {
                return Empty;
            }

            var left = Math.Max(Left, rect.Left);
            var right = Math.Max(Top, rect.Top);
            var width = Math.Max(Math.Min(Right, rect.Right) - left + 1, 0);
            var height = Math.Max(Math.Min(Bottom, rect.Bottom) - right + 1, 0);

            return new PixelRegion(left, right, width, height);
        }

        /// <summary>
        /// Get the area of <see cref="IEnvironmentRectangle"/>.
        /// </summary>
        /// <returns>A positive number that represents the area of <see cref="IEnvironmentRectangle"/>. If <see cref="IEnvironmentRectangle"/> is <see cref="IEnvironmentRectangle.IsEmpty"/> returns 0.</returns>
        public int Area => IsEmpty ? 0 : Width*Height;

        #endregion

        public override string ToString()
        {
            return $"{nameof(PixelRegion)}: ({Left};{Top}). size {Width}x{Height}, {Pixels.Length} bytes";
        }

        #region Equality members

        /// <summary>
        /// Indicates whether this instance and a specified <paramref name="obj"/> are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance. </param> 
        /// <returns>true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if ((obj is PixelRegion) == false)
            {
                return false;
            }

            var region = (PixelRegion) obj;
            return Top == region.Top && Left == region.Left && Width == region.Width && Height == region.Height;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Top;
                hashCode = (hashCode*397) ^ Left;
                hashCode = (hashCode*397) ^ Width;
                hashCode = (hashCode*397) ^ Height;
                return hashCode;
            }
        }

        #endregion

        #region Empty

        /// <summary>
        /// Create empty <see cref="PixelRegion"/>.
        /// </summary>
        /// <returns>empty <see cref="PixelRegion"/>.</returns>
        private static PixelRegion CreateEmpty()
        {
            return new PixelRegion(0, 0, 0, 0);
        }

        /// <summary>
        /// Gets a special <see cref="PixelRegion"/> that represents a rectangle with no area.
        /// </summary>
        /// <returns>The empty <see cref="PixelRegion"/>, which has has <see cref="Width" /> and <see cref="Height" /> property values of 0.</returns>
        public static PixelRegion Empty { get; } = CreateEmpty();

        #endregion
    }
}

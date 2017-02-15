using System;
using System.IO;

namespace ThemePack.Common.Models
{
    public struct PackedRegion
    {
        public int Top { get; }
        public int Left { get; }
        public int Width { get; }
        public int Height { get; }

        public int SizeInBytes { get { return Width * Height * ProtocolSettings.PixelSize; } }
        public int CompressedSizeInBytes { get { return CompressedPixels.Length; } }

        public byte[] CompressedPixels { get; }

        public PackedRegion(int left, int top, int width, int height, byte[] compressedPixels)
        {
            if (compressedPixels == null)
            {
                throw new ArgumentNullException(nameof(compressedPixels));
            }

            Left = left;
            Top = top;
            Width = width;
            Height = height;

            CompressedPixels = compressedPixels;
        }

        public static PackedRegion Read(BinaryReader reader)
        {
            var left = reader.ReadInt32();
            var top = reader.ReadInt32();
            var width = reader.ReadInt32();
            var height = reader.ReadInt32();
            var dataLength = reader.ReadInt32();
            var compressedPixels = reader.ReadBytes(dataLength);

            return new PackedRegion(left, top, width, height, compressedPixels);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Left);
            writer.Write(Top);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(CompressedPixels.Length);
            writer.Write(CompressedPixels);
        }

        public override string ToString()
        {
            return $"{nameof(PackedRegion)}: ({Left};{Top}). size {Width}x{Height}, {CompressedPixels.Length} bytes";
        }
    }
}

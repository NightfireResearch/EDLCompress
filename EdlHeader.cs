using System;
using System.IO;
using System.Linq;

namespace wdcossey;

public partial class EdlCompress
{
    public record EdlHeader
    {
        private static readonly char[] EdlHeaderIdentifier = { 'E', 'D', 'L' };

        /// <summary>compression type 0-2</summary>
        public int CompressionType { get; set; }

        /// <summary>big(1) or little(0) endian</summary>
        public EdlEndianType Endian { get; set; }

        /// <summary>compressed size</summary>
        public long CompressedSize { get; set; }

        /// <summary>decompressed size</summary>
        public long DecompressedSize { get; set; }

        private static bool ValidateHeader(BinaryReader reader)
        {
            var headerChars = reader.ReadChars(3);
            return headerChars.SequenceEqual(EdlHeaderIdentifier);
        }

        public static EdlHeader Parse(BinaryReader reader)
        {
            if (!ValidateHeader(reader))
                throw new InvalidOperationException("Does not contain a valid EDL header");

            var compressionType = reader.ReadByte();
            var endianType = (EdlEndianType)(compressionType >> 7);

            var compressedSize = reader.ReadUInt32();
            var decompressedSize = reader.ReadUInt32();

            if (endianType == EdlEndianType.Big)
            {
                compressedSize = ByteSwap(compressedSize);
                decompressedSize = ByteSwap(decompressedSize);
            }

            return new EdlHeader
            {
                Endian = endianType,
                CompressionType = compressionType & 0xF,
                CompressedSize = Convert.ToInt64(compressedSize),
                DecompressedSize = Convert.ToInt64(decompressedSize),
            };
        }
    }
}

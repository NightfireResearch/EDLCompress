using System;
using System.IO;

namespace wdcossey
{
    public partial class EdlCompress
    {
        public record EdlOffsetHeader : EdlHeader
        {
            public long Offset { get; set; }

            public new static EdlHeader Parse(BinaryReader reader)
            {
                var offset = reader.BaseStream.Position - 3;
                var x = reader.ReadByte();
                var compressionType = x & 0xF;
                var endianType = (EdlEndianType)(x >> 7);

                var compressedSize = reader.ReadUInt32();
                var decompressedSize = reader.ReadUInt32();

                if (endianType == EdlEndianType.Big)
                {
                    compressedSize = ByteSwap(compressedSize);
                    decompressedSize = ByteSwap(decompressedSize);
                }

                return new EdlOffsetHeader
                {
                    Endian = endianType,
                    CompressionType = compressionType,
                    CompressedSize = Convert.ToInt64(compressedSize),
                    DecompressedSize = Convert.ToInt64(decompressedSize),
                    Offset = offset
                };
            }
        }
    }
}

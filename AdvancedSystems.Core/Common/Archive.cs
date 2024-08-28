using System;
using System.IO;
using System.IO.Compression;

namespace AdvancedSystems.Core.Common;

public static class Archive
{
    public static byte[] Compress(ReadOnlyMemory<byte> expandedBuffer, CompressionLevel compressionLevel)
    {
        if (expandedBuffer.IsEmpty) throw new ArgumentException("Buffer to compress cannot be empty.", nameof(expandedBuffer));
        using var compressedStream = new MemoryStream();

        unsafe
        {
            fixed (byte* pointer = &expandedBuffer.Span[0])
            {
                using var expandedStream = new UnmanagedMemoryStream(pointer, expandedBuffer.Length);

                using var gzipStream = new GZipStream(compressedStream, compressionLevel, leaveOpen: false);
                expandedStream.CopyTo(gzipStream);
            }
        }

        return compressedStream.ToArray();
    }

    public static byte[] Expand(ReadOnlyMemory<byte> compressedBuffer)
    {
        if (compressedBuffer.IsEmpty) throw new ArgumentException("Buffer to expand cannot be empty.", nameof(compressedBuffer));
        using var expandedStream = new MemoryStream();
        
        unsafe
        {
            fixed (byte* pointer = &compressedBuffer.Span[0])
            {
                using var compressedStream = new UnmanagedMemoryStream(pointer, compressedBuffer.Length);

                using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress, leaveOpen: false);
                gzipStream.CopyTo(expandedStream);
            }
        }

        return expandedStream.ToArray();
    }
}

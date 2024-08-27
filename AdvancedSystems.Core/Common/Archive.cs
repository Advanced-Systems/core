using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace AdvancedSystems.Core.Common;

public static class Archive
{
    public static byte[] Compress(Stream expandedStream, CompressionLevel compressionLevel)
    {
        using var compressedStream = new MemoryStream();

        using var gzipStream = new GZipStream(expandedStream, compressionLevel, leaveOpen: false);
        expandedStream.CopyTo(gzipStream);

        return compressedStream.ToArray();
    }

    public static byte[] Compress(ReadOnlyMemory<byte> expandedBuffer, CompressionLevel compressionLevel)
    {
        using var compressedStream = new MemoryStream();
        GCHandle handle = GCHandle.Alloc(expandedBuffer, GCHandleType.Pinned);

        try
        {
            unsafe
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                using var expandedStream = new UnmanagedMemoryStream((byte*)pointer, expandedBuffer.Length);

                using var gzipStream = new GZipStream(compressedStream, compressionLevel, leaveOpen: false);
                expandedStream.CopyTo(gzipStream);
            }

            return compressedStream.ToArray();
        }
        finally
        {
            handle.Free();
        }
    }

    public static byte[] Expand(Stream compressedStream)
    {
        using var uncompressedStream = new MemoryStream();

        using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress, leaveOpen: false);
        gzipStream.CopyTo(uncompressedStream);

        return uncompressedStream.ToArray();
    }

    public static byte[] Expand(ReadOnlyMemory<byte> compressedBuffer)
    {
        using var expandedStream = new MemoryStream();
        GCHandle handle = GCHandle.Alloc(compressedBuffer, GCHandleType.Pinned);

        try
        {
            unsafe
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                using var compressedStream = new UnmanagedMemoryStream((byte*)pointer, compressedBuffer.Length);

                using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress, leaveOpen: false);
                gzipStream.CopyTo(expandedStream);
            }

            return expandedStream.ToArray();
        }
        finally
        {
            handle.Free();
        }
    }
}

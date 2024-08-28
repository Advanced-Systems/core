using System;
using System.IO.Compression;

namespace AdvancedSystems.Core.Abstractions;

/// <summary>
///     Defines methods for compressing and decompressing data.
/// </summary>
public interface ICompressionService
{
    #region Methods

    /// <summary>
    ///     Compresses the given byte array using the specified compression level.
    /// </summary>
    /// <param name="expandedBuffer">
    ///     The data to be compressed.
    /// </param>
    /// <param name="compressionLevel">
    ///     The level of compression to apply.
    /// </param>
    /// <returns>
    ///     A byte array containing the compressed data.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="expandedBuffer"/> is empty.
    /// </exception>
    byte[] Compress(ReadOnlyMemory<byte> expandedBuffer, CompressionLevel compressionLevel);

    /// <summary>
    ///     Expands (decompresses) the given byte array that has been compressed.
    /// </summary>
    /// <param name="compressedBuffer">
    ///     The compressed data to be expanded
    /// </param>
    /// <returns>
    ///     A byte array containing the decompressed data.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="compressedBuffer"/> is empty.
    /// </exception>
    /// <remarks>
    ///     This method assumes that the provided <paramref name="compressedBuffer"/> contains data that was compressed using
    ///     a compatible compression algorithm (see also: <seealso cref="Compress(ReadOnlyMemory{byte}, CompressionLevel)"/>).
    /// </remarks>
    byte[] Expand(ReadOnlyMemory<byte> compressedBuffer);

    #endregion
}

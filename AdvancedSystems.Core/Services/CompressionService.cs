using System;
using System.IO.Compression;

using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Common;

using Microsoft.Extensions.Logging;

namespace AdvancedSystems.Core.Services;

/// <inheritdoc cref="ICompressionService" />
public sealed class CompressionService : ICompressionService
{
    private readonly ILogger<CompressionService> _logger;

    public CompressionService(ILogger<CompressionService> logger)
    {
        this._logger = logger;
    }

    #region Methods

    /// <inheritdoc />
    public byte[] Compress(ReadOnlyMemory<byte> expandedBuffer, CompressionLevel compressionLevel)
    {
        this._logger.LogTrace("Compression Level = {CompressionLevel}", compressionLevel);
        return Archive.Compress(expandedBuffer, compressionLevel);
    }

    /// <inheritdoc />
    public byte[] Expand(ReadOnlyMemory<byte> compressedBuffer)
    {
        return Archive.Expand(compressedBuffer);
    }

    #endregion
}

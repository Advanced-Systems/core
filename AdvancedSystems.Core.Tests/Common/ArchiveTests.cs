using System;
using System.IO.Compression;
using System.Linq;

using AdvancedSystems.Core.Common;

using Xunit;

namespace AdvancedSystems.Core.Tests.Common;

public class ArchiveTests
{
    #region Tests

    [Fact]
    public void TestCompress()
    {
        // Arrange
        byte[] buffer = Enumerable.Repeat<byte>(0xFF, 100).ToArray();

        // Act
        byte[] compressedBuffer = Archive.Compress(buffer, CompressionLevel.Optimal);

        // Assert
        Assert.NotEmpty(compressedBuffer);
        Assert.True(buffer.Length > compressedBuffer.Length);
    }

    [Fact]
    public void TestCompress_ThrowsIfEmpty()
    {
        // Arrange
        byte[] buffer =  Enumerable.Empty<byte>().ToArray();

        // Act
        void Compress()
        {
            Archive.Compress(buffer, CompressionLevel.Optimal);
        }

        // Assert
        Assert.Throws<ArgumentException>(Compress);
    }

    [Fact]
    public void TestExpand()
    {
        // Arrange
        byte[] buffer = Enumerable.Repeat<byte>(0xFF, 100).ToArray();

        // Act
        byte[] compressedBuffer = Archive.Compress(buffer, CompressionLevel.Optimal);
        byte[] expandedBuffer = Archive.Expand(compressedBuffer);

        // Assert
        Assert.Equal(buffer, expandedBuffer);
    }

    [Fact]
    public void TestExpand_ThrowsIfEmpty()
    {
        // Arrange
        byte[] buffer = Enumerable.Empty<byte>().ToArray();

        // Act
        void Expand()
        {
            Archive.Expand(buffer);
        }

        // Assert
        Assert.Throws<ArgumentException>(Expand);
    }

    #endregion
}

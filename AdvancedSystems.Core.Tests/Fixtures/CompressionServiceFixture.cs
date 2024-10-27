using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

using Microsoft.Extensions.Logging;

using Moq;

namespace AdvancedSystems.Core.Tests.Fixtures;

public class CompressionServiceFixture
{
    public CompressionServiceFixture()
    {
        this.CompressionService = new CompressionService(this.Logger.Object);
    }

    #region Properties

    public Mock<ILogger<CompressionService>> Logger { get; private set; } = new();

    public ICompressionService CompressionService { get; private set; }

    #endregion
}
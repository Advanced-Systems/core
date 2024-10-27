using AdvancedSystems.Core.Abstractions;
using AdvancedSystems.Core.Services;

namespace AdvancedSystems.Core.Tests.Fixtures;

public class SerializationServiceFixture
{
    public SerializationServiceFixture()
    {
        this.SerializationService = new SerializationService();
    }

    #region Properties

    public ISerializationService SerializationService { get; private set; }

    #endregion
}
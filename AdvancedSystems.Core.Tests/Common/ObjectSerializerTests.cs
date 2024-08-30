using AdvancedSystems.Core.Common;
using AdvancedSystems.Core.Tests.Models;

using Xunit;

namespace AdvancedSystems.Core.Tests.Common;

public class ObjectSerializerTests
{
    #region Tests

    [Fact]
    public void TestSerializationRoundtrip()
    {
        // Arrange
        var expected = new Person("Stefan", "Greve");

        // Act
        var serialized = ObjectSerializer.Serialize(expected, PersonContext.Default.Person);
        Person? actual = ObjectSerializer.Deserialize(serialized, PersonContext.Default.Person);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
    }

    #endregion
}

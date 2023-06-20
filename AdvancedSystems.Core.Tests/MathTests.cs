using AdvancedSystems.Core;

using Xunit;

namespace AdvancedSystems.Core.Tests;

public class MathTests
{
    [Fact]
    public static void AddTest()
    {
        // arrange
        int a = 3;
        int b = 2;

        // act
        int c = Math.Add(a, b);

        // assert
        Assert.Equal(5, c);
    }
}

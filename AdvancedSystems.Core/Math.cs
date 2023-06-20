using System.Numerics;

namespace AdvancedSystems.Core;

public class Math
{
    public static T Add<T>(T a, T b) where T : INumber<T> => a + b;
}

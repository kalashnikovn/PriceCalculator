using PriceCalculator.Dal.Entities;

namespace PriceCalculator.UnitTests.Comparers;

public class GoodEntityV1Comparer : IEqualityComparer<GoodEntityV1>
{
    public bool Equals(GoodEntityV1? x, GoodEntityV1? y)
    {
        return x?.Length == y?.Length
               && x?.Height == y?.Height
               && x?.Width == y?.Width
               && x?.Weight == y?.Weight;
    }

    public int GetHashCode(GoodEntityV1 obj)
    {
        return HashCode.Combine(obj.Length, obj.Height, obj.Weight, obj.Width);
    }
}

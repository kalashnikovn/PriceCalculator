namespace PriceCalculator.UnitTests.Extensions;

public static class ComparisonExtensions
{
    public static bool SequenceEqualIgnoreOrder<T>(
        this IEnumerable<T> left,
        IEnumerable<T> right)
        where T : IEquatable<T>
    {
        return left.OrderBy(t => t.GetHashCode())
            .SequenceEqual(right.OrderBy(t => t.GetHashCode()));
    }
}
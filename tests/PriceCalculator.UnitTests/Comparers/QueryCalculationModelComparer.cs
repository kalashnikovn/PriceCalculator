using PriceCalculator.Bll.Models;

namespace PriceCalculator.UnitTests.Comparers;

public class QueryCalculationModelComparer: IEqualityComparer<QueryCalculationModel>
{
    private const double tolerance = 1e-8;
    
    public bool Equals(QueryCalculationModel? x, QueryCalculationModel? y)
    {
        if (x is null && y is null)
        {
            return true;
        }

        if ((x is null && y is not null) || (x is not null && y is null))
        {
            return false;
        }

        return x!.UserId == y!.UserId
               && x.Price == y.Price
               && Math.Abs(x.TotalVolume - y.TotalVolume) < tolerance
               && Math.Abs(x.TotalWeight - y.TotalWeight) < tolerance
               && x.GoodIds.SequenceEqual(y.GoodIds);
    }

    public int GetHashCode(QueryCalculationModel obj)
    {
        return HashCode.Combine(
            obj.UserId, 
            obj.Price, 
            obj.TotalVolume, 
            obj.TotalWeight,
            obj.GoodIds);
    }
    
}

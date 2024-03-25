using AutoBogus;
using Bogus;
using PriceCalculator.Dal.Models;

namespace PriceCalculator.UnitTests.Fakers;

public static class GetHistoryQueryModelFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<GetHistoryQueryModel> Faker = new AutoFaker<GetHistoryQueryModel>()
        .RuleFor(x => x.UserId, f => f.Random.Long(0L))
        .RuleFor(x => x.Limit, f => f.Random.Int(1, 5))
        .RuleFor(x => x.Offset, f => f.Random.Int(0, 5));
    
    public static GetHistoryQueryModel Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }
    
    public static GetHistoryQueryModel WithUserId(
        this GetHistoryQueryModel src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static GetHistoryQueryModel WithLimit(
        this GetHistoryQueryModel src, 
        int limit)
    {
        return src with { Limit = limit };
    }
    
    public static GetHistoryQueryModel WithOffset(
        this GetHistoryQueryModel src, 
        int offset)
    {
        return src with { Offset = offset };
    }
}
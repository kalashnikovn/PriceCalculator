using AutoBogus;
using Bogus;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.UnitTests.Fakers;

public static class QueryCalculationModelFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<QueryCalculationModel> Faker = new AutoFaker<QueryCalculationModel>()
        .RuleFor(x => x.Id, f => f.Random.Long(0L))
        .RuleFor(x => x.UserId, f => f.Random.Long(0L));
    
    public static QueryCalculationModel[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Enumerable.Repeat(Faker.Generate(), count)
                .ToArray();
        }
    }
    
    public static QueryCalculationModel WithUserId(
        this QueryCalculationModel src, 
        long userId)
    {
        return src with { UserId = userId };
    }
}
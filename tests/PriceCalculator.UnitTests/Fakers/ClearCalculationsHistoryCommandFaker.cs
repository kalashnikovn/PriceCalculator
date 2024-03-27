using AutoBogus;
using Bogus;
using PriceCalculator.Bll.Commands;

namespace PriceCalculator.UnitTests.Fakers;

public static class ClearCalculationsHistoryCommandFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<ClearCalculationsHistoryCommand> Faker =
        new AutoFaker<ClearCalculationsHistoryCommand>()
            .RuleFor(x => x.UserId, f => f.Random.Long(0L));

    public static ClearCalculationsHistoryCommand Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }

    public static ClearCalculationsHistoryCommand WithUserId(
        this ClearCalculationsHistoryCommand src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static ClearCalculationsHistoryCommand WithCalculationIds(
        this ClearCalculationsHistoryCommand src, 
        long[] calculationIds)
    {
        return src with { CalculationIds = calculationIds };
    }
}
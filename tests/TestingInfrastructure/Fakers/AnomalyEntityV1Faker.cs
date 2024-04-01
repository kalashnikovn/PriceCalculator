using AutoBogus;
using Bogus;
using PriceCalculator.Dal.Entities;

namespace TestingInfrastructure.Fakers;

public static class AnomalyEntityV1Faker
{
    private static readonly object Lock = new();

    private static readonly Faker<AnomalyEntityV1> Faker = new AutoFaker<AnomalyEntityV1>()
        .RuleFor(x => x.Id, f => f.Random.Long(0L))
        .RuleFor(x => x.GoodId, f => f.Random.Long(0L))
        .RuleFor(x => x.Price, f => f.Random.Decimal());

    public static AnomalyEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Enumerable
                .Repeat(Faker.Generate(), count)
                .ToArray();
        }
    }

    public static AnomalyEntityV1 WithId(
        this AnomalyEntityV1 src, 
        long id)
    {
        return src with { Id = id };
    }

    public static AnomalyEntityV1 WithGoodId(
        this AnomalyEntityV1 src,
        long goodId)
    {
        return src with { GoodId = goodId };
    }
    
    public static AnomalyEntityV1 WithPrice(
        this AnomalyEntityV1 src,
        decimal price)
    {
        return src with { Price = price };
    }
    
}
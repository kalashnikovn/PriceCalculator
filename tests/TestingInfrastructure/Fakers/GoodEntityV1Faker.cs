using AutoBogus;
using Bogus;
using PriceCalculator.Dal.Entities;

namespace TestingInfrastructure.Fakers;

public static class GoodEntityV1Faker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<GoodEntityV1> Faker = new AutoFaker<GoodEntityV1>()
        .RuleFor(x => x.Id, f => f.Random.Long(0L));

    public static GoodEntityV1[] Generate(int count = 1)
    {
        lock (Lock)
        {
            return Enumerable.Repeat(Faker.Generate(), count)
                .ToArray();
        }
    }

    public static GoodEntityV1 WithUserId(
        this GoodEntityV1 src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static GoodEntityV1 WithHeight(
        this GoodEntityV1 src, 
        double height)
    {
        return src with { Height = height };
    }
    
    public static GoodEntityV1 WithLength(
        this GoodEntityV1 src, 
        double length)
    {
        return src with { Length = length };
    }
    
    public static GoodEntityV1 WithWidth(
        this GoodEntityV1 src, 
        double width)
    {
        return src with { Width = width };
    }
    
    public static GoodEntityV1 WithWeight(
        this GoodEntityV1 src, 
        double weight)
    {
        return src with { Weight = weight };
    }
}
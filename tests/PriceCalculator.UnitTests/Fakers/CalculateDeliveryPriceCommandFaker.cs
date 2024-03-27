using AutoBogus;
using Bogus;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.UnitTests.Fakers;

public static class CalculateDeliveryPriceCommandFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<CalculateDeliveryPriceCommand> Faker = new AutoFaker<CalculateDeliveryPriceCommand>()
        .RuleFor(x => x.UserId, f => f.Random.Long(0L))
        .RuleFor(x => x.Goods, f => GoodModelFaker.Generate(f.Random.Int(1, 4)).ToArray());

    public static CalculateDeliveryPriceCommand Generate()
    {
        lock (Lock)
        {
            return Faker.Generate();
        }
    }

    public static CalculateDeliveryPriceCommand WithUserId(
        this CalculateDeliveryPriceCommand src, 
        long userId)
    {
        return src with { UserId = userId };
    }
    
    public static CalculateDeliveryPriceCommand WithGoods(
        this CalculateDeliveryPriceCommand src, 
        GoodModel[] goods)
    {
        return src with { Goods = goods };
    }
    
}
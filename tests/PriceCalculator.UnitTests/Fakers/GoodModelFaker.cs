using AutoBogus;
using Bogus;
using PriceCalculator.Bll.Models;

namespace PriceCalculator.UnitTests.Fakers;

public static class GoodModelFaker
{
    private static readonly object Lock = new();
    
    private static readonly Faker<GoodModel> Faker = new AutoFaker<GoodModel>();
    
    public static IEnumerable<GoodModel> Generate(int count = 1)
    {
        lock (Lock)
        {
            return Enumerable.Repeat(Faker.Generate(), count);
        }
    }
}
using AutoBogus;
using Bogus;
using Microsoft.Extensions.Options;
using PriceCalculator.ProducerApp.Interfaces;
using PriceCalculator.ProducerApp.Messages;
using PriceCalculator.ProducerApp.Options;

namespace PriceCalculator.ProducerApp.Fakers;

public sealed class CalculateRequestMessageFaker : IModelFaker<CalculateRequestMessage>
{
    private readonly Faker<CalculateRequestMessage> _faker;

    public CalculateRequestMessageFaker(
        IOptions<RandomOptions> options)
    {
        var randomOptions = options.Value;
        
        _faker = new AutoFaker<CalculateRequestMessage>()
            .RuleFor(x => x.GoodId,
                f => f.Random.Long(1L, randomOptions.MaxGoodId))
            .RuleFor(x => x.Height,
                f => f.Random.Double(0.01D, randomOptions.MaxHeight))
            .RuleFor(x => x.Length,
                f => f.Random.Double(0.01D, randomOptions.MaxLength))
            .RuleFor(x => x.Width,
                f => f.Random.Double(0.01D, randomOptions.MaxWidth))
            .RuleFor(x => x.Weight,
                f => f.Random.Double(0.01D, randomOptions.MaxWeight));
    }
    
    public List<CalculateRequestMessage> GenerateMany(int count = 1) => _faker.Generate(count);
}
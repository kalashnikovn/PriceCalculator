using FluentAssertions;
using PriceCalculator.Dal.Repositories.Interfaces;
using PriceCalculator.IntegrationTests.Fixtures;
using TestingInfrastructure.Fakers;
using Xunit;

namespace PriceCalculator.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class AnomalyRepositoryTests
{
    private readonly IAnomalyRepository _anomalyRepository;
    
    public AnomalyRepositoryTests(
        TestFixture fixture)
    {
        _anomalyRepository = fixture.AnomalyRepository;
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public async Task Add_Anomaly_Success(int count)
    {
        // Arrange
        var calculations = AnomalyEntityV1Faker.Generate(count)
            .ToArray();
        
        // Act
        var calculationIds = await _anomalyRepository.Add(calculations, default);

        // Asserts
        calculationIds.Should().HaveCount(count);
        calculationIds.Should().OnlyContain(x => x > 0);
    }
}
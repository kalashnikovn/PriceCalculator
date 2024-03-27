using Xunit;

namespace PriceCalculator.IntegrationTests.Fixtures;

[CollectionDefinition(nameof(TestFixture))]
public  class FixtureDefinition : ICollectionFixture<TestFixture>
{
    
}
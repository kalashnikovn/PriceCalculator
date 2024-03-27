using FluentAssertions;
using PriceCalculator.Dal.Models;
using PriceCalculator.IntegrationTests.Fixtures;
using PriceCalculator.Dal.Repositories.Interfaces;
using TestingInfrastructure.Creators;
using TestingInfrastructure.Fakers;
using Xunit;
using Xunit.Abstractions;

namespace PriceCalculator.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class CalculationsRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly double _requiredDoublePrecision = 0.00001d;
    private readonly decimal _requiredDecimalPrecision = 0.00001m;
    private readonly TimeSpan _requiredDateTimePrecision = TimeSpan.FromMilliseconds(100);
    
    private readonly ICalculationsRepository _calculationRepository;

    public CalculationsRepositoryTests(
        TestFixture fixture,
        ITestOutputHelper testOutputHelper
        )
    {
        _testOutputHelper = testOutputHelper;
        _calculationRepository = fixture.CalculationRepository;
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public async Task Add_Calculations_Success(int count)
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(count)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();
        
        // Act
        var calculationIds = await _calculationRepository.Add(calculations, default);

        // Asserts
        calculationIds.Should().HaveCount(count);
        calculationIds.Should().OnlyContain(x => x > 0);
    }
    
    [Fact]
    public async Task Query_SingleCalculation_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate()
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();
        var expected = calculations.Single();
        
        var calculationId = (await _calculationRepository.Add(calculations, default))
            .Single();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new GetHistoryQueryModel(userId, 1, 0), 
            default);

        // Asserts
        foundCalculations.Should().HaveCount(1);
        var calculation = foundCalculations.Single();

        calculation.Id.Should().Be(calculationId);
        calculation.UserId.Should().Be(expected.UserId);
        calculation.At.Should().BeCloseTo(expected.At, _requiredDateTimePrecision);
        calculation.Price.Should().BeApproximately(expected.Price, _requiredDecimalPrecision);
        calculation.GoodsId.Should().BeEquivalentTo(expected.GoodsId);
        calculation.TotalVolume.Should().BeApproximately(expected.TotalVolume, _requiredDoublePrecision);
        calculation.TotalWeight.Should().BeApproximately(expected.TotalWeight, _requiredDoublePrecision);
    }
    
    [Fact]
    public async Task Query_SingleCalculationById_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate()
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();
        var expected = calculations.Single();
        
        var calculationId = (await _calculationRepository.Add(calculations, default))
            .Single();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new []{calculationId}, 
            default);

        // Asserts
        foundCalculations.Should().HaveCount(1);
        var calculation = foundCalculations.Single();

        calculation.Id.Should().Be(calculationId);
        calculation.UserId.Should().Be(expected.UserId);
        calculation.At.Should().BeCloseTo(expected.At, _requiredDateTimePrecision);
        calculation.Price.Should().BeApproximately(expected.Price, _requiredDecimalPrecision);
        calculation.GoodsId.Should().BeEquivalentTo(expected.GoodsId);
        calculation.TotalVolume.Should().BeApproximately(expected.TotalVolume, _requiredDoublePrecision);
        calculation.TotalWeight.Should().BeApproximately(expected.TotalWeight, _requiredDoublePrecision);
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Query_AllCalculationsByIds_Success(int count)
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(count)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();
        
        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            calculationIds.ToArray(), 
            default);

        // Asserts
        foundCalculations.Should().HaveCount(count);
        foundCalculations.Should().NotBeEmpty();
        foundCalculations.Should().OnlyContain(x => x.UserId == userId);
        foundCalculations.Should().OnlyContain(x => calculationIds.Contains(x.Id));
    }
    
    [Theory]
    [InlineData(3,  2, 3)]
    [InlineData(1,  6, 1)]
    [InlineData(2,  8, 2)]
    [InlineData(3, 10, 0)]
    public async Task Query_CalculationsInRange_Success(int take, int skip, int expectedCount)
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(10)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        await _calculationRepository.Add(calculations, default);

        var allCalculations = await _calculationRepository.Query(
            new GetHistoryQueryModel(userId, 100, 0), 
            default);
        
        var expected = allCalculations
            .OrderByDescending(x => x.At)
            .Skip(skip)
            .Take(take);
        
        // Act
        var foundCalculations = await _calculationRepository.Query(
            new GetHistoryQueryModel(userId, take, skip), 
            default);

        // Asserts
        foundCalculations.Should().HaveCount(expectedCount);

        if (expectedCount > 0)
        {
            foundCalculations.Should().BeEquivalentTo(expected);
        }
    }
    
    [Fact]
    public async Task Query_AllCalculations_Success()
    {
        // Arrange
        var userId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new GetHistoryQueryModel(userId, 100, 0), 
            default);

        // Assert
        foundCalculations.Should().NotBeEmpty();
        foundCalculations.Should().OnlyContain(x => x.UserId == userId);
        foundCalculations.Should().OnlyContain(x => calculationIds.Contains(x.Id));
        foundCalculations.Should().BeInDescendingOrder(x => x.At);
    }
    
    [Fact]
    public async Task Query_Calculations_ReturnsEmpty_WhenForWrongUser()
    {
        // Arrange
        var userId = Create.RandomId();
        var anotherUserId = Create.RandomId();
        var now = DateTimeOffset.UtcNow;

        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = (await _calculationRepository.Add(calculations, default))
            .ToHashSet();

        // Act
        var foundCalculations = await _calculationRepository.Query(
            new GetHistoryQueryModel(anotherUserId, 100, 0), 
            default);

        // Assert
        foundCalculations.Should().BeEmpty();
    }
    
    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public async Task Remove_CalculationsByIds_Success(int count)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var calculations = CalculationEntityV1Faker.Generate(count)
            .Select(x => x.WithAt(now))
            .ToArray();
        
        var ids = await _calculationRepository.Add(calculations, default);
        
        // Act
        var rowsAffected = await _calculationRepository.Remove(ids, default);

        // Assert
        _testOutputHelper.WriteLine($"Rows affected: {rowsAffected}");
        Assert.True(rowsAffected > 0);
        rowsAffected.Should().Be(calculations.Length);
        
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Remove_CalculationsByUserId_Success(int count)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var userId = Create.RandomId();
        var calculations = CalculationEntityV1Faker.Generate(count)
            .Select(x => x
                .WithUserId(userId)
                .WithAt(now))
            .ToArray();

        var calculationIds = await _calculationRepository.Add(calculations, default);

        // Act
        var rowsAffected = await _calculationRepository.Remove(userId, default);
        
        // Assert
        _testOutputHelper.WriteLine($"Rows affected: {rowsAffected}");
        Assert.True(rowsAffected > 0);
        rowsAffected.Should().Be(calculationIds.Length);
    }
}

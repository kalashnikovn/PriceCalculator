using AutoFixture;
using Moq;
using PriceCalculator.Domain.Entities;
using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Separated;
using PriceCalculator.Domain.Services;
using PriceCalculator.Domain.Services.Interfaces;
using Xunit.Abstractions;

namespace PriceCalculator.Domain.UnitTests;

public class GoodPriceCalculatorServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public GoodPriceCalculatorServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void GoodPriceCalculatorService_WhenCalcAny_ShouldSuccess()
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            WeightToPriceRatio = 1
        };
        
        var mockGoodsRepository = new Mock<IGoodsRepository>();

        var entity = new Fixture().Create<GoodEntity>();
        
        mockGoodsRepository
            .Setup(repo => repo.Get(entity.Id))
            .Returns(entity);

        var storageRepository = CreateStorageRepositoryMock();
        var priceCalculatorService = new PriceCalculatorService(options, storageRepository.Object);
        var goodPriceCalculatorService = new GoodPriceCalculatorService(
                    mockGoodsRepository.Object,
                    priceCalculatorService);

        // Act
        var calculatedPrice = goodPriceCalculatorService.CalculatePrice(entity.Id, 1);

        // Assert
        mockGoodsRepository.Verify(x => x.Get(entity.Id), Times.Once);
        _testOutputHelper.WriteLine($"{calculatedPrice}");
    }
    
    [Theory]
    [MemberData(nameof(CalcByManyMemberData))]
    public void GoodPriceCalculatorService_WhenCalcMany_ShouldSuccess(GoodEntity model, decimal distance, decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1,
            WeightToPriceRatio = 1
        };
        
        var mockGoodsRepository = new Mock<IGoodsRepository>();
        mockGoodsRepository
            .Setup(repo => repo.Get(model.Id))
            .Returns(model);

        var storageRepository = CreateStorageRepositoryMock();
        var priceCalculatorService = new PriceCalculatorService(options, storageRepository.Object);
        var goodPriceCalculatorService = new GoodPriceCalculatorService(
            mockGoodsRepository.Object,
            priceCalculatorService);

        // Act
        var calculatedPrice = goodPriceCalculatorService.CalculatePrice(model.Id, distance);

        // Assert
        Assert.Equal(expected, calculatedPrice);
        mockGoodsRepository.Verify(x => x.Get(model.Id), Times.Once);
    }
    
    private static Mock<IStorageRepository> CreateStorageRepositoryMock()
    {
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Strict);
        repositoryMock.Setup(x => x.Save(It.IsAny<StorageEntity>()));
        return repositoryMock;
    }
    
    public static IEnumerable<object[]> CalcByManyMemberData => CalcByMany();
    private static IEnumerable<object[]> CalcByMany()
    {
        yield return new object[]
        {
            new GoodEntity("Язь",1, 10, 10,10, 0, 1, 100), 1, 1
        };
        
        yield return new object[]
        {
            new GoodEntity("Язь2",1, 10, 10,10, 0, 1, 100), 3, 3
        };

        
    }
    
}
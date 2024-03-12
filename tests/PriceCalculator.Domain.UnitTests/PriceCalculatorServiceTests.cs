using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using PriceCalculator.Domain.Entities;
using PriceCalculator.Domain.Exceptions;
using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Separated;
using PriceCalculator.Domain.Services;

namespace PriceCalculator.Domain.UnitTests;

public class PriceCalculatorServiceTests
{
    [Fact]
    public void PriceCalculatorService_WhenGoodsEmptyArray_ShouldThrow()
    {
        // Arrange
        var options = new PriceCalculatorOptions();
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Default);
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        var goods = Array.Empty<GoodModel>();

        // Act, Assert
        Assert.Throws<DomainException>(() => cut.CalculatePrice(new CalculateRequestModel(goods, 0)));
    }
    
    [Fact]
    public void PriceCalculatorService_WhenCalcAny_ShouldSave()
    {
        StorageEntity storageEntity = null;
        
        // Arrange
        var options = new PriceCalculatorOptions { VolumeToPriceRatio = 1, WeightToPriceRatio = 1 };
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Strict);
        repositoryMock
            .Setup(x => x.Save(It.IsAny<StorageEntity>()))
            .Callback<StorageEntity>(x => storageEntity = x);
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        var goods = new Fixture().CreateMany<GoodModel>().ToArray();

        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(goods, 0));
        
        // Assert
        Assert.NotNull(storageEntity);
        repositoryMock.Verify(x => x.Save(It.IsAny<StorageEntity>()));
        repositoryMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(0, 0)]
    public void PriceCalculatorService_WhenCalcByVolume_ShouldSuccess(decimal volumeToPriceRatio, decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        var good = new GoodModel(10, 10, 10, 0);

        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(new[] { good }, 0));

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(3.27, 3, 9.81)]
    [InlineData(10, 2, 20)]
    [InlineData(0, 1, 0)]
    public void PriceCalculatorService_WhenCalcByVolumeAndDistance_ShouldSuccess(
        decimal volumeToPriceRatio,
        decimal distance,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = volumeToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        var good = new GoodModel(10, 10, 10, 0);

        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(new[] { good }, distance));

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 4)]
    [InlineData(1.34, 2.68)]
    [InlineData(0, 0)]
    public void PriceCalculatorService_WhenCalcByWeight_ShouldSuccess(decimal weightToPriceRatio, decimal expected) 
    {
        // Arrange
        var options = new PriceCalculatorOptions()
        {
            WeightToPriceRatio = weightToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        var good = new GoodModel(10, 10, 10, 2000);

        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(new[] { good }, 0));
        
        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData(1, 2, 10)]
    [InlineData(1.34, 3, 20.1)]
    [InlineData(10, 2, 100)]
    [InlineData(0, 1, 0)]
    public void PriceCalculatorService_WhenCalcByWeightAndDistance_ShouldSuccess(
        decimal weightToPriceRatio,
        decimal distance,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = weightToPriceRatio
        };

        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        var good = new GoodModel(10, 10, 10, 5000);

        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(new[] { good }, distance));

        // Assert
        Assert.Equal(expected, result);
    }

    private static Mock<IStorageRepository> CreateRepositoryMock()
    {
        var repositoryMock = new Mock<IStorageRepository>(MockBehavior.Strict);
        repositoryMock.Setup(x => x.Save(It.IsAny<StorageEntity>()));
        return repositoryMock;
    }
    
    private static IOptionsSnapshot<PriceCalculatorOptions> CreateOptionsSnapshot(
        PriceCalculatorOptions options)
    {
        var repositoryMock = new Mock<IOptionsSnapshot<PriceCalculatorOptions>>(MockBehavior.Strict);
        
        repositoryMock
            .Setup(x => x.Value)
            .Returns(() => options);

        return repositoryMock.Object;
    }

    [Theory]
    [MemberData(nameof(CalcByVolumeManyMemberData))]
    public void PriceCalculatorService_WhenCalcByVolumeMany_ShouldSuccess(
        GoodModel[] goods,
        decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            VolumeToPriceRatio = 1, 
        };
        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        
        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(goods, 0));

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [MemberData(nameof(CalcByWeightManyMemberData))]
    public void PriceCalculatorService_WhenCalcByWeightMany_ShouldSuccess(GoodModel[] goods, decimal expected)
    {
        // Arrange
        var options = new PriceCalculatorOptions
        {
            WeightToPriceRatio = 1, 
        };
        var repositoryMock = CreateRepositoryMock();
        var cut = new PriceCalculatorService(options, repositoryMock.Object);
        
        // Act
        var result = cut.CalculatePrice(new CalculateRequestModel(goods, 0));

        // Assert
        Assert.Equal(expected, result);
    }
    

    public static IEnumerable<object[]> CalcByVolumeManyMemberData => CalcByVolumeMany();
    private static IEnumerable<object[]> CalcByVolumeMany()
    {
        yield return new object[]
        {
            new GoodModel[] { new(10, 10, 10, 0), }, 1
        };

        yield return new object[]
        {
            Enumerable
                .Range(1, 2)
                .Select(x => new GoodModel(10, 10, 10, 0))
                .ToArray(),
            2
        };
    }
    
    public static IEnumerable<object[]> CalcByWeightManyMemberData => CalcByWeightMany();
    private static IEnumerable<object[]> CalcByWeightMany()
    {
        yield return new object[]
        {
            new GoodModel[] { new(1, 1, 1, 1000), }, 1
        };
        
        yield return new object[]
        {
            new GoodModel[] { new(1, 1, 1, 2000), }, 2
        };

        yield return new object[]
        {
            Enumerable
                .Range(1, 2)
                .Select(x => new GoodModel(10, 10, 10, 5000))
                .ToArray(),
            10
        };
    }
}
using Moq;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Repositories.Interfaces;
using PriceCalculator.UnitTests.Comparers;

namespace PriceCalculator.UnitTests.Extensions;

public static class GoodsRepositoryExtensions
{
    public static Mock<IGoodsRepository> SetupAddGoods(
        this Mock<IGoodsRepository> repository,
        long[] ids)
    {
        repository.Setup(p =>
                p.Add(It.IsAny<GoodEntityV1[]>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ids);

        return repository;
    }
    
    public static Mock<IGoodsRepository> SetupQueryGoods(
        this Mock<IGoodsRepository> repository,
        GoodEntityV1[] goods)
    {
        repository.Setup(p =>
                p.Query(It.IsAny<long>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(goods);

        return repository;
    }
    
    public static Mock<IGoodsRepository> SetupRemoveGoodsByIds(
        this Mock<IGoodsRepository> repository)
    {
        repository.Setup(p =>
                p.Remove(It.IsAny<long[]>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        return repository;
    }
    
    public static Mock<IGoodsRepository> SetupRemoveGoodsByUserId(
        this Mock<IGoodsRepository> repository)
    {
        repository.Setup(p =>
                p.Remove(It.IsAny<long>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        return repository;
    }

    public static void VerifyAddWasCalledOnce(
        this Mock<IGoodsRepository> repository,
        GoodEntityV1[] goods)
    {
        repository.Verify(p =>
                p.Add(
                    It.Is<GoodEntityV1[]>(x => x.SequenceEqual(goods, new GoodEntityV1Comparer())),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    public static void VerifyQueryWasCalledOnce(
        this Mock<IGoodsRepository> repository,
        long userId)
    {
        repository.Verify(p =>
                p.Query(
                    It.Is<long>(x => x == userId),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    public static void VerifyRemoveGoodsWasCalledOnce(
        this Mock<IGoodsRepository> repository)
    {
        repository.Verify(p =>
                p.Remove(
                    It.IsAny<long[]>(),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    public static void VerifyRemoveGoodsWasCalledOnce(
        this Mock<IGoodsRepository> repository,
        long userId)
    {
        repository.Verify(p =>
                p.Remove(
                    It.Is<long>(x => x == userId),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
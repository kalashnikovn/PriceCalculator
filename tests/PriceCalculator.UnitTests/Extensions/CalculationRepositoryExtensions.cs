using System.Transactions;
using Moq;
using PriceCalculator.Dal.Entities;
using PriceCalculator.Dal.Models;
using PriceCalculator.Dal.Repositories.Interfaces;
using PriceCalculator.UnitTests.Comparers;

namespace PriceCalculator.UnitTests.Extensions;

public static class CalculationRepositoryExtensions
{
    public static Mock<ICalculationsRepository> SetupAddCalculations(
        this Mock<ICalculationsRepository> repository,
        long[] ids)
    {
        repository.Setup(p =>
                p.Add(It.IsAny<CalculationEntityV1[]>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(ids);

        return repository;
    }
    
    public static Mock<ICalculationsRepository> SetupCreateTransactionScope(
        this Mock<ICalculationsRepository> repository)
    {
        repository.Setup(p =>
                p.CreateTransactionScope(It.IsAny<IsolationLevel>()))
            .Returns(new TransactionScope());

        return repository;
    }
    
    public static Mock<ICalculationsRepository> SetupQueryCalculation(
        this Mock<ICalculationsRepository> repository,
        CalculationEntityV1[] calculations)
    {
        repository.Setup(p =>
                p.Query(It.IsAny<GetHistoryQueryModel>(), 
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(calculations);

        return repository;
    }
    
    public static Mock<ICalculationsRepository> SetupQueryCalculationByIds(
        this Mock<ICalculationsRepository> repository,
        CalculationEntityV1[] calculations)
    {
        repository.Setup(p =>
                p.Query(It.IsAny<long[]>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(calculations);

        return repository;
    }

    public static Mock<ICalculationsRepository> SetupRemoveCalculationByIds(
        this Mock<ICalculationsRepository> repository,
        int calculationsCount)
    {
        repository.Setup(p => 
            p.Remove(It.IsAny<long[]>(),
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(calculationsCount);

        return repository;
    }
    
    public static Mock<ICalculationsRepository> SetupRemoveCalculationByUserId(
        this Mock<ICalculationsRepository> repository,
        int calculationsCount)
    {
        repository.Setup(p => 
            p.Remove(It.IsAny<long>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(calculationsCount);

        return repository;
    }

    public static Mock<ICalculationsRepository> VerifyAddWasCalledOnce(
        this Mock<ICalculationsRepository> repository,
        CalculationEntityV1[] calculations)
    {
        repository.Verify(p =>
                p.Add(
                    It.Is<CalculationEntityV1[]>(x => x.SequenceEqual(calculations, new CalculationEntityV1Comparer())),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        return repository;
    }
    
    public static Mock<ICalculationsRepository> VerifyQueryWasCalledOnce(
        this Mock<ICalculationsRepository> repository,
        GetHistoryQueryModel query)
    {
        repository.Verify(p =>
                p.Query(
                    It.Is<GetHistoryQueryModel>(x => x == query),
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        return repository;
    }
    
    public static Mock<ICalculationsRepository> VerifyQueryByIdsWasCalledOnce(
        this Mock<ICalculationsRepository> repository,
        long[] calculationIds)
    {
        repository.Verify(p =>
                p.Query(
                    It.Is<long[]>(x => x == calculationIds),
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        return repository;
    }
    
    public static Mock<ICalculationsRepository> VerifyCreateTransactionScopeWasCalledOnce(
        this Mock<ICalculationsRepository> repository,
        IsolationLevel isolationLevel)
    {
        repository.Verify(p =>
                p.CreateTransactionScope(
                    It.Is<IsolationLevel>(x => x == isolationLevel)),
            Times.Once);
        
        return repository;
    }
    
    public static Mock<ICalculationsRepository> VerifyRemoveCalculationsWasCalledOnce(
        this Mock<ICalculationsRepository> repository,
        long[] calculationIds)
    {
        repository.Verify(p =>
                p.Remove(It.Is<long[]>(x => x.SequenceEqual(calculationIds)),
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        return repository;
    }
    
    public static Mock<ICalculationsRepository> VerifyRemoveCalculationsWasCalledOnce(
        this Mock<ICalculationsRepository> repository,
        long userId)
    {
        repository.Verify(p =>
                p.Remove(It.Is<long>(x => x == userId),
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        return repository;
    }
}
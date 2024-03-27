using Moq;
using PriceCalculator.Bll.Models;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.UnitTests.Comparers;

namespace PriceCalculator.UnitTests.Extensions;

public static class CalculationServiceExtensions
{
    public static Mock<ICalculationService> SetupSaveCalculation(
        this Mock<ICalculationService> service,
        long id)
    {
        service.Setup(p =>
                p.SaveCalculation(It.IsAny<SaveCalculationModel>(), 
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(id);

        return service;
    }
    
    public static Mock<ICalculationService> SetupCalculatePriceByVolume(
        this Mock<ICalculationService> service,
        double volume,
        decimal price)
    {
        service.Setup(p =>
                p.CalculatePriceByVolume(It.IsAny<GoodModel[]>(), 
                    out volume))
            .Returns(price);

        return service;
    }
    
    public static Mock<ICalculationService> SetupCalculatePriceByWeight(
        this Mock<ICalculationService> service,
        double weight,
        decimal price)
    {
        service.Setup(p =>
                p.CalculatePriceByWeight(It.IsAny<GoodModel[]>(), 
                    out weight))
            .Returns(price);

        return service;
    }

    public static Mock<ICalculationService> SetupQueryCalculations(
        this Mock<ICalculationService> service,
        QueryCalculationModel[] model)
    {
        service.Setup(p =>
                p.QueryCalculations(It.IsAny<QueryCalculationFilter>(), 
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);

        return service;
    }
    
    public static Mock<ICalculationService> SetupQueryCalculationsByIds(
        this Mock<ICalculationService> service,
        QueryCalculationModel[] model)
    {
        service.Setup(p =>
                p.QueryCalculations(It.IsAny<long[]>(), 
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(model);

        return service;
    }
    
    public static Mock<ICalculationService> SetupRemoveCalculations(
        this Mock<ICalculationService> service)
    {
        service.Setup(p =>
                p.RemoveCalculations(It.IsAny<QueryCalculationModel[]>(), 
                    It.IsAny<CancellationToken>()));

        return service;
    }
    
    public static Mock<ICalculationService> SetupRemoveCalculationsByUserId(
        this Mock<ICalculationService> service)
    {
        service.Setup(p =>
            p.RemoveCalculations(It.IsAny<long>(), 
                It.IsAny<CancellationToken>()));

        return service;
    }
    
    
    
    public static Mock<ICalculationService> VerifySaveCalculationWasCalledOnce(
        this Mock<ICalculationService> service,
        SaveCalculationModel model)
    {
        service.Verify(p =>
                p.SaveCalculation(
                    It.Is<SaveCalculationModel>(x => new CalculationModelComparer().Equals(x, model)),
                    It.IsAny<CancellationToken>()),
            Times.Once);
        
        return service;
    }
    
    public static Mock<ICalculationService> VerifyCalculatePriceByVolumeWasCalledOnce(
        this Mock<ICalculationService> service,
        GoodModel[] model)
    {
        service.Verify(p =>
                p.CalculatePriceByVolume(
                    It.Is<GoodModel[]>(x => x.SequenceEqual(model)),
                    out It.Ref<double>.IsAny),
            Times.Once);
        
        return service;
    }
    
    public static Mock<ICalculationService> VerifyCalculatePriceByWeightWasCalledOnce(
        this Mock<ICalculationService> service,
        GoodModel[] model)
    {
        service.Verify(p =>
                p.CalculatePriceByWeight(
                    It.Is<GoodModel[]>(x => x.SequenceEqual(model)),
                    out It.Ref<double>.IsAny),
            Times.Once);

        return service;
    }
    
    public static Mock<ICalculationService> VerifyQueryCalculationsWasCalledOnce(
        this Mock<ICalculationService> service,
        QueryCalculationFilter filter)
    {
        service.Verify(p =>
                p.QueryCalculations(
                    It.Is<QueryCalculationFilter>(x => x == filter),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        return service;
    }
    
    public static Mock<ICalculationService> VerifyQueryCalculationsWasCalledOnce(
        this Mock<ICalculationService> service,
        long[] calculationIds)
    {
        service.Verify(p =>
                p.QueryCalculations(
                    It.Is<long[]>(x => x.SequenceEqual(calculationIds)),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        return service;
    }
    
    public static Mock<ICalculationService> VerifyRemoveCalculationsWasCalledOnce(
        this Mock<ICalculationService> service,
        QueryCalculationModel[] models)
    {
        service.Verify(p =>
                p.RemoveCalculations(
                    It.Is<QueryCalculationModel[]>(x => 
                        x.SequenceEqual(models, new QueryCalculationModelComparer())),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        return service;
    }
    
    public static Mock<ICalculationService> VerifyRemoveCalculationsWasCalledOnce(
        this Mock<ICalculationService> service,
        long userId)
    {
        service.Verify(p =>
                p.RemoveCalculations(
                    It.Is<long>(x => x == userId),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        return service;
    }
}
using System.Transactions;
using FluentAssertions;
using PriceCalculator.Bll.Services;
using PriceCalculator.UnitTests.Builders;
using PriceCalculator.UnitTests.Extensions;
using PriceCalculator.UnitTests.Fakers;
using TestingInfrastructure.Creators;
using TestingInfrastructure.Fakers;
using Xunit;

namespace PriceCalculator.UnitTests.ServicesTests;

public class CalculationServiceTests
{
    [Fact]
    public async Task SaveCalculation_Success()
    {
        // arrange
        const int goodsCount = 5;
        
        var userId = Create.RandomId();
        var calculationId = Create.RandomId();
        
        var goodModels = GoodModelFaker.Generate(goodsCount)
            .ToArray();
        
        var goods = goodModels
            .Select(x => GoodEntityV1Faker.Generate().Single()
                .WithUserId(userId)
                .WithHeight(x.Height)
                .WithWidth(x.Width)
                .WithLength(x.Length)
                .WithWeight(x.Weight))
            .ToArray();
        var goodIds = goods.Select(x => x.Id)
            .ToArray();

        var calculationModel = CalculationModelFaker.Generate()
            .Single()
            .WithUserId(userId)
            .WithGoods(goodModels);
        
        var calculations = CalculationEntityV1Faker.Generate(1)
            .Select(x => x
                .WithId(calculationId)
                .WithUserId(userId)
                .WithPrice(calculationModel.Price)
                .WithTotalWeight(calculationModel.TotalWeight)
                .WithTotalVolume(calculationModel.TotalVolume))
            .ToArray();
        
        var builder = new CalculationServiceBuilder();
        builder.CalculationRepository
            .SetupAddCalculations(new [] { calculationId })
            .SetupCreateTransactionScope();
        builder.GoodsRepository
            .SetupAddGoods(goodIds);

        var service = builder.Build();

        // act
        var result = await service.SaveCalculation(calculationModel, default);

        // assert
        result.Should().Be(calculationId);
        service.CalculationRepository
            .VerifyAddWasCalledOnce(calculations)
            .VerifyCreateTransactionScopeWasCalledOnce(IsolationLevel.ReadCommitted);
        service.GoodsRepository
            .VerifyAddWasCalledOnce(goods);
        service.VerifyNoOtherCalls();
    }

    [Fact]
    public void CalculatePriceByVolume_Success()
    {
        // arrange
        var goodModels = GoodModelFaker.Generate(5)
            .ToArray();
        
        var builder = new CalculationServiceBuilder();
        var service = builder.Build();

        //act
        var price = service.CalculatePriceByVolume(goodModels, out var volume);

        //asserts
        volume.Should().BeApproximately(goodModels.Sum(x => x.Height * x.Width * x.Length), 1e-9d);
        price.Should().Be((decimal)volume * CalculationService.VolumeToPriceRatio);
    }
    
    [Fact]
    public void CalculatePriceByWeight_Success()
    {
        // arrange
        var goodModels = GoodModelFaker.Generate(5)
            .ToArray();
        
        var builder = new CalculationServiceBuilder();
        var service = builder.Build();

        //act
        var price = service.CalculatePriceByWeight(goodModels, out var weight);

        //asserts
        weight.Should().Be(goodModels.Sum(x => x.Weight));
        price.Should().Be((decimal)weight * CalculationService.WeightToPriceRatio);
    }
    
    [Fact]
    public async Task QueryCalculations_Success()
    {
        // arrange
        var userId = Create.RandomId();

        var filter = QueryCalculationFilterFaker.Generate()
            .WithUserId(userId);
        
        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var queryModel = GetHistoryQueryModelFaker.Generate()
            .WithUserId(userId)
            .WithLimit(filter.Limit)
            .WithOffset(filter.Offset);
        
        var builder = new CalculationServiceBuilder();
        builder.CalculationRepository
            .SetupQueryCalculation(calculations);
        var service = builder.Build();

        //act
        var result = await service.QueryCalculations(filter, default);

        //asserts
        service.CalculationRepository
            .VerifyQueryWasCalledOnce(queryModel);
        
        service.VerifyNoOtherCalls();

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(x => x.UserId == userId);
        result.Should().OnlyContain(x => x.Id > 0);
        result.Select(x => x.TotalWeight)
            .Should().IntersectWith(calculations.Select(x => x.TotalWeight));
        result.Select(x => x.TotalVolume)
            .Should().IntersectWith(calculations.Select(x => x.TotalVolume));
        result.Select(x => x.Price)
            .Should().IntersectWith(calculations.Select(x => x.Price));
    }
    
    
    [Fact]
    public async Task QueryCalculationsByIds_Success()
    {
        // arrange
        var userId = Create.RandomId();
        
        
        var calculations = CalculationEntityV1Faker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var calculationIds = calculations.Select(x => x.Id)
            .ToArray();
        
        var builder = new CalculationServiceBuilder();
        builder.CalculationRepository
            .SetupQueryCalculationByIds(calculations);
        var service = builder.Build();

        //act
        var result = await service.QueryCalculations(calculationIds, default);

        //asserts
        service.CalculationRepository
            .VerifyQueryByIdsWasCalledOnce(calculationIds);
        
        service.VerifyNoOtherCalls();

        result.Should().NotBeEmpty();
        result.Should().OnlyContain(x => x.UserId == userId);
        result.Should().OnlyContain(x => x.Id > 0);
        result.Select(x => x.TotalWeight)
            .Should().IntersectWith(calculations.Select(x => x.TotalWeight));
        result.Select(x => x.TotalVolume)
            .Should().IntersectWith(calculations.Select(x => x.TotalVolume));
        result.Select(x => x.Price)
            .Should().IntersectWith(calculations.Select(x => x.Price));
    }
    
    [Fact]
    public async Task RemoveCalculationsByIds_Success()
    {
        // arrange
        var userId = Create.RandomId();
        
        
        var calculations = QueryCalculationModelFaker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var calculationIds = calculations.Select(x => x.Id)
            .ToArray();

        
        var builder = new CalculationServiceBuilder();
        builder.CalculationRepository
            .SetupRemoveCalculationByIds(calculationIds.Length)
            .SetupCreateTransactionScope();
        builder.GoodsRepository
            .SetupRemoveGoodsByIds();
        var service = builder.Build();

        //act
        await service.RemoveCalculations(calculations, default);

        //asserts
        service.CalculationRepository
            .VerifyRemoveCalculationsWasCalledOnce(calculationIds)
            .VerifyCreateTransactionScopeWasCalledOnce(IsolationLevel.ReadCommitted);
        service.GoodsRepository
            .VerifyRemoveGoodsWasCalledOnce();
        
        service.VerifyNoOtherCalls();
        
    }
    
    
    [Fact]
    public async Task RemoveCalculationsByUserId_Success()
    {
        // arrange
        var userId = Create.RandomId();
        
        
        var calculations = QueryCalculationModelFaker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var calculationIds = calculations.Select(x => x.Id)
            .ToArray();

        
        var builder = new CalculationServiceBuilder();
        builder.CalculationRepository
            .SetupRemoveCalculationByUserId(calculationIds.Length)
            .SetupCreateTransactionScope();
        builder.GoodsRepository
            .SetupRemoveGoodsByUserId();
        var service = builder.Build();

        // act
        await service.RemoveCalculations(userId, default);

        //asserts
        service.CalculationRepository
            .VerifyRemoveCalculationsWasCalledOnce(userId)
            .VerifyCreateTransactionScopeWasCalledOnce(IsolationLevel.ReadCommitted);
        service.GoodsRepository
            .VerifyRemoveGoodsWasCalledOnce(userId);
        
        service.VerifyNoOtherCalls();
        
    }
}

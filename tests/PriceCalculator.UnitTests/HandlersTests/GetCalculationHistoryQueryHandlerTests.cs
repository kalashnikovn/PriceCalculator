using FluentAssertions;
using PriceCalculator.UnitTests.Builders;
using PriceCalculator.UnitTests.Extensions;
using PriceCalculator.UnitTests.Fakers;
using TestingInfrastructure.Creators;
using Xunit;

namespace PriceCalculator.UnitTests.HandlersTests;

public class GetCalculationHistoryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidCalculationIdsPassed_Success()
    {
        //arrange
        var userId = Create.RandomId();

        var command = GetCalculationHistoryQueryFaker.Generate()
            .WithUserId(userId);

        var queryModels = QueryCalculationModelFaker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var queryModelsIds = queryModels.Select(x => x.Id)
            .ToArray();

        command = command.WithCalculationIds(queryModelsIds);

        var filter = QueryCalculationFilterFaker.Generate()
            .WithUserId(userId)
            .WithLimit(command.Take)
            .WithOffset(command.Skip);

        var builder = new GetCalculationHistoryHandlerBuilder();
        builder.CalculationService
            .SetupQueryCalculations(queryModels);

        var handler = builder.Build();

        //act
        var result = await handler.Handle(command, default);

        //asserts
        handler.CalculationService
            .VerifyQueryCalculationsWasCalledOnce(filter);

        handler.VerifyNoOtherCalls();

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(queryModels.Length);
        result.Items.Select(x => x.Price)
            .Should().IntersectWith(queryModels.Select(x => x.Price));
        result.Items.Select(x => x.Volume)
            .Should().IntersectWith(queryModels.Select(x => x.TotalVolume));
        result.Items.Select(x => x.Weight)
            .Should().IntersectWith(queryModels.Select(x => x.TotalWeight));
    }
    
    
    // [Fact]
    // public async Task Handle_WrongCalculationIdsPassed_ReturnsEmpty()
    // {
    //     //arrange
    //     var userId = Create.RandomId();
    //
    //     var command = GetCalculationHistoryQueryFaker.Generate()
    //         .WithUserId(userId);
    //
    //     var queryModels = QueryCalculationModelFaker.Generate(5)
    //         .Select(x => x.WithUserId(userId))
    //         .ToArray();
    //     
    //
    //     var filter = QueryCalculationFilterFaker.Generate()
    //         .WithUserId(userId)
    //         .WithLimit(command.Take)
    //         .WithOffset(command.Skip);
    //
    //     var builder = new GetCalculationHistoryHandlerBuilder();
    //     builder.CalculationService
    //         .SetupQueryCalculations(queryModels);
    //
    //     var handler = builder.Build();
    //
    //     //act
    //     var result = await handler.Handle(command, default);
    //
    //     //asserts
    //     handler.CalculationService
    //         .VerifyQueryCalculationsWasCalledOnce(filter);
    //
    //     handler.VerifyNoOtherCalls();
    //
    //     result.Should().NotBeNull();
    //     result.Items.Should().BeEmpty();
    //
    // }
    
    [Fact]
    public async Task Handle_NotAllCalculationIdsWrong_Success()
    {
        //arrange
        var userId = Create.RandomId();
        
        var queryModels = QueryCalculationModelFaker.Generate(5)
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var existingIds = queryModels.Select(x => x.Id)
            .ToArray();
        
        var wrongIds = Enumerable.Repeat(Create.RandomId(), 3)
            .Except(existingIds)
            .ToArray();

        var calculationIds = existingIds.Union(wrongIds)
            .ToArray();

        var command = GetCalculationHistoryQueryFaker.Generate()
            .WithUserId(userId)
            .WithCalculationIds(calculationIds);

        var filter = QueryCalculationFilterFaker.Generate()
            .WithUserId(userId)
            .WithLimit(command.Take)
            .WithOffset(command.Skip);

        var builder = new GetCalculationHistoryHandlerBuilder();
        builder.CalculationService
            .SetupQueryCalculations(queryModels);

        var handler = builder.Build();

        //act
        var result = await handler.Handle(command, default);

        //asserts
        handler.CalculationService
            .VerifyQueryCalculationsWasCalledOnce(filter);

        handler.VerifyNoOtherCalls();

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(existingIds.Length);
    }
    
    

}
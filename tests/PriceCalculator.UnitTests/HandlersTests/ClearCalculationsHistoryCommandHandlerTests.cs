using FluentAssertions;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Exceptions;
using PriceCalculator.Bll.Models;
using PriceCalculator.UnitTests.Builders;
using PriceCalculator.UnitTests.Extensions;
using PriceCalculator.UnitTests.Fakers;
using TestingInfrastructure.Creators;
using Xunit;

namespace PriceCalculator.UnitTests.HandlersTests;

public class ClearCalculationsHistoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_RemoveRequestedCalculations()
    {
        //arrange
        var userId = Create.RandomId();

        var command = ClearCalculationsHistoryCommandFaker.Generate()
            .WithUserId(userId);

        var calculationModels = QueryCalculationModelFaker.Generate()
            .Select(x => x.WithUserId(userId))
            .ToArray();

        var calculationIds = calculationModels
            .Select(x => x.Id)
            .ToArray();

        command = command.WithCalculationIds(calculationIds);

        var builder = new ClearCalculationsHistoryCommandHandlerBuilder();
        builder.CalculationService
            .SetupQueryCalculationsByIds(calculationModels)
            .SetupRemoveCalculations();

        var handler = builder.Build();
        
        //act
        await handler.Handle(command, default);

        //asserts
        handler.CalculationService
            .VerifyRemoveCalculationsWasCalledOnce(calculationModels)
            .VerifyQueryCalculationsWasCalledOnce(calculationIds);
        
        handler.VerifyNoOtherCalls();
        
    }
    
    
    [Fact]
    public async Task Handle_RemoveAllUserCalculations()
    {
        //arrange
        var userId = Create.RandomId();

        var command = ClearCalculationsHistoryCommandFaker.Generate()
            .WithUserId(userId);
        
        command = command.WithCalculationIds(Array.Empty<long>());
        
        
        var builder = new ClearCalculationsHistoryCommandHandlerBuilder();
        builder.CalculationService
            .SetupRemoveCalculationsByUserId();

        var handler = builder.Build();
        
        //act
        await handler.Handle(command, default);

        //asserts
        handler.CalculationService
            .VerifyRemoveCalculationsWasCalledOnce(userId);
        
        handler.VerifyNoOtherCalls();
        
    }
    
    [Fact]
    public async Task Handle_ThrowsWhenNotFound()
    {
        //arrange
        var userId = Create.RandomId();

        var command = ClearCalculationsHistoryCommandFaker.Generate()
            .WithUserId(userId);
        
        
        var builder = new ClearCalculationsHistoryCommandHandlerBuilder();
        builder.CalculationService
            .SetupQueryCalculationsByIds(Array.Empty<QueryCalculationModel>());
        
        var handler = builder.Build();
        
        //act
        var act = () => handler.Handle(command, default);

        //asserts
        await Assert.ThrowsAsync<OneOrManyCalculationsNotFoundException>(act);
        
    }
    
    
    [Fact]
    public async Task Handle_ThrowsWhenBelongsToAnotherUser()
    {
        //arrange
        var userId = Create.RandomId();
        var anotherUser = Create.RandomId();

        var command = ClearCalculationsHistoryCommandFaker.Generate()
            .WithUserId(userId);
        
        var calculationModels = QueryCalculationModelFaker.Generate()
            .Select(x => x.WithUserId(anotherUser))
            .ToArray();
        
        var calculationIds = calculationModels
            .Select(x => x.Id)
            .ToArray();

        command = command.WithCalculationIds(calculationIds);
        
        
        var builder = new ClearCalculationsHistoryCommandHandlerBuilder();
        builder.CalculationService
            .SetupQueryCalculationsByIds(calculationModels);
        
        var handler = builder.Build();
        
        //act
        var act = () => handler.Handle(command, default);

        //asserts
        await Assert.ThrowsAsync<OneOrManyCalculationsBelongsToAnotherUserException>(act);
        
    }
}
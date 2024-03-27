using Moq;
using PriceCalculator.Bll.Commands;
using PriceCalculator.Bll.Handlers;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.UnitTests.Stubs;

public class ClearCalculationsHistoryCommandHandlerBuilderStub : ClearCalculationsHistoryCommandHandler
{
    public Mock<ICalculationService> CalculationService { get; }
    
    public ClearCalculationsHistoryCommandHandlerBuilderStub(
        Mock<ICalculationService> calculationService) 
        : base(
            calculationService.Object)
    {
        CalculationService = calculationService;
    }
    
    public void VerifyNoOtherCalls()
    {
        CalculationService.VerifyNoOtherCalls();
    }
}
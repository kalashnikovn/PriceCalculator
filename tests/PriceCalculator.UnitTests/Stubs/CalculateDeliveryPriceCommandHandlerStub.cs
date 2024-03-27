using Moq;
using PriceCalculator.Bll.Handlers;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.UnitTests.Stubs;

public class  CalculateDeliveryPriceCommandHandlerStub : CalculateDeliveryPriceCommandHandler
{
    public Mock<ICalculationService> CalculationService { get; }
    
    public CalculateDeliveryPriceCommandHandlerStub(
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
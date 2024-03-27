using Moq;
using PriceCalculator.Bll.Handlers;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.UnitTests.Stubs;

public class GetCalculationHistoryHandlerStub : GetCalculationHistoryQueryHandler
{
    public Mock<ICalculationService> CalculationService { get; }
    
    public GetCalculationHistoryHandlerStub(
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
using Moq;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.UnitTests.Stubs;

namespace PriceCalculator.UnitTests.Builders;

public class GetCalculationHistoryHandlerBuilder
{
    public Mock<ICalculationService> CalculationService;
    
    public GetCalculationHistoryHandlerBuilder()
    {
        CalculationService = new Mock<ICalculationService>();
    }
    
    public GetCalculationHistoryHandlerStub Build()
    {
        return new GetCalculationHistoryHandlerStub(
            CalculationService);
    }
}
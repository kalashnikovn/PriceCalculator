using Moq;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.UnitTests.Stubs;

namespace PriceCalculator.UnitTests.Builders;

public class ClearCalculationsHistoryCommandHandlerBuilder
{
    public Mock<ICalculationService> CalculationService;
    
    public ClearCalculationsHistoryCommandHandlerBuilder()
    {
        CalculationService = new Mock<ICalculationService>();
    }
    
    public ClearCalculationsHistoryCommandHandlerBuilderStub Build()
    {
        return new ClearCalculationsHistoryCommandHandlerBuilderStub(
            CalculationService);
    }
}
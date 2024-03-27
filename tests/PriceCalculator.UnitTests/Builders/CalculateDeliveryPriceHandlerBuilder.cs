using Moq;
using PriceCalculator.Bll.Services.Interfaces;
using PriceCalculator.UnitTests.Stubs;

namespace PriceCalculator.UnitTests.Builders;

public class CalculateDeliveryPriceHandlerBuilder
{
    public Mock<ICalculationService> CalculationService;
    
    public CalculateDeliveryPriceHandlerBuilder()
    {
        CalculationService = new Mock<ICalculationService>();
    }
    
    public CalculateDeliveryPriceCommandHandlerStub Build()
    {
        return new CalculateDeliveryPriceCommandHandlerStub(
            CalculationService);
    }
}
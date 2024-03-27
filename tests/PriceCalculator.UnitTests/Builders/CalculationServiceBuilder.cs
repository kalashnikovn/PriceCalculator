using Moq;
using PriceCalculator.Dal.Repositories.Interfaces;
using PriceCalculator.UnitTests.Stubs;

namespace PriceCalculator.UnitTests.Builders;

public class CalculationServiceBuilder
{
    public Mock<ICalculationsRepository> CalculationRepository;
    public Mock<IGoodsRepository> GoodsRepository;
    
    public CalculationServiceBuilder()
    {
        CalculationRepository = new Mock<ICalculationsRepository>();
        GoodsRepository = new Mock<IGoodsRepository>();
    }
    
    public CalculationServiceStub Build()
    {
        return new CalculationServiceStub(
            CalculationRepository,
            GoodsRepository);
    }
}
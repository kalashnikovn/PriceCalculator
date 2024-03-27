using Moq;
using PriceCalculator.Bll.Services;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.UnitTests.Stubs;

public class CalculationServiceStub : CalculationService
{
    public Mock<ICalculationsRepository> CalculationRepository { get; }
    public Mock<IGoodsRepository> GoodsRepository { get; }
    
    public CalculationServiceStub(
        Mock<ICalculationsRepository> calculationRepository,
        Mock<IGoodsRepository> goodsRepository) 
        : base(
            calculationRepository.Object, 
            goodsRepository.Object)
    {
        CalculationRepository = calculationRepository;
        GoodsRepository = goodsRepository;
    }
    
    public void VerifyNoOtherCalls()
    {
        CalculationRepository.VerifyNoOtherCalls();
        GoodsRepository.VerifyNoOtherCalls();
    }
}
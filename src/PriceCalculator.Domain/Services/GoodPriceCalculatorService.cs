using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Separated;
using PriceCalculator.Domain.Services.Interfaces;

namespace PriceCalculator.Domain.Services;

public sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
{
    private readonly IGoodsRepository _goodsRepository;
    private readonly IPriceCalculatorService _priceCalculatorService;


    public GoodPriceCalculatorService(
        IGoodsRepository goodsRepository,
        IPriceCalculatorService priceCalculatorService
        )
    {
        _goodsRepository = goodsRepository;
        _priceCalculatorService = priceCalculatorService;
    }
    
    public decimal CalculatePrice(int goodId, decimal distance)
    {
        var good = _goodsRepository.Get(goodId);

        return _priceCalculatorService.CalculatePrice(new[]
            { new GoodModel(good.Length, good.Width, good.Height, good.Weight) });
    }
}
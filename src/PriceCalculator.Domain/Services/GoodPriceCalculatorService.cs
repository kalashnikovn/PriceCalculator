using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Separated;
using PriceCalculator.Domain.Services.Interfaces;

namespace PriceCalculator.Domain.Services;

internal sealed class GoodPriceCalculatorService : IGoodPriceCalculatorService
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
        var goods = new[] { new GoodModel(good.Length, good.Width, good.Height, good.Weight) };

        return _priceCalculatorService.CalculatePrice(new CalculateRequestModel(goods, distance));
    }
}
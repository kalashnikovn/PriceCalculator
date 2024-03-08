using PriceCalculator.Api.Bll.Models.PriceCalculator;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.Bll.Services;

public class GoodsFullPriceService : IGoodsFullPriceService
{
    private readonly IGoodsRepository _repository;
    private readonly IPriceCalculatorService _priceCalculatorService;

    public GoodsFullPriceService(
        IGoodsRepository repository,
        IPriceCalculatorService priceCalculatorService
    )
    {
        _repository = repository;
        _priceCalculatorService = priceCalculatorService;
    }
    
    public decimal GetFullPrice(int id)
    {
        var good = _repository.Get(id);

        var model = new GoodModel(
            good.Height,
            good.Length,
            good.Width,
            good.Weight
        );

        var deliveryPrice = _priceCalculatorService.CalculatePrice(new[] { model });

        var fullPrice = deliveryPrice + good.Price;

        return fullPrice;
    }
}
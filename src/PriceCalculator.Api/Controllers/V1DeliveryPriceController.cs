using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Requests.V1;
using PriceCalculator.Api.Responses.V1;
using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Services.Interfaces;

namespace PriceCalculator.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class V1DeliveryPriceController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V1DeliveryPriceController(
        IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public CalculateResponse Calculate(
        CalculateRequest request)
    {
        var price = _priceCalculatorService.CalculatePrice(
            request.Goods
                .Select(x => new GoodModel(
                    x.Height,
                    x.Length,
                    x.Width,
                    0 /* для v1 рассчет по весу не предусмотрен */))
                .ToArray());
        
        return new CalculateResponse(price);
    }
    
    /// <summary>
    /// Метод получения истории вычисления
    /// </summary>
    /// <returns></returns>
    [HttpPost("get-history")]
    public GetHistoryResponse[] History(GetHistoryRequest request)
    {
        var log = _priceCalculatorService.QueryLog(request.Take);

        return log
            .Select(x => new GetHistoryResponse(
                new CargoResponse(
                    x.Volume,
                    x.Weight),
                x.Price))
            .ToArray();
    }
}
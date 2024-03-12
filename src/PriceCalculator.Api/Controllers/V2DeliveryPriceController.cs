using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Requests.V2;
using PriceCalculator.Api.Responses.V2;
using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Services.Interfaces;

namespace PriceCalculator.Api.Controllers;

[ApiController]
[Route("/v2/[controller]")]
public class V2DeliveryPriceController : Controller
{
    private readonly ILogger<V2DeliveryPriceController> _logger;
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V2DeliveryPriceController(
        ILogger<V2DeliveryPriceController> logger,
        IHttpContextAccessor httpContextAccessor,
        IPriceCalculatorService priceCalculatorService)
    {
        _logger = logger;
        _priceCalculatorService = priceCalculatorService;
    }
    
    /// <summary>
    /// Метод расчета стоимости доставки на основе объема товаров
    /// или веса товара. Окончательная стоимость принимается как наибольшая
    /// </summary>
    /// <returns></returns>
    [HttpPost("calculate")]
    public CalculateResponse Calculate(CalculateRequest request)
    {
        var goods = request.Goods
            .Select(x => new GoodModel(
                x.Height,
                x.Length,
                x.Width,
                x.Weight))
            .ToArray();
        
        var price = _priceCalculatorService.CalculatePrice(new CalculateRequestModel(goods, 0));
        
        return new CalculateResponse(price);
    }
}
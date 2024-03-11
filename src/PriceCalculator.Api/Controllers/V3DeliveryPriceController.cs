using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Requests.V3;
using PriceCalculator.Api.Responses.V3;
using PriceCalculator.Domain.Models.PriceCalculator;
using PriceCalculator.Domain.Services.Interfaces;

namespace PriceCalculator.Api.Controllers;

[ApiController]
[Route("v3/[controller]")]
public class V3DeliveryPriceController : ControllerBase
{
    private readonly IGoodPriceCalculatorService _goodPriceCalculatorService;
    private readonly IPriceCalculatorService _priceCalculatorService;

    public V3DeliveryPriceController(
        IGoodPriceCalculatorService goodPriceCalculatorService,
        IPriceCalculatorService priceCalculatorService)
    {
        _goodPriceCalculatorService = goodPriceCalculatorService;
        _priceCalculatorService = priceCalculatorService;
    }

    [HttpPost("calculate")]
    public CalculateResponse Calculate(
        Requests.V3.CalculateRequest request)
    {
        var price = _priceCalculatorService.CalculatePrice(
            new CalculateRequestModel(
                request.Goods
                    .Select(x => new GoodModel(
                        x.Height,
                        x.Length,
                        x.Width,
                        x.Weight))
                    .ToArray(),
                request.Distance));

        return new CalculateResponse(price);
    }

    [HttpPost("good/calculate")]
    public Task<CalculateResponse> Calculate(GoodCalculateRequest request)
    {
        var price = _goodPriceCalculatorService.CalculatePrice(request.GoodId, request.Distance);

        return Task.FromResult(new CalculateResponse(price));
    }
    
}
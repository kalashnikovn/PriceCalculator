using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Bll.Models;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Requests.V3;
using PriceCalculator.Api.Responses.V3;

namespace PriceCalculator.Api.Controllers.V3;

[ApiController]
[Route("v3/[controller]")]
public class DeliveryPriceController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public DeliveryPriceController(
        IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    [HttpPost("calculate")]
    public CalculateResponse Calculate(CalculateRequest request)
    {
        var result = _priceCalculatorService.CalculatePrice(
            request.Goods.Select(x => new GoodModel(
                    x.Length,
                    x.Width,
                    x.Height,
                    x.Weight))
                .ToArray(), request.Distance);

        return new CalculateResponse(result);
    }

    [HttpPost("get-history")]
    public GetHistoryResponse[] GetHistory(GetHistoryRequest request)
    {
        var result = _priceCalculatorService.QueryLog(request.Take);

        return result
            .Select(x => new GetHistoryResponse(
                new CargoResponse(x.Volume, x.Weight),
                x.Price,
                x.Distance
            ))
            .ToArray();
    }
    
}
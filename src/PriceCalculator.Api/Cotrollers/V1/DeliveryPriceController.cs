using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Bll.Models;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Requests.V1;
using PriceCalculator.Api.Responses.V1;

namespace PriceCalculator.Api.Cotrollers.V1;

[ApiController]
[Route("v1/[controller]")]
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
                    x.Height))
                .ToArray());

        return new CalculateResponse(result);
    }

    [HttpPost("get-history")]
    public GetHistoryResponse GetHistory(GetHistoryRequest request)
    {
        throw new NotImplementedException();
    }
    
}
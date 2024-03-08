using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Bll.Models.PriceCalculator;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Requests.V2;
using PriceCalculator.Api.Responses.V2;
using PriceCalculator.Api.Validators;

namespace PriceCalculator.Api.Controllers.V2;

[ApiController]
[Route("v2/[controller]")]
public class DeliveryPriceController : ControllerBase
{
    private readonly IPriceCalculatorService _priceCalculatorService;

    public DeliveryPriceController(
        IPriceCalculatorService priceCalculatorService)
    {
        _priceCalculatorService = priceCalculatorService;
    }

    [HttpPost("calculate")]
    public async Task<CalculateResponse> Calculate(CalculateRequest request)
    {
        var validator = new CalculateRequestValidator();
        await validator.ValidateAndThrowAsync(request);
        
        var result = _priceCalculatorService.CalculatePrice(
            request.Goods.Select(x => new GoodModel(
                    x.Length,
                    x.Width,
                    x.Height,
                    x.Weight))
                .ToArray());

        return new CalculateResponse(result);
    }

    [HttpPost("get-history")]
    public GetHistoryResponse[] GetHistory(GetHistoryRequest request)
    {
        var result = _priceCalculatorService.QueryLog(request.Take);

        return result
            .Select(x => new GetHistoryResponse(
                new CargoResponse(x.Volume, x.Weight),
                x.Price
            ))
            .ToArray();
    }
    
}
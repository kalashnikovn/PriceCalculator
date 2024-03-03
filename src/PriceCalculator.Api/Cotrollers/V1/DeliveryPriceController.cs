using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Requests.V1;
using PriceCalculator.Api.Responses.V1;

namespace PriceCalculator.Api.Cotrollers.V1;

[ApiController]
[Route("v1/[controller]")]
public class DeliveryPriceController : ControllerBase
{

    public CalculateResponse Calculate(CalculateRequest request)
    {
        throw new NotImplementedException();
    }

    public GetHistoryResponse GetHistory(GetHistoryRequest request)
    {
        throw new NotImplementedException();
    }
    
}
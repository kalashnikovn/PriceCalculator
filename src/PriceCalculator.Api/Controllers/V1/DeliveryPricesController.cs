using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.Requests.V1;

namespace PriceCalculator.Api.Controllers.V1;

[ApiController]
[Route("v1/delivery-prices")]
public sealed class DeliveryPricesController : ControllerBase
{
    
    [HttpPost("calculate")]
    public ActionResult Calculate(CalculateRequest request, CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet("get-history")]
    public ActionResult GetHistory(GetHistoryRequest request, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    
}
using Microsoft.AspNetCore.Mvc;

namespace PriceCalculator.Api.ActionFilters;

public class ResponseTypeAttribute : ProducesResponseTypeAttribute
{
    public ResponseTypeAttribute(int statusCode) : base(typeof(ErrorResponse), statusCode)
    {
    }
}
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PriceCalculator.Bll.Exceptions;

namespace PriceCalculator.Api.ActionFilters;

public class ClearHistoryExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case OneOrManyCalculationsNotFoundException:
                var result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                context.Result = result;
                return;
            
            case OneOrManyCalculationsBelongsToAnotherUserException exception:
                var jsonResult = new JsonResult(
                    new OneOrManyCalculationsBelongsToAnotherUserResponse(exception.WrongCalculationIds)
                    );
                context.Result = jsonResult;
                return;
        }
    }
    
    
}
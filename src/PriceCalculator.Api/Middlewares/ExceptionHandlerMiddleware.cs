using PriceCalculator.Bll.Exceptions;

namespace PriceCalculator.Api.Middlewares;

public sealed class ExceptionHandlerMiddleware
{
    
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.StatusCode = 500;
            response.ContentType = "text/plain";
            
            await response.WriteAsync($"{exception.GetType().FullName}: {exception.Message}");
        }
    }
}
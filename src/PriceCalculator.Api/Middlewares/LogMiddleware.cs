using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;

namespace PriceCalculator.Api.Middlewares;

public class LogMiddleware
{
    private readonly ILogger<LogMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LogMiddleware(
        ILogger<LogMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        var sr = new StreamReader(context.Request.Body);
        var bodyString = await sr.ReadToEndAsync();
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        var headersString = JsonSerializer.Serialize(context.Request.Headers);

        var requestUrl = context.Request.GetDisplayUrl();

        var timeStamp = DateTime.UtcNow;
        
        _logger.LogInformation("Request");
        _logger.LogInformation($"\tTimestamp: {timeStamp}\n Body: {bodyString}\n Headers: {headersString}\n Path: {requestUrl}");
        
        var originalResponseBody = context.Response.Body;
        using var newResponseBody = new MemoryStream();
        context.Response.Body = newResponseBody;
        
        await _next.Invoke(context);
        
        newResponseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        newResponseBody.Seek(0, SeekOrigin.Begin);
        
        await newResponseBody.CopyToAsync(originalResponseBody);
        
        
        _logger.LogInformation("Response");
        _logger.LogInformation($"\t Body: {responseBodyText}");
    }
    
}
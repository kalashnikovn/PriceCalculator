using System.Net;
using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.ActionFilters;
using PriceCalculator.Infrastructure;

namespace PriceCalculator.Api;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDomain(_configuration)
            .AddInfrastructure()
            .AddControllers()
            .AddMvcOptions(ConfigureMvc)
            .Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(o => o.CustomSchemaIds(x => x.FullName?.Replace("+", ".")))
            .AddHttpContextAccessor();
    }

    private static void ConfigureMvc(MvcOptions x)
    {
        x.Filters.Add(new ExceptionFilterAttribute());
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
        x.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
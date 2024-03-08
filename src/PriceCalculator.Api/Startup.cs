using System.Net;
using Microsoft.AspNetCore.Mvc;
using PriceCalculator.Api.ActionFilters;
using PriceCalculator.Api.Bll;
using PriceCalculator.Api.Bll.Services;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Repositories;
using PriceCalculator.Api.Dal.Repositories.Interfaces;
using PriceCalculator.Api.HostedServices;
using PriceCalculator.Api.Middlewares;

namespace PriceCalculator.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(
        IConfiguration configuration
        )
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
            .AddMvcOptions(x =>
            {
                x.Filters.Add(new ExceptionFilterAttribute());
                
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
                x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
                x.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
            });
        
        
        services.Configure<PriceCalculatorOptions>(_configuration.GetSection("PriceCalculatorOptions"));
        services.Configure<GoodsServiceOptions>(_configuration.GetSection("GoodsServiceOptions"));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(x => x.FullName);
        });
        services.AddHostedService<GoodsSyncHostedService>();
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();
        services.AddScoped<IGoodsService, GoodsService>();
    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next.Invoke();
        });

        app.UseMiddleware<ErrorMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
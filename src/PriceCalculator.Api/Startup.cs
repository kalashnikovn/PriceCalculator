using FluentValidation.AspNetCore;
using PriceCalculator.Api.Middlewares;
using PriceCalculator.BackgroundServices.Extensions;
using PriceCalculator.Bll.Extensions;
using PriceCalculator.Dal.Extensions;
using PriceCalculator.SerializeUtils.NamingPolicies;

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
            .AddBll()
            .AddDalInfrastructure(_configuration)
            .AddDalRepositories()
            .AddBackgroundServices(_configuration)
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy())
            .AddFluentValidation(conf =>
            {
                conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
                conf.AutomaticValidationEnabled = true;
            })
            .Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(o => o.CustomSchemaIds(x => x.FullName?.Replace("+", ".")));

    }
    

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
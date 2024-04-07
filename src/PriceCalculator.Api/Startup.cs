using Calzolari.Grpc.AspNetCore.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using PriceCalculator.Api.GrpcServices;
using PriceCalculator.Api.GrpcServices.Interceptors;
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
            .Services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(typeof(Program).Assembly)
            .AddGrpcValidation()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(o => o.CustomSchemaIds(x => x.FullName?.Replace("+", ".")))
            .AddGrpcReflection()
            .AddGrpc(options =>
            {
                options.Interceptors.Add<ExceptionInterceptor>();
                options.EnableMessageValidation();
            });
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
            endpoints.MapGrpcService<DeliveryPriceCalculatorService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}
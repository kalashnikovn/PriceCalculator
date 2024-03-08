using PriceCalculator.Api.Bll;
using PriceCalculator.Api.Bll.Services;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Repositories;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

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
        services.Configure<PriceCalculatorOptions>(_configuration.GetSection("PriceCalculatorOptions"));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(x => x.FullName);
        });
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
        services.AddSingleton<IStorageRepository, StorageRepository>();
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
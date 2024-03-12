using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PriceCalculator.Domain;
using PriceCalculator.Domain.Separated;
using PriceCalculator.Domain.Services;
using PriceCalculator.Domain.Services.Interfaces;
using PriceCalculator.Infrastructure.Dal.Repositories;
using PriceCalculator.Infrastructure.External;

namespace PriceCalculator.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PriceCalculatorOptions>(configuration.GetSection("PriceCalculatorOptions"));
        
        services.AddScoped<IPriceCalculatorService, PriceCalculatorService>(x =>
        {
            var options = x.GetRequiredService<IOptionsSnapshot<PriceCalculatorOptions>>().Value;
            var repository = x.GetRequiredService<IStorageRepository>();
            return new PriceCalculatorService(options, repository);
        });
        
        
        services.AddScoped<IGoodPriceCalculatorService, GoodPriceCalculatorService>();

        return services;
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IStorageRepository, StorageRepository>();
        services.AddSingleton<IGoodsRepository, GoodsRepository>();
        services.AddScoped<IGoodsService, GoodsService>();


        return services;
    }
}
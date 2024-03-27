using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceCalculator.Dal.Infrastructure;
using PriceCalculator.Dal.Options;
using PriceCalculator.Dal.Repositories;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.Dal.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGoodsRepository, GoodsRepository>();
        services.AddScoped<ICalculationsRepository, CalculationsRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddDalInfrastructure(
        this IServiceCollection services, 
        IConfiguration config)
    {
        //read config
        services.Configure<DalOptions>(config.GetSection(nameof(DalOptions)));

        //configure postrges types
        Postgres.MapCompositeTypes();
        
        //add migrations
        Postgres.AddMigrations(services);
        
        return services;

    }
}
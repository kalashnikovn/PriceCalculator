using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceCalculator.Dal.Infrastructure;
using PriceCalculator.Dal.Options;

namespace PriceCalculator.Dal.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDalInfrastructure(
        this IServiceCollection services, 
        IConfiguration config)
    {
        //read config
        services.Configure<DalOptions>(config.GetSection(nameof(DalOptions)));

        //configure postrges types
        //Postgres.MapCompositeTypes();
        
        //add migrations
        Postgres.AddMigrations(services);
        
        return services;

    }
}
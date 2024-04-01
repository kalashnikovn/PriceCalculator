using Microsoft.Extensions.DependencyInjection;
using PriceCalculator.Bll.Services;
using PriceCalculator.Bll.Services.Interfaces;

namespace PriceCalculator.Bll.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services
            .AddTransient<ICalculationService, CalculationService>()
            .AddTransient<IAnomalyService, AnomalyService>()
            .AddMediatR(c =>
                c.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            
        
        return services;
    }
}
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceCalculator.Dal.Extensions;
using PriceCalculator.Dal.Repositories.Interfaces;

namespace PriceCalculator.IntegrationTests.Fixtures;

public class TestFixture
{
    public ICalculationsRepository CalculationRepository { get; }
    
    public IGoodsRepository GoodsRepository { get; }
    
    public TestFixture()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddDalInfrastructure(config)
                    .AddDalRepositories();
            })
            .Build();
        
        ClearDatabase(host);
        host.MigrateUp();

        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        CalculationRepository = serviceProvider.GetRequiredService<ICalculationsRepository>();
        GoodsRepository = serviceProvider.GetRequiredService<IGoodsRepository>();
    }
    
    private static void ClearDatabase(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateDown(1);
    }
}
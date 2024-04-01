using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceCalculator.ProducerApp.Fakers;
using PriceCalculator.ProducerApp.Interfaces;
using PriceCalculator.ProducerApp.Messages;
using PriceCalculator.ProducerApp.Options;

namespace PriceCalculator.ProducerApp;

public static class Program
{
    public static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        services
            .Configure<ProducerAppOptions>(configuration.GetSection("ProducerAppOptions"))
            .Configure<RandomOptions>(configuration.GetSection("RandomOptions"))
            .AddTransient<IModelFaker<CalculateRequestMessage>, CalculateRequestMessageFaker>()
            .AddSingleton<ProducerApp>();

        var serviceProvider = services.BuildServiceProvider();
        var app = serviceProvider.GetRequiredService<ProducerApp>();
        
        await app.Run();
    }
}
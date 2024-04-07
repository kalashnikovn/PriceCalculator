using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceCalculator.GrpcClient.Interfaces;
using PriceCalculator.GrpcClient.Options;

namespace PriceCalculator.GrpcClient;

public class Program
{
    public static async Task Main()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var services = new ServiceCollection();

        services
            .Configure<GrpcClientOptions>(configuration.GetSection("GrpcClientOptions"))
            .AddTransient<IContext, Context>()
            .AddTransient<ClientApp>();

        var serviceProvider = services.BuildServiceProvider();
        var app = serviceProvider.GetRequiredService<ClientApp>();

        await app.Run();
    }
}
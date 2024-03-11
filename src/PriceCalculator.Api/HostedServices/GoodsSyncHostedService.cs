using Microsoft.Extensions.Options;
using PriceCalculator.Api.Bll;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.HostedServices;

public class GoodsSyncHostedService : BackgroundService
{
    private readonly IGoodsRepository _repository;
    private readonly IServiceProvider _serviceProvider;
    
    public GoodsSyncHostedService(
        IGoodsRepository repository,
        IServiceProvider serviceProvider)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var goodsService = scope.ServiceProvider.GetRequiredService<IGoodsService>();
                var goods = goodsService.GetGoods().ToList();
                foreach (var good in goods)
                    _repository.AddOrUpdate(good);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
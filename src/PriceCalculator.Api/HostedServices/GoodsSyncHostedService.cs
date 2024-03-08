using Microsoft.Extensions.Options;
using PriceCalculator.Api.Bll;
using PriceCalculator.Api.Bll.Services.Interfaces;
using PriceCalculator.Api.Dal.Repositories.Interfaces;

namespace PriceCalculator.Api.HostedServices;

public class GoodsSyncHostedService : BackgroundService
{
    private readonly IGoodsRepository _repository;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GoodsSyncHostedService> _logger;
    private int _taskDelay = 10;
    
    public GoodsSyncHostedService(
        IGoodsRepository repository,
        IServiceProvider serviceProvider,
        ILogger<GoodsSyncHostedService> logger,
        IOptionsMonitor<GoodsServiceOptions> optionsMonitor)
    {
        _repository = repository;
        _serviceProvider = serviceProvider;
        _logger = logger;

        optionsMonitor.OnChange(x =>
        {
            _taskDelay = x.TaskDelay;
        });
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: get and update goods
            _logger.LogInformation("Hosted service: getting goods..");
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var goodsService = scope.ServiceProvider.GetRequiredService<IGoodsService>();

                var goods = goodsService.GetGoods();
                
                foreach (var good in goods)
                    _repository.AddOrUpdate(good);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(_taskDelay), stoppingToken);
        }
    }
}
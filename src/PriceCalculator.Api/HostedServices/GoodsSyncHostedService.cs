using Microsoft.Extensions.Options;
using PriceCalculator.Api.Bll;

namespace PriceCalculator.Api.HostedServices;

public class GoodsSyncHostedService : BackgroundService
{
    private readonly ILogger<GoodsSyncHostedService> _logger;
    private int _taskDelay = 10;
    
    public GoodsSyncHostedService(
        ILogger<GoodsSyncHostedService> logger,
        IOptionsMonitor<GoodsServiceOptions> optionsMonitor)
    {
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
            _logger.LogInformation("Hosted service");
            
            await Task.Delay(TimeSpan.FromSeconds(_taskDelay), stoppingToken);
        }
    }
}
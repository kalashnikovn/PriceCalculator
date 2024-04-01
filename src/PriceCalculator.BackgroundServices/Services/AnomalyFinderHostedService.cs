using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PriceCalculator.BackgroundServices.Messages;
using PriceCalculator.BackgroundServices.Options;
using PriceCalculator.Bll.Commands;

namespace PriceCalculator.BackgroundServices.Services;

public sealed class AnomalyFinderHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptionsMonitor<AnomalyFinderHostedServiceOptions> _optionsMonitor;
    private readonly IConsumer<long, CalculateResultMessage> _consumer;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public AnomalyFinderHostedService(
        IServiceProvider serviceProvider,
        IOptionsMonitor<AnomalyFinderHostedServiceOptions> optionsMonitor,
        IConsumer<long, CalculateResultMessage> consumer)
    {
        _serviceProvider = serviceProvider;
        _optionsMonitor = optionsMonitor;
        _consumer = consumer;
        _cancellationTokenSource = new CancellationTokenSource();
        
        _consumer.Subscribe(_optionsMonitor.CurrentValue.CalculateResultsTopic);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cancellationToken = _cancellationTokenSource.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            var  result = await Task.Run(() => _consumer.Consume(stoppingToken));

            var messageValue = result.Message.Value;

            if (messageValue.Price > _optionsMonitor.CurrentValue.DeliveryPriceThreshold)
                await SaveAnomalyCalculation(messageValue, stoppingToken);
            
            _consumer.StoreOffset(result);
        }
        
        _consumer.Close();
    }

    private async Task SaveAnomalyCalculation(
        CalculateResultMessage resultMessage,
        CancellationToken cancellationToken)
    {
        var saveAnomalyCommand = new SaveAnomalyCommand(
            resultMessage.GoodId,
            resultMessage.Price);
        
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(saveAnomalyCommand, cancellationToken);
    }
}
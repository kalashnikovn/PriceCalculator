using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PriceCalculator.BackgroundServices.Messages;
using PriceCalculator.BackgroundServices.Options;

namespace PriceCalculator.BackgroundServices.Services;

public sealed class GoodPriceCalculatorHostedService : BackgroundService
{
    private readonly IOptionsMonitor<GoodPriceCalculatorHostedServiceOptions> _optionsMonitor;
    private readonly IConsumer<long, CalculateRequestMessage> _consumer;

    public GoodPriceCalculatorHostedService(
        IOptionsMonitor<GoodPriceCalculatorHostedServiceOptions> optionsMonitor,
        IConsumer<long, CalculateRequestMessage> consumer)
    {
        _optionsMonitor = optionsMonitor;
        _consumer = consumer;
        
        _consumer.Subscribe(optionsMonitor.CurrentValue.CalculateRequestsTopic);
    }
    
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            ConsumeResult<long, CalculateRequestMessage> result;
            try
            {
                result = await Task.Run(() => _consumer.Consume(stoppingToken));
            }
            catch (ConsumeException consumeException)
            {
                var topicPartitionOffset = new TopicPartitionOffset(
                    consumeException.ConsumerRecord.TopicPartition,
                    consumeException.ConsumerRecord.Offset + 1,
                    consumeException.ConsumerRecord.LeaderEpoch
                    );
                
                _consumer.StoreOffset(topicPartitionOffset);
                continue;
            }
            

            var message = result.Message.Value;
            
            Console.WriteLine(message.GoodId);
            
            _consumer.StoreOffset(result);
        }
        
        _consumer.Close();
    }
}
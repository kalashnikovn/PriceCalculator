using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PriceCalculator.BackgroundServices.Messages;
using PriceCalculator.BackgroundServices.Options;
using PriceCalculator.BackgroundServices.Validators;

namespace PriceCalculator.BackgroundServices.Services;

public sealed class GoodPriceCalculatorHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptionsMonitor<GoodPriceCalculatorHostedServiceOptions> _optionsMonitor;
    private readonly IConsumer<long, CalculateRequestMessage> _consumer;

    public GoodPriceCalculatorHostedService(
        IServiceProvider serviceProvider,
        IOptionsMonitor<GoodPriceCalculatorHostedServiceOptions> optionsMonitor,
        IConsumer<long, CalculateRequestMessage> consumer)
    {
        _serviceProvider = serviceProvider;
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
                await SendMessageToDeadLetterQueue(
                    consumeException.ConsumerRecord,
                    consumeException.Error.Reason,
                    stoppingToken);
                
                var topicPartitionOffset = new TopicPartitionOffset(
                    consumeException.ConsumerRecord.TopicPartition,
                    consumeException.ConsumerRecord.Offset + 1,
                    consumeException.ConsumerRecord.LeaderEpoch
                );

                _consumer.StoreOffset(topicPartitionOffset);
                continue;
            }

            var messageValue = result.Message.Value;
            
            var validator = new CalculateRequestMessageValidator();
            var validationResult = await validator.ValidateAsync(messageValue, stoppingToken);

            if (!validationResult.IsValid)
            {
                await SendMessageToDeadLetterQueue(
                    result,
                    validationResult.ToString(),
                    stoppingToken);
                
                _consumer.StoreOffset(result);
                continue;
            }

            
            Console.WriteLine(messageValue.GoodId);

            _consumer.StoreOffset(result);
        }

        _consumer.Close();
    }

    private async Task SendMessageToDeadLetterQueue<TKey, TValue>(
        ConsumeResult<TKey, TValue> consumeResult,
        string errorReason,
        CancellationToken cancellationToken)
    {
        var message = new Message<TKey, TValue>()
        {
            Headers = new()
            {
                { "Reason", Encoding.Default.GetBytes(errorReason) },
            },
            Key = consumeResult.Message.Key,
            Value = consumeResult.Message.Value
        };

        var producer = _serviceProvider.GetRequiredService<IProducer<TKey, TValue>>();

        await producer.ProduceAsync(
            _optionsMonitor.CurrentValue.DeadLetterQueueTopic,
            message,
            cancellationToken);
    }
}
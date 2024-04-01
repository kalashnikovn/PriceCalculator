using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PriceCalculator.BackgroundServices.Messages;
using PriceCalculator.BackgroundServices.Options;
using PriceCalculator.BackgroundServices.Serializers;
using PriceCalculator.BackgroundServices.Services;

namespace PriceCalculator.BackgroundServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .Configure<GoodPriceCalculatorHostedServiceOptions>(
                configuration.GetSection("GoodPriceCalculatorHostedServiceOptions"))
            .AddHostedService<GoodPriceCalculatorHostedService>()
            .AddTransient(CreateCalculateRequestMessagesConsumer)
            .AddTransient(CreateProducer<long, CalculateRequestMessage>)
            .AddTransient(CreateProducer<byte[], byte[]>);

        return services;
    }

    private static IConsumer<long, CalculateRequestMessage> CreateCalculateRequestMessagesConsumer(
        IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptions<GoodPriceCalculatorHostedServiceOptions>>()
            .Value;

        var kafkaConsumer = new ConsumerBuilder<long, CalculateRequestMessage>(
                new ConsumerConfig()
                {
                    BootstrapServers = options.BootstrapServers,
                    GroupId = options.GroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    EnableAutoCommit = true,
                    EnableAutoOffsetStore = false
                })
            .SetValueDeserializer(new JsonValueSerializer<CalculateRequestMessage>())
            .Build();

        return kafkaConsumer;
    }

    private static IProducer<TKey, TValue> CreateProducer<TKey, TValue>(
        IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptions<GoodPriceCalculatorHostedServiceOptions>>()
            .Value;

        var kafkaProducer = new ProducerBuilder<TKey, TValue>(
                new ProducerConfig()
                {
                    BootstrapServers = options.BootstrapServers,
                    Acks = Acks.None
                })
            .SetValueSerializer(new JsonValueSerializer<TValue>())
            .Build();

        return kafkaProducer;
    }
}
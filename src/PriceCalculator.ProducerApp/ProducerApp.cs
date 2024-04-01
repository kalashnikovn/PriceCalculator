using Confluent.Kafka;
using Microsoft.Extensions.Options;
using PriceCalculator.ProducerApp.Interfaces;
using PriceCalculator.ProducerApp.Messages;
using PriceCalculator.ProducerApp.Options;
using PriceCalculator.ProducerApp.Serializers;

namespace PriceCalculator.ProducerApp;

public sealed class ProducerApp
{
    private readonly IOptionsMonitor<ProducerAppOptions> _optionsMonitor;
    private readonly IModelFaker<CalculateRequestMessage> _modelFaker;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly IProducer<long, CalculateRequestMessage> _producer;

    public ProducerApp(
        IOptionsMonitor<ProducerAppOptions> optionsMonitor,
        IModelFaker<CalculateRequestMessage> modelFaker)
    {
        _optionsMonitor = optionsMonitor;
        _modelFaker = modelFaker;
        _cancellationTokenSource = new CancellationTokenSource();

        _producer = new ProducerBuilder<long, CalculateRequestMessage>(new ProducerConfig()
            {
                BootstrapServers = _optionsMonitor.CurrentValue.BootstrapServers,
                Acks = Acks.None
            })
            .SetValueSerializer(new JsonValueSerializer<CalculateRequestMessage>())
            .Build();
        
        
    }
    
    public async Task Run()
    {
        var cancellationToken = _cancellationTokenSource.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("\nВведите количество сообщений для отправки (exit для выхода)");
            Console.Write("/>>> ");

            var command = Console.ReadLine();

            if (command == "exit")
            {
                _cancellationTokenSource.Cancel();
            }
            else if (int.TryParse(command, out var count))
            {
                var messages = _modelFaker.GenerateMany(count);

                await ProduceMessages(messages, cancellationToken);
                Console.WriteLine($"Отправлено сообщений: {messages.Count}");
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте снова");
            }
        }
    }


    private async Task ProduceMessages(
        IEnumerable<CalculateRequestMessage> messages,
        CancellationToken cancellationToken)
    {
        foreach (var calculateRequestMessage in messages)
        {
            await _producer.ProduceAsync(
                _optionsMonitor.CurrentValue.CalculateRequestsTopic,
                new Message<long, CalculateRequestMessage>()
                {
                    Key = calculateRequestMessage.GoodId,
                    Value = calculateRequestMessage
                },
                cancellationToken);
        }
    }
}
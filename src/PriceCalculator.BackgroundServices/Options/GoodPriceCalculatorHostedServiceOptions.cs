namespace PriceCalculator.BackgroundServices.Options;

public sealed class GoodPriceCalculatorHostedServiceOptions
{
    public string BootstrapServers { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string CalculateRequestsTopic { get; set; } = null!;
    public string CalculateResultsTopic { get; set; } = null!;
    public string DeadLetterQueueTopic { get; set; } = null!;
}
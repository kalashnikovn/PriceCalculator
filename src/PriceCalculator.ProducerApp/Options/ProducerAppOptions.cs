namespace PriceCalculator.ProducerApp.Options;

public sealed class ProducerAppOptions
{
    public string BootstrapServers { get; set; } = null!;
    public string CalculateRequestsTopic { get; set; } = null!;
}
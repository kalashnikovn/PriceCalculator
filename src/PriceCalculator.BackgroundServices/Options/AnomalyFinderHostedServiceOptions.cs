namespace PriceCalculator.BackgroundServices.Options;

public class AnomalyFinderHostedServiceOptions
{
    public string BootstrapServers { get; set; } = null!;
    public string GroupId { get; set; } = null!;
    public string CalculateResultsTopic { get; set; } = null!;
    public decimal DeliveryPriceThreshold { get; set; }
}
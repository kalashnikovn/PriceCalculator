namespace PriceCalculator.Dal.Options;

public record DalOptions
{
    public string ConnectionString { get; set; } = string.Empty;
}
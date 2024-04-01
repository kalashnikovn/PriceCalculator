namespace PriceCalculator.Dal.Entities;

public record AnomalyEntityV1
{
    public long Id { get; init; }
    public long GoodId { get; init; }
    public decimal Price { get; init; }
};
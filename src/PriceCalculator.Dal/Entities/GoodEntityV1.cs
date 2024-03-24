namespace PriceCalculator.Dal.Entities;

public record GoodEntityV1
{
    public long Id { get; init; }
    
    public long UserId { get; init; }
    
    public long Width { get; init; }
    
    public long Height { get; init; }
    
    public long Length { get; init; }
}
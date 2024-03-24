namespace PriceCalculator.Dal.Entities;

public record GoodEntityV1
{
    public long Id { get; init; }
    
    public long UserId { get; init; }
    
    public double Width { get; init; }
    
    public double Height { get; init; }
    
    public double Length { get; init; }
    
    public double Weight { get; init; }
}
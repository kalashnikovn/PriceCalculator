namespace PriceCalculator.Api.Dal.Entities;

public record GoodEntity(
    string Name,
    int Id,
    int Height,
    int Length,
    int Width,
    int Weight,
    int Count,
    decimal Price
);
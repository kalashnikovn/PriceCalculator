namespace PriceCalculator.Domain.Models.PriceCalculator;

public record GoodModel(
    decimal Length,
    decimal Width,
    decimal Height,
    decimal Weight = 0
    );
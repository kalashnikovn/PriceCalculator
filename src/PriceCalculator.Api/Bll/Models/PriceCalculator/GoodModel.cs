namespace PriceCalculator.Api.Bll.Models.PriceCalculator;

public record GoodModel(
    decimal Length,
    decimal Width,
    decimal Height,
    decimal Weight = 0
    );
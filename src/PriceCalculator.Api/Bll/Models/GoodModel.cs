namespace PriceCalculator.Api.Bll.Models;

public record GoodModel(
    int Length,
    int Width,
    int Height,
    double Weight = 0
    );
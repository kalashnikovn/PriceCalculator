namespace PriceCalculator.Api.Bll.Models.Goods;

public record GoodModel(
    string Name,
    int Id,
    int Height,
    int Length,
    int Width,
    int Weight,
    decimal Price);
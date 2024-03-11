namespace PriceCalculator.Domain.Models.PriceCalculator;

public sealed record CalculateRequestModel(GoodModel[] Goods, decimal Distance);
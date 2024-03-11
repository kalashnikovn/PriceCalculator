namespace PriceCalculator.Api.Bll.Models.PriceCalculator;

public sealed record CalculateRequestModel(GoodModel[] Goods, decimal Distance);
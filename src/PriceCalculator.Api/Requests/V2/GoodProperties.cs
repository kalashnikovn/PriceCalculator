namespace PriceCalculator.Api.Requests.V2;

/// <summary>
/// Харектеристики товара
/// </summary>
public record GoodProperties(
    decimal Height,
    decimal Length,
    decimal Width,
    decimal Weight
    );
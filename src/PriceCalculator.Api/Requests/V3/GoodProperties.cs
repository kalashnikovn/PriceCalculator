namespace PriceCalculator.Api.Requests.V3;

public record GoodProperties(
    decimal Length,
    decimal Width,
    decimal Height,
    decimal Weight
    );
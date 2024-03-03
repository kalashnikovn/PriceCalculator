namespace PriceCalculator.Api.Requests.V2;

public record CalculateRequest(
    GoodProperties[] Goods
    );
namespace PriceCalculator.Api.Requests.V1;

public record CalculateRequest(
    GoodProperties[] goods
    );
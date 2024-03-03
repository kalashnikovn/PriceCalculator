namespace PriceCalculator.Api.Responses.V3;

public record GetHistoryResponse(
    CargoResponse Cargo,
    decimal Price,
    decimal Distance
    );
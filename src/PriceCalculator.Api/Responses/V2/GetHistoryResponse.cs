namespace PriceCalculator.Api.Responses.V2;

public record GetHistoryResponse(
    CargoResponse Cargo,
    decimal Price
    );
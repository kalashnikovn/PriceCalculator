namespace PriceCalculator.Api.Responses.V1;

public record GetHistoryResponse(
    GetHistoryResponse.CargoResponse Cargo,
    decimal Price
)
{
    public record CargoResponse(
        decimal Volume,
        decimal Weight,
        long[] GoodIds
    );
}
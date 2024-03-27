namespace PriceCalculator.Api.Requests.V1;

public record ClearHistoryRequest(
    long UserId,
    long[] CalculationIds);
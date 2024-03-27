namespace PriceCalculator.Api.Requests.V1;

public record GetHistoryRequest(
    long UserId,
    int Take,
    int Skip,
    long[] CalculationIds
    );
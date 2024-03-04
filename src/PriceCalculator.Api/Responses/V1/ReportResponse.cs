namespace PriceCalculator.Api.Responses.V1;

public record ReportResponse(
    decimal MaxWeight,
    decimal MaxVolume,
    decimal MaxDistanceForHeaviestGood,
    decimal MaxDistanceForLargestGood,
    decimal WavgPrice
    );
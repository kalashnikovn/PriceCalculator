namespace PriceCalculator.Api.Bll.Models.Analytics;

public record ReportModel(
    decimal MaxWeight,
    decimal MaxVolume,
    decimal MaxDistanceForHeaviestGood,
    decimal MaxDistanceForLargestGood,
    decimal WavgPrice
    );
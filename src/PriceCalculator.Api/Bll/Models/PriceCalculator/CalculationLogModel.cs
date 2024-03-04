namespace PriceCalculator.Api.Bll.Models.PriceCalculator;

public record CalculationLogModel(
    decimal Volume,
    decimal Price,
    decimal Weight,
    decimal Distance
    );
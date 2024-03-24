namespace PriceCalculator.Bll.Models;

public record QueryCalculationFilter(
    long UserId,
    int Limit,
    int Offset);
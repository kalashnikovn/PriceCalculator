namespace PriceCalculator.Bll.Models;

public record RemoveCalculationsModel(
    long UserId,
    long[] CalculationIds);
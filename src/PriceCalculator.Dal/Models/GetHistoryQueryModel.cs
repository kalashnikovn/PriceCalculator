namespace PriceCalculator.Dal.Models;

public record GetHistoryQueryModel(
    long UserId,
    int Limit,
    int Offset);
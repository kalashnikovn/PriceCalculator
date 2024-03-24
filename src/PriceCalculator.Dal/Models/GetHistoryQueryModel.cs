namespace PriceCalculator.Dal.Models;

public record GetHistoryQueryModel(
    long UserId,
    int Take,
    int Offset);
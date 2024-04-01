namespace PriceCalculator.BackgroundServices.Messages;

public record CalculateResultMessage(
    long GoodId,
    decimal Price);
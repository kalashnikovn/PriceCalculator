namespace PriceCalculator.ProducerApp.Messages;

public record CalculateRequestMessage(
    long GoodId,
    double Height,
    double Length,
    double Width,
    double Weight);
namespace PriceCalculator.GrpcClient.Models;

public record CalculateRequestModel(
    long GoodId,
    double Height,
    double Length,
    double Width,
    double Weight);
namespace PriceCalculator.Api.Dal.Entities;

public record StorageEntity(
    decimal Volume,
    decimal Price,
    DateTime At,
    decimal Weight,
    decimal Distance
    );
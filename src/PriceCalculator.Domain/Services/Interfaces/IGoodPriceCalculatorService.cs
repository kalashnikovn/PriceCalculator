namespace PriceCalculator.Domain.Services.Interfaces;

public interface IGoodPriceCalculatorService
{
    decimal CalculatePrice(int goodId, decimal distance);
}
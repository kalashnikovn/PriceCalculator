namespace PriceCalculator.Api.Bll.Services.Interfaces;

public interface IGoodPriceCalculatorService
{
    decimal CalculatePrice(int goodId, decimal distance);
}
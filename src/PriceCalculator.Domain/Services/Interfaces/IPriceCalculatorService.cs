using PriceCalculator.Domain.Models.PriceCalculator;

namespace PriceCalculator.Domain.Services.Interfaces;

public interface IPriceCalculatorService
{
    CalculationLogModel[] QueryLog(int take);
    decimal CalculatePrice(CalculateRequestModel requestModel);
}
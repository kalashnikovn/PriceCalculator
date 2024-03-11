using PriceCalculator.Domain.Models.PriceCalculator;

namespace PriceCalculator.Domain.Services.Interfaces;

public interface IPriceCalculatorService
{
    CalculationLogModel[] QueryLog(int take);
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods);
    decimal CalculatePrice(CalculateRequestModel requestModel);
}
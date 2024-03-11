using PriceCalculator.Api.Bll.Models.PriceCalculator;

namespace PriceCalculator.Api.Bll.Services.Interfaces;

public interface IPriceCalculatorService
{
    CalculationLogModel[] QueryLog(int take);
    decimal CalculatePrice(IReadOnlyList<GoodModel> goods);
    decimal CalculatePrice(CalculateRequestModel requestModel);
}